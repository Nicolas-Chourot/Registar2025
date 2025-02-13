using JSON_DAL;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JsonDemo.Models
{

    public sealed class NextSession
    {
        public const int January = 1; // month limit for winter registrations
        public const int August = 8;  // month limit for automn registrations

        private static readonly NextSession instance = new NextSession();
        public static NextSession Instance
        {
            get
            {
                return instance;
            }
        }

        static public List<int> ValidSessions
        {
            get
            {
                List<int> result = new List<int>();
                if (DB.CurrentDate.Month > August)
                {
                    result.Add(2);
                    result.Add(4);
                    result.Add(6);
                }
                if (DB.CurrentDate.Month > January)
                {
                    result.Add(1);
                    result.Add(3);
                    result.Add(5);
                }
                return result;
            }
        }
        static public int Year
        {
            get
            {
                int value = DB.CurrentDate.Year;
                if (DB.CurrentDate.Month > August) value++;
                return value;
            }
        }
        static public string Caption
        {
            get
            {
                return "session " + (ValidSessions.Contains(1) ? " d'automne " : " d'hiver ") + Year;
            }
        }
    }
}