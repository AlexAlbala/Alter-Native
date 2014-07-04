using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;


namespace DemManager
{
    //-------------------------------------------------------------------------
    /// <summary>
    /// Icc class implements all of the necessary methods inherited from DEM 
    /// abstract class to implement the capability for reading Icc 
    /// ASCII/ArcInfo format Dem files.
    /// </summary>
    public class Icc : ArcInfo
    {
        //---------------------------------------------------------------------
        /// <summary>
        /// The cell size in meters.
        /// </summary>
        //---------------------------------------------------------------------
        public static readonly int METCELLSIZE = 30;

        //---------------------------------------------------------------------
        /// <summary>
        /// The value that represents whether there is no altitude data in a 
        /// specified cell.
        /// </summary>
        //---------------------------------------------------------------------
        public static readonly int NODATA = -32678;

        //---------------------------------------------------------------------
        /// <summary>
        /// The time zone of the terrain represented by the DEM.
        /// </summary>
        //---------------------------------------------------------------------
        public static readonly int timeZone = 31;

        //---------------------------------------------------------------------
        /// <summary>
        /// The cell size of the elevation matrix.
        /// </summary>
        //---------------------------------------------------------------------
        public static readonly int CELLSIZE = 30;

        //---------------------------------------------------------------------
        //Top-Right and Bottom-Left corner coordinates of the territory where 
        //there is information avaliable.
        /// <summary>
        /// Botton-Left corner X coordinate.
        /// </summary>
        //---------------------------------------------------------------------
        public static readonly double MINHAYUTMX = 257995;

        //---------------------------------------------------------------------
        /// <summary>
        /// Bottom-Left corner Y coordinate.
        /// </summary>
        //---------------------------------------------------------------------
        public static readonly double MINHAYUTMY = 4484975;

        //---------------------------------------------------------------------
        /// <summary>
        /// Top-Right corner X coordinate.
        /// </summary>
        //---------------------------------------------------------------------
        public static readonly double MAXHAYUTMX = 535075;

        //---------------------------------------------------------------------
        /// <summary>
        /// Top-Right corner Y coordinate.
        /// </summary>
        //---------------------------------------------------------------------
        public static readonly double MAXHAYUTMY = 4752035;

        //---------------------------------------------------------------------
        // Paths of the files that contain the west and east matrix elevation 
        //respectively.
        /// <summary>
        /// Catalonia western region elevation matrix file path.
        /// </summary>
        //---------------------------------------------------------------------
        public static readonly string PATHWEST = "cata30_oest_rev1.txt";

        //---------------------------------------------------------------------
        /// <summary>
        /// Catalonia eastern region elevation matrix file path.
        /// </summary>
        //---------------------------------------------------------------------
        public static readonly string PATHEAST = "cata30_est_rev1.txt";

        //---------------------------------------------------------------------
        /// <summary>
        /// Main Constructor.
        /// </summary>
        /// <param name="path">
        /// The path of the DEM file
        /// </param>
        //---------------------------------------------------------------------
        public Icc(string path)
            : base(path, Precision.high)
        { }

        //---------------------------------------------------------------------
        /// <summary>
        /// Alternative constructor.
        /// </summary>
        /// <param name="rows">
        /// Number of rows of the altitude matrix.
        /// </param>
        /// <param name="cols">
        /// Number of columns of the altitude matrix.
        /// </param>
        /// <param name="bottomLeft">
        /// Waypoint that represents the bottom left corner position of 
        /// altitude matrix.
        /// </param>
        /// <param name="altitude">
        /// The altitude matrix.
        /// </param>
        /// <param name="noData">
        /// The value that represent whether there is no altitude data in a 
        /// specified cell.
        /// </param>
        /// <param name="cellSize">
        /// The geometric distance among adjacent cells.
        /// </param>
        /// <param name="coordSystem">
        /// The coordinate system that DEM is referred.
        /// </param>
        //---------------------------------------------------------------------
        public Icc(int rows, int cols, Point bottomLeft, 
            double[,] altitude)
            : base(rows, cols, bottomLeft, altitude, NODATA, CELLSIZE, 
            METCELLSIZE, Precision.high) 
        { }

        //---------------------------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        //---------------------------------------------------------------------
        public Icc()
            : base()
        { }

        //---------------------------------------------------------------------
        /// <summary>
        /// Reads DEM file header.
        /// </summary>
        //---------------------------------------------------------------------
        protected override void readHeader()
        {
            this.sr = new StreamReader(this.path);
            string line;
            line = sr.ReadLine();
            string[] words = line.Split(' ');
            int j = 1;
            double x = 0, y = 0;
            for (int i = 0; i < 6; i++)
            {
                switch (i)
                {
                    case 0:
                        while (words[words.Length - j] == "")
                            j++;
                        this.cols = Convert.ToInt32(words[words.Length - j]);
                        j = 1;
                        break;
                    case 1:
                        while (words[words.Length - j] == "")
                            j++;
                        this.rows = Convert.ToInt32(words[words.Length - j]);
                        j = 1;
                        break;
                    case 2:
                        while (words[words.Length - j] == "")
                            j++;
                        x = Convert.ToDouble(words[words.Length - j]);
                        j = 1;
                        break;
                    case 3:
                        while (words[words.Length - j] == "")
                            j++;
                        y = Convert.ToDouble(words[words.Length - j]);
                        j = 1;
                        break;
                    case 4:
                        while (words[words.Length - j] == "")
                            j++;
                        this.cellSize = 
                            Convert.ToDouble(words[words.Length - j]);
                        j = 1;
                        break;
                    case 5:
                        while (words[words.Length - j] == "")
                            j++;
                        this.noData = 
                            Convert.ToDouble(words[words.Length - j]);
                        j = 1;
                        break;
                }
                if (i != 5)
                {
                    line = sr.ReadLine();
                    words = line.Split(' ');
                }
            }
            this.bottomLeft = new HayPoint(x, y, null, timeZone, 'N');
        }

        //---------------------------------------------------------------------
        /// <summary>
        /// Reads DEM file altitudes.
        /// </summary>
        //---------------------------------------------------------------------
        protected override void readAltitudes()
        {
            string line;
            this.altitude = new double[this.rows, this.cols];
            int count = 0;
            line = sr.ReadLine();
            while (count < this.cols * this.rows && line != null)
            {
                string[] words = line.Split(' ');
                foreach (string str in words)
                {
                    if (str.Length > 0)
                    {
                        int r = Convert.ToInt32(Math.Floor(Convert.ToDouble(count) / this.cols));
                        int c = count % this.cols;
                        this.altitude[r, c] = Convert.ToDouble(str);
                        count++;
                    }
                }
                line = sr.ReadLine();
            }
            sr.Close();
        }

        //---------------------------------------------------------------------
        /// <summary>
        /// Returns an altitude submatrix delimited by the specified waypoints.
        /// </summary>
        /// <param name="bottomLeft">Bottom left corner waypoint.</param>
        /// <param name="topRight">Top right corner waypoint.</param>
        /// <returns>The altiude submatrix.</returns>
        //---------------------------------------------------------------------
        public override Dem getSelection(Point bottomLeft, 
            Point topRight)
        {
            bottomLeft = new HayPoint(bottomLeft.toWgs());
            topRight = new HayPoint(topRight.toWgs());
            Index indBottomLeft = 
                new Index(pointToIndex(bottomLeft).getRow() + 2, 
                                pointToIndex(bottomLeft).getCol() - 2);
            Index indTopRight = 
                new Index(pointToIndex(topRight).getRow() - 2, 
                                pointToIndex(topRight).getCol() + 2);

            int selectRows = 
                (int)(indBottomLeft.getRow() - indTopRight.getRow() + 1);
            int selectCols = 
                (int)(indTopRight.getCol() - indBottomLeft.getCol() + 1);
            double[,] selection = new double[selectRows, selectCols];
            for (int i = indTopRight.getRow();
                i < (indTopRight.getRow() + selectRows);
                i++)
            {
                for (int j = indBottomLeft.getCol();
                    j < (indBottomLeft.getCol() + selectCols);
                    j++)
                {
                    selection[i - indTopRight.getRow(), j - indBottomLeft.getCol()] 
                        =  this.altitude[i - 1, j];
                }
            }
            return new Icc(selectRows, selectCols, bottomLeft, selection);
        }

        //---------------------------------------------------------------------
        /// <summary>
        /// Return the Reference System used in this Dem.
        /// </summary>
        /// <returns>
        /// The reference System.
        /// </returns>
        //---------------------------------------------------------------------
        protected override Point.refSystem getRefSystem()
        {
            return Point.refSystem.HAYFORD;
        }
    }
}
