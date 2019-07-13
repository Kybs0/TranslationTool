using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Windows;
using Translation.Business;

namespace TranslationTool
{
    public class WebResourceDownloadHelper
    {
        public static bool Download(string resourceUri,out string downloadPath)
        {
            downloadPath = string.Empty;
            if (string.IsNullOrWhiteSpace(resourceUri))
            {
                return false;
            }
            try
            {
                //"http://ydschool-online.nos.netease.com/account_v0205.mp3"
                WebRequest request = WebRequest.Create(resourceUri);
                WebResponse response = request.GetResponse();
                using (Stream reader = response.GetResponseStream())
                {
                    var userDownloadFolder = CustomPathUtil.GetUserDownloadFolder();
                    downloadPath = Path.Combine(userDownloadFolder, $"{Guid.NewGuid()}{Path.GetExtension(resourceUri)}");        //图片路径命名 
                    using (FileStream writer = new FileStream(downloadPath, FileMode.OpenOrCreate, FileAccess.Write))
                    {
                        byte[] buff = new byte[512];
                        int c = 0;                                           //实际读取的字节数   
                        while ((c = reader.Read(buff, 0, buff.Length)) > 0)
                        {
                            writer.Write(buff, 0, c);
                        }
                        response.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                return false;
            }
            //下载成功
            return true;
        }
    }
}
