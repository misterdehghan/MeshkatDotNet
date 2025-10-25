using EndPoint.Site.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EndPoint.Site.Useful.Static
{
    public static class StaticList
    {
        public static List<KeyValuePair<string, int>> listeDarajeh = new List<KeyValuePair<string, int>>() {
                new KeyValuePair<string, int>("گروهبان سوم", 5),
                new KeyValuePair<string, int>("گروهبان دوم", 6),
                new KeyValuePair<string, int>("گروهبان یکم", 7),
                new KeyValuePair<string, int>("استوار دوم", 8),
                new KeyValuePair<string, int>("استوار یکم", 9),
                new KeyValuePair<string, int>("ستون سوم", 10),
                new KeyValuePair<string, int>("ستوان دوم", 11),
                new KeyValuePair<string, int>("ستوان یکم", 12),
                new KeyValuePair<string, int>("سروان", 13),
                new KeyValuePair<string, int>("سرگرد", 14),
                new KeyValuePair<string, int>("سرهنگ دوم", 15),
                new KeyValuePair<string, int>("سرهنگ", 16),
                new KeyValuePair<string, int>("سرتیپ دوم", 17),
                new KeyValuePair<string, int>("سرتیپ", 18),
                new KeyValuePair<string, int>("روحانی", 20)

            };
        public static Listoption listObjDarajeh()
        {
            Listoption result = new Listoption();
            result.lstoption.Add(new option("گروهبان سوم", 5));
            result.lstoption.Add(new option("گروهبان دوم", 6));
            result.lstoption.Add(new option("گروهبان یکم", 7));
            result.lstoption.Add(new option("استوار دوم", 8));
            result.lstoption.Add(new option("استوار یکم", 9));
            result.lstoption.Add(new option("ستون سوم", 10));
            result.lstoption.Add(new option("ستوان دوم", 11));
            result.lstoption.Add(new option("ستوان یکم", 12));
            result.lstoption.Add(new option("سروان", 13));
            result.lstoption.Add(new option("سرگرد", 14));
            result.lstoption.Add(new option("سرهنگ دوم", 15));
            result.lstoption.Add(new option("سرهنگ", 16));
            result.lstoption.Add(new option("سرتیپ دوم", 17));
            result.lstoption.Add(new option("سرتیپ", 18));
            result.lstoption.Add(new option("روحانی", 20));
            return result;
        }
        public static Listoption listObjRotbeh() {
            Listoption result = new Listoption();
            result.lstoption.Add(new option("کارمند رتبه 5", 5));
            result.lstoption.Add(new option("کارمند رتبه 6", 6));
            result.lstoption.Add(new option("کارمند رتبه 7", 7));
            result.lstoption.Add(new option("کارمند رتبه 8", 8));
            result.lstoption.Add(new option("کارمند رتبه 9", 9));
            result.lstoption.Add(new option("کارمند رتبه 10", 10));
            result.lstoption.Add(new option("کارمند رتبه 11", 11));
            result.lstoption.Add(new option("کارمند رتبه 12", 12));
            result.lstoption.Add(new option("کارمند رتبه 13", 13));
            result.lstoption.Add(new option("کارمند رتبه 14", 14));
            result.lstoption.Add(new option("کارمند رتبه 15", 15));
            result.lstoption.Add(new option ("کارمند رتبه 16", 16));
            result.lstoption.Add(new option("کارمند رتبه 17", 17));
            result.lstoption.Add(new option("کارمند رتبه 18", 18));
            return result;
            }
        public static Listoption listObjRotbehRoohani()
        {
            Listoption result = new Listoption();
            result.lstoption.Add(new option("روحانی رتبه 5", 5));
            result.lstoption.Add(new option("روحانی رتبه 6", 6));
            result.lstoption.Add(new option("روحانی رتبه 7", 7));
            result.lstoption.Add(new option("روحانی رتبه 8", 8));
            result.lstoption.Add(new option("روحانی رتبه 9", 9));
            result.lstoption.Add(new option("روحانی رتبه 10", 10));
            result.lstoption.Add(new option("روحانی رتبه 11", 11));
            result.lstoption.Add(new option("روحانی رتبه 12", 12));
            result.lstoption.Add(new option("روحانی رتبه 13", 13));
            result.lstoption.Add(new option("روحانی رتبه 14", 14));
            result.lstoption.Add(new option("روحانی رتبه 15", 15));
            result.lstoption.Add(new option("روحانی رتبه 16", 16));
            result.lstoption.Add(new option("روحانی رتبه 17", 17));
            result.lstoption.Add(new option("روحانی رتبه 18", 18));
            result.lstoption.Add(new option("روحانی رتبه 19", 19));
            return result;
        }
        public static List<optionAllDarajeh> listObjAllDarajeh()
        {
            List<optionAllDarajeh> result = new List<optionAllDarajeh>();
            result.Add(new optionAllDarajeh("گروهبان سوم", 5 ,1));
            result.Add(new optionAllDarajeh("گروهبان دوم", 6, 1));
            result.Add(new optionAllDarajeh("گروهبان یکم", 7, 1));
            result.Add(new optionAllDarajeh("استوار دوم", 8, 1));
            result.Add(new optionAllDarajeh("استوار یکم", 9, 1));
            result.Add(new optionAllDarajeh("ستون سوم", 10, 1));
            result.Add(new optionAllDarajeh("ستوان دوم", 11, 1));
            result.Add(new optionAllDarajeh("ستوان یکم", 12, 1));
            result.Add(new optionAllDarajeh("سروان", 13, 1));
            result.Add(new optionAllDarajeh("سرگرد", 14, 1));
            result.Add(new optionAllDarajeh("سرهنگ دوم", 15, 1));
            result.Add(new optionAllDarajeh("سرهنگ", 16, 1));
            result.Add(new optionAllDarajeh("سرتیپ دوم", 17, 1));
            result.Add(new optionAllDarajeh("سرتیپ", 18, 1));
            result.Add(new optionAllDarajeh("روحانی رتبه 5", 5 ,2));
            result.Add(new optionAllDarajeh("روحانی رتبه 6", 6, 2));
            result.Add(new optionAllDarajeh("روحانی رتبه 7", 7, 2));
            result.Add(new optionAllDarajeh("روحانی رتبه 8", 8, 2));
            result.Add(new optionAllDarajeh("روحانی رتبه 9", 9, 2));
            result.Add(new optionAllDarajeh("روحانی رتبه 10", 10, 2));
            result.Add(new optionAllDarajeh("روحانی رتبه 11", 11, 2));
            result.Add(new optionAllDarajeh("روحانی رتبه 12", 12, 2));
            result.Add(new optionAllDarajeh("روحانی رتبه 13", 13, 2));
            result.Add(new optionAllDarajeh("روحانی رتبه 14", 14, 2));
            result.Add(new optionAllDarajeh("روحانی رتبه 15", 15, 2));
            result.Add(new optionAllDarajeh("روحانی رتبه 16", 16, 2));
            result.Add(new optionAllDarajeh("روحانی رتبه 17", 17, 2));
            result.Add(new optionAllDarajeh("روحانی رتبه 18", 18, 2));
            result.Add(new optionAllDarajeh("روحانی رتبه 19", 19, 2));
            result.Add(new optionAllDarajeh("کارمند رتبه 5", 5 ,0));
            result.Add(new optionAllDarajeh("کارمند رتبه 6", 6, 0));
            result.Add(new optionAllDarajeh("کارمند رتبه 7", 7, 0));
            result.Add(new optionAllDarajeh("کارمند رتبه 8", 8, 0));
            result.Add(new optionAllDarajeh("کارمند رتبه 9", 9, 0));
            result.Add(new optionAllDarajeh("کارمند رتبه 10", 10, 0));
            result.Add(new optionAllDarajeh("کارمند رتبه 11", 11, 0));
            result.Add(new optionAllDarajeh("کارمند رتبه 12", 12, 0));
            result.Add(new optionAllDarajeh("کارمند رتبه 13", 13, 0));
            result.Add(new optionAllDarajeh("کارمند رتبه 14", 14, 0));
            result.Add(new optionAllDarajeh("کارمند رتبه 15", 15, 0));
            result.Add(new optionAllDarajeh("کارمند رتبه 16", 16, 0));
            result.Add(new optionAllDarajeh("کارمند رتبه 17", 17, 0));
            result.Add(new optionAllDarajeh("کارمند رتبه 18", 18, 0));
            return result; 
        }

        public static List<KeyValuePair<string, int>> listTypeDarajeh = new List<KeyValuePair<string, int>>() {
                new KeyValuePair<string, int>("نظامی", 1),
                new KeyValuePair<string, int>("روحانی", 2),
                new KeyValuePair<string, int>("کارمند", 0)
        };
      
    }
}
