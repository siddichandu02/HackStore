using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace StoreBuy.Utilities
{
    class JSONConversion
    {
        public static T DeserializeResponseToModel<T>(string Response)
        {
            T DeserializedResponse = JsonConvert.DeserializeObject<T>(Response);
            return DeserializedResponse;
        }

        public static string SerializeObjectToString<T>(T obj)
        {
            string SerializedObject = JsonConvert.SerializeObject(obj);
            return SerializedObject;
        }
    }
}
