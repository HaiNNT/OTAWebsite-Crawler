using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ClassLibrary1.MainObject;
using System.Threading;
using ClassLibrary1;
using ClassLibrary1.Dal;
using IdealHotelMVC4.Models;

namespace ClassLibrary1.Manager
{
    class CrawlerManager
    {
        //Assigning values for they before run any method
        public CrawlerSetup CrawlerSetup = new CrawlerSetup();


        //Use at Runtime
        public List<Crawler> Crawlers = new List<Crawler>();
        public List<string> DefaultUrl = new List<string>();
        public SetupDal setupDal = new SetupDal();
        
        public void CreateCrawler()
        {
            
            for (int i = 0; i < 6; i++)
            {
                Crawler crawler = new Crawler();
                crawler.HotelRegexs = setupDal.GetRegex(CrawlerSetup.CrawlerName, "HotelRegex");
                crawler.PlaceRegexs = setupDal.GetRegex(CrawlerSetup.CrawlerName, "PlaceRegex");
                crawler.CrawlerSetup = CrawlerSetup;
                Crawlers.Add(crawler);
            }
        }

        public void GetDefaultUrl()
        {
            Crawlers[0].PlaceUrls.Add(CrawlerSetup.WebsiteUrl);
            Crawlers[0].GetUrl(0);
            DefaultUrl = Crawlers[0].PlaceUrls;
        }

        public bool GetHotelUrl()
        {
            for (int i = 0; i < Crawlers[0].PlaceUrls.Count; i++)
            {
                if (i % 5 == 0)
                    Crawlers[1].PlaceUrls.Add(Crawlers[0].PlaceUrls[i]);
                if (i % 5 == 1)
                    Crawlers[2].PlaceUrls.Add(Crawlers[0].PlaceUrls[i]);
                if (i % 5 == 2)
                    Crawlers[3].PlaceUrls.Add(Crawlers[0].PlaceUrls[i]);
                if (i % 5 == 3)
                    Crawlers[4].PlaceUrls.Add(Crawlers[0].PlaceUrls[i]);
                if (i % 5 == 4)
                    Crawlers[5].PlaceUrls.Add(Crawlers[0].PlaceUrls[i]);
            }

            Thread thread1 = new Thread(Crawlers[1].GetUrl);
            Thread thread2 = new Thread(Crawlers[2].GetUrl);
            Thread thread3 = new Thread(Crawlers[3].GetUrl);
            Thread thread4 = new Thread(Crawlers[4].GetUrl);
            Thread thread5 = new Thread(Crawlers[5].GetUrl);

            thread1.Start(1);
            thread2.Start(1);
            thread3.Start(1);
            thread4.Start(1);
            thread5.Start(1);

            
            for (; ; )
            {
                if (Crawlers[1].success == 1 &&
                    Crawlers[2].success == 1 &&
                    Crawlers[3].success == 1 &&
                    Crawlers[4].success == 1 &&
                    Crawlers[5].success == 1)
                {
                    return true;
                }
            }

        }

        public List<HotelUrl> GetHotelUrls()
        {
            List<HotelUrl> list = new List<HotelUrl>();
            foreach (var item in Crawlers)
            {
                list.AddRange(item.HotelUrls);
                //foreach (var item2 in item.HotelUrls)
                //{
                //     list.Add(item2);
                //}
               
            }
            return list;
        }
    }
}
