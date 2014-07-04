using System.Collections;
using System.Drawing;
using System;

namespace RTDP
{
    public class Segmentation
    {
        FPFImage irImage;   // Input thermal image
        Metadata metadata;
        CAMProperties camera; 

        float refArea;     // Reference area for estimation of the hot spot magnitude

        public Segmentation(CAMProperties camera)
        {
            this.camera = camera;
        }

        public void Init(FPFImage img, Metadata mt)
        {
            this.irImage = img;
            this.metadata = mt;

            this.refArea = 1;
        }

        // IE'09 segmentation algorithm to find hot spots
        // Returns the list of hotspots
        public ArrayList Solve()
        {
            float threshold = RTDPSettings.segmentationSettings.hotspotThreshold;

            ArrayList hotspots = new ArrayList();

            if (irImage.DataTmax < threshold)
                return hotspots;

            float[] data = irImage.FPFData;
            HotSpot[] line = new HotSpot[irImage.Width];
            int x, y, i;
            float t;

            // process first line
            x = y = i = 0;
            t = data[i++];
            if (t < threshold)
            {
                line[x] = null;
            }
            else
            {
                HotSpot h = new HotSpot(x, y, t);
                hotspots.Add(h);
                line[x] = h;
            }

            for (x = 1; x < irImage.Width; x++)
            {
                t = data[i++];

                if (t < threshold)
                {
                    line[x] = null;
                }
                else if (line[x - 1] == null)
                {
                    HotSpot h = new HotSpot(x, y, t);
                    hotspots.Add(h);
                    line[x] = h;
                }
                else
                {
                    line[x - 1].AddPixel(x, y, t);
                    line[x] = line[x - 1];
                }
            }

            // process next lines
            for (y = 1; y < irImage.Height; y++)
            {

                x = 0;
                t = data[i++];

                if (t < threshold)
                {
                    line[x] = null;
                }
                else if (line[x] == null)
                {
                    HotSpot h = new HotSpot(x, y, t);
                    hotspots.Add(h);
                    line[x] = h;
                }
                else
                {
                    line[x].AddPixel(x, y, t);
                }

                for (x = 1; x < irImage.Width; x++)
                {

                    t = data[i++];

                    if (t < threshold)
                    {
                        line[x] = null;
                    }
                    else if ((line[x] == null) && (line[x - 1] == null))
                    {
                        HotSpot h = new HotSpot(x, y, t);
                        hotspots.Add(h);
                        line[x] = h;
                    }
                    else if ((line[x] == null) && (line[x - 1] != null))
                    {
                        line[x - 1].AddPixel(x, y, t);
                        line[x] = line[x - 1];
                    }
                    else if ((line[x] != null) && (line[x - 1] == null))
                    {
                        line[x].AddPixel(x, y, t);
                    }
                    else if (line[x] == line[x - 1])
                    {
                        line[x].AddPixel(x, y, t);
                    }
                    else
                    {
                        HotSpot h = line[x - 1];
                        line[x].AddHotSpot(h);
                        line[x].AddPixel(x, y, t);
                        for (int j = 0; j < irImage.Width; j++)
                        {
                            if (line[j] == h) line[j] = line[x];
                        }
                        hotspots.Remove(h);
                    }
                }
            }

            float k = (float)((metadata.AGL * metadata.AGL * camera.pixelPitch * camera.pixelPitch) / (camera.focalLength * camera.focalLength * refArea));
            foreach (HotSpot h in hotspots)
            {
                h.Magnitude = k * (float)h.NumPixels * (h.AvgTemp - threshold) / (irImage.RangeTmax - threshold);
            }

            return hotspots;
        }
    }
}
