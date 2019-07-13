using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Translation.Api
{
    /// <summary>
    /// 近义词信息
    /// </summary>
    public class SynonymInfo
    {
        public string WordType { get; set; }
        public string Translation { get; set; }
        public List<string> Synonyms { get; set; }
    }

}
