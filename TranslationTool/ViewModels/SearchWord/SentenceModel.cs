using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TranslationTool.ViewModels
{
    public class SentenceModel : ViewModelBase
    {
        private string _chineseSentence;
        public string ChineseSentence
        {
            get => _chineseSentence;
            set
            {
                _chineseSentence = value;
                OnPropertyChanged();
            }
        }
        private string _englishSentenceUri;
        public string EnglishSentenceUri
        {
            get => _englishSentenceUri;
            set
            {
                _englishSentenceUri = value;
                OnPropertyChanged();
            }
        }
        private string _englishSentence;
        public string EnglishSentence
        {
            get => _englishSentence;
            set
            {
                _englishSentence = value;
                OnPropertyChanged();
            }
        }
    }
}
