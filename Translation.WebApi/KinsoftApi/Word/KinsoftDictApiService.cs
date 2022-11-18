using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Translation.Api;

namespace Translation.WebApi.KinsoftApi
{
    /// <summary>
    /// 金山字典数据查询
    /// </summary>
    public class KinsoftDictApiService
    {
        public static async Task<EnglishWordTranslationData> GetWordsAsync(string queryText)
        {
            EnglishWordTranslationData englishWordTranslationData = null;
            var unOfficalResult=await KinsoftUnOfficialApiService.GetWordsAsync(queryText);
            if (unOfficalResult==null)
            {
                var officalResult =await JinShanOfficialApiService.GetWordsAsync(queryText);
                if (officalResult!=null)
                {
                    englishWordTranslationData = officalResult;
                }
            }
            else
            {
                englishWordTranslationData = unOfficalResult;
            }
            return englishWordTranslationData??new EnglishWordTranslationData();
        }
    }
}
