using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Reflection;
using System.Drawing;
using System.Collections;
using System.IO;

using USAL;

using System.Threading;

namespace RTDP
{
    class RTDP
    {
        private RTDPService service;

        // Current Image Package 
        Metadata metadata;
        FPFImage irImage;
        ViImage viImage;
        ArrayList hotSpots;

        // Main actions
        Segmentation segmentation;
        Fusion fusion;
        Geolocation irGeolocation;
        Geolocation viGeolocation;

        // Input/output
        string irName, viName, mtName;
        string tmpPath = Environment.CurrentDirectory + @"\..\..\Tmp\";
        string fusionName;
        Stream fusionStream;
        int fusionNum = 0;

        // State
        bool ready = false; // RTDP is (not) ready to process a new package
        bool _processed = false;

        public bool processed { get { return _processed; } }

        public bool IsReady { set { ready = value; } get { return ready; } }

        public int NumHotSpots { get { return hotSpots.Count; } }

        public ArrayList HotSpotsList { get { return hotSpots; } }

        public string FusionName { get { return fusionName; } }

        public Stream FusionStream { get { return fusionStream; } }

        public int FusionNum { set { fusionNum = value; } get { return fusionNum; } }

        public RTDP(RTDPService service)
        {
            this.service = service;

            hotSpots = new ArrayList();

            if (RTDPSettings.doSegmentation)
                segmentation = new Segmentation(UASProperties.irCamProperties);

            if (RTDPSettings.doImageFusion)
                fusion = new Fusion(UASProperties.irCamProperties, UASProperties.viCamProperties);

            if (RTDPSettings.doGeolocation)
            {
                if (RTDPSettings.geolocationSettings.geolocateVIimage)
                    viGeolocation = new Geolocation(UASProperties.viCamProperties, UASProperties.gpsProperties);

                if (RTDPSettings.geolocationSettings.geolocateHotspots || RTDPSettings.geolocationSettings.geolocateIRimage)
                    irGeolocation = new Geolocation(UASProperties.irCamProperties, UASProperties.gpsProperties);
            }

            DemManager.DemManagerService.GetInstance().FunctionCall("SetPrecision", new object[] { RTDPSettings.terrainModelSettings.demPrecision });

            metadata = new Metadata();

            this.DeleteTempFiles();

            // RTDP is now ready to process a package
            ready = true;
        }

        private void DeleteTempFiles()
        {
            DirectoryInfo di = new DirectoryInfo(tmpPath);

            foreach (FileInfo f in di.GetFiles())
            {
                f.Delete();
            }
        }

        public void ProcessThermalImage(string thermalName, string metadataName)
        {
            _processed = false;
            hotSpots.Clear();
            fusionName = null;

            irName = thermalName;
            mtName = metadataName;
            if (mtName != null)
            {
                if (RTDPSettings.doGeolocation || RTDPSettings.doImageFusion)
                {
                    try
                    {
                        // Load TELEMETRY
                        metadata.ReadXML(mtName);
                    }
                    catch (Exception ex)
                    {
                        _processed = true;
                        return;
                    }
                    float elevation = 0;
                    metadata.SetHeights(RTDPSettings.terrainModelSettings.localGeoidHeight, elevation);
                }
            }

            if (irName != null)
            {
                if (RTDPSettings.geolocationSettings.geolocateIRimage || RTDPSettings.doSegmentation || RTDPSettings.doImageFusion)
                {
                    // Load IR IMAGE
                    try
                    {
                        irImage = new FPFImage(irName);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.ToString());
                        _processed = true;
                        return;
                    }

                    if (RTDPSettings.doSegmentation)
                    {
                        // SEGMENTATION
                        segmentation.Init(irImage, metadata);
                        hotSpots = segmentation.Solve();
                    }

                    if (mtName != null)
                    {
                        if (RTDPSettings.geolocationSettings.geolocateIRimage || (RTDPSettings.geolocationSettings.geolocateHotspots && hotSpots.Count > 0))
                        {
                            // GEOLOCATION
                            irGeolocation.Init(metadata.uavTelemetry);
                            if (RTDPSettings.geolocationSettings.geolocateHotspots && hotSpots.Count > 0)
                            {
                                foreach (HotSpot hs in hotSpots)
                                {
                                    hs.Geolocation = irGeolocation.GetPixelGeolocation(hs.M, hs.N);
                                }
                            }
                            if (RTDPSettings.geolocationSettings.geolocateIRimage)
                            {
                                irImage.GeorefInfo = irGeolocation.GetImageGeolocation();
                            }
                        }
                    }

                    if (RTDPSettings.geolocationSettings.geolocateVIimage || (RTDPSettings.doImageFusion && NumHotSpots > 0))
                    {
                        //TODO: Load VISUAL IMAGE                        
                        //On error -> _processed = true;
                        this.ProcessVisualImage();
                        viImage = new ViImage(viName);
                    }

                    if (NumHotSpots == 0)
                        _processed = true;

                }
            }
        }

        private void ProcessVisualImage()
        {
            // GEOLOCATION
            if (RTDPSettings.geolocationSettings.geolocateVIimage)
            {
                viGeolocation.Init(metadata.uavTelemetry);
                viImage.GeorefInfo = viGeolocation.GetImageGeolocation();
            }

            // FUSION
            if (RTDPSettings.doImageFusion && NumHotSpots > 0)
            {
                fusion.Init(irImage, viImage, metadata);
                if (RTDPSettings.fusionSettings.fusionMode == FusionSettings.FusionMode.Merge)
                {
                    fusion.Merge();
                }
                if (RTDPSettings.fusionSettings.fusionMode == FusionSettings.FusionMode.Mark)
                {
                    fusion.MarkHotSpots(hotSpots);
                }

                // Save fused image to a stream as Jpeg 70% quality
                try
                {
                    fusionStream = new MemoryStream();
                    viImage.SaveJpeg(fusionStream, 70);
                    fusionName = @"\Fusion\F" + fusionNum.ToString("D4") + "_" + System.IO.Path.GetFileName(viName);
                    viImage.Dispose();
                    File.Delete(viName);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    fusionName = null;
                    _processed = true;
                    return;
                }
            }
            _processed = true;
        }

        // Free bitmaps
        public void Dispose()
        {
            if (irImage != null)
                irImage.Dispose();

            if (viImage != null)
                viImage.Dispose();
        }
    }
}
