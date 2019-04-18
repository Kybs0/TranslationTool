using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Translation.WebApi.KinsoftApi.Translate
{
    [DataContract]
    public class KinsoftUnOfficialTranslationResponse
    {
        //0：单词 ，1：翻译
        [DataMember(Name = "status")]
        public int Status { get; set; }
        [DataMember(Name = "content")]
        public KinsoftUnOfficialTranslationContent TranslationContent { get; set; }
    }
    [DataContract]
    public class KinsoftUnOfficialTranslationContent
    {
        //翻译
        [DataMember(Name = "from")]
        public string From { get; set; }
        [DataMember(Name = "to")]
        public string To { get; set; }
        [DataMember(Name = "vendor")]
        public string Vendor { get; set; }
        [DataMember(Name = "out")]
        public string Translation { get; set; }
        [DataMember(Name = "err_no")]
        public string ErrorCode { get; set; }

        //单词
        [DataMember(Name = "word_mean")]
        public List<string> WordMeanings { get; set; }
        [DataMember(Name = "ph_en")]
        public string EnPh { get; set; }
        [DataMember(Name = "ph_am")]
        public string UsPh { get; set; }
        [DataMember(Name = "ph_en_mp3")]
        public string EnMP3 { get; set; }
        [DataMember(Name = "ph_am_mp3")]
        public string UsMP3 { get; set; }
        [DataMember(Name = "ph_tts_mp3")]
        public string TTSMP3 { get; set; }
    }
}
