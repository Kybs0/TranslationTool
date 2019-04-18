using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Translation.Api
{
    /// <summary>
    /// 词组信息
    /// </summary>
    public class PhraseInfo
    {
        public string Phrase { get; set; }
        public string PhraseTranslation { get; set; }
    }
    /// <summary>
    /// 扩展词组信息
    /// </summary>
    public class ExternalPhraseInfo
    {
        public string Phrase { get; set; }
        public string PhraseTranslation { get; set; }
    }


}
