using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;

namespace Islands.UWP.Data
{
    public static class Json
    {
        public static T Deserialize<T>(string json)
        {
            T model = default(T);
            StringReader sr = new StringReader(json);
            JsonSerializer serializer = new JsonSerializer();
            model = (T)serializer.Deserialize(new JsonTextReader(sr), typeof(T));
            return model;
        }

        public static bool TryDeserializeObject(string json,out JObject JObj)
        {
            JObj = null;
            try
            {
                JObj = (JObject)JsonConvert.DeserializeObject(json);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool TryDeserialize<T>(string json, out T Object)
        {
            Object = default(T);
            try
            {
                Object = (T)JsonConvert.DeserializeObject(json);
                return true;
            }
            catch
            {
                return false;
            }
        }        
    }
}
