using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DemManager
{
    //-------------------------------------------------------------------------
    /// <summary>
    /// This class implements a set of features related to DEM manipulation.
    /// </summary>
    //-------------------------------------------------------------------------
    public partial class Features
    {
        //---------------------------------------------------------------------
        /// <summary>
        /// A <see cref="Dem.DemList"/>.
        /// </summary>
        //---------------------------------------------------------------------
        private DemList demList;
		
		private Dem.Precision precision;
		
		private bool isWorkingAreaSet;

        //---------------------------------------------------------------------
        /// <summary>
        /// Default constructor.
        /// </summary>
        //---------------------------------------------------------------------
		public Features()
		{
			this.demList = new DemList();
			this.isWorkingAreaSet = false;
		}
		
		//---------------------------------------------------------------------
		/// <summary>
		/// Sets a working area and a precision in order to use it during the 
		/// whole mission. When requesting a point outside this area, all
		/// features will return always null.
		/// </summary>
		/// <param name="bottomLeft">
		/// A <see cref="Point"/> as the bottom left corner of the working 
		/// area.
		/// </param>
		/// <param name="topRight"></para>
		/// A <see cref="Point"/> as the bottom left corner of the working 
		/// area.
		/// </param>
		/// <param name="precision">
		/// A <see cref="Dem.Precision"/> as the selected DEM precision.
		/// </param>
		public Features(Point bottomLeft, Point topRight, Dem.Precision precision)
        {
			this.precision = precision;
            this.demList = new DemList(bottomLeft, topRight, precision);
			this.isWorkingAreaSet = true;
			this.SetWorkingArea(bottomLeft, topRight, precision);
        }
		
		public double getAltitude(Point p)
		{
			return this.demList.getAltitude(p, this.precision);
		}

        public double getAltitude(Point p, Dem.Precision precision)
        {
            return demList.getAltitude(p, precision);
        }

        public void SetWorkingArea(Point bottomLeft, Point topRight, Dem.Precision precision)
        {
            demList.getSelection(bottomLeft, topRight, precision);
        }
    }
}
