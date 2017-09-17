using System;
using System.IO;
using Config;
using DBreeze;
using Newtonsoft.Json;

namespace NativeLayer.Database
{
    /// <summary>
    ///     Wrapper for embedded object database.
    /// </summary>
    public class ScpDb : IDisposable
    {
        public DBreezeEngine Engine { get; private set; }

        private static string DbPath
        {
            get
            {
                var path = Path.Combine(GlobalConfiguration.AppDirectory, "Db");

                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);

                return path;
            }
        }

        public ScpDb()
        {
            var jSettings = new JsonSerializerSettings()
            {
                TypeNameHandling = TypeNameHandling.All
            };

            DBreeze.Utils.CustomSerializator.Serializator =
                o => JsonConvert.SerializeObject(o, Formatting.Indented, jSettings);

            DBreeze.Utils.CustomSerializator.Deserializator = 
                (s, type) => JsonConvert.DeserializeObject(s, jSettings);

            Engine = new DBreezeEngine(DbPath);
        }

        ~ScpDb()
        {
            Dispose();
        }

        public static string TableDevices => "tScpDevices";

        public static string TableProfiles => "tScpProfiles";

        public void Dispose()
        {
            Engine?.Dispose();
        }
    }
}
