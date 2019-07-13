using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Translation.Api;

namespace Translation.Business.ExportService
{
    public class EnglishWordExportService
    {
        public static void SaveWord(EnglishWordSqliteHelper englishWordSource,
            EnglishWordSqliteHelper englishWordOutput, EnglishWordTranslationData wordData)
        {
            if (string.IsNullOrEmpty(wordData.Word))
            {
                return;
            }
            //var transformToWordSaveInfo = TransformToWordSaveInfo(wordData);
            var previousWord = englishWordSource.GetWord(wordData.Word);

            WordInfo newWord = new WordInfo();
            newWord.Word = wordData.Word;
            if (previousWord != null)
            {
                newWord.Pronounce = previousWord.Pronounce;
                newWord.PronounceFileName = previousWord.PronounceFileName;
            }
            else
            {
                newWord.Word = wordData.Word;
            }
            SetWordPronounceInfo(newWord, wordData);

            var newSemanticList = GetSemanticSaveInfos(wordData);
            if (newSemanticList.Count > 0)
            {
                newWord.Semantic = JsonConvert.SerializeObject(GetSemanticSaveInfos(wordData));
            }

            newWord.Sentence = JsonConvert.SerializeObject(GetSentenceSaveInfos(wordData));
            newWord.Phrase = JsonConvert.SerializeObject(GetPhraseSaveInfos(wordData));
            newWord.Synonym = JsonConvert.SerializeObject(GetSynonymInfos(wordData));
            newWord.Cognate = JsonConvert.SerializeObject(GetCognateInfos(wordData));
            englishWordOutput.SaveWord(newWord);
        }
        /// <summary>
        /// 设置音频
        /// </summary>
        /// <param name="word"></param>
        /// <param name="wordData"></param>
        private static void SetWordPronounceInfo(WordInfo word, EnglishWordTranslationData wordData)
        {
            if (wordData.UkPronounce != null && !string.IsNullOrEmpty(wordData.UkPronounce.Pronounce))
            {
                word.Pronounce = $"[{wordData.UkPronounce.Pronounce}]";
                //如果音频为空，则下载最新音频
                if (string.IsNullOrEmpty(word.PronounceFileName))
                {
                    var downloadSuccess = WebResourceDownloadHelper.Download(wordData.UkPronounce.PronounceUri, CustomPathUtil.WordAudioFolder, out var downloadPath);
                    if (downloadSuccess && File.Exists(downloadPath))
                    {
                        word.PronounceFileName = Path.GetFileName(downloadPath);
                    }
                }
            }
            else if (wordData.UsPronounce != null && !string.IsNullOrEmpty(wordData.UsPronounce.Pronounce))
            {
                word.Pronounce = $"[{wordData.UsPronounce.Pronounce}]";
                //如果音频为空，则下载最新音频
                if (string.IsNullOrEmpty(word.PronounceFileName))
                {
                    var downloadSuccess = WebResourceDownloadHelper.Download(wordData.UsPronounce.PronounceUri, CustomPathUtil.WordAudioFolder, out var downloadPath);
                    if (downloadSuccess && File.Exists(downloadPath))
                    {
                        word.PronounceFileName = Path.GetFileName(downloadPath);
                    }
                }
            }
        }

        private static List<SematicSaveInfo> GetSemanticSaveInfos(EnglishWordTranslationData wordData)
        {
            return wordData.Translations.Select(i => new SematicSaveInfo()
            {
                WordType = i.WordType,
                Translation = i.Translation.Trim().Replace(@"\t", string.Empty)
            }).ToList();
        }
        private static List<CognateSaveInfo> GetCognateInfos(EnglishWordTranslationData wordData)
        {
            var cognateSaveInfos = new List<CognateSaveInfo>();
            foreach (var info in wordData.Cognates)
            {
                var cognateSaveInfo = new CognateSaveInfo()
                {
                    WordType = info.WordType,
                    Translation = info.Translation,
                    Cognate = info.Cognate,
                };

                cognateSaveInfos.Add(cognateSaveInfo);
            }

            return cognateSaveInfos;
        }
        private static List<SynonymSaveInfo> GetSynonymInfos(EnglishWordTranslationData wordData)
        {
            var synonymSaveInfos = new List<SynonymSaveInfo>();
            foreach (var info in wordData.Synonyms)
            {
                var synonymSaveInfo = new SynonymSaveInfo()
                {
                    WordType = info.WordType,
                    Translation = info.Translation,
                    Synonyms = info.Synonyms,
                };

                synonymSaveInfos.Add(synonymSaveInfo);
            }

            return synonymSaveInfos;
        }

        private static List<PhraseSaveInfo> GetPhraseSaveInfos(EnglishWordTranslationData wordData)
        {
            var phraseSaveInfos = new List<PhraseSaveInfo>();
            foreach (var info in wordData.Phrases)
            {
                var sentenceSaveInfo = new PhraseSaveInfo()
                {
                    Phrase = info.Phrase,
                    Translation = info.PhraseTranslation,
                };

                phraseSaveInfos.Add(sentenceSaveInfo);
            }

            return phraseSaveInfos;
        }

        private static List<SentenceSaveInfo> GetSentenceSaveInfos(EnglishWordTranslationData wordData)
        {
            List<SentenceSaveInfo> sentenceSaveInfos = new List<SentenceSaveInfo>();
            foreach (var sentenceInfo in wordData.Sentences)
            {
                var sentenceSaveInfo = new SentenceSaveInfo()
                {
                    Sentence = sentenceInfo.Sentence,
                    Translation = sentenceInfo.Translation,
                };
                if (sentenceInfo.EnglishSentenceUri != null && sentenceInfo.EnglishSentenceUri.ToLower().EndsWith(".mp3"))
                {
                    var downloaded = WebResourceDownloadHelper.Download(sentenceInfo.EnglishSentenceUri, CustomPathUtil.SentenceAudioFolder, out var downloadPath);
                    if (downloaded && File.Exists(downloadPath))
                    {
                        sentenceSaveInfo.SentenceFileName = Path.GetFileName(downloadPath);
                    }
                }

                sentenceSaveInfos.Add(sentenceSaveInfo);
            }

            return sentenceSaveInfos;
        }
    }
    [DataContract]
    internal class CognateSaveInfo
    {
        [DataMember]
        public string WordType { get; set; }
        [DataMember]
        public string Cognate { get; set; }
        [DataMember]
        public string Translation { get; set; }
    }

    [DataContract]
    internal class SematicSaveInfo
    {
        [DataMember(Name = "PartOfSpeech")]
        public string WordType { get; set; }
        [DataMember(Name = "Semantic")]
        public string Translation { get; set; }
    }

    [DataContract]
    internal class SynonymSaveInfo
    {
        [DataMember]
        public string WordType { get; set; }
        [DataMember]
        public string Translation { get; set; }
        [DataMember]
        public List<string> Synonyms { get; set; }
    }

    [DataContract]
    internal class PhraseSaveInfo
    {
        [DataMember]
        public string Phrase { get; set; }
        [DataMember]
        public string Translation { get; set; }
    }

    /// <summary>
    /// 例句信息
    /// </summary>
    [DataContract]
    public class SentenceSaveInfo
    {
        [DataMember]
        public string Sentence { get; set; }
        [DataMember]
        public string SentenceFileName { get; set; }
        [DataMember]
        public string Translation { get; set; }
    }
}
