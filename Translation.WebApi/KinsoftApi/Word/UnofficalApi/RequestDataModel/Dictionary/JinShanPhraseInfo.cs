using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using Translation.Api;

namespace Translation.WebApi.KinsoftApi
{
    [DataContract]
    public class KinsoftPhraseInfo
    {
        [DataMember(Name = "cizu_name")]
        public string Phrase { get; set; }

        [DataMember(Name = "jx")]
        public List<KinsoftPhraseExplanation> PhraseExplanations { get; set; }
        public List<PhraseInfo> GetPhrases()
        {
            var phraseInfos = new List<PhraseInfo>();
            if (PhraseExplanations != null)
            {
                var explanations = PhraseExplanations.Where(i => !string.IsNullOrWhiteSpace(i.CnExplanation)).Select(j => j.CnExplanation).ToList();
                string explanationText = string.Join(";", explanations);
                phraseInfos.Add(new PhraseInfo()
                {
                    Phrase = Phrase,
                    PhraseTranslation = explanationText
                });
            }

            return phraseInfos;
        }
    }
    [DataContract]
    public class KinsoftPhraseExplanation
    {
        [DataMember(Name = "jx_cn_mean")]
        public string CnExplanation { get; set; }
    }
}
