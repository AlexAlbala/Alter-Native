using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;


namespace DemManager
{
    //-------------------------------------------------------------------------
    /// <summary>
    /// This class implements the SRTM Dem format for a 30'' cellsize.
    /// </summary>
    //-------------------------------------------------------------------------
    public class Srtm30 : Srtm
    {
        //---------------------------------------------------------------------
        /// <summary>
        /// The cell size in meters.
        /// </summary>
        //---------------------------------------------------------------------
        public static readonly int METCELLSIZE = 900;

        //---------------------------------------------------------------------
        /// <summary>
        /// The cell size of the elevation matrix.
        /// </summary>
        //---------------------------------------------------------------------
        public static readonly double CELLSIZE = 30.0 / 3600.0;

        //---------------------------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        //---------------------------------------------------------------------
        public static readonly int NODATA = -9999;

        //---------------------------------------------------------------------
        /// <summary>
        /// Main Constructor.
        /// </summary>
        /// <param name="path">
        /// The path of STRM file
        /// </param>
        //---------------------------------------------------------------------
        public Srtm30(string dataPath, string headerPath)
            : base(dataPath, headerPath, Precision.low)
        { }

        //---------------------------------------------------------------------
        /// <summary>
        /// Alternative Constructor.
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
        //---------------------------------------------------------------------
        public Srtm30(int rows, int cols, WgsPoint bottomLeft,
            double[,] altitude)
            : base(rows, cols, bottomLeft, altitude, NODATA, CELLSIZE, 
            METCELLSIZE, Precision.low)
        { }

        //---------------------------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        //---------------------------------------------------------------------
        public Srtm30()
            : base()
        { }

        //---------------------------------------------------------------------
        /// <summary>
        /// Reads header info.
        /// </summary>
        //---------------------------------------------------------------------
        protected override void readHeader()
        {
            //Read Header File
            StreamReader sr = new StreamReader(this.headerPath);
            sr.ReadLine(); sr.ReadLine();
            string[] aux = sr.ReadLine().Split(' ');
            this.rows = Convert.ToInt32(aux[aux.Length - 1]);
            aux = sr.ReadLine().Split(' ');
            this.cols = Convert.ToInt32(aux[aux.Length - 1]);
            sr.ReadLine(); sr.ReadLine(); sr.ReadLine(); sr.ReadLine(); sr.ReadLine();
            aux = sr.ReadLine().Split(' ');
            this.noData = Convert.ToInt32(aux[aux.Length - 1]);
            aux = sr.ReadLine().Split(' ');
            double lon = Convert.ToDouble(aux[aux.Length - 1]);
            aux = sr.ReadLine().Split(' ');
            double lat = Convert.ToDouble(aux[aux.Length - 1]);
            this.cellSize = CELLSIZE;
            this.altitude = new double[this.rows, this.cols];
            lat = Convert.ToDouble(Math.Floor(lat - this.cellSize * this.rows + 1));
            this.bottomLeft = new WgsPoint(lat, lon, null);
        }

        //---------------------------------------------------------------------
        /// <summary>
        /// Returns an altitude submatrix delimited by the specified waypoints.
        /// </summary>
        /// <param name="bottomLeft">
        /// Bottom left corner waypoint.
        /// </param>
        /// <param name="topRight">
        /// Top right corner waypoint.
        /// </param>
        /// <returns>
        /// The altiude submatrix.
        /// </returns>
        //---------------------------------------------------------------------
        public override Dem getSelection(Point bottomLeft, 
            Point topRight)
        {
            bottomLeft = bottomLeft.toWgs();
            topRight = topRight.toWgs();
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
                    selection
                        [i - indTopRight.getRow(),
                        j - indBottomLeft.getCol()] = this.altitude[i - 1, j];
                }
            }
            return new Srtm30(selectRows, selectCols, 
                (WgsPoint)this.indexToPoint(indBottomLeft), selection);
        }

        //---------------------------------------------------------------------
        /// <summary>
        /// Return the Reference System used in this Dem.
        /// </summary>
        /// <returns>
        /// The reference System
        /// </returns>
        //---------------------------------------------------------------------
        protected override Point.refSystem getRefSystem()
        {
            return Point.refSystem.WGS84;
        }
    }
}
