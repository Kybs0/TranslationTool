using System.Runtime.Serialization;

namespace Translation.Business
{
    /// <summary>
    /// 词组
    /// </summary>
    [DataContract]
    public class PhraseDbData
    {
        [DataMember]
        public string Phrase { get; set; }
        [DataMember]
        public string Translation { get; set; }
    }
}
