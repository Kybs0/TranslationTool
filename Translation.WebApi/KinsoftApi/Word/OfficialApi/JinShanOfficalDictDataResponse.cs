using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace Translation.WebApi.KinsoftApi
{
    [XmlRoot(ElementName = "dict")]
    [Serializable]
    public class JinShanOfficalDictDataResponse
    {
        [XmlElement(ElementName = "key")]
        public string Word { set; get; }
        [XmlElement(ElementName = "ps")]
        public string PronounceUs { set; get; }
        [XmlElement(ElementName = "pron")]
        public string PronounceUsAudio { set; get; }
        [XmlElement(ElementName = "ps")]
        public string PronounceUk { set; get; }
        [XmlElement(ElementName = "pron")]
        public string PronounceUkAudio { set; get; }
    }
}
