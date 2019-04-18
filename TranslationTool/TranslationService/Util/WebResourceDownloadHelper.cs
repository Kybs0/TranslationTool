using System;
using System.IO;
using System.Net;

namespace Translation.Util
{
    public class WebResourceDownloadHelper
    {
        public static bool Download(string resourceUri,string appdataFolder, out string downloadPath)
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
                    var userDownloadFolder = GetUserDownloadFolder(appdataFolder);
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

        private static string GetUserDownloadFolder(string appdataFolder)
        {
            if (!Directory.Exists(appdataFolder))
            {
                Directory.CreateDirectory(appdataFolder);
            }

            return appdataFolder;
        }
    }
}
