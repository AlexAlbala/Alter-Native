using System;
using System.Collections.Generic;
using System.Text;

namespace USAL
{
    [Serializable]
    public class id_wp
    {
        /// <summary>UNKNOWN</summary>
        public uint id_ref;
        /// <summary>UNKNOWN</summary>
        public uint id_leg;
        /// <summary>UNKNOWN</summary>
        public uint id_stage;

        public id_wp()
        {

            id_ref = 0;
            id_leg = 0;
            id_stage = 0;
        }

        public id_wp(uint id_ref, uint id_leg, uint id_stage)
        {
            this.id_leg = id_leg;
            this.id_ref = id_ref;
            this.id_stage = id_stage;
        }

        public id_wp(int value)
        {
            //TODO: CREATE ALGORITHM BY AN INTEGER
            this.id_leg = (uint)value;
            this.id_ref = (uint)value;
            this.id_stage = (uint)value;
        }

        public string get_idwp()
        {
            return (this.id_leg.ToString() + " " + this.id_ref.ToString() + " " + this.id_stage.ToString());

        }

        public void translate_info()
        {
            //maths and final value to be done
            id_leg = 1;
            id_stage = 2;
            id_ref = 3;
        }

    }
}
