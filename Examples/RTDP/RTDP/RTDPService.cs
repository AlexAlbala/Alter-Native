using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Reflection;
using System.Collections;
using System.IO;
using System.Threading;

namespace RTDP
{
    public class RTDPService
    {
        private bool end;
        
        // Main RTDP object
        private RTDP rtdp;

        // Input/Output
        private Queue<string> inputThermalQueue;
        private Queue<string> inputMetadataQueue;

        private string lastThermalFile;
        private string lastMetadataFile;

        string tmpPath;

        //FileId metadataFile;
        //FileId thermalFile;

        private object monitor;

        //FileId fusionPrimitive;
        int nPacketProcess;

        public static void Main()
        {
            RTDPService rtd = new RTDPService();
            rtd.Start();
        }

        public bool Start()
        {
            end = false;
            monitor = new object();

            // Start Data Procesing (this must be done before subscribing to image package!)
            RTDPSettings.ReadXML();
            UASProperties.ReadXML();
            rtdp = new RTDP(this);

            inputThermalQueue = new Queue<string>();
            inputMetadataQueue = new Queue<string>();

            nPacketProcess = 0;

            // CurrentDirectory is the Marea.exe directory
            tmpPath = Environment.CurrentDirectory + @"\..\..\Tmp\";
            if (!System.IO.Directory.Exists(tmpPath))
            {
                try
                {
                    System.IO.Directory.CreateDirectory(tmpPath);
                }
                catch
                {
                    end = true;
                }
            }

            //Subscribe to thermal images
            //container.SubscribeEvent("CC.NewThermalImage", this);
            RunRTDP();
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
                            if (RTDPSettings.doImageFusion)
                            {

                                Stream fusionStream = rtdp.FusionStream;
                                if (fusionStream == null)
                                {
                                    //log.Error("Stream of the fusion is null. This fusion will be ignored");
                                    continue;
                                }
                                else
                                {
                                    //Save file
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
                            Console.WriteLine("Could not delete file. Original error: " + ex.Message);
                        }

                        nPacketProcess++;
                    }
                }
            }
        }

        public bool Stop()
        {
            end = true;            
            return true;
        }       
    }
}
