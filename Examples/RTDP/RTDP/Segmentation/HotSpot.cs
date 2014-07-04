using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Drawing;
using USAL;

namespace RTDP
{
    public class HSPixel
    {
        int x;      // column
        int y;      // row
        float t;    // temperature

        public HSPixel(int x, int y, float t)
        {
            this.x = x;
            this.y = y;
            this.t = t;
        }

        public int X
        {
            get { return x; }
        }

        public int Y
        {
            get { return y; }
        }

        public float T
        {
            get { return t; }
        }
    }

    public class HotSpot
    {
        static int next_id = 1;

        string id;

        // Information computed from the IR image by the SEGMENTATION module
        // "_n" suffix refers to the horizontal/column coordinate
        // "_m" suffix refers to the vertical/row coordinate

        ArrayList pixels;   // Pixels that belong to the hotspot

        int center_n;       // Center of mass pixel 
        int center_m;       // Center of mass pixel 
        int max_n;
        int max_m;
        int min_n;
        int min_m;

        float max_t;
        float sum_t;
        float sum_nt;
        float sum_mt;

        float magnitude;

        // Information computed by the GEOLOCATION module
        Position centerGeolocation; // LLA position of the center in the terrain


        // desv : sqrt(1/(N-1) · sum (x_i - avg)^2

        public string Name { get { return id; } }

        public int N { get { return center_n; } }

        public int M { get { return center_m; } }

        public int MinN { get { return min_n; } }

        public int MaxN { get { return max_n; } }

        public int MinM { get { return min_m; } }

        public int MaxM { get { return max_m; } }

        public ArrayList Pixels { get { return pixels; } }

        public int NumPixels { get { return pixels.Count; } }

        public float MaxTemp { get { return max_t; } }

        public float AvgTemp { get { return (sum_t / pixels.Count); } }


        public float Magnitude
        {
            set { magnitude = value; }
            get { return magnitude; }
        }

        public Position Geolocation
        {
            set { centerGeolocation = value; }
            get { return centerGeolocation; }
        }

        public HotSpot(int column, int row, float temp)
        {
            id = "HS_" + next_id++;
            pixels = new ArrayList();
            pixels.Add(new HSPixel(column, row, temp));
            center_n = max_n = min_n = column;
            center_m = max_m = min_m = row;
            sum_nt = column * temp;
            sum_mt = row * temp;
            max_t = sum_t = temp;
        }

        public void AddPixel(int column, int row, float temp)
        {
            pixels.Add(new HSPixel(column, row, temp));
            if (column > max_n) max_n = column;
            if (column < min_n) min_n = column;
            if (row > max_m) max_m = row;
            if (row < min_m) min_m = row;
            if (temp > max_t) max_t = temp;
            sum_nt += column * temp;
            sum_mt += row * temp;
            sum_t += temp;
            center_n = (int)(sum_nt / sum_t);
            center_m = (int)(sum_mt / sum_t);
        }

        public void AddHotSpot(HotSpot h)
        {
            pixels.AddRange(h.pixels); 
            if (h.max_n > max_n) max_n = h.max_n;
            if (h.min_n < min_n) min_n = h.min_n;
            if (h.max_m > max_m) max_m = h.max_m;
            if (h.min_m < min_m) min_m = h.min_m;
            if (h.max_t > max_t) max_t = h.max_t;
            sum_nt += h.sum_nt;
            sum_mt += h.sum_mt;
            sum_t += h.sum_t;
            center_n = (int)(sum_nt / sum_t);
            center_m = (int)(sum_mt / sum_t);
        }

        public override string ToString()
        {
            float avg = sum_t / pixels.Count;

            string s = id + "\n";
            s += "  Center:      [" + center_n + ", " + center_m + "]\n";
            //s += "  Left: " + min_n;
            //s += "  Top: " + min_m;
            //s += "  Right: " + max_n;
            //s += "  Bottom: " + max_m + "\n";
            s += "  MaxTemp:     " + max_t.ToString("#0.00") + " K\n";
            s += "  AvgTemp:     " + avg.ToString("#0.00") + " K\n";
            s += "  NumPixels:   " + pixels.Count + "\n";
            s += "  Magnitude:   " + magnitude.ToString("#0.0000") + "\n";
            if (centerGeolocation != null)
                s += "  GeoLocation: " + centerGeolocation.ToStringDMS() + "\n";


            //foreach (HSPixel p in pixels)
            //{
            //    s += " [" + p.X + ", " + p.Y + "]";
            //}
            //s += "\n";
 
            return s;
        }
    }

    //public class HotSpotsList
    //{
    //    ArrayList hotSpots = new ArrayList();

    //    public int NumHotSpots
    //    {
    //        get { return hotSpots.Count; }
    //    }

    //    public ArrayList HotSpots
    //    {
    //        get { return hotSpots; }
    //    }

    //    public void Clear()
    //    {
    //        hotSpots.Clear();
    //    }

    //    public void Add(HotSpot h)
    //    {
    //        hotSpots.Add(h);
    //    }

    //    public void Remove(HotSpot h)
    //    {
    //        hotSpots.Remove(h);
    //    }

    //    public override string ToString()
    //    {
    //        string s = hotSpots.Count + " HOT SPOTS FOUND!!!\n";

    //        foreach (HotSpot h in hotSpots)
    //        {
    //            s += "\n" + h.PixelInfoString();
    //        }

    //        return s;
    //    }
    //}
}
