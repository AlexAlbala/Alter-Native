using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;
using ICSharpCode.SharpZipLib.Zip;

namespace DemManager
{
    //TODO: Maybe abstract factory is needed
    //---------------------------------------------------------------------
    /// <summary>
    /// This static class implements a factory in order to load the 
    /// preferred DEM file depending on the cell size and the avaliable 
    /// data.
    /// </summary>
    //---------------------------------------------------------------------
    public static class DemFactory
    {
        //---------------------------------------------------------------------
        /// <summary>
        /// A URL to a remote server that contains all SRTM 3' files 
        /// (worldwide).
        /// </summary>
        //---------------------------------------------------------------------
        private static readonly string SRTM3URL = 
            "http://dds.cr.usgs.gov/srtm/version2_1/SRTM3/";

        //---------------------------------------------------------------------
        /// <summary>
        /// A URL to a remote server that contains all SRTM 30' files 
        /// (worldwide).
        /// </summary>
        //---------------------------------------------------------------------
        private static readonly string SRTM30URL = 
            "http://dds.cr.usgs.gov/srtm/version2_1/SRTM30/";

        //---------------------------------------------------------------------
        /// <summary>
        /// The different DEM types.
        /// </summary>
        //---------------------------------------------------------------------
        private enum DemType { Icc, Srtm3, Srtm30 };

        //---------------------------------------------------------------------
        /// <summary>
        /// Loads DEM data depending on the cell size and the avaliable data.
        /// </summary>
        /// <param name="p">
        /// The waypoint that must be included in the DEM.
        /// </param>
        /// <param name="cellSize">
        /// The preferred cell size.
        /// </param>
        /// <param name="demList">
        /// The DEM List where loaded DEMs will added.
        /// </param>
        /// <returns>
        /// The DEM List with the new DEMs included.
        /// </returns>
        //---------------------------------------------------------------------
        public static Dem createDem(Point p, Dem.Precision precision, 
            List<Dem> demList)
        {
            DemType demType = selectDemGenerator(precision);
            Dem dem;
            string path = buildPath(p, demType);
            if (existsPath(demType, path, demList))
            {
                if (demType == DemType.Icc)
                    dem = new Icc(path);
                else if (demType == DemType.Srtm3)
                    dem = new Srtm3(path);
                else
                    dem = new Srtm30(
                            path, string.Format(path.Split('.')[0] + ".HDR"));
                return dem;
            }
            else
                return null;
        }

        //---------------------------------------------------------------------
        /// <summary>
        /// Selects the DEM generator depending on the cell size.
        /// </summary>
        /// <param name="p">
        /// The included waypoint.
        /// </param>
        /// <param name="cellSize">
        /// The preferred cellsize.
        /// </param>
        /// <returns>
        /// A DEM type.
        /// </returns>
        //---------------------------------------------------------------------
        private static DemType selectDemGenerator(Dem.Precision precision)
        {
            if (precision == Dem.Precision.low)
                return DemType.Srtm30;
            else if (precision == Dem.Precision.medium)
                return DemType.Srtm3;
            else
                return DemType.Icc;
        }

        //---------------------------------------------------------------------
        /// <summary>
        /// Builds the specified file path.
        /// </summary>
        /// <param name="p">The included waypoint.</param>
        /// <param name="demType">The DEM type</param>
        /// <returns>The file path(s)</returns>
        //---------------------------------------------------------------------
        private static string buildPath(Point p, DemType demType)
        {
            string path = string.Empty;
            HayPoint hayPoint = new HayPoint(p.toWgs());
            WgsPoint wgsPoint = p.toWgs();

            if (demType == DemType.Icc)
            {
                if (couldBeICCInfo(p))
                {
                    if (hayPoint.getUtmX() < 
                        (Icc.MAXHAYUTMX + Icc.MINHAYUTMX) / 2)
                        path = Icc.PATHWEST;
                    else
                        path = Icc.PATHEAST;
                }
                else
                    path = string.Empty;
            }
            else if (demType == DemType.Srtm3)
            {
                int lon = Convert.ToInt32(Math.Floor(wgsPoint.getLongitude()));
                int lat = Convert.ToInt32(Math.Floor(wgsPoint.getLatitude()));
                if (lat < 0)
                    path += "S";
                else
                    path += "N";
                if (Math.Abs(lat) < 10)
                    path += "0" + Math.Abs(lat);
                else
                    path += Math.Abs(lat);
                if (lon < 0)
                    path += "W";
                else
                    path += "E";
                if (Math.Abs(lon) < 10)
                    path += "00" + Math.Abs(lon);
                else if (Math.Abs(lon) < 100)
                    path += "0" + Math.Abs(lon);
                else
                    path += Math.Abs(lon);
                path += ".hgt";
            }
            else if (demType == DemType.Srtm30)
            {
                int lon = Convert.ToInt32(Math.Floor(wgsPoint.getLongitude()));
                int lat = Convert.ToInt32(Math.Floor(wgsPoint.getLatitude()));
                if (lat > -60)
                {
                    if (lon < -140)
                        path += "w180";
                    else if (lon < -100)
                        path += "w140";
                    else if (lon < -60)
                        path += "w100";
                    else if (lon < -20)
                        path += "w060";
                    else if (lon < 20)
                        path += "w020";
                    else if (lon < 60)
                        path += "e020";
                    else if (lon < 100)
                        path += "e060";
                    else if (lon < 140)
                        path += "e100";
                    else
                        path += "e140";
                }
                else
                {
                    if(lon < -120)
                        path += "w180";
                    else if(lon < -60)
                        path += "w120";
                    else if(lon < 0)
                        path += "w060";
                    else if(lon < 60)
                        path += "e000";
                    else if(lon < 120)
                        path += "e060";
                    else
                        path += "e120";
                }
                if (lat < -60)
                    path += "s60";
                else if (lat < -10)
                    path += "s10";
                else if (lat < 40)
                    path += "n40";
                else
                    path += "n90";
                
                path += ".dem";
            }
            return path;
        }

        //---------------------------------------------------------------------
        /// <summary>
        /// Returns whether a file exists. If it does not exist automatically 
        /// tries to download from the web.
        /// </summary>
        /// <param name="demType">
        /// The DEM type.
        /// </param>
        /// <param name="inPaths">
        /// The paths that have to be checked.
        /// </param>
        /// <param name="paths">
        /// The paths that exist.
        /// </param>
        /// <param name="demList">
        /// The list of loaded DEMs
        /// </param>
        /// <returns>
        /// FALSE if none of the paths inPaths can be loaded. 
        /// Otherwise, true. 
        /// </returns>
        private static bool existsPath(DemType demType, string path, List<Dem> demList)
        {
            foreach (Dem dem in demList)
            {
                if (dem.getPath() == path)
                    return false;
            }
            try
            {
                //StreamReader sr = new StreamReader(path);
                StreamReader sr = new StreamReader(@"C:\N41E001.hgt");
            }
            catch (FileNotFoundException)
            {
                try
                {
                    downloadFromInternet(demType, path);
                }
                catch (Exception)
                {
                    return false;
                }
            }
            return true;
        }

        //---------------------------------------------------------------------
        /// <summary>
        /// Download a DEM file from internet.
        /// </summary>
        /// <param name="demType">
        /// The type of DEM.</param>
        /// <param name="path">The name of the file to download.</param>
        /// <returns>TRUE if the file has ben downloaded properly. 
        /// Otherwise, FALSE.</returns>
        //---------------------------------------------------------------------
        private static bool downloadFromInternet(DemType demType, string path)
        {
            byte[] data = new byte[1];
            bool ok = false;
            if (demType == DemType.Icc)
            {
                return false;
            }
            if (demType == DemType.Srtm3)
            {
                WebClient web = new WebClient();
                try
                {
                    data = web.DownloadData(string.Format(SRTM3URL + "/Eurasia/" + path + ".zip"));
                    ok = true;
                }
                catch (WebException) { }
                if (!ok)
                {
                    try
                    {
                        data = web.DownloadData(string.Format(SRTM3URL + "/North_America/" + path + ".zip"));
                        ok = true;
                    }
                    catch (WebException) { }
                }
                if (!ok)
                {
                    try
                    {
                        data = web.DownloadData(string.Format(SRTM3URL + "/South_America/" + path + ".zip"));
                        ok = true;
                    }
                    catch (WebException) { }
                }
                if (!ok)
                {
                    try
                    {
                        data = web.DownloadData(string.Format(SRTM3URL + "/Africa/" + path + ".zip"));
                        ok = true;
                    }
                    catch (WebException) { }
                }
                if (!ok)
                {
                    try
                    {
                        data = web.DownloadData(string.Format(SRTM3URL + "/Australia/" + path + ".zip"));
                        ok = true;
                    }
                    catch (WebException) { }
                }
                if (!ok)
                {
                    try
                    {
                        data = web.DownloadData(string.Format(SRTM3URL + "/Islands/" + path + ".zip"));
                        ok = true;
                    }
                    catch (WebException) { return false; }
                }
                string zipFilePath = string.Format(path + ".zip");
                FileStream fs = File.Create(zipFilePath);
                fs.Write(data, 0, data.Length);
                fs.Close();
                unZipFile(zipFilePath);
                return true;
            }
            if (demType == DemType.Srtm30)
            {
                WebClient web = new WebClient();
                try
                {
                    data = web.DownloadData(string.Format(SRTM30URL + "/" + path.Split('.')[0] + "/" + path + ".zip"));
                }
                catch (WebException)
                {
                    return false;
                }
                string zipFilePath = string.Format(path.Split('.')[0] + ".zip");
                FileStream fs = File.Create(zipFilePath);
                fs.Write(data, 0, data.Length);
                fs.Close();
                unZipFile(zipFilePath);
                try
                {
                    data = web.DownloadData(string.Format(SRTM30URL + "/" + path.Split('.')[0] + "/" + path.Split('.')[0] + ".hdr.zip"));
                }
                catch (WebException)
                {
                    return false;
                }
                zipFilePath = string.Format(path.Split('.')[0] + ".zip");
                fs = File.Create(zipFilePath);
                fs.Write(data, 0, data.Length);
                fs.Close();
                unZipFile(zipFilePath);
                return true;
            }
            return false;
        }

        //---------------------------------------------------------------------
        /// <summary>
        /// UnZips the specified file.
        /// </summary>
        /// <param name="path">
        /// The file path.
        /// </param>
        //---------------------------------------------------------------------
        private static void unZipFile(string path)
        {
            ZipInputStream s = new ZipInputStream(File.OpenRead(path));
            ZipEntry entry = s.GetNextEntry();
            string fileName = Path.GetFileName(entry.Name);
            FileStream streamWriter = File.Create(fileName);
            int size = 2048;
            byte[] data = new byte[size];
            while (true)
            {
                size = s.Read(data, 0, data.Length);
                if (size > 0)
                    streamWriter.Write(data, 0, size);
                else
                    break;
            }
            streamWriter.Close();
            s.Close();
            File.Delete(path);
        }

        //---------------------------------------------------------------------
        /// <summary>
        /// Checks whether Icc data is avaliable.
        /// </summary>
        /// <param name="p">
        /// The included point.
        /// </param>
        /// <returns>
        /// TRUE if there is avaliable data. 
        /// Otherwise, FALSE.
        /// </returns>
        //---------------------------------------------------------------------
        private static bool couldBeICCInfo(Point p)
        {
            //TODO:: Hemisphere issues?
            HayPoint hayPoint = new HayPoint(p.toWgs());
            double utmX = hayPoint.getUtmX();
            double utmY = hayPoint.getUtmY();
            if (utmX > Icc.MINHAYUTMX && utmX < Icc.MAXHAYUTMX && utmY > Icc.MINHAYUTMY && utmY < Icc.MAXHAYUTMY)
                return true;
            else
                return false;
        }

        //---------------------------------------------------------------------
        /// <summary>
        /// STATIC: Creates the DEM that complies with the input parametres.
        /// </summary>
        /// <param name="bottomLeft">
        /// A <see cref="Point"/> representing the bottom left corner 
        /// coordinates
        /// </param>
        /// <param name="topRight">
        /// A <see cref="Point"/> representing the top right corner coordinates
        /// </param>
        /// <param name="demList">
        /// A List of <see cref="Dem"/>.
        /// </param>
        /// <param name="precision">
        /// A <see cref="Dem.Precision"/>.
        /// </param>
        /// <returns>
        /// The created <see cref="Dem"/>.
        /// </returns>
        //---------------------------------------------------------------------
        public static Dem createDem(Point bottomLeft, 
            Point topRight,List<Dem> demList, Dem.Precision precision)
        {
            Dem dem = null;
            DemType demType = selectDemGenerator(precision);
            if (demType == DemType.Icc)
            {
                if (!couldBeICCInfo(bottomLeft) || !couldBeICCInfo(topRight))
                {
                    return null;
                }
                else
                {
                    bottomLeft = new HayPoint(bottomLeft.toWgs());
                    topRight = new HayPoint(topRight.toWgs());
                    string path1 = buildPath(bottomLeft, demType);
                    string path2 = buildPath(topRight, demType);
                    if (path1 != path2)
                    {
                        Icc icc1 = new Icc(path1);
                        Icc icc2 = new Icc(path2);
                        dem = Dem.concatenate(icc1, icc2);

                    }
                    else
                    {
                        dem = new Icc(path1);
                    }
                    return dem;
                }
            }
            else if (demType == DemType.Srtm3)
            {
                List<Dem> aux = new List<Dem>();
                bottomLeft = bottomLeft.toWgs();
                topRight = topRight.toWgs();
                int latBL = 
                    Convert.ToInt32(Math.Floor(bottomLeft.getLatitude()));
                int lonBL = 
                    Convert.ToInt32(Math.Floor(bottomLeft.getLongitude()));
                int latTR = 
                    Convert.ToInt32(Math.Floor(topRight.getLatitude()));
                int lonTR = 
                    Convert.ToInt32(Math.Floor(topRight.getLongitude()));
                List<string> paths = new List<string>();
                for (int i = latBL; i <= latTR; i++)
                {
                    for (int j = lonBL; j <= lonTR; j++)
                    {
                        Point p = new WgsPoint(i, j, null);
                        paths.Add(buildPath(p, demType));
                    }
                }
                bool ok = false;
                foreach (string path in paths)
                {
                    foreach (Dem d in demList)
                    {
                        if (d.getPath() == path)
                        {
                            ok = true;
                            aux.Add(d);
                        }
                    }
                    if (!ok && existsPath(demType, path, demList))
                    {
                        aux.Add(new Srtm3(path));
                    }
                    ok = false;
                }
                dem = aux[0];
                List<Dem> aux2 = new List<Dem>();
                int count = 0;
                for (int i = latBL; i <= (latTR); i++)
                {
                    for (double j = lonBL; j <= (lonTR - 1); j++)
                    {
                        count++;
                        dem = Dem.concatenate(dem, aux[count]);
                    }
                    aux2.Add(dem);
                    count++;
                    if (count < aux.Count)
                        dem = aux[count];
                }
                dem = aux2[0];
                for (int i = 1; i < aux2.Count; i++)
                    dem = Dem.concatenate(dem, aux2[i]);
            }
            else if (demType == DemType.Srtm30)
            {
                List<Dem> aux = new List<Dem>();
                bottomLeft = bottomLeft.toWgs();
                topRight = topRight.toWgs();
                int latBL = 
                    Convert.ToInt32(Math.Floor(bottomLeft.getLatitude()));
                int lonBL = 
                    Convert.ToInt32(Math.Floor(bottomLeft.getLongitude()));
                int latTR =
                    Convert.ToInt32(Math.Floor(topRight.getLatitude()));
                int lonTR = 
                    Convert.ToInt32(Math.Floor(topRight.getLongitude()));
                List<string> paths = new List<string>();
                for (double i = latBL; i <= latTR; i = i + 60)
                {
                    for (double j = lonBL; j <= lonTR; j = j + 40)
                    {
                        Point p = new WgsPoint(i, j, null);
                        paths.Add(buildPath(p, demType));
                    }
                }
                bool ok = false;
                foreach (string path in paths)
                {
                    foreach (Dem d in demList)
                    {
                        if (d.getPath() == path)
                        {
                            ok = true;
                            aux.Add(d);
                        }
                    }
                  
                    if (!ok && existsPath(demType, path, demList))
                    {
                        aux.Add(
                            new Srtm30(
                                path, string.Format(
                                    path.Split('.')[0] + ".HDR")));
                    }
                    ok = false;
                }
                dem = aux[0];
                List<Dem> aux2 = new List<Dem>();
                int count = 0;
                bool isFirst = true;
                for (double i = latBL; isFirst || i <= latTR; i = i + 60)
                {
                    for (double j = lonBL; j <= (lonTR - 40); j = j + 40)
                    {
                        count++;
                        dem = Dem.concatenate(dem, aux[count]);
                    }
                    aux2.Add(dem);
                    count++;
                    if(count < aux.Count)
                        dem = aux[count];
                    isFirst = false;
                }
                dem = aux2[0];
                for (int i = 1; i < aux2.Count; i++)
                    dem = Dem.concatenate(dem, aux2[i]);
            }
            return dem;
        }
    }
}
