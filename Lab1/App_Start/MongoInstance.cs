using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MongoDB.Driver;

namespace Lab1.App_Start
{
    public sealed class MongoInstance
    {
        //volatile: ensure that assignment to the instance variable
        //is completed before the instance variable can be accessed
        private static volatile MongoInstance instance;
        private static object syncLock = new Object();

        const string connectionString = "mongodb://localhost";
        private static IMongoDatabase db = null;

        private MongoInstance()
        {
            var client = new MongoClient();
            db = client.GetDatabase("Lab1-v3");
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
            }
        }
    }
}