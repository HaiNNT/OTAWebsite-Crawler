using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IdealHotelMVC4.Models;
using ClassLibrary1.Service;

namespace ClassLibrary1.MainObject
{
    class Crawler
    {
        //Data to Database
        public List<HotelUrl> HotelUrls = new List<HotelUrl>();
        

        //Data from Database
        //Assigning values for them before run method GetUrl
        public List<Regex> PlaceRegexs = new List<Regex>();
        public List<Regex> HotelRegexs = new List<Regex>();
        public CrawlerSetup CrawlerSetup = new CrawlerSetup();
        
        //Use at Runtime
        HtmlAgilityPack.HtmlDocument HtmlDoc = new HtmlAgilityPack.HtmlDocument();
        public int success = 0;
        public List<string> PlaceUrls = new List<string>();

        public void GetUrl(object n)
        {   //Variable j use to identify the default method        
            int j = Convert.ToInt32(n);

            //
            for (int i = 0; j >= 0 && i < PlaceUrls.Count; i++)
            {
                if (j == 0)
                    j = -1;
                
                HtmlDoc.Load(InternetService.GetHtmlStreamGetMethod(PlaceUrls[i]), Encoding.UTF8);
                foreach (var item2 in HtmlDoc.DocumentNode.SelectNodes("//a"))
                {
                    if (item2.Attributes["href"] != null)
                    {
                        string url = CrawlerSetup.WebsiteUrl + item2.Attributes["href"].Value;
                        if (IsPlaceUrl(url) && !IsAvailableInList(PlaceUrls, url))
                        {
                            PlaceUrls.Add(url);
                        }

                        if (IsHotelUrl(url) && !IsAvailableInList(HotelUrls, url))
                        {
                            HotelUrl hotelurl = new HotelUrl();
                            hotelurl.CrawlerName = CrawlerSetup.CrawlerName;
                            hotelurl.Url = url;
                            HotelUrls.Add(hotelurl);
                        }
                    }
                }
            }
            
            success = 1;
        }

        public bool IsAvailableInList(List<HotelUrl> list, string str)
        {
            foreach (var item in list)
            {
                if (item.Url.CompareTo(str) == 0)
                    return true;
            }
            return false;
        }

        public bool IsAvailableInList(List<string> list, string str)
        {
            foreach (var item in list)
            {
                if (item.CompareTo(str) == 0)
                    return true;
            }
            return false;
        }

        public bool IsPlaceUrl(string url)
        {
            bool check = false;
            foreach (var item in PlaceRegexs)
            {
                System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex(item.Regex1);
                if (regex.Match(url).Length == url.Length)
                    check = true;
            }

            return check;
        }

        public bool IsHotelUrl(string url)
        {
            bool check = false;
            foreach (var item in HotelRegexs)
            {
                System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex(item.Regex1);
                if (regex.Match(url).Length == url.Length)
                    check = true;
            }

            return check;
        }
    }
}
