using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IdealHotelMVC4.Models;
using ClassLibrary1.MediateClass;
using ClassLibrary1.Service;

namespace ClassLibrary1.Dal
{
    class HotelDal
    {
        HotelCompareDBEntities db = new HotelCompareDBEntities();

        /////////////////
        //    Hotel    //
        /////////////////

        public bool InsertHotels(List<MediateHotel> hotels)
        {
            try
            {
                int i = 0;
                foreach (var item in hotels)
                {
                    if (InsertHotel(item))
                        i++;
                }
                return i > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool InsertHotel(MediateHotel hotel)
        {
            try
            {
                if (GetHotel(hotel) == null)
                {
                    List<Place> places = GetPlaces();
                    int placeIndex = CompareService.FindSimilarInPlaces(places, hotel.PlaceName);
                    if (placeIndex != -1)
                    {
                        hotel.Hotel.PlaceId = places[placeIndex].PlaceId;
                        InsertHotel(hotel.Hotel);
                        return true;
                    }
                    return false;
                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public Hotel GetHotel(MediateHotel hotel)
        {
            try
            {
                List<Place> places = GetPlaces();
                List<Hotel> hotels = new List<Hotel>();
                int placeIndex = CompareService.FindSimilarInPlaces(places, hotel.PlaceName);
                if (placeIndex != -1)
                {
                    if (places[placeIndex].PlaceId.CompareTo(places[placeIndex].ProvinceId) == 0)
                    {
                        foreach (var item in GetplacesOfProvince(places[placeIndex].ProvinceId))
                            hotels.AddRange(GetHotelsByPlaceId(item.PlaceId));
                        int hotelIndex = CompareService.FindSimilarInHotels(hotels, hotel.Hotel);
                        if (hotelIndex == -1)
                            return null;
                        else
                            return hotels[hotelIndex];
                    }
                    else
                    {
                        hotels = GetHotelsByPlaceId(places[placeIndex].PlaceId);
                        hotels.AddRange(GetHotelsByPlaceId(places[placeIndex].ProvinceId));
                        int hotelIndex = CompareService.FindSimilarInHotels(hotels, hotel.Hotel);
                        if (hotelIndex == -1)
                            return null;
                        else
                            return hotels[hotelIndex];
                    }
                }
                else
                    return null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public List<Hotel> GetHotelsByPlaceId(string placeId)
        {
            var hotels = (from b in db.Hotels
                          where b.PlaceId == placeId
                          select b).ToList<Hotel>();
            return hotels;
        }

        public bool InsertHotel(Hotel hotel)
        {
            try
            {
                db.Hotels.Add(hotel);
                db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /////////////////
        //   Website   //
        /////////////////


        public Website GetWebsite(string websiteName)
        {
            var website = (from b in db.Websites
                           where b.WebsiteName == websiteName
                           select b).FirstOrDefault();
            return website;
        }

        public string GetWebsiteLogo(string hotelUrl)
        {
            var logo = (from w in db.Websites
                        join hw in db.Hotel_Website on w.WebsiteId equals hw.WebsiteId
                        where hw.HotelUrl == hotelUrl
                        select w.WebsiteLogo).FirstOrDefault();
            return logo;
        }

        /////////////////
        //    Place    //
        /////////////////

        public List<Place> GetPlaces()
        {
            var places = (from b in db.Places
                          select b).ToList<Place>();
            return places;
        }

        public List<Place> GetplacesOfProvince(string provinceId)
        {
            var places = (from b in db.Places
                          where b.ProvinceId == provinceId
                          select b).ToList<Place>();
            return places;
        }

        /////////////////
        //   Facility  //
        /////////////////

        public bool InsertFacilities(List<MediateFacility> facilities)
        {
            try
            {
                int i = 0;
                foreach (var item in facilities)
                {
                    if (InsertFacility(item))
                        i++;
                }
                return i > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool InsertFacility(MediateFacility facility)
        {
            try
            {
                //if (!IsAvailableFacility(facility.Facility.FacilityName))
                //{
                facility.Facility.HotelId = GetHotel(facility.MediateHotel).HotelId;

                db.Facilities.Add(facility.Facility);
                db.SaveChanges();
                return true;
                //}
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool IsAvailableFacility(string facilityName)
        {
            var facility = (from b in db.Facilities
                            where b.FacilityName == facilityName
                            select b).FirstOrDefault();
            if (facility == null)
                return false;
            return true;
        }

        /////////////////
        //    Image    //
        /////////////////

        public bool InsertImages(List<MediateImage> images)
        {

            int i = 0;
            foreach (var item in images)
            {
                try
                {
                    if (InsertImage(item))
                        i++;
                }
                catch (Exception)
                {
                    return false;
                }
            }
            return i > 0;
        }

        public bool InsertImage(MediateImage image)
        {
            //try
            //{
            if (!IsAvailableImage(image.Image.Url))
            {
                image.Image.HotelId = GetHotel(image.MediateHotel).HotelId;
                db.Images.Add(image.Image);
                db.SaveChanges();
                return true;
            }
            return false;
            //}
            //catch (Exception)
            //{
            //    return false;
            //}
        }

        public bool IsAvailableImage(string imageUrl)
        {
            var image = (from b in db.Images
                         where b.Url == imageUrl
                         select b).FirstOrDefault();
            if (image == null)
                return false;
            return true;
        }

        /////////////////
        //Hotel_WebSite//
        /////////////////

        public bool InsertHoWes(List<MediateHW> hoWes)
        {
            try
            {
                int i = 0;
                foreach (var item in hoWes)
                {
                    if (InsertHoWe(item))
                        i++;
                }
                return i > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool InsertHoWe(MediateHW hoWe)
        {
            try
            {
                hoWe.HotelWebsite.HotelId = GetHotel(hoWe.MediateHotel).HotelId;
                hoWe.HotelWebsite.WebsiteId = GetWebsite(hoWe.WebsiteName).WebsiteId;
                if (!IsAvailableHotelWebsite(hoWe.HotelWebsite.HotelUrl))
                {
                    db.Hotel_Website.Add(hoWe.HotelWebsite);
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

        public List<Hotel_Website> GetHotelWebsites(int websiteId)
        {
            try
            {
                var hoWes = (from b in db.Hotel_Website
                             where b.WebsiteId == websiteId
                             select b).ToList<Hotel_Website>();
                return hoWes;
            }
            catch (Exception)
            {
                return null;
            }

        }

        public bool IsAvailableHotelWebsite(string hotelUrl)
        {
            var hotel = (from b in db.Hotel_Website
                         where b.HotelUrl == hotelUrl
                         select b).FirstOrDefault();
            if (hotel == null)
                return false;
            return true;

        }

        public List<Hotel_Website> GetHotelWebsiteByHotelId(int hotelId)
        {
            try
            {
                var hoWes = (from b in db.Hotel_Website
                             where b.HotelId == hotelId
                             select b).ToList<Hotel_Website>();
                return hoWes;
            }
            catch (Exception)
            {
                return null;
            }

        }



    }
}
