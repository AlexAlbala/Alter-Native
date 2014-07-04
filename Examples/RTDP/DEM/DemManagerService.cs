using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Threading;
using USAL;


namespace DemManager
{
    public class DemManagerService// : IService
    {
        //private static readonly log4net.ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        //private IServiceContainer container;

        private static DemManagerService instance;
        private Features f;
        private Dem.Precision demPrecision = Dem.Precision.low;
        private bool isDemPrecisionSet = false;

        public static DemManagerService GetInstance()
        {
            if (instance == null)
                instance = new DemManagerService();


            return instance;
        }
        public bool Start()
        {
            //Console.WriteLine("Dem Manager Service started");
            f = new Features();
            //container.SubscribeEvent("ECEPL.WPGenerated", this);
            //container.RegisterService(this);
            //this.container = container;            
            return true;
        }

        public bool Stop()
        {
            //DONE Unregister a service should unsubscribe its events.
            //container.UnregisterService(this);
            //Console.WriteLine("Flight Plan Manager service stopped");
            return true;
        }

        public void VariableChanged(string id, object value)
        {
        }

        public void EventFired(String id, object value)
        {
            
        }
        public object FunctionCall(String id, object[] parameters)
        {
            if (!isDemPrecisionSet && id != "SetPrecision")
                Console.WriteLine("WARNING: DemPrecision is not set. DemManagerService will use default precisions.");
            switch (id)
            {
                case "SetWorkingArea":
                    WgsPoint bottomLeft = this.convertUsalPosToWgsPoint((USAL.Position)parameters[0]);
                    WgsPoint topRight = this.convertUsalPosToWgsPoint((USAL.Position)parameters[1]);
                    f.SetWorkingArea(bottomLeft, topRight, demPrecision);
                    break;
                case "SetPrecision":
                    demPrecision = (Dem.Precision)parameters[0];
                    isDemPrecisionSet = true;
                    Console.WriteLine("DemPrecision set to " + demPrecision);
                    break;
                case "GetAltitude":
                    WgsPoint pWgs = this.convertUsalPosToWgsPoint((USAL.Position)parameters[0]);
                    double alt = f.getAltitude(pWgs, demPrecision);
                    return alt;
            }
            return null;
        }
        //public ServiceDescription GetDescription()
        //{
        //    ServiceDescription s = new ServiceDescription();
        //    s.Name = "DEM Manager Service";
        //    s.Description = "This service provides a set of DEM support functions.";

        //    s.Functions = new FunctionDescription[3];
            
        //    s.Functions[0] = new FunctionDescription();
        //    s.Functions[0].Name = "GetAltitude";
        //    s.Functions[0].parameters = new ParameterDescription[1];
        //    s.Functions[0].parameters[0] = new ParameterDescription();
        //    s.Functions[0].parameters[0].Name = "point";
        //    s.Functions[0].parameters[0].Type = "USAL.Position";
        //    s.Functions[0].ReturnType = "double";

        //    s.Functions[1] = new FunctionDescription();
        //    s.Functions[1].Name = "SetPrecision";
        //    s.Functions[1].parameters = new ParameterDescription[1];
        //    s.Functions[1].parameters[0] = new ParameterDescription();
        //    s.Functions[1].parameters[0].Name = "precision";
        //    s.Functions[1].parameters[0].Type = "Dem.Precison";
        //    s.Functions[1].ReturnType = "void";

        //    s.Functions[2] = new FunctionDescription();
        //    s.Functions[2].Name = "SetWorkingArea";
        //    s.Functions[2].parameters = new ParameterDescription[2];
        //    s.Functions[2].parameters[0] = new ParameterDescription();
        //    s.Functions[2].parameters[0].Name = "bottomLeft";
        //    s.Functions[2].parameters[0].Type = "USAL.Position";
        //    s.Functions[2].parameters = new ParameterDescription[2];
        //    s.Functions[2].parameters[1] = new ParameterDescription();
        //    s.Functions[2].parameters[1].Name = "topRight";
        //    s.Functions[2].parameters[1].Type = "USAL.Position";
        //    s.Functions[2].ReturnType = "void";

        //    return s;
        //}

        private WgsPoint convertUsalPosToWgsPoint(USAL.Position usal_wp)
        {
            return new WgsPoint(usal_wp.Latitude * 180 / Math.PI, usal_wp.Longitude * 180 / Math.PI, usal_wp.Altitude);
        }
    }
}
