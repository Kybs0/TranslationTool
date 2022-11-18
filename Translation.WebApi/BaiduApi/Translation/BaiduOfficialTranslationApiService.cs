using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Newtonsoft.Json;
using Translation.WebApi.WebRequestHelper;

namespace Translation.WebApi.BaiduApi.Translation
{
    /// <summary>
    /// 百度官方翻译调用-不免费
    /// </summary>
    public class BaiduOfficialTranslationApiService : WebRequestBase
    {
        public static async Task<string> GetTranslationAsync(string queryText)
        {
            var requestUrl = GetRequestUrl(queryText);
            try
            {
                var result = await RequestUrlAsync(requestUrl);
                var convertedResult = Unicode2String(result);
                var response = JsonConvert.DeserializeObject<BaiduOfficialTranslationResponse>(convertedResult);
                var translation = response.GetTranslation();
                return translation;
            }
            catch (Exception e)
            {
                return string.Empty;
            }
        }

        private static string GetRequestUrl(string queryText)
        {
            //随机数
            string randomNum = System.DateTime.Now.Millisecond.ToString();
            string appId = "20181125000238742";
            string appSecret = "XI_vrGeRIYLZcplIfOKF";

            string md5Str = appId + queryText + randomNum + appSecret;
            string sign = GetMD5WithString(md5Str);

            //url
            string requestUrl = String.Format("http://api.fanyi.baidu.com/api/trans/vip/translate?q={0}&from={1}&to={2}&appid={3}&salt={4}&sign={5}",
                HttpUtility.UrlEncode(queryText, Encoding.UTF8),
                "auto",
                "auto",
                appId,
                randomNum,
                sign
            );
            return requestUrl;

        }
        //对字符串做md5加密
        private static string GetMD5WithString(string input)
        {
            if (input == null)
            {
                return null;
            }
            MD5 md5Hash = MD5.Create();
            //将输入字符串转换为字节数组并计算哈希数据  
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));
            //创建一个 Stringbuilder 来收集字节并创建字符串  
            StringBuilder sBuilder = new StringBuilder();
            //循环遍历哈希数据的每一个字节并格式化为十六进制字符串  
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }
            //返回十六进制字符串  
            return sBuilder.ToString();
        }
    }
}
