using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DemManager
{
    /// <summary>
    /// This class is designed to manage the DEM matrix index with some more rich info.
    /// </summary>
    public class Index
    {
        /// <summary>
        /// Row index.
        /// </summary>
        private int row;

        /// <summary>
        /// Column index.
        /// </summary>
        private int col;

        /// <summary>
        /// DEM list index. 
        /// </summary>
        private Dem dem;

        /// <summary>
        /// Altitude.
        /// </summary>
        private Nullable<double> alt;

        /// <summary>
        /// Default constructor.
        /// </summary>
        public Index()
        {
            this.row = -1;
            this.col = -1;
            this.dem = null;
            this.alt = null;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="row">Row index</param>
        /// <param name="col">Column index</param>
        public Index(int row, int col)
        {
            this.row = row;
            this.col = col;
            this.dem = null;
            this.alt = null;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="row">Row index.</param>
        /// <param name="col">Column index.</param>
        /// <param name="nDem">DEM list index.</param>
        public Index(int row, int col, Dem dem)
        {
            this.col = col;
            this.row = row;
            this.dem = dem;
            this.alt = null;
        }

        
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="row">Row index.</param>
        /// <param name="col">Column index.</param>
        /// <param name="alt">Altitude.</param>
        public Index(int row, int col, double alt)
        {
            this.row = row; 
            this.col = col;
            this.alt = alt;
            this.dem = null;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="row">Row index.</param>
        /// <param name="col">Column index.</param>
        /// <param name="alt">Altitude.</param>
        /// <param name="nDem">DEM list index.</param>
        public Index(int row, int col, double alt, Dem dem)
        {
            this.row = row;
            this.col = col;
            this.dem = dem;
            this.alt = alt;
        }

        /// <summary>
        /// Returns row index.
        /// </summary>
        /// <returns>The row index</returns>
        public int getRow()
        {
            return this.row;
        }

        /// <summary>
        /// Returns the column index.
        /// </summary>
        /// <returns>The column index</returns>
        public int getCol()
        {
            return this.col;
        }

        /// <summary>
        /// Returns the DEM list index.
        /// </summary>
        /// <returns>The DEM list index.</returns>
        public Dem getDem()
        {
            if (dem == null)
                throw new NullReferenceException();
            return this.dem;
        }

        /// <summary>
        /// Returns the altitude.
        /// </summary>
        /// <returns>The altitude.</returns>
        public Nullable<double>getAlt()
        {
            return this.alt;
        }
    }
}