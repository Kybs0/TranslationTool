using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Translation.Api;
using Translation.WebApi.WebRequestHelper;

namespace Translation.WebApi.YouDaoApi
{
    /// <summary>
    /// 有道非官方API翻译
    /// </summary>
    public class YouDaoUnOfficialWordApiService : WebRequestBase
    {
        public static async Task<EnglishWordTranslationData> GetWordsAsync(string queryText)
        {
            if (string.IsNullOrWhiteSpace(queryText))
            {
                return new EnglishWordTranslationData();
            }

            try
            {
                var requestUrl = GetRequestUrl(queryText.Trim());

                var result = await RequestUrlAsync(requestUrl);
                var youDaoResponse = JsonConvert.DeserializeObject<IntergateYouDaoDataResponse>(result);
                var translationData = new EnglishWordTranslationData
                {
                    Word = youDaoResponse.Word ?? string.Empty,
                    DetailJson = result,
                    UkPronounce = youDaoResponse.EnglishToChineseData?.GetUkPronounce() ?? new PronounceInfo(),
                    UsPronounce = youDaoResponse.EnglishToChineseData?.GetUsPronounce() ?? new PronounceInfo(),
                    Translations = youDaoResponse.EnglishToChineseData?.GetTranslation() ?? new List<SematicInfo>(),
                    Phrases = youDaoResponse.PhraseDictionary?.GetPhrases() ?? new List<PhraseInfo>(),
                    Synonyms = youDaoResponse.SynonymDictionary?.GetSynonymWords() ?? new List<SynonymInfo>(),
                    Cognates = youDaoResponse.CognateWordDictionary?.GetCognateWords() ?? new List<CognateInfo>(),
                    Sentences = youDaoResponse.CollinsDictionary?.GetCollinsSentences() ?? new List<SentenceInfo>(),
                };

                return translationData;
            }
            catch (Exception e)
            {
                return new EnglishWordTranslationData();
            }
        }

        private static string GetRequestUrl(string queryWord)
        {
            var dictString = GetDictString();
            var requestUrl = string.Format("http://dict.youdao.com/jsonapi?xmlVersion=5.1&jsonversion=2&client=mobile&" +
                "q={0}&dicts={1}&network=5g", queryWord, dictString);
            //var requestUrl = string.Format("http://dict.youdao.com/jsonapi?xmlVersion=5.1&jsonversion=2&client=mobile&" +
            //                               "q={0}&network=5g", queryWord);

            return requestUrl;
        }

        /// <summary>
        /// 获取字典字符串
        /// 需要查询哪些字典。目前已知 
        /// {"count":99,"dicts":[["ec","ce","newcj","newjc","kc","ck","fc","cf","multle","jtj",
        /// "pic_dict","tc","ct","typos","special","tcb","baike","lang","simple","wordform",
        /// "exam_dict","ctc","web_search","auth_sents_part","ec21","phrs","input",
        /// "wikipedia_digest","ee","collins","ugc","media_sents_part","syno","rel_word",
        /// "longman","ce_new","le","newcj_sents","blng_sents_part","hh"],["ugc"],["longman"],
        /// ["newjc"],["newcj"],["web_trans"],["fanyi"]]}。可为空，为空则返回全部字段
        /// </summary>
        /// <returns></returns>
        private static string GetDictString()
        {
            var dictionaryQueryModel = new DictionaryQueryModel()
            {
                Dictionaries = new List<List<string>>()
                {
                    new List<string>()
                    {
                        "ec",
                        //"ec21",
                        "phrs",
                        "syno",
                        "rel_word",
                        "collins"
                    },
                },
            };
            var requestString = WebRequestTransformHelper.GetRequestString(dictionaryQueryModel);
            return requestString;
        }
    }
}
