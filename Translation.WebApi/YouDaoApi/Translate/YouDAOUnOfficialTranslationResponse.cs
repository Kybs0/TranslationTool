using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Translation.WebApi.YouDaoApi.Translate
{
    //{"type":"ZH_CN2EN","errorCode":0,"elapsedTime":12,"translateResult":[[{"src":"\u201C我的爱人\u201D","tgt":"\"My love\""}]]}
    [DataContract]
    public class YouDaoUnOfficialTranslationResponse
    {
        [DataMember(Name = "errorCode")] 
        public int ErrorCode { get; set; }
        [DataMember(Name = "type")]
        public string Type { get; set; }

        [DataMember(Name = "elapsedTime")]
        public int ElapsedTime { get; set; }

        [DataMember(Name = "translateResult")]
        public List<List<TranslateResult>> Translations { get; set; }
    }
    [DataContract]
    public class TranslateResult
    {
        [DataMember(Name = "src")]
        public string Source { get; set; }
        [DataMember(Name = "tgt")]
        public string Translation { get; set; }
    }
}
