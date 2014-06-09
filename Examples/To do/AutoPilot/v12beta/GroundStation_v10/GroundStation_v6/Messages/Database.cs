using System;
using System.Collections.Generic;
using System.Text;

/// <summary>
/// Database. This class logs all the telemtry data.
/// </summary>
namespace GroundStation
{
    public class Database
    {
		/// <summary>
		/// The gps message list.
		/// </summary>
        public List<GpsMessage> gpsList;
		
		/// <summary>
		/// The adc message list.
		/// </summary>
        public List<AdcMessage> adcList;
		
		/// <summary>
		/// The imu (euler) message list.
		/// </summary>
        public List<ImuEulerMessage> imuEulerList;
		
		/// <summary>
		/// The imu (raw) message list.
		/// </summary>
        public List<ImuRawMessage> imuRawList;
		
		/// <summary>
		/// The pwm message list.
		/// </summary>
        public List<PwmMessage> pwmList;
		
		/// <summary>
		/// The gps message
		/// </summary>
        private GpsMessage gps;
		
		/// <summary>
		/// The adc message
		/// </summary>
        private AdcMessage adc;
		
		/// <summary>
		/// The imu (euler) message
		/// </summary>
        private ImuEulerMessage imuEuler;
		
		/// <summary>
		/// The imu (raw) message
		/// </summary>
        private ImuRawMessage imuRaw;
		
		/// <summary>
		/// The (pwm) message
		/// </summary>
        private PwmMessage pwm;
		
		/*Singleton Implementation*/
		/// <summary>
		/// Singleton. The instance.
		/// </summary>
        private static Database instance = null;
		
		/// <summary>
		/// Gets the instance.
		/// </summary>
		/// <returns>
		/// The instance.
		/// </returns>
        public static Database GetInstance()
        {
            if (instance == null)
                instance = new Database();
            return instance;
        }
		
		/// <summary>
		/// Initializes a new instance of the <see cref="GroundStation.Database"/> class.
		/// </summary>
        private Database()
        {
            this.gpsList = new List<GpsMessage>();
            this.adcList = new List<AdcMessage>();
            this.imuRawList = new List<ImuRawMessage>();
            this.imuEulerList = new List<ImuEulerMessage>();
            this.pwmList = new List<PwmMessage>();
            this.gps = new GpsMessage();
            this.imuEuler = new ImuEulerMessage();
            this.imuRaw = new ImuRawMessage();
            this.pwm = new PwmMessage();
            this.adc = new AdcMessage();
        }
		/*End of Singleton Implementation*/
		
		/// <summary>
		/// Add the specified message.
		/// </summary>
		/// <param name='m'>
		/// The message to add.
		/// </param>
        public void Add(Message m)
        {
            Type t = m.GetType();
            
            if (t == gps.GetType())
                this.gpsList.Add(m as GpsMessage);

            else if (t == imuEuler.GetType())
                this.imuEulerList.Add(m as ImuEulerMessage);

            else if (t == imuRaw.GetType())
                this.imuRawList.Add(m as ImuRawMessage);

            else if (t == adc.GetType())
                this.adcList.Add(m as AdcMessage);

            else if (t == pwm.GetType())
                this.pwmList.Add(m as PwmMessage);
        }
		
		/// <summary>
		/// Initialize this instance.
		/// </summary>
        public void Initialize()
        {
            this.gpsList = new List<GpsMessage>();
            this.adcList = new List<AdcMessage>();
            this.imuEulerList = new List<ImuEulerMessage>();
            this.imuRawList = new List<ImuRawMessage>();
            this.pwmList = new List<PwmMessage>();
        }
    }
}
