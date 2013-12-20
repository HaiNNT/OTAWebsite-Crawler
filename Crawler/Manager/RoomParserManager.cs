using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using ClassLibrary1;
using ClassLibrary1.MainObject;
using IdealHotelMVC4.Models;
using ClassLibrary1.Dal;
namespace ClassLibrary1.Manager
{
    class RoomParserManager
    {
        //Assigning values for they before run any method
        public CrawlerSetup CrawlerSetup = new CrawlerSetup();
        public DateTime FromDate = new DateTime();

        //Use at Runtime
        public List<RoomParser> Parsers = new List<RoomParser>();
        SetupDal Dal = new SetupDal();
        HotelDal hotelDal = new HotelDal();
        RoomDal roomDal = new RoomDal();

        //Data from Database
        public List<Hotel_Website> HotelWebsites = new List<Hotel_Website>();


        public void CreateParser()
        {
            for (int i = 0; i < 3; i++)
            {
                ParserSetup parserSetup = Dal.GetParserSetup(CrawlerSetup.CrawlerName, "RoomParser");
                RoomParser parser = new RoomParser();
                parser.RoomXpaths = Dal.GetXathsByParserId(parserSetup.ParserId);
                parser.FormDatas = Dal.GetFormDataByParserId(parserSetup.ParserId);
                foreach (var item in parser.FormDatas)
                {
                    if (item.FormDataType.CompareTo("FromDate") == 0)
                    {
                        item.FormDataValue = FromDate.Date.ToString().Substring(0, 10);
                    }
                    if (item.FormDataType.CompareTo("ToDate") == 0)
                    {
                        item.FormDataValue = FromDate.AddDays(1).Date.ToString().Substring(0, 10);
                    }
                }
                parser.Method = parserSetup.Method;
                Parsers.Add(parser);
            }
        }

        public bool GetRooms()
        {
            //HotelWebsites = hotelDal.GetHotelWebsites();
            for (int i = 0; i < HotelWebsites.Count; i++)
            {
                if (i % 3 == 0)
                    Parsers[0].HotelWebsites.Add(HotelWebsites[i]);
                if (i % 3 == 1)
                    Parsers[1].HotelWebsites.Add(HotelWebsites[i]);
                if (i % 3 == 2)
                    Parsers[2].HotelWebsites.Add(HotelWebsites[i]);
            }

            Thread thread1 = new Thread(Parsers[0].GetAllRooms);
            Thread thread2 = new Thread(Parsers[1].GetAllRooms);
            Thread thread3 = new Thread(Parsers[2].GetAllRooms);
            //Parsers[0].GetHotels();
            thread1.Start();
            thread2.Start();
            thread3.Start();

            for (; ; )
            {
                if (Parsers[0].Success == 1 &&
                   Parsers[1].Success == 1 &&
                    Parsers[2].Success == 1)
                {
                    return true;
                }

            }

        }
        public List<int> InsertAllDataToDatabase()
        {
            List<int> l = new List<int>();
            int count = 0;
            int c = 0;
            try
            {
                List<Room> rooms = new List<Room>();
                foreach (var item in Parsers)
                {
                    rooms.AddRange(item.Rooms);
                    count += item.Count;
                    c += item.C;
                }
                //HotelDal dal = new HotelDal();
                //dal.InsertHotels(Hotels);
                //dal.InsertFacilities(Facilities);
                //dal.InsertImages(Images);
                roomDal.InsertRooms(rooms);
                l.Add(count);
                l.Add(c);

            }
            catch (Exception)
            {
                l.Add(-1);
                l.Add(-1);
                return l;
            }
            return l;
        }

    }
}
