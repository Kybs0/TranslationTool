using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Translation.Business.Util
{
    public static class IniFileHelper
    {
        [DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);
        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);

        /// <summary> 
        /// 写入INI文件 
        /// </summary> 
        /// <param name="Section">项目名称(如 [TypeName] )</param> 
        /// <param name="Key">键</param> 
        /// <param name="Value">值</param> 
        public static void IniWriteValue(string Section, string Key, string Value)
        {
            WritePrivateProfileString(Section, Key, Value, GetIniPath());
        }

        /// <summary> 
        /// 读出INI文件 
        /// </summary> 
        /// <param name="Section">项目名称(如 [TypeName] )</param> 
        /// <param name="Key">键</param> 
        public static string IniReadValue(string Section, string Key)
        {
            StringBuilder temp = new StringBuilder(500);
            int i = GetPrivateProfileString(Section, Key, "", temp, 500, GetIniPath());
            return temp.ToString();
        }

        /// <summary> 
        /// 验证文件是否存在 
        /// </summary> 
        /// <returns>布尔值</returns> 
        public static string GetIniPath()
        {
            var dataFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "TranslationTool");
            if (!Directory.Exists(dataFolder))
            {
                Directory.CreateDirectory(dataFolder);
            }

            var iniFile = new DirectoryInfo(dataFolder).GetFiles().FirstOrDefault(i => i.Name == "User.ini")?.FullName;
            if (!File.Exists(iniFile))
            {
                iniFile = Path.Combine(dataFolder, "User.ini");
                File.Create(iniFile);
                return iniFile;
            }
            return iniFile;
        }
    }
}
