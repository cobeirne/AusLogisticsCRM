using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AusLogisticsWebsite.Models
{
    /// <summary>
    /// Project:    SIT322 Distributed Systems - Assignmnet 1
    /// Written By: Chris O'Beirne - Student #211347444
    /// Date:       27/03/16
    /// </summary>

    public class PublicHoliday
    {
        public string HolidayName { get; set; }

        public int HolidayDate { get; set; }

        public int HolidayMonth { get; set; }

        public bool TasHoliday { get; set; }

        public bool VicHoliday { get; set; }

        public bool NswHoliday { get; set; }

        public bool ActHoliday { get; set; }

        public bool QldHoliday { get; set; }

        public bool NtHoliday { get; set; }

        public bool SaHoliday { get; set; }

        public bool WaHoliday { get; set; }

        Dictionary<string, int> Months { get; set; }

        public PublicHoliday(string holidayName, string holidayDate, string tasHoliday, string vicHoliday,
            string nswHoliday, string actHoliday, string qldHoliday, string ntHoliday, string saHoliday,
            string waHoliday)
        {
            this.HolidayName = holidayName;

            BuildMonthCollection();
            SplitDate(holidayDate);

            this.TasHoliday = tasHoliday.ToUpper() == "YES" ? true : false;
            this.VicHoliday = vicHoliday.ToUpper() == "YES" ? true : false; 
            this.NswHoliday = nswHoliday.ToUpper() == "YES" ? true : false; 
            this.ActHoliday = actHoliday.ToUpper() == "YES" ? true : false; 
            this.QldHoliday = qldHoliday.ToUpper() == "YES" ? true : false; 
            this.NtHoliday = ntHoliday.ToUpper() == "YES" ? true : false; 
            this.SaHoliday = saHoliday.ToUpper() == "YES" ? true : false; 
            this.WaHoliday = waHoliday.ToUpper() == "YES" ? true : false; 
        }

        public void SplitDate(string holidayDate)
        {
            string[] split = holidayDate.Split('-');
            this.HolidayDate = Convert.ToInt32(split[0]);
            this.HolidayMonth = Months[split[1].ToUpper()];
        }

        private void BuildMonthCollection()
        {
            this.Months = new Dictionary<string, int>();
            this.Months.Add("JAN", 1);
            this.Months.Add("FEB", 2);
            this.Months.Add("MAR", 3);
            this.Months.Add("APR", 4);
            this.Months.Add("MAY", 5);
            this.Months.Add("JUN", 6);
            this.Months.Add("JUL", 7);
            this.Months.Add("AUG", 8);
            this.Months.Add("SEP", 9);
            this.Months.Add("OCT", 10);
            this.Months.Add("NOV", 11);
            this.Months.Add("DEC", 12);
        }
    }
}