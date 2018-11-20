using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Net;
using System.IO;
using System.Net.Cache;
using System.Data;

namespace Re.Common
{
    public class HttpRequestUtil
    {

        //获取并返回网页源文件中所有字符
        public static string GetHtml(string url)
        {
            try
            {
                HttpWebRequest myRq = (HttpWebRequest)HttpWebRequest.Create(url);
                myRq.Timeout = 1000*60*5;
                HttpRequestCachePolicy noCachePolicy = new HttpRequestCachePolicy(HttpRequestCacheLevel.Default);
                myRq.CachePolicy = noCachePolicy;
                HttpWebResponse myResp = (HttpWebResponse)myRq.GetResponse();
                Stream myStream = myResp.GetResponseStream();
                Encoding encode = Encoding.GetEncoding("utf-8");
                StreamReader sr = new StreamReader(myStream, encode);
                string allStr = sr.ReadToEnd();
                myResp.Close();
                myStream.Close();
                sr.Close();
                return allStr;
            }
            catch
            {
                return "";
            }
        }


        public static void DownLoadFile(string url,string filepath)
        {
            Uri uri = new Uri(url);
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
            WebResponse response = request.GetResponse();
            Stream stream = response.GetResponseStream();

            if (!response.ContentType.ToLower().StartsWith("text/"))
            {
                //Value = SaveBinaryFile(response, FileName); 
                byte[] buffer = new byte[1024];
                Stream outStream = System.IO.File.Create(filepath);
                Stream inStream = response.GetResponseStream();

                int l;
                do
                {
                    l = inStream.Read(buffer, 0, buffer.Length);
                    if (l > 0)
                        outStream.Write(buffer, 0, l);
                }
                while (l > 0);

                outStream.Close();
                inStream.Close();
            }
        }



        /// <summary>
        /// 存放本地路径
        /// </summary>
        /// <param name="content"></param>
        /// <param name="jsonFilePath"></param>
        public  static void SaveLocalFilePath(string content, string jsonFilePath)
        {

            FileInfo fi = new FileInfo(jsonFilePath);
            if (!Directory.Exists(fi.DirectoryName))
            {
                Directory.CreateDirectory(fi.DirectoryName);
            }


            if (content.Length > 10)
            {
                string tempFile = jsonFilePath + ".temp";
                StreamWriter sw = new StreamWriter(tempFile, false, System.Text.Encoding.GetEncoding("utf-8"));
                try
                {
                    sw.Write(content);
                    sw.Flush();
                }
                finally
                {
                    if (sw != null) sw.Close();
                }

                File.Copy(tempFile, jsonFilePath, true);
                File.Delete(tempFile);
            }
        }


    }
}
