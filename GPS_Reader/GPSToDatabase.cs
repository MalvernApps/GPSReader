using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Text;

namespace GPS_Reader
{
    class GPSToDatabase
    {
        public void WriteNavToDB( DateTime time , double latitude , double longitude , double velocity , double bearing )
        {
            NavigationData nd = new NavigationData();

            nd.Altitude = 0;
            nd.Azimuth = bearing;
            nd.Latitude = latitude;
            nd.Longitude = longitude;
            nd.OleDateTime = time.ToOADate();
            nd.Pitch = 0;
            nd.Roll = 0;
            nd.SensorInstance = 12345678;
            nd.Time = time;
            nd.Velocity = velocity;

            RegistryAccess ra = new RegistryAccess();

            // DataContext takes userName connection string 
            DataContext db = new DataContext(ra.GetDatabaseConenctionString());
            db.Log = Console.Out;
            // Get userName typed table to run queries
            Table<NavigationData> mem = db.GetTable<NavigationData>();

            mem.InsertOnSubmit(nd);
            db.SubmitChanges();   

        }
    }
}
