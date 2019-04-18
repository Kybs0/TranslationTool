using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using Newtonsoft.Json;
using Translation.Api;
using Translation.WebApi.WebRequestHelper;
using Translation.WebApi.YouDaoApi;

namespace Translation.WebApi.KinsoftApi
{
    /// <summary>
    /// 金山非官方API翻译
    /// </summary>
    public class KinsoftUnOfficialApiService : WebRequestBase
    {
        public static async Task<EnglishWordTranslationData> GetWordsAsync(string queryText)
        {
            var requestUrl = GetRequestUrl(queryText);
            try
            {
                var result = await RequestUrlAsync(requestUrl);

                var response = JsonConvert.DeserializeObject<KinsoftIntergateDataResponse>(result);
                var translationData = new EnglishWordTranslationData()
                {
                    Word = response.KinsoftBaseInfoDictionary?.WordName ?? string.Empty,
                    DetailJson = result,
                };
                translationData.UkPronounce = response.KinsoftBaseInfoDictionary?.GetUkPronounce() ?? new PronounceInfo();
                translationData.UsPronounce = response.KinsoftBaseInfoDictionary?.GetUsPronounce() ?? new PronounceInfo();
                translationData.Translations = response.KinsoftBaseInfoDictionary?.GetTranslations() ?? new List<SematicInfo>();

                var phraseInfos = new List<PhraseInfo>();
                response.KinsoftPhraseInfos?.ForEach(i => phraseInfos.AddRange(i.GetPhrases()));
                translationData.Phrases = phraseInfos;

                var synonymInfos = new List<SynonymInfo>();
                response.KinsoftSynonymInfos?.ForEach(i => synonymInfos.AddRange(i.GetSynonymWords()));
                translationData.Synonyms = synonymInfos;

                translationData.Cognates = response.KinsoftBaseInfoDictionary?.GetCognateWords() ?? new List<CognateInfo>(); ;

                var sentences = new List<SentenceInfo>();
                response.KinsoftSentences?.ForEach(i => sentences.Add(new SentenceInfo()
                {
                    Translation = i.ChineseSentence,
                    Sentence = i.EnglishSentence,
                    EnglishSentenceUri = i.SentenceMp3Uri
                }));
                translationData.Sentences = sentences;
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
            var requestUrl = "http://www.iciba.com/index.php?a=getWordMean&c=search&" +
                             $"list={dictString}&word={queryWord}";

            return requestUrl;
        }

        /// <summary>
        /// 获取字典字符串
        /// 需要查询哪些字典。目前已知 
        /// list：以 1,3,4,5,8,9,10,12,13,14,18,21,3003,3005 为字符串进行组合 
        /// 1：对应 json 中 baesInfo 字段，基础释义
        /// 3：对应 json 中 collins 字段，柯林斯高阶英汉双解学习词典
        /// 4：对应 json 中 ee_mean 字段，英英词典
        /// 5：对应 json 中 trade_means 字段，行业词典
        /// 8：对应 json 中 sentence 字段，双语例句
        ///  9：对应 json 中 netmean 字段，网络释义
        /// 10：对应 json 中 auth_sentence 字段，权威例句
        /// 12：对应 json 中 synonym 字段，同义词
        /// 13：对应 json 中 antonym 字段，反义词
        /// 14：对应 json 中 phrase 字段，词组搭配
        /// 18：对应 json 中 encyclopedia 字段，百科全书
        /// 21：对应 json 中 cetFour 字段，四级真题
        /// 3003：对应 json 中 bidec 字段，英汉双向大词典
        /// 3005：对应 json 中 jushi 字段，例句
        /// </summary>
        /// <returns></returns>
        private static string GetDictString()
        {
            var dictionaryQueryString = "1,3,4,5,8,9,10,12,13,14,18,21,3003,3005";
            var requestString = WebRequestTransformHelper.GetRequestString(dictionaryQueryString);
            return requestString;
        }
    }
}
