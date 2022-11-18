using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Translation.WebApi.BaiduApi.Translation
{
    [DataContract]
    public class BaiduOfficialTranslationResponse
    {
        [DataMember(Name = "from")]
        public string From { get; set; }
        [DataMember(Name = "to")]
        public string To { get; set; }
        [DataMember(Name = "trans_result")]
        public List<BaiduOfficialTranslationContent> Translations { get; set; }

        public string GetTranslation()
        {
            var list = Translations?.Select(i => i.Translation).ToList() ?? new List<string>();
            string translation = string.Join(";",list);
            return translation;
        }
    }
    [DataContract]
    public class BaiduOfficialTranslationContent
    {
        [DataMember(Name = "src")]
        public string QueryText { get; set; }
        [DataMember(Name = "dst")]
        public string Translation { get; set; }
    }
}
