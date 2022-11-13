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
                var youDaoResponse = JsonConvert.DeserializeObject<YouDaoUnOfficialTranslationResponse>(convertedResult);
                var translation =  string.Empty;
                if (youDaoResponse.ErrorCode==0&& youDaoResponse.Translations?.Count>0)
                {
                    var list = youDaoResponse.Translations[0].Select(i=>i.Translation);
                    translation = string.Join("\r\n", list);
                }
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
            //http://fanyi.youdao.com/translate?&doctype=json&type=AUTO&i=
            var requestUrl = "http://fanyi.youdao.com/translate?doctype=json" +
                             $"&i={queryText}&network=5g";
            return requestUrl;
        }
    }
}
