using System;
using System.Threading.Tasks;

namespace Translation.WebApi.GooleApi.BrokenTranslation
{
    /// <summary>
    /// Goole破解版翻译服务
    /// </summary>
    public class GooleBrokenTranslationService:WebRequestBase
    {
        public static async Task<string> GetTranslationAsync(string queryText)
        {
            try
            {
                var result = new GooleBrokenTranslationApi().GoogleTranslate(queryText,"auto","auto");
                var convertedResult = Unicode2String(result);
                return convertedResult;
            }
            catch (Exception e)
            {
                return string.Empty;
            }
        }
    }
}
