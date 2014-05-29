using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Reflection;
using System.Collections;
using System.IO;
using log4net;
using Marea;
using System.Threading;

using RTDPInterface;
using StorageInterface;
using CameraControllerInterface;

namespace RTDP
{
    public class RTDPService : IService
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private IServiceContainer container;
        private bool end;

        private Thread segmentationThread;
        // Main RTDP object
        private RTDP rtdp;

        // Input/Output
        private Queue<string> inputThermalQueue;
        private Queue<string> inputMetadataQueue;

        private string lastThermalFile;
        private string lastMetadataFile;

        string tmpPath;

        FileId metadataFile;
        FileId thermalFile;

        private object monitor;

        FileId fusionPrimitive;
        int nPacketProcess;

        public bool Start(IServiceContainer container)
        {
            log.Info("RTDP service started");
            container.RegisterService(this);
            this.container = container;
            end = false;
            monitor = new object();

            // Start Data Procesing (this must be done before subscribing to image package!)
            RTDPSettings.ReadXML();
            UASProperties.ReadXML();
            rtdp = new RTDP(this, container);

            inputThermalQueue = new Queue<string>();
            inputMetadataQueue = new Queue<string>();

            nPacketProcess = 0;

            // CurrentDirectory is the Marea.exe directory
            tmpPath = Environment.CurrentDirectory + @"\..\RTDP\Tmp\";
            if (!System.IO.Directory.Exists(tmpPath))
            {
                try
                {
                    System.IO.Directory.CreateDirectory(tmpPath);
                }
                catch
                {
                    log.Info("Unable to create temp dir " + tmpPath);
                    end = true;
                }
            }

            //Subscribe to thermal images
            container.SubscribeEvent("CC.NewThermalImage", this);

            segmentationThread = new Thread(new ThreadStart(RunRTDP));
            segmentationThread.Start();

            return true;
        }

        public void RunRTDP()
        {
            while (!end)
            {
                Thread.Sleep(1000);

                while (inputThermalQueue.Count > 0 && inputMetadataQueue.Count > 0)
                {
                    string thName = inputThermalQueue.Dequeue();
                    string mtName = inputMetadataQueue.Dequeue();

                    if (System.IO.File.Exists(thName) && System.IO.File.Exists(mtName))
                    {
                        // PROCESS IMAGE PACKAGE
                        rtdp.ProcessThermalImage(thName, mtName);

                        int attempts = 0;
                        while (!rtdp.processed)
                        {
                            Thread.Sleep(500);
                            attempts++;
                            if (attempts == 10)
                                break;
                        }

                        if (!rtdp.processed)
                            continue;

                        // If one or more hotspots are found, save the fusion image and send an alarm for each hotspot
                        if (rtdp.NumHotSpots > 0)
                        {
                            if (RTDPSettings.sendHotspotAlarm)
                            {
                                // Send hotspot alarm  
                                foreach (HotSpot hs in rtdp.HotSpotsList)
                                {
                                    HotSpotAlarm hsAlarm = new HotSpotAlarm(hs.Geolocation.Latitude, hs.Geolocation.Longitude, hs.MaxTemp, rtdp.FusionName);
                                    container.EventFired("RTDP.HotSpotAlarm", hsAlarm);
                                    log.Info("RTDP.HotSpotAlarm event");
                                }
                            }

                            if (RTDPSettings.doImageFusion)
                            {
                                if (container.IsStarted("Storage"))//CAUTION. This feature only will work if Storage is working in this same container
                                {                                  //NAMING will resolve this conflict

                                    container.EventFired("Storage.CreateDir", new CreateDir("RTDP", "Fusion"));

                                    // Save fusion image in storage
                                    Stream fusionStream = rtdp.FusionStream;
                                    if (fusionStream == null)
                                    {
                                        //log.Error("Stream of the fusion is null. This fusion will be ignored");
                                        continue;
                                    }
                                    else
                                    {
                                        string filePrimitive = "FusionImage";
                                        SaveItem saveItem = new SaveItem(rtdp.FusionName, filePrimitive, "RTDP");
                                        container.EventFired("Storage.Save", saveItem);

                                        if (fusionPrimitive == null)
                                        {
                                            fusionPrimitive = container.PublishFile(filePrimitive, fusionStream, this);
                                        }
                                        else
                                        {
                                            container.UpdateFile(fusionPrimitive, fusionStream);
                                        }

                                        rtdp.FusionNum++;
                                        log.Info(filePrimitive + " saved");

                                        container.EventFired("RTDP.NewFusionImage", new Parameter(rtdp.FusionName, filePrimitive));
                                    }
                                }
                                else
                                {
                                    log.Info("Could not save the file: Storage service is not started.");
                                }
                            }
                        }

                        // Free bitmaps and streams
                        rtdp.Dispose();

                        // Delete zip file
                        try
                        {
                            //System.IO.File.Delete(viName);
                            System.IO.File.Delete(thName);
                            System.IO.File.Delete(mtName);
                        }
                        catch (Exception ex)
                        {
                            log.Info("Could not delete file. Original error: " + ex.Message);
                        }

                        log.Info("Process " + nPacketProcess + " completed");
                        nPacketProcess++;
                    }
                }
            }
        }

        public bool Stop(IServiceContainer container)
        {
            end = true;
            segmentationThread.Abort();
            container.UnregisterService(this);
            log.Info("RTDP service stopped");

            return true;
        }

        public void VariableChanged(string id, object value)
        {
        }

        public void EventFired(String id, object value)
        {
            switch (id)
            {
                case "CC.NewThermalImage":
                    Parameter p = (Parameter)value;
                    lastThermalFile = p.value + ".FPF";
                    if ((bool)container.CallFunctionT("isFileAvailable", this, new object[] { @"/FTV/" + lastThermalFile, "CameraController" }))
                    {
                        string primitive = "LoadThermal";
                        LoadItem li = new LoadItem(@"/FTV/" + lastThermalFile, primitive, "CameraController");
                        container.EventFired("Storage.Load", li);
                        log.Info("Loading thermal" + lastThermalFile);

                        if (thermalFile == null)
                        {
                            thermalFile = container.GetFile(primitive, tmpPath + lastThermalFile);
                            log.Info("GetFile(" + primitive + ", " + tmpPath + lastThermalFile + ")");
                            thermalFile.OnCompletion += new OnFileCompletionHandler(f_OnCompletion);
                            thermalFile.OnUpdateAvailable += new OnFileUpdateAvailableHandler(f_OnUpdateAvailable);
                        }
                    }
                    else
                    {
                        log.Info("Could not load thermal image: " + lastThermalFile);
                    }
                    break;
            }
        }

        void f_OnUpdateAvailable(FileId file)
        {
            container.UpdateFile(file, tmpPath + lastThermalFile);
        }

        void f_OnCompletion(FileId file)
        {
            log.Info("Completed!!!");
            lock (monitor)
            {
                inputThermalQueue.Enqueue(tmpPath + lastThermalFile);

                string xmlFileName = lastThermalFile.TrimEnd(new char[] { '.', 'F', 'P', 'F' }) + ".XML";
                if (container.IsStarted("Storage"))//CAUTION. This feature only will work if Storage is working in this same container
                {                                  //NAMING will resolve this conflict
                    int attempts = 0;
                    while (!(bool)container.CallFunctionT("isFileAvailable", this, new object[] { @"/XML/" + xmlFileName, "CameraController" }))
                    {
                        //Wait 1 second and try again if the file is available
                        Thread.Sleep(1000);
                        attempts++;
                        //After 5 times, I will stop
                        if (attempts == 5)
                            return;
                    }

                    //The file is now available
                    //Get the metadata XML file     
                    string primitive = "LoadMetadata";
                    LoadItem li = new LoadItem(@"/XML/" + xmlFileName, primitive, "CameraController");
                    container.EventFired("Storage.Load", li);
                    lastMetadataFile = xmlFileName + ".XML";

                    if (metadataFile == null)
                    {
                        metadataFile = container.GetFile(primitive, tmpPath + lastMetadataFile);
                        metadataFile.OnCompletion += new OnFileCompletionHandler(mtF_OnCompletion);
                        metadataFile.OnUpdateAvailable += new OnFileUpdateAvailableHandler(mtF_OnUpdateAvailable);
                    }
                }
                else
                {
                    log.Info("Could not load the file: Storage service is not started.");
                }
            }
        }

        void mtF_OnUpdateAvailable(FileId file)
        {
            lock (monitor)
            {
                container.UpdateFile(file, tmpPath + lastMetadataFile);
            }
        }

        void mtF_OnCompletion(FileId file)
        {
            inputMetadataQueue.Enqueue(tmpPath + lastMetadataFile);
            //XML COMPLETED !!!
        }

        public object FunctionCall(String id, object[] parameters)
        {
            return null;
        }

        public ServiceDescription GetDescription()
        {
            ServiceDescription s = new ServiceDescription();
            s.Name = "RTDP";
            s.Description = "This service subscribes to the Visual and IR camaras and process the images for hotspot detection and fusion.";
            s.Events = new EventDescription[5];
            s.Events[0] = new EventDescription();
            s.Events[0].Name = "RTDP.HotSpotAlarm";

            s.Events[1] = new EventDescription();
            s.Events[1].Name = "Storage.Save";

            s.Events[2] = new EventDescription();
            s.Events[2].Name = "Storage.Load";

            s.Events[3] = new EventDescription();
            s.Events[3].Name = "Storage.CreateDir";

            s.Events[4] = new EventDescription();
            s.Events[4].Name = "RTDP.NewFusionImage";

            return s;
        }
    }
}