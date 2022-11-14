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
using Translation.Business;
using TranslationExportTool;
using CustomPathUtil = Translation.Util.CustomPathUtil;

namespace TranslationExportTool
{
    public class EnglishWordExportService
    {
        /// <summary>
        /// 设置音频
        /// </summary>
        /// <param name="wordData"></param>
        /// <param name="pronounceFileName"></param>
        /// <param name="pronounce"></param>
        private static void SetWordPronounceInfo(EnglishWordTranslationData wordData, ref string pronounceFileName, out string pronounce)
        {
            pronounce = string.Empty;
            if (wordData.UkPronounce != null && !string.IsNullOrEmpty(wordData.UkPronounce.Pronounce) && !string.IsNullOrEmpty(wordData.UkPronounce.PronounceUri))
            {
                //如果音频为空，则下载最新音频
                if (string.IsNullOrEmpty(pronounceFileName))
                {
                    var downloadSuccess = WebResourceDownloadHelper.Download(wordData.UkPronounce.PronounceUri, CustomPathUtil.WordAudioFolder, out var downloadPath);
                    if (downloadSuccess && File.Exists(downloadPath))
                    {
                        pronounce = $"[{wordData.UkPronounce.Pronounce}]";
                        pronounceFileName = Path.GetFileName(downloadPath);
                        return;
                    }
                }
            }
            else if (wordData.UsPronounce != null && !string.IsNullOrEmpty(wordData.UsPronounce.Pronounce) && !string.IsNullOrEmpty(wordData.UsPronounce.PronounceUri))
            {
                //如果音频为空，则下载最新音频
                if (string.IsNullOrEmpty(pronounceFileName))
                {
                    var downloadSuccess = WebResourceDownloadHelper.Download(wordData.UsPronounce.PronounceUri, CustomPathUtil.WordAudioFolder, out var downloadPath);
                    if (downloadSuccess && File.Exists(downloadPath))
                    {
                        pronounce = $"[{wordData.UsPronounce.Pronounce}]";
                        pronounceFileName = Path.GetFileName(downloadPath);
                        return;
                    }
                }
            }

            if (!string.IsNullOrEmpty(pronounceFileName))
            {
                pronounceFileName = string.Empty;
            }
            if (wordData.UkPronounce != null && !string.IsNullOrEmpty(wordData.UkPronounce.Pronounce))
            {
                pronounce = $"[{wordData.UkPronounce.Pronounce}]";
            }
            else if (wordData.UsPronounce != null && !string.IsNullOrEmpty(wordData.UsPronounce.Pronounce))
            {
                pronounce = $"[{wordData.UsPronounce.Pronounce}]";
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

        public static void SaveWord(EnglishWordSqliteHelper englishWordDbHleper, EnglishWordTranslationData wordData)
        {
            if (string.IsNullOrEmpty(wordData.Word))
            {
                return;
            }
            var previousWord = englishWordDbHleper.GetWord(wordData.Word);

            WordInfo newWord = new WordInfo();
            newWord.Word = wordData.Word;
            var sentenceSaveInfos = GetSentenceSaveInfos(wordData);
            var newSemanticList = GetSemanticSaveInfos(wordData);
            var phraseSaveInfos = GetPhraseSaveInfos(wordData);
            var synonymSaveInfos = GetSynonymInfos(wordData);
            var cognateSaveInfos = GetCognateInfos(wordData);
            if (previousWord != null)
            {
                var pronounceFileName = previousWord.PronounceFileName;
                SetWordPronounceInfo(wordData, ref pronounceFileName, out string pronounce);
                newWord.Pronounce = pronounce;
                newWord.PronounceFileName = pronounceFileName;
                newWord.Semantic = string.IsNullOrEmpty(previousWord.Semantic) ?
                    newSemanticList.Count > 0 ? JsonConvert.SerializeObject(newSemanticList) : string.Empty : previousWord.Semantic;
                newWord.Phrase = string.IsNullOrEmpty(previousWord.Phrase) ?
                    phraseSaveInfos.Count > 0 ? JsonConvert.SerializeObject(phraseSaveInfos) : string.Empty : previousWord.Phrase;
                newWord.Sentence = string.IsNullOrEmpty(previousWord.Sentence) ?
                    sentenceSaveInfos.Count > 0 ? JsonConvert.SerializeObject(sentenceSaveInfos) : string.Empty : previousWord.Sentence;
                newWord.Synonym = string.IsNullOrEmpty(previousWord.Synonym) ?
                    synonymSaveInfos.Count > 0 ? JsonConvert.SerializeObject(synonymSaveInfos) : string.Empty : previousWord.Synonym;
                newWord.Cognate = string.IsNullOrEmpty(previousWord.Cognate) ?
                    cognateSaveInfos.Count > 0 ? JsonConvert.SerializeObject(cognateSaveInfos) : string.Empty : previousWord.Cognate;

                englishWordDbHleper.UpdateWordInfo(newWord);
            }
            else
            {
                var pronounceFileName = string.Empty;
                SetWordPronounceInfo(wordData, ref pronounceFileName, out string pronounce);
                newWord.Pronounce = pronounce;
                newWord.PronounceFileName = pronounceFileName;

                newWord.Semantic = newSemanticList.Count > 0 ? JsonConvert.SerializeObject(newSemanticList) : string.Empty;
                newWord.Phrase = phraseSaveInfos.Count > 0 ? JsonConvert.SerializeObject(phraseSaveInfos) : string.Empty;
                newWord.Sentence = sentenceSaveInfos.Count > 0 ? JsonConvert.SerializeObject(sentenceSaveInfos) : string.Empty;
                newWord.Synonym = synonymSaveInfos.Count > 0 ? JsonConvert.SerializeObject(synonymSaveInfos) : string.Empty;
                newWord.Cognate = cognateSaveInfos.Count > 0 ? JsonConvert.SerializeObject(cognateSaveInfos) : string.Empty;
                englishWordDbHleper.SaveWord(newWord);
            }
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
    public class SynonymSaveInfo
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
