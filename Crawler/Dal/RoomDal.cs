using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IdealHotelMVC4.Models;
using ClassLibrary1.Manager;
namespace ClassLibrary1.Dal
{
    class RoomDal
    {
        HotelCompareDBEntities db = new HotelCompareDBEntities();

        public bool InsertRooms(List<Room> rooms)
        {
            try
            {
                int i = 0;
                foreach (var item in rooms)
                {
                    if (InsertRoom(item))
                        i++;
                }
                return i > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool InsertRoom(Room room)
        {
            try
            {
                if (!IsAvailableRoom(room))
                {
                    db.Rooms.Add(room);
                    db.SaveChanges();
                    return true;
                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }

        }

        public bool IsAvailableRoom(Room r)
        {
            var room = (from b in db.Rooms
                        where b.HotelUrl.CompareTo(r.HotelUrl) == 0 && b.RoomName == r.RoomName && b.Date == r.Date
                        select b).FirstOrDefault();
            if (room == null)
                return false;
            return true;
        }
    }
}
