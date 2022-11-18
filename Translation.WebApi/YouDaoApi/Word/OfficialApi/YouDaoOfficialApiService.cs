using System;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Newtonsoft.Json;

namespace Translation.WebApi.YouDaoApi
{
    /// <summary>
    /// 有道官方API
    /// </summary>
    public class YouDaoOfficialApiService : WebRequestBase
    {
        const string _appKey = "75766d8fc97f34a3";
        const string _from = "auto";
        const string _to = "zhs";
        const string _appSecret = "rFkTqsDws1bCoETcxSL7afG33emwJdr5";

        public static async Task<YouDaoTranslationResponse> GetWordsAsync(string queryText)
        {
            try
            {
                var requestUrl = GetRequestUrl(queryText);
                string result = await RequestUrlAsync(requestUrl);
                var youDaoTranslationResponse = JsonConvert.DeserializeObject<YouDaoTranslationResponse>(result);
                return youDaoTranslationResponse;
            }
            catch (Exception e)
            {
                return new YouDaoTranslationResponse();
            }
        }

        private static string GetRequestUrl(string queryText)
        {
            string salt = DateTime.Now.Millisecond.ToString();

            MD5 md5 = new MD5CryptoServiceProvider();
            string md5Str = _appKey + queryText + salt + _appSecret;
            byte[] output = md5.ComputeHash(Encoding.UTF8.GetBytes(md5Str));
            string sign = BitConverter.ToString(output).Replace("-", "");

            var requestUrl = string.Format(
                "http://openapi.youdao.com/api?appKey={0}&q={1}&from={2}&to={3}&sign={4}&salt={5}",
                _appKey,
                HttpUtility.UrlDecode(queryText, Encoding.GetEncoding("UTF-8")),
                _from, _to, sign, salt);

            return requestUrl;
        }
    }
}
