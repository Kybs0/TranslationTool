using System.Runtime.Serialization;

namespace Translation.WebApi.KinsoftApi
{
    [DataContract]
    public class KinsoftSentence
    {
        [DataMember(Name = "Network_en")]
        public string EnglishSentence { get; set; }
        [DataMember(Name = "Network_cn")]
        public string ChineseSentence { get; set; }
        [DataMember(Name = "tts_mp3")]
        public string SentenceMp3Uri { get; set; }
    }
}
