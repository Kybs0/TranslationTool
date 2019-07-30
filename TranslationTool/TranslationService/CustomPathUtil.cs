using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Translation.Business
{
    public static class CustomPathUtil
    {
        public static string DbSourcePath => Path.Combine(Directory.GetCurrentDirectory(), "Resources/EnglishDict.db3");
        public static string DbOuputPath => Path.Combine(Directory.GetCurrentDirectory(), "Resources/EnglishDictTemplate.db3");
        public static string WordAudioFolder => Path.Combine(Directory.GetCurrentDirectory(), "AudioFiles/Words");
        public static string SentenceAudioFolder => Path.Combine(Directory.GetCurrentDirectory(), "AudioFiles/Sentences");

        public static string GetUserDownloadFolder()
        {
            string appdataFolder = Path.Combine(@"C:\Users\" + Environment.UserName + @"\AppData\Roaming\TranslationTool\Downloads");
            if (!Directory.Exists(appdataFolder))
            {
                Directory.CreateDirectory(appdataFolder);
            }

            return appdataFolder;
        }

    }
}
