using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Translation.Api
{

    /// <summary>
    /// 单词发音
    /// </summary>
    public class PronounceInfo
    {
        /// <summary>
        /// 发音
        /// </summary>
        public string Pronounce { get; set; }
        /// <summary>
        /// 发音音频地址
        /// </summary>
        public string PronounceUri { get; set; }
    }
}
