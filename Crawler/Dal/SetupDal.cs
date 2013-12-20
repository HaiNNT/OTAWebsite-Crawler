using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IdealHotelMVC4.Models;

namespace ClassLibrary1.Dal
{
    public class SetupDal
    {
        GetDataSetupDBEntities db = new GetDataSetupDBEntities();

        /////////////////
        //Crawler Setup//
        /////////////////
        public List<CrawlerSetup> GetCrawlerSetup()
        {
            var cs = (from b in db.CrawlerSetups
                      select b).ToList<CrawlerSetup>();
            return cs;
        }

        public bool InsertCrawlerSetup(CrawlerSetup crawler)
        {
            try
            {
                if (!IsAvailableCrawlerSetup(crawler))
                {
                    db.CrawlerSetups.Add(crawler);
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

        public bool IsAvailableCrawlerSetup(CrawlerSetup crawler)
        {
            var sc = (from b in db.CrawlerSetups
                      where b.CrawlerName == crawler.CrawlerName
                      select b).SingleOrDefault();
            if (sc == null)
                return false;
            return true;
        }

        /////////////////
        //    Regex    //
        /////////////////

        public List<Regex> GetRegex(string crawlerName, string regexName)
        {
            var rg = (from b in db.Regexes
                      where b.CrawlerName == crawlerName && b.RegexName == regexName
                      select b).ToList<Regex>();
            return rg;
        }

        public bool InsertRegex(Regex regex)
        {
            try
            {
                if (!IsAvailableRegex(regex))
                {
                    db.Regexes.Add(regex);
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

        public bool IsAvailableRegex(Regex regex)
        {
            var sc = (from b in db.Regexes
                      where b.Regex1 == regex.Regex1
                      select b).SingleOrDefault();
            if (sc == null)
                return false;
            return true;
        }


        /////////////////
        //  HotelUrl   //
        /////////////////

        public List<HotelUrl> GetHotelUrl(string crawlerName)
        {
            var hu = (from b in db.HotelUrls
                      where b.CrawlerName == crawlerName
                      select b).ToList<HotelUrl>();
            return hu;
        }

        public bool InsertHotelUrl(HotelUrl hotelUrl)
        {
            try
            {
                if (!IsAvailableHotelUrl(hotelUrl))
                {
                    db.HotelUrls.Add(hotelUrl);
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

        public bool IsAvailableHotelUrl(HotelUrl hotelUrl)
        {
            var hu = (from b in db.HotelUrls
                      where b.Url == hotelUrl.Url
                      select b).SingleOrDefault();
            if (hu == null)
                return false;
            return true;
        }

        /////////////////
        //    Parser   //
        /////////////////

        public ParserSetup GetParserSetup(string crawlerName, string parserName)
        {
            var parserSetup = (from b in db.ParserSetups
                               where b.CrawlerName == crawlerName && b.ParserName == parserName
                               select b).SingleOrDefault();
            return parserSetup;
        }

        public bool InsertParserSetup(ParserSetup parserSetup)
        {
            try
            {
                if (!IsAvailableParserSetup(parserSetup))
                {
                    db.ParserSetups.Add(parserSetup);
                    db.SaveChanges();
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
            return false;
        }


        public bool IsAvailableParserSetup(ParserSetup parserSetup)
        {
            var parserSetupDatabase = (from b in db.ParserSetups
                                       where b.ParserName == parserSetup.ParserName && b.CrawlerName == parserSetup.CrawlerName
                                       select b).SingleOrDefault();
            if(parserSetupDatabase != null)
                return true;
            return false;
        }


        /////////////////
        //    Xpath    //
        /////////////////

        public List<Xpath> GetXathsByParserId(int parserId)
        {
            var xpaths = (from b in db.Xpaths
                          where b.ParserId == parserId
                          select b).ToList<Xpath>();
            return xpaths;
        }

        public bool InsertXpaths(List<Xpath> xpaths)
        {
            try
            {
                foreach (var item in xpaths)
                {
                    if (!IsAvailableXpath(item))
                        db.Xpaths.Add(item);
                }
                db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool IsAvailableXpath(Xpath xpath)
        {
            var databseXpath = (from b in db.Xpaths
                                where b.Xpath1 == xpath.Xpath1
                                select b).SingleOrDefault();
            if (databseXpath == null)
                return false;
            return true;
        }

        /////////////////
        //   FormData  //
        /////////////////

        public List<FormData> GetFormDataByParserId(int parserId)
        {
            var formDatas = (from b in db.FormDatas
                             where b.ParserId == parserId
                             select b).ToList<FormData>();
            return formDatas;
        }

        public bool InsertFormDatas(List<FormData> formDatas)
        {
            try
            {
                foreach (var item in formDatas)
                {
                    if (!IsAvailableFormData(item))
                        db.FormDatas.Add(item);
                }
                db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool IsAvailableFormData(FormData formData)
        {
            var databseformData = (from b in db.FormDatas
                                   where b.FormDataName == formData.FormDataName && b.ParserId == formData.ParserId
                                   select b).SingleOrDefault();
            if (databseformData == null)
                return false;
            return true;
        }
    }
}
