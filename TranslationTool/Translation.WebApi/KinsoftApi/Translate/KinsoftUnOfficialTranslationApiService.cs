using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Translation.WebApi.WebRequestHelper;

namespace Translation.WebApi.KinsoftApi.Translate
{
    public class KinsoftUnOfficialTranslationApiService : WebRequestBase
    {
        public static async Task<string> GetTranslationAsync(string queryText)
        {
            var requestUrl = GetRequestUrl(queryText);
            try
            {
                var result = await RequestUrlAsync(requestUrl);
                var convertedResult = Unicode2String(result);
                var youDaoResponse = JsonConvert.DeserializeObject<KinsoftUnOfficialTranslationResponse>(convertedResult);
                var translation=youDaoResponse.TranslationContent?.Translation ?? string.Empty;
                if (string.IsNullOrEmpty(translation))
                {
                    var list = youDaoResponse.TranslationContent?.WordMeanings??new List<string>();
                    translation = string.Join(";\r\n", list);
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
            var requestUrl = "http://fy.iciba.com/ajax.php?a=fy" +
                             $"&f=auto&t=auto&w={queryText}";
            return requestUrl;
        }
    }
}
