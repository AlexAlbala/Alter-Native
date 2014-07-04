using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;



namespace DemManager
{
    //-------------------------------------------------------------------------
    /// <summary>
    /// This class implements all of the necessary methods inherited from DEM 
    /// abstract class to implement the capability for reading Strm binary 
    /// (Big-Endian) format Dem files.
    /// </summary>
    //---------------------------------------------------------------------
    public class Srtm3 : Srtm
    {
        //---------------------------------------------------------------------
        /// <summary>
        /// The cell size in meters.
        /// </summary>
        //---------------------------------------------------------------------
        public static readonly int METCELLSIZE = 90;

        //---------------------------------------------------------------------
        /// <summary>
        /// The cell size of the elevation matrix.
        /// </summary>
        //---------------------------------------------------------------------
        public static readonly double CELLSIZE = 3.0 / 3600.0;

        //---------------------------------------------------------------------
        /// <summary>
        /// The value that represents whether there is no altitude data in a 
        /// specified cell.
        /// </summary>
        //---------------------------------------------------------------------
        public static readonly int NODATA = -32678;

        //---------------------------------------------------------------------
        /// <summary>
        /// Main Constructor.
        /// </summary>
        /// <param name="path">
        /// The path of STRM file.
        /// </param>
        //---------------------------------------------------------------------
        public Srtm3(string path)
            : base(path, Precision.medium)
        { }

        //---------------------------------------------------------------------
        /// <summary>
        /// Alternative Constructor.
        /// </summary>
        /// <param name="bottomLeft">
        /// Waypoint that represents the bottom left corner position of 
        /// altitude matrix.
        /// </param>
        /// <param name="altitude">
        /// The altitude matrix.
        /// </param>
        //---------------------------------------------------------------------
        public Srtm3(WgsPoint bottomLeft, double[,] altitude)
            : base(1201, 1201, bottomLeft, altitude, NODATA, CELLSIZE, 
            METCELLSIZE, Precision.medium)
        { }

        //---------------------------------------------------------------------
        /// <summary>
        /// Alternative constructor.
        /// </summary>
        /// <param name="rows">
        /// The number of rows of the elevation matrix.
        /// </param>
        /// <param name="cols">
        /// The number of columns of the elevation matrix.
        /// </param>
        /// <param name="bottomLeft">
        /// Waypoint that represents the bottom left corner position of 
        /// altitude matrix.
        /// </param>
        /// <param name="altitude">
        /// The altitude matrix.
        /// </param>
        //---------------------------------------------------------------------
        public Srtm3(int rows, int cols, WgsPoint bottomLeft, 
            double[,] altitude)
            : base(rows, cols, bottomLeft, altitude, NODATA, CELLSIZE, 
            METCELLSIZE, Precision.medium)
        { }

        //---------------------------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        //---------------------------------------------------------------------
        public Srtm3()
            : base()
        { }

        //---------------------------------------------------------------------
        /// <summary>
        /// Reads header info.
        /// </summary>
        //---------------------------------------------------------------------
        protected override void readHeader()
        {
            string[] str = this.path.Split('N', 'E', 'S', 'W', '.');
            int lat, lon;
            if (this.path.Contains("N"))
                lat = Convert.ToInt32(str[1]);
            else
                lat = (-1) * Convert.ToInt32(str[1]);
            if (path.Contains("E"))
                lon = Convert.ToInt32(str[2]);
            else
                lon = (-1) * Convert.ToInt32(str[2]);
            this.bottomLeft = new WgsPoint(lat, lon, null);
            this.cellSize = CELLSIZE;
            this.noData = NODATA;
            this.cols = 1201;
            this.rows = 1201;
            this.altitude = new double[this.rows, this.cols];
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
                    selection[
                        i - indTopRight.getRow(),
                        j - indBottomLeft.getCol()] =
                        this.altitude[i - 1, j];
                }
            }
            return new Srtm3(selectRows, selectCols, 
                (WgsPoint)this.indexToPoint(indBottomLeft), 
                selection);
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
            return Point.refSystem.WGS84;
        }
    }
}



           



            