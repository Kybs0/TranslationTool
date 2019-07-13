using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Translation.WebApi.WebRequestHelper;
using Translation.WebApi.KinsoftApi.Translate;

namespace Translation.WebApi.YouDaoApi.Translate
{
    public class YouDaoUnOfficialTranslationApiService:WebRequestBase
    {
        public static async Task<string> GetTranslationAsync(string queryText)
        {
            var requestUrl = GetRequestUrl(queryText);
            try
            {
                var result = await RequestUrlAsync(requestUrl);
                var convertedResult = Unicode2String(result);
                //var youDaoResponse = JsonConvert.DeserializeObject<YouDaoUnOfficialTranslationResponse>(convertedResult);
                var translation =  string.Empty;
                //if (string.IsNullOrEmpty(translation))
                //{
                //    var list = youDaoResponse.TranslationContent?.WordMeanings ?? new List<string>();
                //    translation = string.Join(";", list);
                //}
                return translation;
            }
            catch (Exception e)
            {
                return string.Empty;
            }
        }

        private static string GetRequestUrl(string queryText)
        {
            queryText = WebRequestTransformHelper.SetSpecialCharToUnicode(queryText);

            //原有的有道非官方翻译接口，已被禁用。非官方接口不存在http://fanyi.youdao.com/translate?doctype=json&jsonversion=&type=&keyfrom=&model=&mid=&imei=&vendor=&screen=&ssid=&network=&abtest=
            //转至官方翻译接口
            var requestUrl = "http://fanyi.youdao.com/translate?doctype=json" +
                             $"&q={queryText}&network=5g";
            return requestUrl;
        }
    }
}
