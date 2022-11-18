using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TranslationTool.Helper
{
    public static class WordTypeHelper
    {
        public static string ConvertToChinese(string wordType)
        {
            if (WordTypeDict == null)
            {
                WordTypeDict = InitWordTypeDict();
            }

            wordType = (wordType ?? string.Empty).Replace(".", string.Empty);
            if (WordTypeDict.ContainsKey(wordType))
            {
                return WordTypeDict[wordType];
            }

            return wordType;
        }

        private static Dictionary<string, string> InitWordTypeDict()
        {
            var wordTypeDict = new Dictionary<string, string>();
            wordTypeDict.Add("v", "动词");
            wordTypeDict.Add("verb", "动词");
            wordTypeDict.Add("third", "动词第三人称");
            wordTypeDict.Add("ing", "进行式");
            wordTypeDict.Add("past", "过去式");
            wordTypeDict.Add("done", "过去分词");
            wordTypeDict.Add("er", "比较级");
            wordTypeDict.Add("est", "最高级");

            wordTypeDict.Add("prep", "介词");

            wordTypeDict.Add("n", "名词");
            wordTypeDict.Add("noun", "名词");
            wordTypeDict.Add("pl", "复数");

            wordTypeDict.Add("adj", "形容词");

            wordTypeDict.Add("adv", "副词");
            return wordTypeDict;
        }

        private static Dictionary<string, string> WordTypeDict;
    }
}
