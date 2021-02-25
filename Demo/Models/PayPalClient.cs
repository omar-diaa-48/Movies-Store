using PayPalCheckoutSdk.Core;
using PayPalHttp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Models
{
    public class PayPalClient
    {
        public static PayPalEnvironment environment()
        {
            SandboxEnvironment e = new SandboxEnvironment(
                 System.Environment.GetEnvironmentVariable("PAYPAL_CLIENT_ID") != null ?
                 System.Environment.GetEnvironmentVariable("PAYPAL_CLIENT_ID") : "AUzXMqocpNRiSklEJVwW2NVAyFJWpM5W6HF3u5ZfE9Q84xWyzXd1trIKX531Z8oyU_62Dg0robvJHi9U",
                System.Environment.GetEnvironmentVariable("PAYPAL_CLIENT_SECRET") != null ?
                 System.Environment.GetEnvironmentVariable("PAYPAL_CLIENT_SECRET") : "EBROKA7H9paW6bJ9CEY3xM0C3bBn1RmjmTutN68fdOa8X2RnyWskM4voyDP7r6bMesbfhjcrcGVCfkd1");
            return e;
        }

       
         
        public static HttpClient client()
        {
            return new PayPalHttpClient(environment());
        }

        public static HttpClient client(string refreshToken)
        {
            return new PayPalHttpClient(environment(), refreshToken);
        }

        
        public static String ObjectToJSONString(Object serializableObject)
        {
            MemoryStream memoryStream = new MemoryStream();
            var writer = JsonReaderWriterFactory.CreateJsonWriter(
                        memoryStream, Encoding.UTF8, true, true, "  ");
            DataContractJsonSerializer ser = new DataContractJsonSerializer(serializableObject.GetType(), new DataContractJsonSerializerSettings { UseSimpleDictionaryFormat = true });
            ser.WriteObject(writer, serializableObject);
            memoryStream.Position = 0;
            StreamReader sr = new StreamReader(memoryStream);
            return sr.ReadToEnd();
        }
    }
}
