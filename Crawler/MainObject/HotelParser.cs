using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ClassLibrary1.MediateClass;
using ClassLibrary1;
using ClassLibrary1.Service;
using IdealHotelMVC4.Models;
using System.IO;
namespace ClassLibrary1.MainObject
{
    class HotelParser
    {
        //Data from Database
        public List<HotelUrl> HotelUrls = new List<HotelUrl>();
        public List<Xpath> Xpaths = new List<Xpath>();
        public List<FormData> FormDatas = new List<FormData>();
        public string Method = "";


        //Data to Database
        public List<MediateHotel> Hotels = new List<MediateHotel>();
        public List<MediateFacility> Facilities = new List<MediateFacility>();

        public List<MediateImage> Images = new List<MediateImage>();
        public List<MediateHW> HoWes = new List<MediateHW>();

        //Use at Runtime       
        public int Success = 0;
        HtmlAgilityPack.HtmlDocument HtmlDoc = new HtmlAgilityPack.HtmlDocument();

        //Methods
        public void GetHotel(HotelUrl hotelUrl)
        {
            string hotelNameXpath = "";
            string placeXpath = "";
            string descriptionXpath = "";
            string addressXpath = "";
            string starXpath = "";
            string rateXpath = "";
            string facilityXpath = "";
            string imageXpath = "";
            foreach (var item in Xpaths)
            {
                if (item.XpathName.CompareTo("HotelName") == 0)
                    hotelNameXpath = item.Xpath1;
                if (item.XpathName.CompareTo("PlaceName") == 0)
                    placeXpath = item.Xpath1;
                if (item.XpathName.CompareTo("Description") == 0)
                    descriptionXpath = item.Xpath1;
                if (item.XpathName.CompareTo("Address") == 0)
                    addressXpath = item.Xpath1;
                if (item.XpathName.CompareTo("Star") == 0)
                    starXpath = item.Xpath1;
                if (item.XpathName.CompareTo("Rate") == 0)
                    rateXpath = item.Xpath1;
                if (item.XpathName.CompareTo("Facility") == 0)
                    facilityXpath = item.Xpath1;
                if (item.XpathName.CompareTo("Image") == 0)
                    imageXpath = item.Xpath1;
            }

            //Require form data as parameters
            bool check = false;
            if (Method.CompareTo("Get") == 0)
            {
                Stream stream = InternetService.GetHtmlStreamGetMethod(hotelUrl.Url);
                if (stream != null)
                {
                    HtmlDoc.Load(stream, Encoding.UTF8);
                    check = true;
                }
            }
            else
            {
                Stream stream = InternetService.GetHtmlStreamPostMethod(hotelUrl.Url, FormDatas);
                if (stream != null)
                {
                    HtmlDoc.Load(stream, Encoding.UTF8);
                    check = true;
                }
            }
            if (check)
            {
                MediateHotel hotel = new MediateHotel();
                hotel.Hotel.OrderNum = 0;

                //Match Xpath
                hotel.PlaceName = StringService.FixPlaceName(HtmlDoc.DocumentNode.SelectSingleNode(placeXpath).InnerText.Trim());
                hotel.Hotel.HotelName = StringService.FixHotelName(HtmlDoc.DocumentNode.SelectSingleNode(hotelNameXpath).InnerText.Trim());
                hotel.Hotel.Address = StringService.FixAddress(HtmlDoc.DocumentNode.SelectSingleNode(addressXpath).InnerText.Trim());
                if (HtmlDoc.DocumentNode.SelectSingleNode(rateXpath) != null)
                    hotel.Hotel.Rate = StringService.FixRate(HtmlDoc.DocumentNode.SelectSingleNode(rateXpath).InnerText.Trim());
                else
                    hotel.Hotel.Rate = 0;
                if (HtmlDoc.DocumentNode.SelectSingleNode(starXpath) != null)
                    hotel.Hotel.Star = StringService.FixStar(HtmlDoc.DocumentNode.SelectSingleNode(starXpath));
                else
                    hotel.Hotel.Star = 1;
                if (HtmlDoc.DocumentNode.SelectSingleNode(descriptionXpath) != null)
                {
                    hotel.Hotel.Description = HtmlDoc.DocumentNode.SelectSingleNode(descriptionXpath).InnerText.Trim();
                    if (hotel.Hotel.Description.CompareTo("") == 0)
                        hotel.Hotel.Description = "Don't have Description";
                }
                else
                    hotel.Hotel.Description = "Don't have Description";
                Hotels.Add(hotel);


                MediateHW howe = new MediateHW();
                howe.MediateHotel = hotel;
                howe.WebsiteName = hotelUrl.CrawlerSetup.WebsiteName;
                howe.HotelWebsite.HotelUrl = hotelUrl.Url;
                HoWes.Add(howe);
                GetFacilities(hotel, facilityXpath);
                GetImageUrls(hotel, imageXpath);
            }
        }

        public void GetFacilities(MediateHotel hotel, string xpath)
        {
            //Match Xpath
            if (HtmlDoc.DocumentNode.SelectNodes(xpath) != null)
            {
                foreach (var item in HtmlDoc.DocumentNode.SelectNodes(xpath))
                {
                    MediateFacility facility = new MediateFacility();
                    facility.Facility.FacilityName = item.InnerText.Trim();
                    facility.MediateHotel = hotel;
                    Facilities.Add(facility);
                }
            }
        }

        public void GetImageUrls(MediateHotel hotel, string xpath)
        {

            if (HtmlDoc.DocumentNode.SelectNodes(xpath) != null)
            {
                int i = 0;
                //*[@id="hotel-image"]/tbody/tr[2]/td[2]/div/a/img
                foreach (var item in HtmlDoc.DocumentNode.SelectNodes(xpath))
                {

                    MediateImage image = new MediateImage();
                    if (xpath[xpath.Length - 1] == 'a')
                        image.Image.Url = item.Attributes["href"].Value;
                    //if (xpath[xpath.Length - 1] == 'g')
                    else
                        image.Image.Url = item.Attributes["src"].Value;
                    image.MediateHotel = hotel;
                    i++;
                    string imageName = StringService.MakeImageName(hotel.Hotel.HotelName, HotelUrls[0].CrawlerName.Replace("Crawler", ""), i);
                    if (CompareService.IsImageUrl(image.Image.Url))
                        image.Image.Url = InternetService.DownloadImage(image.Image.Url, imageName);
                    else
                        image.Image.Url = "";

                    Images.Add(image);
                }
            }
        }

        public void GetHotels()
        {
            foreach (var item in HotelUrls)
            {
                //try
                //{
                GetHotel(item);
                //}
                //catch (Exception)
                //{                    
                //}               
            }

            Success = 1;
        }



    }
}
