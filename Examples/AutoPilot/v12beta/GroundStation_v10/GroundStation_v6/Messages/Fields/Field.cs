using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GroundStation
{
	/// <summary>
	/// Field. Implements a fault tolerant field for
	/// telemetry data.
	/// </summary>
    public class Field
    {
		/// <summary>
		/// Current value.
		/// </summary>
        private double v;
		
		/// <summary>
		/// Gets or sets the current value v.
		/// </summary>
		/// <value>
		/// The current value
		/// </value>
        public double V
        {
            get
            {
                return this.v;
            }

            set
            {
                this.v = value;
                this.PreProcessValue();
            }
        }
		
		/// <summary>
		/// The minimum expected value.
		/// </summary>
        public double min;
		
		/// <summary>
		/// The maximum expected value.
		/// </summary>
        public double max;
		
		/// <summary>
		/// The previous received value.
		/// </summary>
        private double prevValue;
		
		/// <summary>
		/// The value before the previously received one.
		/// </summary>
        private double prevPrevValue;
		
		/// <summary>
		/// The maximum expected variation.
		/// </summary>
        private double maxVar;
		
        private bool isFirst;
		
		/// <summary>
		/// Initializes a new instance of the <see cref="GroundStation.Field"/> class.
		/// </summary>
		/// <param name='min'>
		/// Minimum expected value
		/// </param>
		/// <param name='max'>
		/// Maximum expected value
		/// </param>
		/// <param name='prevValue'>
		/// The previous received value.
		/// </param>
		/// <param name='prevPrevValue'>
		/// The value before the previously received one.
		/// </param>
		/// <param name='maxVar'>
		/// The maximum expected variation.
		/// </param>
        public Field(double min, double max, double prevValue, double prevPrevValue, double maxVar)
        {
            this.min = min;
            this.max = max;
            this.prevValue = prevValue;
            this.prevPrevValue = prevPrevValue;
            this.maxVar = maxVar;
            this.isFirst = true;
        }
		
		/// <summary>
		/// Processes the current value
		/// </summary>
        protected void PreProcessValue()
        {
			//if current value is valid...
            if (this.min < v && this.max > v && (Math.Abs(this.v - this.prevValue) < this.maxVar || this.isFirst))
            {
                this.prevPrevValue = this.prevValue;
                this.prevValue = this.v;
                this.isFirst = false;
            }
			//otherwise let's predict it
            else
            {
                this.v = 2 * this.prevValue - this.prevPrevValue;
                this.prevPrevValue = this.prevValue;
                this.prevValue = this.v;
            }
        }
		
		/// <summary>
		/// Creates a clone  of the field f
		/// </summary>
		/// <returns>
		/// The copy.
		/// </returns>
		/// <param name='f'>
		/// The field to clone
		/// </param>
        public static Field DeepCopy(Field f)
        {
            Field ans = new Field(f.min, f.max, f.prevValue, f.prevPrevValue, f.maxVar);
            ans.v = f.v;
            ans.isFirst = f.isFirst;
            return ans;
        }
    }
}
