using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;

namespace OptionProcessingService
{
    class JsonSerializeHelper
    {
        public static string Serialize<T>(T obj)
        {
            using (var m = new MemoryStream())
            {
                var ser = new DataContractJsonSerializer(typeof(T));
                ser.WriteObject(m, obj);
                m.Position = 0;
                using (var r = new StreamReader(m))
                    return r.ReadToEnd();
            }
        }

        public static T Deserialize<T>(string json)
        {
            using (var m = new MemoryStream())
            {
                var w = new StreamWriter(m);
                try
                {
                    w.Write(json);
                    w.Flush();
                    m.Position = 0;

                    var ser = new DataContractJsonSerializer(typeof(T));
                    return (T)ser.ReadObject(m);
                }
                finally
                {
                    w.Dispose();
                }
            }
        }
    }
}
