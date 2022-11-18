using System.Collections.Generic;
using System.Runtime.Serialization;
using Translation.Api;

namespace Translation.WebApi.KinsoftApi
{
    [DataContract]
    public class KinsoftSynonymInfo
    {
        [DataMember(Name = "part_name")]
        public string SynonymType { get; set; }
        [DataMember(Name = "means")]
        public List<KinsoftSynonymContent> SynonymContentList { get; set; }

        public List<SynonymInfo> GetSynonymWords()
        {
            var synonymInfos = new List<SynonymInfo>();
            if (SynonymContentList != null)
            {
                foreach (var synonymWord in SynonymContentList)
                {
                    if (synonymWord.SynonymWords != null)
                    {
                        synonymInfos.Add(new SynonymInfo()
                        {
                            WordType = SynonymType,
                            Translation = synonymWord.WordTranslation,
                            Synonyms = synonymWord.SynonymWords
                        });
                    }
                }
            }

            return synonymInfos;
        }
    }

    [DataContract]
    public class KinsoftSynonymContent
    {
        [DataMember(Name = "word_mean")]
        public string WordTranslation { get; set; }
        [DataMember(Name = "cis")]
        public List<string> SynonymWords { get; set; }
    }
}
