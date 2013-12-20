using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace ClassLibrary1.Service
{
    class StringService
    {

        public static int FixStar(HtmlAgilityPack.HtmlNode node)
        {
            if (node.Attributes["src"] != null)
                return Convert.ToInt16(node.Attributes["src"].Value.Substring(25, 1));
            if(node.Attributes["class"] != null)
                return Convert.ToInt16(node.Attributes["class"].Value.Substring(19,1));
            return 1;
        }

        public static string FixHotelName(string hotelName)
        {
            List<string> cuttedStrings = new List<string>();
            cuttedStrings.Add("Hotel");
            cuttedStrings.Add("Khách sạn");
            cuttedStrings.Add("Khách Sạn");
            cuttedStrings.Add("Khu nghỉ dưỡng");
            cuttedStrings.Add("Vịnh");
            cuttedStrings.Add("Bay");

            //cuttedStrings.Add("Street");
            //cuttedStrings.Add("Spa");
            //cuttedStrings.Add(placeName);

            foreach (var item in cuttedStrings)
            {
                //if (hotelName.StartsWith(item))
                   hotelName = hotelName.Replace(item, "");
                //if (hotelName.EndsWith(item))
                    //hotelName.Replace(item, "");
            }
           
            return hotelName.Trim();
        }

        public static string FixAddress(string address)
        {
            address = address.Replace("(Hiển thị bản đồ)", "").Replace("Địa chỉ :", "");
            address = address.Trim();
            return address;
        }

        public static double FixRate(string rate)
        {
            rate = rate.Replace(",",".").Trim();
            return Convert.ToDouble(rate);
        }

        public static string FixPlaceName(string placeName)
        {
            placeName = placeName.Replace("Khách sạn tại","");
            placeName = placeName.Trim();
            return placeName;
        }

        public static string ConvertToUnSign3(string s)
        {
            Regex regex = new Regex("\\p{IsCombiningDiacriticalMarks}+");
            string temp = s.Normalize(NormalizationForm.FormD);
            return regex.Replace(temp, String.Empty).Replace('\u0111', 'd').Replace('\u0110', 'D');
        }

        public static DateTime FixDateToDateTime(string date)
        {
            string d = date.Substring(3, 2) + "/" + date.Substring(0, 2) + "/" + date.Substring(6, 4);
            return Convert.ToDateTime(d);
        }

        public static string FixDateToString(string date)
        {
           
            return (date.Substring(3, 2) + "/" + date.Substring(0, 2) + "/" + date.Substring(6, 4));
        }

        public static string MakeImageName(string hotelName, string websiteName, int i)
        {
            string name = StringService.ConvertToUnSign3(FixHotelName(hotelName)).Replace(" ", "").ToLower() + websiteName +i.ToString() + ".jpg";
            return name;
        }

    }
}
