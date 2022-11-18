using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Translation.Api
{
    /// <summary>
    /// 例句信息
    /// </summary>
    public class SentenceInfo
    {
        public string Sentence { get; set; }
        public string EnglishSentenceUri { get; set; }
        public string Translation { get; set; }
    }

}
