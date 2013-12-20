using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;
using IdealHotelMVC4.Models;
namespace ClassLibrary1.Service
{
    class InternetService
    {
        public static Stream GetHtmlStreamGetMethod(string url)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";
            request.ContentLength = 0;
            try
            {
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                return response.GetResponseStream();
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static Stream GetHtmlStreamPostMethod(string url, List<FormData> formDatas)
        {
            WebClient wc = new WebClient();
            //Thong Tin cua Header
            //wc.Headers["Content-Type"] = "application/x-www-form-urlencoded";
            //wc.Headers["User-Agent"] = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/29.0.1547.66 Safari/537.36";
            //wc.Headers["Cookie"] = "__utma=31260783.1257323130.1379434877.1379758919.1379844307.10; __utmb=31260783.14.10.1379844307; __utmc=31260783; __utmz=31260783.1379434877.1.1.utmcsr=google|utmccn=(organic)|utmcmd=organic|utmctr=(not%20provided)";

            //postData la bien chua Form Data
            System.Collections.Specialized.NameValueCollection postData = new System.Collections.Specialized.NameValueCollection();
            foreach (var item in formDatas)
            {
                if (item.FormDataType.CompareTo("FromDate") == 0 || item.FormDataType.CompareTo("ToDate") == 0)
                {
                    item.FormDataValue = StringService.FixDateToString(item.FormDataValue);
                }
                postData.Add(item.FormDataName, item.FormDataValue);
            }
            //postData.Add("data[start_date]", "23/09/2013");//DateTime.Today.AddDays(1).ToString("dd/MM/yyyy")
            //postData.Add("data[end_date]", "23/09/2013");

            try
            {
                Stream html = new MemoryStream(wc.UploadValues(url, "POST", postData));
                return html;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static string DownloadImage(string imageUrl, string imageName)
        {
            System.Drawing.Image image = null;
            try
            {
            HttpWebRequest webRequest = (HttpWebRequest)HttpWebRequest.Create(imageUrl);
            webRequest.Method = "GET";
            //webRequest.ContentType = "image/jpeg";
            ////webRequest.ContentLength = 0;
            ////webRequest.Headers["Content-Type"] = "image/webp";
            ////webRequest.Headers["Cookie"] = "__sbzss=1; subiz_name=Hai; subiz_phone=0902323244; booking=eyJ0aW1lZnJvbSI6MTM4MTU5NzIwMCwidGltZXRvIjoxMzgxNjgzNjAwfQ%3D%3D; PHPSESSID=nguk8t01v78mn78elq9ibc25k3; __utma=200660658.272604398.1379436233.1381777741.1381797863.27; __utmc=200660658; __utmz=200660658.1379436233.1.1.utmcsr=google|utmccn=(organic)|utmcmd=organic|utmctr=(not%20provided); subiz_client_id=da939b6d7342bca44091f7fea3c3c3b4; hotel_recent=OTg3MSwzMDYxLDMzMzMsMjA0NCwzOTk5LDUwMCwxMDAwLDQ5OTksNDAwMiw0MDAzLDQwMDUsNDAyMCw0MDUwLDQwMDEsNDEwMCw0MjAwLDQ1MDAsNTAwMCw0MDAwLDM0NTYsMTE1MiwxMTUxLDkwNTYsOTk2NiwyODQwLDQ5NywxMDE5MSwxMDY3MCwyODkzLDI3ODQsNDM3LDQ0NSwyNDA3LDEzMQ%3D%3D; RCTS=2";
            webRequest.AllowWriteStreamBuffering = true;
            webRequest.Timeout = 30000;
            webRequest.Proxy = null;
            
            ////webRequest.CookieContainer.Add(new Cookie("", ""));
            ////imageUrl = imageUrl.Replace(":8080","");
            System.Net.WebResponse webResponse = webRequest.GetResponse();

            System.IO.Stream stream = webResponse.GetResponseStream();

            image = System.Drawing.Image.FromStream(stream);

            webResponse.Close();              

                string rootPath = "Image/";
                string fileName = rootPath + imageName;

                image.Save(Path.GetFullPath(fileName));
                return fileName;
            }
            catch (Exception)
            {
                return "";
            }
            
        }
    }
}
