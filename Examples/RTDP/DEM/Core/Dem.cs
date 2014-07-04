using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;



namespace DemManager
{
    //-------------------------------------------------------------------------
    /// <summary>
    /// This class defines the base methods that every single DEM data type 
    /// must implement.
    /// </summary>
    /// <remarks>
    /// A Data Terrain Model is essentially a matrix of altitudes from a 
    /// specified region. You need to know a reference position (usually the 
    /// bottom left corner of the matrix, its dimensions, the size of the cells 
    /// (the geometric distance among adjacent cells), a value that represents 
    /// whether there is no altitude data in a specified cell.
    /// </remarks>
    //-------------------------------------------------------------------------
    public abstract class Dem
    {
        //---------------------------------------------------------------------
        /// <summary>
        /// A enumerable specifying the DEM precision.
        /// </summary>
        //---------------------------------------------------------------------
        public enum Precision { high, medium, low }

        //---------------------------------------------------------------------
        /// <summary>
        /// A <see cref="Precision"/> specifying the DEM precision.
        /// </summary>
        //---------------------------------------------------------------------
        public Precision precision;

        //---------------------------------------------------------------------
        /// <summary>
        /// The cell size in meters (approx.).
        /// </summary>
        //---------------------------------------------------------------------
        protected int metCellSize;

        //---------------------------------------------------------------------
        /// <summary>
        /// The file path that contains the headers.
        /// </summary>
        //---------------------------------------------------------------------
        protected string headerPath;

        //---------------------------------------------------------------------
        /// <summary>
        /// The file path that contains the elevation matrix.
        /// </summary>
        //---------------------------------------------------------------------
        protected string path;

        //---------------------------------------------------------------------
        /// <summary>
        /// Number of rows of the altitude matrix.
        /// </summary>
        //---------------------------------------------------------------------
        protected int rows;

        //---------------------------------------------------------------------
        /// <summary>
        /// Number of columns of the altitude matrix.
        /// </summary>
        //---------------------------------------------------------------------
        protected int cols;

        //---------------------------------------------------------------------
        /// <summary>
        /// Coordintes of the bottom left corner of altitude matrix.
        /// </summary>
        //---------------------------------------------------------------------
        protected Point bottomLeft;

        //---------------------------------------------------------------------
        /// <summary>
        /// The altitude matrix.
        /// </summary>
        //---------------------------------------------------------------------
        protected double[,] altitude;

        //---------------------------------------------------------------------
        /// <summary>
        /// The value that represents whether there is no altitude data in a 
        /// specified cell.  
        /// </summary>
        //---------------------------------------------------------------------
        protected double noData;

        //---------------------------------------------------------------------
        /// <summary>
        /// The geometric distance among adjacent cells.
        /// </summary>
        //---------------------------------------------------------------------
        protected double cellSize;

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
        /// <param name="coordSystem">
        /// The coordinate system that DEM is referred.
        /// </param>
        //---------------------------------------------------------------------
        protected Dem(int rows, int cols, Point bottomLeft, 
            double[,] altitude, double noData, double cellSize, 
            int metCellSize, Precision precision)
        {
            this.rows = rows;
            this.cols = cols;
            this.bottomLeft = bottomLeft;
            this.altitude = altitude;
            this.noData = noData;
            this.cellSize = cellSize;
            this.metCellSize = metCellSize;
            this.precision = precision;
        }

        //---------------------------------------------------------------------
        /// <summary>
        /// Main Constructor. Use it when there is not header file.
        /// </summary>
        /// <param name="path">
        /// The path of the DEM file.
        /// </param>
        /// <param name="precision">
        /// A <see cref="Precision"/>.
        /// </param>
        //---------------------------------------------------------------------
        protected Dem(string path, Precision precision)
        {
            this.precision = precision;
            this.path = path;
            readHeader();
            readAltitudes();
        }

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
        /// <param name="precision">
        /// A <see cref="Precision"/>.
        /// </param>
        //---------------------------------------------------------------------
        protected Dem(string path, string headerPath, Precision precision)
        {
            this.precision = precision;
            this.path = path;
            this.headerPath = headerPath;
            readHeader();
            readAltitudes();
        }
        
        //---------------------------------------------------------------------
        /// <summary>
        /// Void constructor.
        /// </summary>
        //---------------------------------------------------------------------
        protected Dem()
        { }
        
        //---------------------------------------------------------------------
        /// <summary>
        /// Read header information.
        /// </summary>
        //---------------------------------------------------------------------
        protected abstract void readHeader();

        //---------------------------------------------------------------------
        /// <summary>
        /// Read altitude matrix.
        /// </summary>
        //---------------------------------------------------------------------
        protected abstract void readAltitudes();

        //---------------------------------------------------------------------
        /// <summary>
        /// Returns the approximate cell size value in meters.
        /// </summary>
        /// <returns>
        /// The <see cref="metCellSize"/>.
        /// </returns>
        public int getMetCellSize()
        {
            return this.metCellSize;
        }

        //---------------------------------------------------------------------
        /// <summary>
        /// Returns the elevation matrix file path.
        /// </summary>
        /// <returns>
        /// The <see cref="path"/>.
        /// </returns>
        //---------------------------------------------------------------------
        public string getPath()
        {
            return this.path;
        }

        //---------------------------------------------------------------------
        /// <summary> 
        /// Returns the number of rows of the altitude matrix.
        /// </summary>
        /// <returns>
        /// The number of rows of the altitude matrix.
        /// </returns>
        //---------------------------------------------------------------------
        public int getRows()
        {
            return this.rows;
        }

        //---------------------------------------------------------------------
        /// <summary>
        /// Returns the number of cols of the altitude matrix.
        /// </summary>
        /// <returns>
        /// The number of cols of the altitude matrix.
        /// </returns>
        //---------------------------------------------------------------------
        public int getCols()
        {
            return this.cols;
        }

        //---------------------------------------------------------------------
        /// <summary>
        /// Returns the cell size of the altitude matrix.
        /// </summary>
        /// <returns>
        /// The cell size of the altitude matrix.
        /// </returns>
        //---------------------------------------------------------------------
        public double getCellSize()
        {
            return this.cellSize;
        }

        //---------------------------------------------------------------------
        /// <summary>
        /// Returns the no data value of the altitude matrix.
        /// </summary>
        /// <returns>
        /// The no data value of the altitude matrix.
        /// </returns>
        //---------------------------------------------------------------------
        public double getNoData()
        {
            return this.noData;
        }

        //---------------------------------------------------------------------
        /// <summary>
        /// Returns the bottom left corner waypoint of the altitude matrix.
        /// </summary>
        /// <returns>
        /// The bottom left corner waypoint of the altitude matrix.
        /// </returns>
        //---------------------------------------------------------------------
        public Point getBottomLeft()
        {
            return this.bottomLeft;
        }

        //---------------------------------------------------------------------
        /// <summary>
        /// Returns the altitude matrix.
        /// </summary>
        /// <returns>
        /// The altitude matrix.
        /// </returns>
        //---------------------------------------------------------------------
        public double[,] getAllAltitudes()
        {
            return this.altitude;
        }

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
        public abstract double getAltitudeFromCoords(Point p);

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
        public abstract Dem getSelection(Point bottomLeft, Point topRight);

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
        public abstract Index pointToIndex(Point p);

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
        public virtual double getAltitudeFromIndex(Index index)
        {
            if (index.getCol() < cols &&
                (index.getRow()) < rows && 
                index.getCol() > -1 && 
                index.getRow() > -1)
                return this.altitude[(index.getRow()),index.getCol()];
            else
                return double.MaxValue;
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
        public abstract WgsPoint indexToPoint(Index index);

        /// <summary>
        /// Check whether a waypoint is included in the elevation matrix
        /// </summary>
        /// <param name="p">
        /// The specified point.
        /// </param>
        /// <returns>
        /// TRUE if the waypoint is included in the elevation matrix. 
        /// FALSE if th waypoint is not included in the matrix.
        /// </returns>
        public abstract bool isIncluded(Point p);

        //---------------------------------------------------------------------
        /// <summary>
        /// Return the Reference System used in this Dem.
        /// </summary>
        /// <returns>
        /// The reference System.
        /// </returns>
        //---------------------------------------------------------------------
        protected abstract Point.refSystem getRefSystem();

        //---------------------------------------------------------------------
        /// <summary>
        /// Returns a <see cref="Points.Point"/> representing the highest 
        /// waypoint in the current <see cref="Dem"/>
        /// </summary>
        /// <returns>
        /// A <see cref="Points.Point"/>.
        /// </returns>
        //---------------------------------------------------------------------
        public Point getMaxAltitude()
        {
            Index index = new Index();
            double altitude = double.MinValue;
            for(int i = 0; i <rows;i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    if (altitude < this.altitude[i,j])
                    {
                        altitude = this.altitude[i, j];
                        index = new Index(i, j, altitude);
                    }
                }
                
            }
            return indexToPoint(index);
        }

        //---------------------------------------------------------------------
        /// <summary>
        /// Returns a <see cref="System.Collections.Generic.List"/> of
        /// <see cref="Points.Point"/> higher than the specified altitude.
        /// </summary>
        /// <param name="alt">
        /// A <see cref="double"/> representing the altitude threshold.
        /// </param>
        /// <returns>
        /// A <see cref="System.Collections.Generic.List"/> of
        /// <see cref="Points.Point"/> higher than the specified altitude.
        /// </returns>
        //---------------------------------------------------------------------
        public List<Point> areHigherThan(double alt)
        {
            List<Point> wpList = new List<Point>();
            for (int i = 0; i < this.rows; i++)
            {
                for (int j = 0; j < this.cols; j++)
                {
                    if (this.altitude[i, j] > alt)
                    {
                        wpList.Add(
                            this.indexToPoint(
                                new Index(i, j, altitude[i, j])
                                )
                            );
                    }
                }
            }
            return wpList;
        }


        //---------------------------------------------------------------------
        /// <summary>
        /// Concatenates a pair of <see cref="Dem"/>
        /// </summary>
        /// <param name="bigDem">
        /// The bigger <see cref="Dem"/>.
        /// </param>
        /// <param name="smallDem">
        /// The smaller <see cref="Dem"/>.
        /// </param>
        /// <returns>
        /// A <see cref="Dem"/>.
        /// </returns>
        //---------------------------------------------------------------------
        public static Dem concatenate(Dem bigDem, Dem smallDem)
        {
            if (bigDem.GetType() == smallDem.GetType())
            {
                int rows;
                int cols;
                Point bottomLeft;
                double[,] altitude;

                if (bigDem.bottomLeft.getLatitude() == 
                    smallDem.bottomLeft.getLatitude() && 
                    bigDem.rows == smallDem.rows || 
                    bigDem is Icc)
                {
                    rows = bigDem.rows;
                    cols = bigDem.cols + smallDem.cols;
                    altitude = new double[rows, cols];
                    if (bigDem.bottomLeft.getLongitude() < 
                        smallDem.bottomLeft.getLongitude())
                    {
                        bottomLeft = bigDem.bottomLeft;
                        for (int i = 0; i < bigDem.rows; i++)
                        {
                            for (int j = 0; j < bigDem.cols; j++)
                            {
                                altitude[i, j] = bigDem.altitude[i, j];
                                if(j < smallDem.cols)
                                    altitude[i, j + bigDem.cols] = 
                                        smallDem.altitude[i, j];
                            }
                        }
                    }
                    else
                    {
                        bottomLeft = smallDem.bottomLeft;
                        for (int i = 0; i < bigDem.rows; i++)
                        {
                            for (int j = 0; j < bigDem.cols; j++)
                            {
                                if(j < smallDem.cols)
                                    altitude[i, j] = smallDem.altitude[i, j];
                                altitude[i, j + bigDem.cols] = 
                                    bigDem.altitude[i, j];
                            }
                        }
                    }
                }
                else if (bigDem.bottomLeft.getLongitude() == 
                    smallDem.bottomLeft.getLongitude() && 
                    bigDem.cols == smallDem.cols)
                {
                    rows = bigDem.rows + smallDem.rows;
                    cols = bigDem.cols;
                    altitude = new double[rows, cols];
                    if (bigDem.bottomLeft.getLatitude() < 
                        smallDem.bottomLeft.getLatitude())
                    {
                        bottomLeft = bigDem.bottomLeft;
                        for (int i = 0; i < bigDem.rows; i++)
                        {
                            for (int j = 0; j < bigDem.cols; j++)
                            {
                                altitude[i + smallDem.rows, j] = 
                                    bigDem.altitude[i, j];
                                if (i < smallDem.rows)
                                    altitude[i, j] = smallDem.altitude[i, j];
                            }
                        }
                    }
                    else
                    {
                        bottomLeft = smallDem.bottomLeft;
                        for (int i = 0; i < bigDem.rows; i++)
                        {
                            for (int j = 0; j < bigDem.cols; j++)
                            {
                                if(i < smallDem.rows)
                                    altitude[i + bigDem.rows, j] = 
                                        smallDem.altitude[i, j];
                                altitude[i, j] = bigDem.altitude[i, j];
                            }
                        }
                    }
                }
                else
                    return null;
                if (bigDem is Icc)
                    return new Icc(rows, cols, bottomLeft, altitude);
                else if (bigDem is Srtm3)
                    return new Srtm3(rows, cols, bottomLeft.toWgs(), altitude);
                else
                    return new Srtm30(
                        rows, cols, bottomLeft.toWgs(), altitude);
            }
            else
                return null;
        }
    }
}
