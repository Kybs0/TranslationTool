using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Xml;
using Newtonsoft.Json;
using Translation.Api;

namespace Translation.WebApi.KinsoftApi
{
    class JinShanOfficialApiService: WebRequestBase
    {
        //http://dict-co.iciba.com/api/dictionary.php?w=hello&key=0EAE08A016D6688F64AB3EBB2337BFB0
        //http://dict-co.iciba.com/search.php?word=program&submit=%E6%9F%A5%E8%AF%A2

        public static async Task<EnglishWordTranslationData> GetWordsAsync(string queryText)
        {
            var requestUrl = GetRequestUrl(queryText);
            try
            {
                var translationData = new EnglishWordTranslationData();

                var result = await RequestUrlAsync(requestUrl);
                translationData.DetailJson = result;

                XmlDocument doc = new XmlDocument();
                doc.InnerXml = result;
                var  dictXmlNode = doc.SelectSingleNode("dict");
                var word=dictXmlNode.SelectSingleNode("key").InnerText;
                translationData.Word = word;

                var pronouncesList=dictXmlNode.SelectNodes("ps");
                if (pronouncesList?.Count>0)
                {
                    var pronouncesAudioList=dictXmlNode.SelectNodes("pron");
                    var usPronounceInfo = new PronounceInfo();
                    usPronounceInfo.Pronounce=pronouncesList[0].InnerText;
                    usPronounceInfo.PronounceUri=pronouncesAudioList[0].InnerText;
                    translationData.UsPronounce = usPronounceInfo;
                    if (pronouncesList.Count>1)
                    {
                        var ukPronounceInfo = new PronounceInfo();
                        ukPronounceInfo.Pronounce=pronouncesList[1].InnerText;
                        ukPronounceInfo.PronounceUri=pronouncesAudioList[1].InnerText;
                        translationData.UkPronounce = ukPronounceInfo;
                    }
                }

                var sentences = new List<SentenceInfo>();
                var sentenceNodeList=dictXmlNode.SelectNodes("sent");
                for (int i = 0; i < sentenceNodeList.Count; i++)
                {
                    var sentenceNode = sentenceNodeList[i];
                    var sentenceInfo = new SentenceInfo();
                    var sentence = sentenceNode.SelectSingleNode("orig").InnerText.Trim("\r\n".ToCharArray());
                    sentenceInfo.Sentence= sentence;
                    var translation = sentenceNode.SelectSingleNode("trans").InnerText.Trim("\r\n".ToCharArray());
                    sentenceInfo.Translation= translation;
                    sentences.Add(sentenceInfo);
                }
                translationData.Sentences = sentences;
                return translationData;
            }
            catch (Exception)
            {
                return null;
            }
        }

        private static string GetRequestUrl(string queryWord)
        {
            var requestUrl = $"http://dict-co.iciba.com/api/dictionary.php?w={queryWord}&key=0EAE08A016D6688F64AB3EBB2337BFB0";
            return requestUrl;
        }
    }
}
