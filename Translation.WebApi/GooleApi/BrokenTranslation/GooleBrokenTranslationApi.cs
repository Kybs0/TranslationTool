using System;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace Translation.WebApi.GooleApi.BrokenTranslation
{
    /// <summary>
    /// Goole破解版翻译
    /// </summary>
    public class GooleBrokenTranslationApi
    {
        /// <summary>
        /// 谷歌翻译
        /// </summary>
        /// <param name="text">待翻译文本</param>
        /// <param name="fromLanguage">自动检测：auto</param>
        /// <param name="toLanguage">中文：zh-CN，英文：en</param>
        /// <returns>翻译后文本</returns>
        public string GoogleTranslate(string text, string fromLanguage, string toLanguage)
        {
            CookieContainer cc = new CookieContainer();
            string googleTransBaseUrl = "https://translate.google.com/";
            var baseResultHtml = GetResultHtml(googleTransBaseUrl, cc, "");
            Regex re = new Regex(@"(?<=TKK=')(.*?)(?=')");
            var tkkMatchedValue = re.Match(baseResultHtml).ToString();//在返回的HTML中正则匹配TKK的值
            var baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            var gooleJsPath = Path.Combine(baseDirectory, @"Resources\goole.js");
            var jsText = File.ReadAllText(gooleJsPath);
            var tkValue = ExecuteScript("tk(\"" + text + "\",\"" + tkkMatchedValue + "\")", jsText);
            string googleTransUrl = $"{googleTransBaseUrl}translate_a/single?client=t&sl={fromLanguage}&tl={toLanguage}&hl=en&dt=at&dt=bd&dt=ex&dt=ld&dt=md&dt=qca&dt=rw&dt=rm&dt=ss&dt=t&ie=UTF-8&oe=UTF-8&otf=1&ssel=0&tsel=0&kc=1&tk={tkValue}&q={HttpUtility.UrlEncode(text)}";
            var resultHtml = GetResultHtml(googleTransUrl, cc, "https://translate.google.com/");
            dynamic deserializeObject = Newtonsoft.Json.JsonConvert.DeserializeObject(resultHtml);
            string resultText = Convert.ToString(deserializeObject[0][0][0]);
            return resultText;
        }

        public string GetResultHtml(string url, CookieContainer cookie, string referer)
        {
            var html = "";
            var webRequest = WebRequest.Create(url) as HttpWebRequest;
            webRequest.Method = "GET";
            webRequest.CookieContainer = cookie;
            webRequest.Referer = referer;
            webRequest.Timeout = 20000;
            webRequest.Headers.Add("X-Requested-With:XMLHttpRequest");
            webRequest.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";
            webRequest.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/70.0.3538.77 Safari/537.36";
            using (var webResponse = (HttpWebResponse)webRequest.GetResponse())
            {
                using (var reader = new StreamReader(webResponse.GetResponseStream(), Encoding.UTF8))
                {
                    html = reader.ReadToEnd();
                    reader.Close();
                    webResponse.Close();
                }
            }
            return html;
        }

        /// <summary>
        /// 执行JS
        /// </summary>
        /// <param name="sExpression">参数体</param>
        /// <param name="sCode">JavaScript代码的字符串</param>
        /// <returns></returns>
        private string ExecuteScript(string sExpression, string sCode)
        {
            MSScriptControl.ScriptControl scriptControl = new MSScriptControl.ScriptControl();
            scriptControl.UseSafeSubset = true;
            scriptControl.Language = "JScript";
            scriptControl.AddCode(sCode);
            try
            {
                string str = scriptControl.Eval(sExpression).ToString();
                return str;
            }
            catch (Exception ex)
            {
                string str = ex.Message;
            }
            return null;
        }
    }
}