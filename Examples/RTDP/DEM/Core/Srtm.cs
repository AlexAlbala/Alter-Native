using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;


namespace DemManager
{
    //-------------------------------------------------------------------------
    /// <summary>
    /// 
    /// </summary>
    //-------------------------------------------------------------------------
    public abstract class Srtm : Dem
    {
        //---------------------------------------------------------------------
        /// <summary>
        /// Main Constructor.
        /// </summary>
        /// <param name="path">
        /// The path of SRTM file.
        /// </param>
        //---------------------------------------------------------------------
        public Srtm(string path, Precision precision)
            :base(path, precision)
        { }

        //---------------------------------------------------------------------
        /// <summary>
        /// Void Constructor.
        /// </summary>
        //---------------------------------------------------------------------
        public Srtm()
            :base()
        { }

        //---------------------------------------------------------------------
        /// <summary>
        /// Alternative constructor. Use it when a header file is needed to be 
        /// loaded.
        /// </summary>
        /// <param name="path">
        /// The elevation matrix container file path.
        /// </param>
        /// <param name="headerPath">
        /// The header container file path.
        /// </param>
        //---------------------------------------------------------------------
        public Srtm(string path, string headerPath, Precision precision)
            : base(path, headerPath, precision)
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
        /// <param name="cellSize">
        /// The geometric distance among adjacent cells.
        /// </param>
        //---------------------------------------------------------------------
        protected Srtm(int rows, int cols, Point bottomLeft, 
            double[,] altitude, double noData, double cellSize, 
            int metCellSize, Precision precision)
            : base(rows, cols, bottomLeft, altitude, noData, cellSize, 
            metCellSize, precision)
        { }

        //---------------------------------------------------------------------
        /// <summary>
        /// Returns the terrain altitude from a specified waypoint.
        /// </summary>
        /// <param name="p">
        /// The waypoint you want to know the terrain altitude.
        /// </param>
        /// <returns>
        /// The terrain altitude
        /// </returns>
        //---------------------------------------------------------------------
        public override double getAltitudeFromCoords(Point p)
        {
            if (p is HayPoint)
            {
                HayPoint hay = (HayPoint)p;
                p = hay.toWgs();
            }
            int indY = (int)((p.getLatitude() - this.bottomLeft.getLatitude()) / this.cellSize);
            indY = this.rows - indY;
            int indX = (int)((p.getLongitude() - this.bottomLeft.getLongitude()) / this.cellSize);
            if (indX >= this.cols || indY >= this.rows)
                return double.MaxValue;
            else
                return this.altitude[indY, indX];
        }

        //---------------------------------------------------------------------
        /// <summary>
        /// Reads altitude matrix
        /// </summary>
        /// <remarks>
        /// Note that we do not use a Binary reader because of endianess 
        /// issues. We have to read byte per byte and then reverse the array in 
        /// order to read the altitude values properly.
        /// </remarks>
        //---------------------------------------------------------------------
        protected override void readAltitudes()
        {
            try
            {
                FileInfo fInfo = new FileInfo(path);
                FileStream fSteam = fInfo.OpenRead();
                int count = 0;
                int nBytes = 2;
                byte[] byteArray = new byte[2];
                int nBytesRead = fSteam.Read(byteArray, 0, nBytes);
                while (count < rows * cols && nBytesRead == 2)
                {
                    Array.Reverse(byteArray, 0, byteArray.Length);
                    int alt = BitConverter.ToInt16(byteArray, 0);
                    int row = Convert.ToInt32(
                        Math.Floor(Convert.ToDouble(count) / cols));
                    int col = count % cols;
                    this.altitude[row, col] = alt;
                    count++;
                    nBytesRead = fSteam.Read(byteArray, 0, nBytesRead);
                }
            }
            catch (FileNotFoundException)
            {
                this.altitude.Initialize();
            }
        }

        //---------------------------------------------------------------------
        /// <summary>
        /// Returns the corresponding matrix indexs for a specified waypoint.
        /// </summary>
        /// <param name="p">
        /// The specified waypoint.
        /// </param>
        /// <returns>
        /// The indexs.
        /// </returns>
        //---------------------------------------------------------------------
        public override Index pointToIndex(Point p)
        {
            p = p.toWgs();
            int indX = (int)
                ((p.getLongitude() - this.bottomLeft.getLongitude()) / 
                this.cellSize);
            int indY = (int)
                ((p.getLatitude() - this.bottomLeft.getLatitude()) / 
                this.cellSize);
            indY = this.rows - indY;
            if (p.getAltitude() == null)
                return new Index(indY, indX);
            else
                return new Index(indY, indX, (double)p.getAltitude());
        }

        //---------------------------------------------------------------------
        /// <summary>
        /// Returns the correspoding waypoint from the specified indexs.
        /// </summary>
        /// <param name="index">
        /// The specified indexs.
        /// </param>
        /// <returns>
        /// The corresponding waypoint.
        /// </returns>
        //---------------------------------------------------------------------
        public override WgsPoint indexToPoint(Index index)
        {
            double latitude = this.bottomLeft.getLatitude() + 
                this.cellSize * (this.rows - index.getRow());
            double longitude = this.bottomLeft.getLongitude() + 
                this.cellSize * index.getCol();
            return new WgsPoint(latitude, longitude, index.getAlt());
        }

        //---------------------------------------------------------------------
        /// <summary>
        /// Check whether a waypoint is included in the elevation matrix.
        /// </summary>
        /// <param name="p">
        /// The specified point
        /// </param>
        /// <returns>
        /// TRUE if the waypoint is included in the elevation matrix. 
        /// FALSE if th waypoint is not included in the matrix.
        /// </returns>
        //---------------------------------------------------------------------
        public override bool isIncluded(Point p)
        {
            //TODO:: Hemisphere issues?
            p = p.toWgs();
            double minLat = this.bottomLeft.getLatitude();
            double maxLat = minLat + cellSize * rows;
            double minLon = this.bottomLeft.getLongitude();
            double maxLon = minLon + cellSize * cols;
            double lat = p.getLatitude();
            double lon = p.getLongitude();
            if (lat > minLat && lat < maxLat && lon > minLon && lon < maxLon)
                return true;
            else
                return false;
        }
    }
}




            