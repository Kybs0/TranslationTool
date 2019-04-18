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
        [DataMember(Name = "exchange")]
        public KinsoftWordExchange WordExchange { get; set; }
        [DataMember(Name = "symbols")]
        public List<BaseInfoSymbol> BaseInfoSymbols { get; set; }

        /// <summary>
        /// 获取同根词列表
        /// </summary>
        /// <returns></returns>
        public List<CognateInfo> GetCognateWords()
        {
            var cognateInfos = new List<CognateInfo>();
            if (WordExchange != null)
            {
                if (WordExchange.word_pl?.Count > 0)
                {
                    cognateInfos.Add(new CognateInfo()
                    {
                        WordType = "pl",
                        Cognate = string.Join(",", WordExchange.word_pl)
                    });
                }
                if (WordExchange.word_third?.Count > 0)
                {
                    cognateInfos.Add(new CognateInfo()
                    {
                        WordType = "third",
                        Cognate = string.Join(",", WordExchange.word_third)
                    });
                }
                if (WordExchange.word_ing?.Count > 0)
                {
                    cognateInfos.Add(new CognateInfo()
                    {
                        WordType = "ing",
                        Cognate = string.Join(",", WordExchange.word_ing)
                    });
                }
                if (WordExchange.word_past?.Count > 0)
                {
                    cognateInfos.Add(new CognateInfo()
                    {
                        WordType = "past",
                        Cognate = string.Join(",", WordExchange.word_past)
                    });
                }
                if (WordExchange.word_done?.Count > 0)
                {
                    cognateInfos.Add(new CognateInfo()
                    {
                        WordType = "done",
                        Cognate = string.Join(",", WordExchange.word_done)
                    });
                }
                if (WordExchange.word_er?.Count > 0)
                {
                    cognateInfos.Add(new CognateInfo()
                    {
                        WordType = "er",
                        Cognate = string.Join(",", WordExchange.word_er)
                    });
                }
                if (WordExchange.word_est?.Count > 0)
                {
                    cognateInfos.Add(new CognateInfo()
                    {
                        WordType = "est",
                        Cognate = string.Join(",", WordExchange.word_est)
                    });
                }
                if (WordExchange.word_prep?.Count > 0)
                {
                    cognateInfos.Add(new CognateInfo()
                    {
                        WordType = "prep",
                        Cognate = string.Join(",", WordExchange.word_prep)
                    });
                }
                if (WordExchange.word_verb?.Count > 0)
                {
                    cognateInfos.Add(new CognateInfo()
                    {
                        WordType = "verb",
                        Cognate = string.Join(",", WordExchange.word_verb)
                    });
                }
                if (WordExchange.word_noun?.Count > 0)
                {
                    cognateInfos.Add(new CognateInfo()
                    {
                        WordType = "noun",
                        Cognate = string.Join(",", WordExchange.word_noun)
                    });
                }
                if (WordExchange.word_adj?.Count > 0)
                {
                    cognateInfos.Add(new CognateInfo()
                    {
                        WordType = "adj",
                        Cognate = string.Join(",", WordExchange.word_adj)
                    });
                }
                if (WordExchange.word_conn?.Count > 0)
                {
                    cognateInfos.Add(new CognateInfo()
                    {
                        WordType = "conn",
                        Cognate = string.Join(",", WordExchange.word_conn)
                    });
                }
            }

            return cognateInfos;
        }
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
    public class KinsoftWordExchange
    {
        [DataMember(Name = "word_pl")]
        public List<string> word_pl { get; set; }
        [DataMember(Name = "word_third")]
        public List<string> word_third { get; set; }
        [DataMember(Name = "word_past")]
        public List<string> word_past { get; set; }
        [DataMember(Name = "word_done")]
        public List<string> word_done { get; set; }
        [DataMember(Name = "word_ing")]
        public List<string> word_ing { get; set; }
        [DataMember(Name = "word_er")]
        public List<string> word_er { get; set; }
        [DataMember(Name = "word_est")]
        public List<string> word_est { get; set; }
        [DataMember(Name = "word_prep")]
        public List<string> word_prep { get; set; }
        [DataMember(Name = "word_adv")]
        public List<string> word_adv { get; set; }
        [DataMember(Name = "word_noun")]
        public List<string> word_noun { get; set; }
        [DataMember(Name = "word_adj")]
        public List<string> word_verb { get; set; }
        [DataMember(Name = "word_verb")]
        
        public List<string> word_adj { get; set; }
        [DataMember(Name = "word_conn")]
        public List<string> word_conn { get; set; }
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
