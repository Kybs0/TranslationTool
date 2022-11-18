using System.Runtime.Serialization;

namespace Translation.Business
{
    /// <summary>
    /// 释义信息
    /// </summary>
    [DataContract]
    public class SematicDbData
    {
        [DataMember(Name = "PartOfSpeech")]
        public string WordType { get; set; }
        [DataMember(Name = "Semantic")]
        public string Semantic { get; set; }
    }
}
