using IdealHotelMVC4.Models;
using IdealHotelMVC4.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IdealHotelMVC4.ClassLibrary1.Manager
{
    public class SearchRoom
    {
        public List<HotelRoom> HotelRoomLists = new List<HotelRoom>();
        public List<Hotel> Hotels = new List<Hotel>();
        public string FromDate = "";
        public string ToDate = "";

        public int s = 0;

        public void SearchRoomByThread()
        {
            foreach (var item in Hotels)
            {
                HotelRoom hotelRoom = new HotelRoom();
                hotelRoom.Hotel = item;
                hotelRoom.GetRoomsInHotel(item.HotelId, FromDate, ToDate);
                HotelRoomLists.Add(hotelRoom);
            }
            s = 1;

        }
    }
}