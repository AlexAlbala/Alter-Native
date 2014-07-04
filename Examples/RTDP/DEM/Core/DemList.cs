using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace DemManager
{
    //-------------------------------------------------------------------------
    /// <summary>
    /// This class implements a basic list of Dem objects.
    /// </summary>
    //-------------------------------------------------------------------------
    public class DemList
    {
        //---------------------------------------------------------------------
        /// <summary>
        /// The DEM list.
        /// </summary>
        //---------------------------------------------------------------------
        private List<Dem> demList;

        private bool isWorkingAreaSet;

        //---------------------------------------------------------------------
        /// <summary>
        /// Main Constructor. Initializes a empty list.
        /// </summary>
        //---------------------------------------------------------------------
        public DemList()
        {
            this.demList = new List<Dem>();
            this.isWorkingAreaSet = false;
        }

        public DemList(Point bottomLeft, Point topRight, Dem.Precision precision)
        {
            this.demList = new List<Dem>();
            this.demList.Add(DemFactory.createDem(bottomLeft, topRight, this.demList, precision));
            this.isWorkingAreaSet = true;
        }

        //---------------------------------------------------------------------
        /// <summary>
        /// Add a DEM object depending on the cellsize and the point that have 
        /// to be included.
        /// </summary>
        /// <param name="p">
        /// The included waypoint.
        /// </param>
        /// <param name="cellSize">
        /// The cell size.
        /// </param>
        //---------------------------------------------------------------------
        private Dem addDem(Point p, Dem.Precision precision)
        {
            int count = this.demList.Count;
            Dem dem;
            if (!isIncluded(p, out dem, precision))
            {
                if (!this.isWorkingAreaSet)
                {
                    dem = DemFactory.createDem(p, precision, this.demList);
                    this.demList.Add(dem);
                }
                else
                {
                    return null;
                }
            }
            return dem;
        }

        //---------------------------------------------------------------------
        /// <summary>
        /// Adds a DEM that complies with the input parametres unless it has
        /// been added before.
        /// </summary>
        /// <param name="bottomLeft">
        /// A <see cref="Point"/> representing the bottom left corner 
        /// coordinates of the specified DEM.
        /// </param>
        /// <param name="topRight">
        /// A <see cref="Point"/> representing the top right corner coordiantes
        /// of the specified DEM.
        /// </param>
        /// <param name="precision">
        /// A <see cref="Dem.Precision"/>.
        /// </param>
        /// <returns>
        /// A DEM that complies with the input parametres.
        /// </returns>
        //---------------------------------------------------------------------
        private Dem addDem(Point bottomLeft, Point topRight,
            Dem.Precision precision)
        {
            int count = this.demList.Count;
            Dem dem;
            if (!areIncluded(bottomLeft, topRight, out dem, precision))
            {
                if (!this.isWorkingAreaSet)
                {
                    dem =
                        DemFactory.createDem(
                            bottomLeft, topRight, this.demList, precision);
                    this.demList.Add(dem);
                }
                else
                {
                    return null;
                }
            }
            return dem;
        }

        //---------------------------------------------------------------------
        /// <summary>
        /// Returns a
        /// </summary>
        /// <param name="bottomLeft">
        /// </param>
        /// <param name="topRight">
        /// </param>
        /// <param name="precision">
        /// </param>
        /// <returns></returns>
        //---------------------------------------------------------------------
        public Dem getSelection(Point bottomLeft, Point topRight,
            Dem.Precision precision)
        {
            if (bottomLeft.getLatitude() > topRight.getLatitude())
            {
                if (bottomLeft.getLongitude() > topRight.getLongitude())
                {
                    Point aux = bottomLeft;
                    bottomLeft = topRight;
                    topRight = aux;
                }
                else
                {
                    WgsPoint aux = bottomLeft.toWgs();
                    WgsPoint aux2 = topRight.toWgs();
                    bottomLeft = new WgsPoint(aux2.getLatitude(),
                        aux.getLongitude(), aux.getAltitude());
                    topRight = new WgsPoint(aux.getLatitude(),
                        aux2.getLongitude(), aux2.getAltitude());
                }
            }
            else
            {
                if (bottomLeft.getLongitude() > topRight.getLongitude())
                {
                    WgsPoint aux = bottomLeft.toWgs();
                    WgsPoint aux2 = topRight.toWgs();
                    bottomLeft = new WgsPoint(aux.getLatitude(),
                        aux2.getLongitude(), aux.getAltitude());
                    topRight = new WgsPoint(aux2.getLatitude(),
                        aux.getLongitude(), aux2.getAltitude());
                }
            }
            Dem dem = this.addDem(bottomLeft, topRight, precision);
            return dem.getSelection(bottomLeft, topRight);
        }

        //---------------------------------------------------------------------
        /// <summary>
        /// Checks the avalaible DEM data for a specific waypoint depending on 
        /// the cell size.
        /// </summary>
        /// <param name="p">
        /// The specific waypoint
        /// </param>
        /// <param name="dem">
        /// The dem where the waypoint is included.
        /// </param>
        /// <param name="cellSize">
        /// The preferred DEM cell size
        /// </param>
        /// <returns>
        /// </returns>
        //---------------------------------------------------------------------
        public bool isIncluded(Point p, out Dem dem,
            Dem.Precision precision)
        {
            foreach (Dem d in demList)
            {
                if (d.precision == precision)
                {
                    if (d.isIncluded(p))
                    {
                        dem = d;
                        return true;
                    }
                }
            }
            dem = null;
            return false;
        }

        //---------------------------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        /// <param name="bottomLeft">
        /// </param>
        /// <param name="topRight">
        /// </param>
        /// <param name="dem">
        /// </param>
        /// <param name="precision">
        /// </param>
        /// <returns>
        /// </returns>
        //---------------------------------------------------------------------
        public bool areIncluded(Point bottomLeft, Point topRight,
            out Dem dem, Dem.Precision precision)
        {
            if (isIncluded(bottomLeft, out dem, precision) &&
                isIncluded(topRight, out dem, precision))
                return true;
            dem = null;
            return false;
        }

        //---------------------------------------------------------------------
        /// <summary>
        /// Returns the DEM list.
        /// </summary>
        /// <returns>The DEM list.</returns>
        //---------------------------------------------------------------------
        public List<Dem> getDemList()
        {
            return this.demList;
        }

        public double getAltitude(Point p, Dem.Precision precision)
        {
            Dem dem = null;
            if (isIncluded(p, out dem, precision))
                return dem.getAltitudeFromCoords(p);
            else
            {
                dem = this.addDem(p, precision);
                if (dem != null)
                    return dem.getAltitudeFromCoords(p);
                else
                    return 0.0;

            }
        }
    }
}