using System;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Newtonsoft.Json;
using Translation.Api;

namespace Translation.WebApi.YouDaoApi
{
    /// <summary>
    /// 有道官方合作测试API
    /// </summary>
    public class YouDaoOfficialTestApiService : WebRequestBase
    {
        const string _appKey = "zhudytest123";
        const string _from = "auto";
        const string _to = "zhs";
        const string _appSecret = "IoyvG6Zb98nEUA4nIGwkEPUXILBYgrGs";

        public static async Task<EnglishWordTranslationData> GetWordsAsync(string queryText)
        {
            try
            {
                var requestUrl = GetRequestUrl(queryText);
                string result = await RequestUrlAsync(requestUrl);
                var youDaoTranslationResponse = JsonConvert.DeserializeObject<EnglishWordTranslationData>(result);
                return youDaoTranslationResponse;
            }
            catch (Exception e)
            {
                return new EnglishWordTranslationData();
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
                "http://vertifytest.youdao.com/api?appKey={0}&q={1}&from={2}&to={3}&sign={4}&salt={5}&ext=mp3&voice=0&signType=v1",
                _appKey,
                HttpUtility.UrlDecode(queryText, Encoding.GetEncoding("UTF-8")),
                _from, _to, sign, salt);

            return requestUrl;
        }
    }
}
