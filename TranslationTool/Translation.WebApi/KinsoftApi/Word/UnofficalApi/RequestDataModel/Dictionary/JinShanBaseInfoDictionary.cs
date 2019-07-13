using System.Collections.Generic;
using System.Runtime.Serialization;
using Translation.Api;

namespace Translation.WebApi.KinsoftApi
{
    [DataContract]
    public class KinsoftBaseInfoDictionary
    {

        [DataMember(Name = "word_name")]
        public string WordName { get; set; }
        [DataMember(Name = "symbols")]
        public List<BaseInfoSymbol> BaseInfoSymbols { get; set; }

        /// <summary>
        /// 获取英式发音
        /// </summary>
        /// <returns></returns>
        public PronounceInfo GetUkPronounce()
        {
            var pronounceInfo = new PronounceInfo();
            if (BaseInfoSymbols != null && BaseInfoSymbols.Count > 0)
            {
                var baseInfoSymbol = BaseInfoSymbols[0];
                pronounceInfo.Pronounce = baseInfoSymbol.EnPronounce;
                pronounceInfo.PronounceUri = baseInfoSymbol.EnPronounceMP3Url;
            }

            return pronounceInfo;
        }

        /// <summary>
        /// 获取美式发音
        /// </summary>
        /// <returns></returns>
        public PronounceInfo GetUsPronounce()
        {
            var pronounceInfo = new PronounceInfo();
            if (BaseInfoSymbols != null && BaseInfoSymbols.Count > 0)
            {
                var baseInfoSymbol = BaseInfoSymbols[0];
                pronounceInfo.Pronounce = baseInfoSymbol.UsPronounce;
                pronounceInfo.PronounceUri = baseInfoSymbol.UsPronounceMP3Url;
            }

            return pronounceInfo;
        }

        public List<SematicInfo> GetTranslations()
        {
            var translationInfos = new List<SematicInfo>();
            if (BaseInfoSymbols != null && BaseInfoSymbols.Count > 0)
            {
                foreach (var wordData in BaseInfoSymbols[0].BaseInfoSymbolParaphrases)
                {
                    if (wordData != null && wordData.ParaphraseContents != null && wordData.ParaphraseContents.Count > 0)
                    {
                        var translations = string.Join(";", wordData.ParaphraseContents);
                        //var translationText = $"{wordData.ParaphraseType}{translations}";

                        translationInfos.Add(new SematicInfo()
                        {
                            Translation = translations,
                            WordType = wordData.ParaphraseType
                        });
                    }
                }
            }

            return translationInfos;
        }
    }

    [DataContract]
    public class BaseInfoSymbol
    {
        [DataMember(Name = "ph_en")]
        public string EnPronounce { get; set; }
        [DataMember(Name = "ph_am")]
        public string UsPronounce { get; set; }
        [DataMember(Name = "ph_en_mp3")]
        public string EnPronounceMP3Url { get; set; }
        [DataMember(Name = "ph_am_mp3")]
        public string UsPronounceMP3Url { get; set; }
        [DataMember(Name = "parts")]
        public List<BaseInfoSymbolParaphrase> BaseInfoSymbolParaphrases { get; set; }
    }

    [DataContract]
    public class BaseInfoSymbolParaphrase
    {
        [DataMember(Name = "part")]
        public string ParaphraseType { get; set; }
        [DataMember(Name = "means")]
        public List<string> ParaphraseContents { get; set; }
    }
}
