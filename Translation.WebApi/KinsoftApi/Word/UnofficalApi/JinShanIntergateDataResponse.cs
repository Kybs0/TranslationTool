using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Translation.WebApi.KinsoftApi
{
    [DataContract]
    public class KinsoftIntergateDataResponse
    {
        [DataMember(Name = "baesInfo")]
        public KinsoftBaseInfoDictionary KinsoftBaseInfoDictionary { get; set; }
        [DataMember(Name = "sentence")]
        public List<KinsoftSentence> KinsoftSentences { get; set; }

        [DataMember(Name = "phrase")]
        public List<KinsoftPhraseInfo> KinsoftPhraseInfos { get; set; }

        [DataMember(Name = "synonym")]
        public List<KinsoftSynonymInfo> KinsoftSynonymInfos { get; set; }

        [DataMember(Name = "netmean")]
        public KinsoftNetMeanInfoDictionary KinsoftNetMeanInfoDictionary { get; set; }
    }
}
