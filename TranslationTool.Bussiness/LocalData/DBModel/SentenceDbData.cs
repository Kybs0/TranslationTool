using System.Runtime.Serialization;

namespace Translation.Business
{
    /// <summary>
    /// 例句信息
    /// </summary>
    [DataContract]
    public class SentenceDbData
    {
        [DataMember]
        public string Sentence { get; set; }
        [DataMember]
        public string SentenceFileName { get; set; }
        [DataMember]
        public string Translation { get; set; }
    }
}
