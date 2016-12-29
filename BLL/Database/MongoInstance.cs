using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MongoDB.Driver;
using System.Configuration;



namespace BLL
{
    public sealed class MongoInstance
    {
        //volatile: ensure that assignment to the instance variable
        //is completed before the instance variable can be accessed
        private static volatile MongoInstance instance;
        private static object syncLock = new Object();

        const string connectionString = "mongodb://ana:anaana@aws-eu-central-1-portal.0.dblayer.com:15324";
        private static IMongoDatabase db = null;

        private MongoInstance()
        {
            var client = new MongoClient(connectionString);
            db = client.GetDatabase("projekt");
        }

        public static IMongoDatabase GetDatabase
        {
            get
            {
                if (instance == null)
                {
                    lock (syncLock)
                    {
                        if (instance == null)
                            instance = new MongoInstance();
                    }
                }
                return db;
                //return MongoServer.Create(ConfigurationManager.ConnectionStrings["Compose"].ConnectionString).GetDatabase("projekt");
            }  
        }
        public static IMongoDatabase Reconnect
        {
            get
            {
                var client = new MongoClient(connectionString);
                db = client.GetDatabase("projekt");
                return db;
            }
        }
    }
}