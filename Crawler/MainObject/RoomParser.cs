using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ClassLibrary1.MediateClass;
using ClassLibrary1;
using ClassLibrary1.Service;
using IdealHotelMVC4.Models;
using ClassLibrary1.Dal;
using System.IO;

namespace ClassLibrary1.MainObject
{
    class RoomParser
    {
        //Data from Database       
        public List<Hotel_Website> HotelWebsites = new List<Hotel_Website>();
        public List<FormData> FormDatas = new List<FormData>();
        public List<Xpath> RoomXpaths = new List<Xpath>();
        public string Method = "";

        //Data to Database
        public List<Room> Rooms = new List<Room>();
        //public List<MediateRW> RoomWebsites = new List<MediateRW>();

        //Use at Runtime
        HtmlAgilityPack.HtmlDocument HtmlDoc = new HtmlAgilityPack.HtmlDocument();

        public int Success = 0;
        public int Count = 0;
        public int C = 0;

        public string MakeHotelUrl(string hotelUrl)
        {
            foreach (var item in FormDatas)
            {
                hotelUrl += "&" + item.FormDataName + "=";
                if (item.FormDataType.CompareTo("FromDate") == 0 || item.FormDataType.CompareTo("ToDate") == 0)
                    hotelUrl += StringService.FixDateToString(item.FormDataValue);
                else
                    hotelUrl += item.FormDataValue;
            }
            return hotelUrl;
        }

        public void GetAllRooms()
        {
            foreach (var item in HotelWebsites)
            {
                //try
                //{
                GetRoomsOfHotel(item);
                C++;

                //}
                //catch (Exception)
                //{
                //    Count++;
                //}

            }
            Success = 1;
        }

        public void GetRoomsOfHotel(Hotel_Website hotelWebsite)
        {
            string roomNameXpath = "";
            string priceXpath = "";
            string dateXpath = "";

            foreach (var item in RoomXpaths)
            {
                if (item.XpathName.CompareTo("RoomName") == 0)
                    roomNameXpath = item.Xpath1;
                if (item.XpathName.CompareTo("Price") == 0)
                    priceXpath = item.Xpath1;
                if (item.XpathName.CompareTo("Date") == 0)
                    dateXpath = item.Xpath1;
            }


            string hotelUrl = hotelWebsite.HotelUrl;
            bool check = false;
            if (Method.CompareTo("Get") == 0)
            {
                hotelUrl = MakeHotelUrl(hotelUrl);
                Stream stream = InternetService.GetHtmlStreamGetMethod(hotelUrl);
                if (stream != null)
                {
                    HtmlDoc.Load(stream, Encoding.UTF8);
                    check = true;
                }
            }
            else
            {
                Stream stream = InternetService.GetHtmlStreamPostMethod(hotelUrl, FormDatas);
                if (stream != null)
                {
                    HtmlDoc.Load(stream, Encoding.UTF8);
                    check = true;
                }
            }
            if (check)
            {
                List<string> roomNames = new List<string>();
                List<double> roomPrices = new List<double>();

                //Get list of roomNames
                try
                {
                    foreach (var item in HtmlDoc.DocumentNode.SelectNodes(roomNameXpath))
                    {
                        roomNames.Add(item.InnerText.Trim());
                    }

                    //Get list of roomPrices
                    try
                    {
                        foreach (var item in HtmlDoc.DocumentNode.SelectNodes(priceXpath))
                        {

                            roomPrices.Add(Convert.ToDouble(item.InnerText.Trim().Replace(".", "").Replace(",", "")));
                        }
                    }
                    catch (Exception)
                    {
                    }

                    for (int i = 0, n = roomNames.Count - roomPrices.Count; i < n; i++)
                    {
                        roomPrices.Add(0);
                    }
                }
                catch (Exception)
                {
                    roomNames.Add("Not Available");
                    roomPrices.Add(0);
                }


                for (int i = 0; i < roomNames.Count; i++)
                {
                    Room room = new Room();
                    //room.HotelId = hotelWebsite.HotelId;
                    //room.WebsiteId = hotelWebsite.WebsiteId;
                    room.HotelUrl = hotelWebsite.HotelUrl;
                    //if (roomNames[i].CompareTo("Not Available") != 0)
                    string str ="";
                    foreach (var item in FormDatas)
                    {
                        if (item.FormDataType.CompareTo("FromDate") == 0)
                            str = item.FormDataValue;
                    }
                    room.Date = StringService.FixDateToDateTime(str);
                    //else
                    //room.Date = 
                    room.RoomName = roomNames[i];
                    room.Price = roomPrices[i];
                    room.WebsiteLogo = new HotelDal().GetWebsiteLogo(room.HotelUrl);
                    Rooms.Add(room);

                    //MediateRW mediateRW = new MediateRW();
                    //mediateRW.RoomWebsite.WebsiteId = hotelWebsite.WebsiteId;
                    //mediateRW.RoomWebsite.HotelUrl = hotelWebsite.HotelUrl;
                    //mediateRW.RoomName = roomNames[i];
                    //RoomWebsites.Add(mediateRW);
                }

            }
        }
    }
}
