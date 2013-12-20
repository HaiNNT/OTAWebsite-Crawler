using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using ClassLibrary1.MainObject;
using ClassLibrary1;
using IdealHotelMVC4.Models;
using ClassLibrary1.Dal;
using ClassLibrary1.MediateClass;
namespace ClassLibrary1.Manager
{
    class HotelParserManager
    {

        //Assigning values for they before run any method
        public CrawlerSetup CrawlerSetup = new CrawlerSetup();

        //Use at Runtime
        public List<HotelParser> Parsers = new List<HotelParser>();
        SetupDal Dal = new SetupDal(); 

        //Data from Database
        public List<HotelUrl> HotelUrls = new List<HotelUrl>();

        //Data to Database
        List<MediateHotel> Hotels = new List<MediateHotel>();
        List<MediateFacility> Facilities = new List<MediateFacility>();
        //List<MediateHF> HoFas = new List<MediateHF>();
        List<MediateImage> Images = new List<MediateImage>();
        List<MediateHW> HoWes = new List<MediateHW>();

        public void CreateParser()
        {
            for (int i = 0; i < 5; i++)
            {
                
                HotelParser parser = new HotelParser();
                ParserSetup parserSetup = Dal.GetParserSetup(CrawlerSetup.CrawlerName, "HotelParser");
                parser.Xpaths = Dal.GetXathsByParserId(parserSetup.ParserId);
                //parser.FormDatas = Dal.GetFormDataByParserId(parserSetup.ParserId);
                parser.Method = parserSetup.Method;
                Parsers.Add(parser);
            }
        }



        public bool GetHotel()
        {
            HotelUrls = Dal.GetHotelUrl(CrawlerSetup.CrawlerName).GetRange(2000,396);
            for (int i = 0; i < HotelUrls.Count; i++)
            {
                if (i % 5 == 0)
                    Parsers[0].HotelUrls.Add(HotelUrls[i]);
                if (i % 5 == 1)
                    Parsers[1].HotelUrls.Add(HotelUrls[i]);
                if (i % 5 == 2)
                    Parsers[2].HotelUrls.Add(HotelUrls[i]);
                if (i % 5 == 3)
                    Parsers[3].HotelUrls.Add(HotelUrls[i]);
                if (i % 5 == 4)
                    Parsers[4].HotelUrls.Add(HotelUrls[i]);
            }

            Thread thread1 = new Thread(Parsers[0].GetHotels);
            Thread thread2 = new Thread(Parsers[1].GetHotels);
            Thread thread3 = new Thread(Parsers[2].GetHotels);
            Thread thread4 = new Thread(Parsers[3].GetHotels);
            Thread thread5 = new Thread(Parsers[4].GetHotels);
            //Parsers[0].GetHotels();
            thread1.Start();
            thread2.Start();
            thread3.Start();
            thread4.Start();
            thread5.Start();

            for (; ; )
            {
                if (Parsers[0].Success == 1 &&
                    Parsers[1].Success == 1 &&
                    Parsers[2].Success == 1 &&
                    Parsers[3].Success == 1 &&
                    Parsers[4].Success == 1)
                {
                    thread1.Abort();
                    thread2.Abort();
                    thread3.Abort();
                    thread4.Abort();
                    thread5.Abort();
                    return true;
                }
            }

        }

        public bool InsertAllDataToDatabase()
        {
            try
            {
                foreach (var item in Parsers)
                {
                    Hotels.AddRange(item.Hotels);
                    Facilities.AddRange(item.Facilities);
                    Images.AddRange(item.Images);
                    HoWes.AddRange(item.HoWes);
                    //HoFas.AddRange(item.HoFas);                             
                }
                HotelDal dal = new HotelDal();
                dal.InsertHotels(Hotels);
                dal.InsertFacilities(Facilities);
                dal.InsertImages(Images);
                dal.InsertHoWes(HoWes);    
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

    }
}
