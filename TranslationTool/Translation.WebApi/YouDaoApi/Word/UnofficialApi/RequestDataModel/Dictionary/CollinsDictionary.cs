using System.Collections.Generic;
using System.Runtime.Serialization;
using Translation.Api;

namespace Translation.WebApi.YouDaoApi
{
    /// <summary>
    /// 科林大字典
    /// </summary>
    [DataContract]
    public class CollinsDictionary
    {
        [DataMember(Name = "collins_entries")]
        public List<CollinsEntry> CollinsEntries { get; set; }

        public List<SentenceInfo> GetCollinsSentences()
        {
            var sentenceInfos = new List<SentenceInfo>();
            if (CollinsEntries != null)
            {
                foreach (var collinsEntry in CollinsEntries)
                {
                    if (collinsEntry.CollinsEntriesData?.CollinsEntriesDataDetails != null)
                    {
                        foreach (var collinsEntriesDataDetail in collinsEntry.CollinsEntriesData?.CollinsEntriesDataDetails)
                        {
                            if (collinsEntriesDataDetail.CollinsEntryDataExamSentences != null)
                            {
                                foreach (var collinsEntryDataExamSentence in collinsEntriesDataDetail
                                    .CollinsEntryDataExamSentences)
                                {
                                    if (collinsEntryDataExamSentence.CollinsEntryDataExamSentence?.CollinsEntryDataExamSentenceDetail != null)
                                    {
                                        foreach (var entry in collinsEntryDataExamSentence.CollinsEntryDataExamSentence
                                            ?.CollinsEntryDataExamSentenceDetail)
                                        {
                                            sentenceInfos.Add(new SentenceInfo()
                                            {
                                                Sentence = entry.EnglishSentence,
                                                Translation = entry.ChineseSentence
                                            });
                                        }
                                    }

                                }
                            }
                        }
                    }
                }
            }

            return sentenceInfos;
        }
    }
    [DataContract]
    public class CollinsEntry
    {
        [DataMember(Name = "entries")]
        public CollinsEntriesData CollinsEntriesData { get; set; }
    }
    [DataContract]
    public class CollinsEntriesData
    {
        [DataMember(Name = "entry")]
        public List<CollinsEntriesDataDetail> CollinsEntriesDataDetails { get; set; }
    }
    [DataContract]
    public class CollinsEntriesDataDetail
    {
        [DataMember(Name = "tran_entry")]
        public List<CollinsTranslationEntry> CollinsEntryDataExamSentences { get; set; }
    }
    [DataContract]
    public class CollinsTranslationEntry
    {
        [DataMember(Name = "exam_sents")]
        public CollinsEntryDataExamSentence CollinsEntryDataExamSentence { get; set; }
    }
    [DataContract]
    public class CollinsEntryDataExamSentence
    {
        [DataMember(Name = "sent")]
        public List<CollinsEntryDataExamSentenceDetail> CollinsEntryDataExamSentenceDetail { get; set; }
    }
    [DataContract]
    public class CollinsEntryDataExamSentenceDetail
    {
        [DataMember(Name = "chn_sent")]
        public string ChineseSentence { get; set; }
        [DataMember(Name = "eng_sent")]
        public string EnglishSentence { get; set; }
    }
}
