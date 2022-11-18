using System.Collections.Generic;
using System.Runtime.Serialization;
using Translation.Api;

namespace Translation.WebApi.YouDaoApi
{
    /// <summary>
    /// 英汉字典数据
    /// </summary>
    [DataContract]
    public class EnglishToChineseDictionary
    {
        [DataMember(Name = "word")]
        public List<EnglishToChineseWordData> WordDatas { get; set; }

        /// <summary>
        /// 获取英式发音
        /// </summary>
        /// <returns></returns>
        public PronounceInfo GetUkPronounce()
        {
            var translation = string.Empty;
            if (WordDatas != null)
            {
                translation = WordDatas[0].UKPronounce;
            }

            return new PronounceInfo() { Pronounce = translation };
        }
        /// <summary>
        /// 获取美式发音
        /// </summary>
        /// <returns></returns>
        public PronounceInfo GetUsPronounce()
        {
            var translation = string.Empty;
            if (WordDatas != null)
            {
                translation = WordDatas[0].USPronounce;
            }

            return new PronounceInfo() { Pronounce = translation };
        }

        public List<SematicInfo> GetTranslation()
        {
            var translationInfos = new List<SematicInfo>();
            if (WordDatas != null)
            {
                foreach (var wordData in WordDatas)
                {
                    if (wordData.TranslationDatas != null)
                    {
                        foreach (var translationData in wordData.TranslationDatas)
                        {
                            if (translationData.ChineseTranslationDatas != null)
                            {
                                foreach (var chineseTranslationData in translationData.ChineseTranslationDatas)
                                {
                                    if (chineseTranslationData.TranslationDataBase != null && chineseTranslationData.TranslationDataBase.Translations != null)
                                    {
                                        foreach (var wordTranslation in chineseTranslationData.TranslationDataBase.Translations)
                                        {
                                            translationInfos.Add(new SematicInfo()
                                            {
                                                Translation = wordTranslation
                                            });
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return translationInfos;
        }
    }
    [DataContract]
    public class EnglishToChineseWordData
    {
        [DataMember(Name = "trs")]
        public List<EnglishToChineseTranslationDetailData> TranslationDatas { get; set; }

        [DataMember(Name = "ukphone")]
        public string UKPronounce { get; set; }

        [DataMember(Name = "usphone")]
        public string USPronounce { get; set; }
    }
    [DataContract]
    public class EnglishToChineseTranslationDetailData
    {
        [DataMember(Name = "tr")]
        public List<EnglishToChineseTranslationDetailDataL> ChineseTranslationDatas { get; set; }
    }
    [DataContract]
    public class EnglishToChineseTranslationDetailDataL
    {
        [DataMember(Name = "l")]
        public EnglishToChineseTranslationDetailDataLI TranslationDataBase { get; set; }
    }
    [DataContract]
    public class EnglishToChineseTranslationDetailDataLI
    {
        [DataMember(Name = "i")]
        public List<string> Translations { get; set; }
    }
}
