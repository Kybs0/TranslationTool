using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;

namespace Translation.WebApi.WebRequestHelper
{
    public class WebRequestTransformHelper
    {
        public static string GetRequestString(object obj)
        {
            var dataerializer = new DataContractJsonSerializer(obj.GetType());
            MemoryStream stream = new MemoryStream();
            dataerializer.WriteObject(stream, obj);
            byte[] dataBytes = new byte[stream.Length];
            stream.Position = 0;
            stream.Read(dataBytes, 0, (int)stream.Length);
            var requestString = Encoding.UTF8.GetString(dataBytes);
            var convertedRequestUrl = SetSpecialCharToUnicode(requestString);
            return convertedRequestUrl;
        }
        public static string SetSpecialCharToUnicode(string requestUrl)
        {
            var convertedUrl = requestUrl.Replace("{", "%7B").Replace("}", "%7D")
                .Replace("[", "%5B").Replace("]", "%5D")
                .Replace("\"", "%22").Replace(":", "%3A").Replace(",", "%2C").Replace(" ", "%20");
            return convertedUrl;
        }
    }
}
