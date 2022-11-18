using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using Translation.Api;

namespace Translation.WebApi.YouDaoApi
{
    /// <summary>
    /// 近义词
    /// </summary>
    [DataContract]
    public class SynonymDictionary
    {
        [DataMember(Name = "synos")]
        public List<SynonymWord> SynonymWords { get; set; }

        public List<SynonymInfo> GetSynonymWords()
        {
            var synonymInfos = new List<SynonymInfo>();
            if (SynonymWords != null)
            {
                foreach (var synonymWord in SynonymWords)
                {
                    if (synonymWord.SynonymWordData != null)
                    {
                        var wordList = synonymWord.SynonymWordData.SynonymWordDataDetails?.Select(i => i.Word).ToList() ?? new List<string>();
                        synonymInfos.Add(new SynonymInfo()
                        {
                            WordType = synonymWord.SynonymWordData.WordType,
                            Translation = synonymWord.SynonymWordData.Translation,
                            Synonyms = wordList
                        });
                    }
                }
            }

            return synonymInfos;
        }
    }
    [DataContract]
    public class SynonymWord
    {
        [DataMember(Name = "syno")]
        public SynonymWordData SynonymWordData { get; set; }
    }
    [DataContract]
    public class SynonymWordData
    {
        [DataMember(Name = "pos")]
        public string WordType { get; set; }
        [DataMember(Name = "tran")]
        public string Translation { get; set; }
        [DataMember(Name = "ws")]
        public List<SynonymWordDataDetail> SynonymWordDataDetails { get; set; }
    }
    [DataContract]
    public class SynonymWordDataDetail
    {
        [DataMember(Name = "w")]
        public string Word { get; set; }
    }
}
