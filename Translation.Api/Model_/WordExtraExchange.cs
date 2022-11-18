using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Translation.Api
{
    /// <summary>
    /// 单词扩展词性，如名词复数、动词时态、形容词比较级等
    /// </summary>
    public class WordExtraExchange
    {
        /// <summary>
        /// 名词复数
        /// </summary>
        public List<string> pl { get; set; }

        public List<string> third { get; set; }
        /// <summary>
        /// 动词过去时
        /// </summary>
        public List<string> past { get; set; }
        /// <summary>
        /// 动词完成时
        /// </summary>
        public List<string> Done { get; set; }
        /// <summary>
        /// 动词进行时
        /// </summary>
        public List<string> ing { get; set; }

        /// <summary>
        /// 形容词比较级
        /// </summary>
        public List<string> er { get; set; }
        /// <summary>
        /// 形容词最高级
        /// </summary>
        public List<string> est { get; set; }

        /// <summary>
        /// 前置词
        /// </summary>
        public List<string> prep { get; set; }
        /// <summary>
        /// 名词
        /// </summary>
        public List<string> noun { get; set; }
        /// <summary>
        /// 形容词
        /// </summary>
        public List<string> adj { get; set; }
        /// <summary>
        /// 副词
        /// </summary>
        public List<string> adv { get; set; }

        public List<string> conn { get; set; }
    }
}
