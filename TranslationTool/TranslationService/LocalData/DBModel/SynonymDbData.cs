using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Translation.Business
{
    /// <summary>
    /// 近义词
    /// </summary>
    [DataContract]
    public class SynonymDbData
    {
        [DataMember]
        public string WordType { get; set; }
        [DataMember]
        public string Translation { get; set; }
        [DataMember]
        public List<string> Synonyms { get; set; }
    }
}
