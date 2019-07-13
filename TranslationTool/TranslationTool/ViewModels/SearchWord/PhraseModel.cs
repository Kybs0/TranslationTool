using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TranslationTool.ViewModels
{
    public class PhraseModel : ViewModelBase
    {
        private string _phrase;
        public string Phrase
        {
            get => _phrase;
            set
            {
                _phrase = value;
                OnPropertyChanged();
            }
        }
        private string _phraseTranslation;
        public string PhraseTranslation
        {
            get => _phraseTranslation;
            set
            {
                _phraseTranslation = value;
                OnPropertyChanged();
            }
        }
    }
}
