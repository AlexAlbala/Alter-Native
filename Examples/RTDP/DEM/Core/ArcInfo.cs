using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace DemManager
{
    //-------------------------------------------------------------------------
    /// <summary>
    /// This class implements some of Dem class inherited methods for 
    /// ASCII/ArcInfo Dem file format.
    /// </summary>
    //-------------------------------------------------------------------------
    public abstract class ArcInfo : Dem
    {
        //---------------------------------------------------------------------
        /// <summary>
        /// A Stream Reader used for reading text.
        /// </summary>
        //---------------------------------------------------------------------
        protected StreamReader sr;

        //---------------------------------------------------------------------
        /// <summary>
        /// Main constructor.
        /// </summary>
        /// <param name="path">The path of the file.</param>
        //---------------------------------------------------------------------
        protected ArcInfo(string path, Precision precision)
            : base(path, precision)
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
        /// A <see cref="Points.Point"/> object that represents the coordinates
        /// of the bottom left corner of the elevation matrix.
        /// </param>
        /// <param name="altitude">
        /// The elevation matrix
        /// </param>
        /// <param name="noData">
        /// The value that represent whether there is no altitude data in a 
        /// specified cell.
        /// </param>
        /// <param name="cellSize">
        /// The geometric distance among adjacent cells.
        /// </param>
        //---------------------------------------------------------------------
        protected ArcInfo( int rows, int cols, Point bottomLeft, 
            double[,] altitude, double noData, double cellSize, 
            int metCellSize, Precision precision)
            : base(rows, cols, bottomLeft, altitude, noData, cellSize, 
                metCellSize, precision)
        { }

        //---------------------------------------------------------------------
        /// <summary>
        /// Alternative Constructor.
        /// </summary>
        //---------------------------------------------------------------------
        protected ArcInfo()
            : base()
        { }

        //---------------------------------------------------------------------
        /// <summary>
        /// Returns the terrain altitude from a specified waypoint.
        /// </summary>
        /// <param name="p">
        /// The waypoint you want to know the terrain altitude.
        /// </param>
        /// <returns>
        /// The terrain altitude.
        /// </returns>
        //---------------------------------------------------------------------
        public override double getAltitudeFromCoords(Point p)
        {
            if (getRefSystem() == Point.refSystem.HAYFORD)
                p = new HayPoint(p.toWgs());
            else if (getRefSystem() == Point.refSystem.WGS84)
                p = p.toWgs();

            int indY = (int)
                ((p.getUtmY() - this.bottomLeft.getUtmY()) / this.cellSize);
            indY = this.rows - indY;
            int indX = (int)
                ((p.getUtmX() - this.bottomLeft.getUtmX()) / this.cellSize);
            return this.altitude[indY, indX];
        }

        //---------------------------------------------------------------------
        /// <summary>
        /// Returns the corresponding matrix indexs for a specified waypoint .
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
            p = p.fromWgs(p.toWgs());
            int col = (int)
                ((p.getUtmX() - this.bottomLeft.getUtmX()) / this.cellSize);
            int row = (int)
                ((p.getUtmY() - this.bottomLeft.getUtmY()) / this.cellSize);
            row = this.rows - row;
            if (p.getAltitude() == null)
                return new Index(row, col);
            else
                return new Index(row, col, (double)p.getAltitude());
        }

        //---------------------------------------------------------------------
        /// <summary>
        /// Return the terrain altitude from the specified indexs.
        /// </summary>
        /// <param name="index">
        /// The specified indexs.
        /// </param>
        /// <returns>
        /// The terrain altitude.
        /// </returns>
        //---------------------------------------------------------------------
        public override WgsPoint indexToPoint(Index index)
        {
            double utmY = this.bottomLeft.getUtmY() + this.cellSize * 
                (this.rows - index.getRow());
            double utmX = this.bottomLeft.getUtmX() + this.cellSize * 
                index.getCol();
            if (this.bottomLeft.getLatitude() < 0)
                return new WgsPoint(utmX, utmY, index.getAlt(),
                    this.bottomLeft.getTimeZone(), 'S');
            else
                return new WgsPoint(utmX, utmY, index.getAlt(),
                    this.bottomLeft.getTimeZone(), 'N');

        }

        //---------------------------------------------------------------------
        /// <summary>
        /// Check whether a waypoint is included in the elevation matrix.
        /// </summary>
        /// <param name="p">
        /// The specified point.
        /// </param>
        /// <returns>
        /// TRUE if the waypoint is included in the elevation matrix. 
        /// FALSE if th waypoint is not included in the matrix.
        /// </returns>
        //---------------------------------------------------------------------
        public override bool isIncluded(Point p)
        {
            p = p.fromWgs(p.toWgs());
            double minX = this.bottomLeft.getUtmX();
            double maxX = minX + this.cellSize * this.cols;
            double minY = this.bottomLeft.getUtmY();
            double maxY = minY + this.cellSize * this.rows;
            double utmX = p.getUtmX();
            double utmY = p.getUtmY();
            if (utmX > minX && utmX < maxX && utmY > minY && utmY < maxY)
                return true;
            else
                return false;
        }
    }
}