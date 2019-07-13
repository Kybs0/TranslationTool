using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using Translation.Api;

namespace Translation.WebApi.YouDaoApi
{
    [DataContract]
    public class PhraseDictionary
    {
        [DataMember(Name = "phrs")]
        public List<PhraseData> PhraseDatas { get; set; }

        public List<PhraseInfo> GetPhrases()
        {
            var phraseInfos = new List<PhraseInfo>();
            if (PhraseDatas != null)
            {
                foreach (var phraseData in PhraseDatas)
                {
                    var phraseDetailData = phraseData.PhraseDetailData;
                    var phrase = phraseDetailData?.PhraseDetail?.PhraseDetailLIData_L.PhraseDetailLIData_L_I;
                    var phraseTranslations = phraseDetailData?.TranslationDatas?.Select(i => i.PhraseDetailLIData.PhraseDetailLIData_L.PhraseDetailLIData_L_I).ToList() ?? new List<string>();
                    var phraseTranslation = string.Join(";", phraseTranslations);
                    phraseInfos.Add(new PhraseInfo()
                    {
                        Phrase = phrase,
                        PhraseTranslation = phraseTranslation
                    });
                }
            }

            return phraseInfos;
        }
    }
    [DataContract]
    public class PhraseData
    {
        [DataMember(Name = "phr")]
        public PhraseDetailData PhraseDetailData { get; set; }
    }
    [DataContract]
    public class PhraseDetailData
    {
        [DataMember(Name = "headword")]
        public PhraseDetailLIData PhraseDetail { get; set; }

        [DataMember(Name = "trs")]
        public List<PhraseDetailTranslation> TranslationDatas { get; set; }
    }
    [DataContract]
    public class PhraseDetailTranslation
    {
        [DataMember(Name = "tr")]
        public PhraseDetailLIData PhraseDetailLIData { get; set; }
    }

    [DataContract]
    public class PhraseDetailLIData
    {
        [DataMember(Name = "l")]
        public PhraseDetailLIData_L PhraseDetailLIData_L { get; set; }
    }
    [DataContract]
    public class PhraseDetailLIData_L
    {
        [DataMember(Name = "i")]
        public string PhraseDetailLIData_L_I { get; set; }
    }
}
