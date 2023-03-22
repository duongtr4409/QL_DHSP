using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text.RegularExpressions;
using System.Web.UI;
using System.Text;
using System.Security.Cryptography;
using System.IO;
using System.Drawing.Imaging;
using System.Web.UI.WebControls;
using System.Web.Mvc;
using System.Net;
using TEMIS.CMS.Repository;
using TEMIS.Model;
using System.Threading.Tasks;
using TEMIS.CMS.Areas.Admin.Models;
using TEMIS.CMS.Common;
using System.Net.Http;
using Newtonsoft.Json;
using System.Collections;
using System.Globalization;
using System.Diagnostics;
using System.Net.Mail;
using System.Web.Configuration;

namespace TEMIS.CMS.Common
{
    public static class Utility
    {
        public static string Fr_Email = WebConfigurationManager.AppSettings["Fr_Email"].ToString();
        public static string PassEmail = WebConfigurationManager.AppSettings["PassEmail"].ToString();
        public static void SaveLogToFile(string controllerName, string actionName, string createBy, string message, string strPath)
        {
            try
            {
                var strLog = string.Format("{0}: controller {1} action {2} \n Error: {3} \n Authen:{4} ", DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss"),
                    controllerName, actionName, message, createBy);

                if (System.IO.File.Exists(strPath))
                {
                    System.IO.File.AppendAllText(strPath, DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss") + " " + strLog + Environment.NewLine);
                }
                else
                {
                    System.IO.File.Create(strPath).Close();
                    System.IO.File.AppendAllText(strPath, DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss") + " " + strLog + Environment.NewLine);
                }
            }
            catch (Exception ex)
            {

            }
        }

        public static string CurrentDate()
        {
            return DateTime.Now.ToString("HH:mm:ss.ffff dd/MM/yyyy");
            //return string.Format("{dd/MM/yyyy}", DateTime.Now);
        }
        public static int ConvertToDay(int Day, int Month, int Year)
        {
            int songay = 0;
            int Thu = 0;
            int[] a = { 31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31 };

            songay = ((Year - 1) % 7) * 365 + (Year - 1) / 4;
            /* Do so qua lon nen minh lay phan du luon o day
            khong lam sai thuat toan nhe*/
            if (Year % 4 == 0) a[1] = 29;
            for (int i = 0; i < (Month - 1); i++) songay += a[i];
            songay += Day;
            Thu = songay % 7;
            return Thu;
        }
        public static Random random = new Random((int)DateTime.Now.Ticks);
        public static string RandomString(int size)
        {
            StringBuilder builder = new StringBuilder();
            char ch;
            for (int i = 0; i < size; i++)
            {
                ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                builder.Append(ch);
            }

            return builder.ToString();
        }

        public static string RandomStrings(DateTime date, int coso_id)
        {
            string format = "{0}{1}{2}{3}{4}{5}";
            return string.Format(format, date.Year.ToString().Substring(2, 2), date.Month.ToString("00"), date.Day.ToString("00"), date.Hour.ToString("00"), date.Minute.ToString("00"), coso_id.ToString("0000"));
        }
        public static DateTime? ToDataTime(this string obj)
        {
            if (!string.IsNullOrEmpty(obj))
            {
                var date = obj.Split('/');
                if (date != null)
                {
                    if (!string.IsNullOrEmpty(date[0]) && !string.IsNullOrEmpty(date[1]) && !string.IsNullOrEmpty(date[2]))
                    {
                        var day = int.Parse(date[0]).ToString("00");
                        var month = int.Parse(date[1]).ToString("00");
                        var year = int.Parse(date[2]).ToString("0000");
                        return DateTime.ParseExact(string.Format("{0}/{1}/{2}", day, month, year), "dd/MM/yyyy", null);
                    }
                }
                return null;
            }
            else
            {
                return null;
            }
        }
        public static DateTime? ToDateTime(this string obj)
        {
            if (!string.IsNullOrEmpty(obj))
            {
                var date = obj.Split('/');
                if (date != null)
                {
                    if (!string.IsNullOrEmpty(date[0]) && !string.IsNullOrEmpty(date[1]) && !string.IsNullOrEmpty(date[2]))
                    {
                        var day = int.Parse(date[0]).ToString("00");
                        var month = int.Parse(date[1]).ToString("00");
                        var year = int.Parse(date[2]).ToString("0000");
                        return DateTime.ParseExact(string.Format("{0}/{1}/{2}", day, month, year), "dd/MM/yyyy", null);
                    }
                }
                return null;
            }
            else
            {
                return null;
            }
        }
        public static short? ToShortOrNULL(this string obj)
        {
            if (!string.IsNullOrEmpty(obj))
            {
                return short.Parse(obj);
            }
            else
            {
                return null;
            }
        }

        public static int? ToIntOrNULL(this string obj)
        {
            if (!string.IsNullOrEmpty(obj))
            {
                return int.Parse(obj);
            }
            else
            {
                return null;
            }
        }
        public static long? ToLongOrNULL(this string obj)
        {
            if (!string.IsNullOrEmpty(obj))
            {
                return long.Parse(obj);
            }
            else
            {
                return null;
            }
        }
        public static short ToShortOrZero(this string obj)
        {
            if (!string.IsNullOrEmpty(obj))
            {
                return short.Parse(obj);
            }
            else
            {
                return 0;
            }
        }

        public static int ToIntOrZero(this string obj)
        {
            if (!string.IsNullOrEmpty(obj))
            {
                return int.Parse(obj);
            }
            else
            {
                return 0;
            }
        }
        public static double ToDoubleOrZero(this string obj)
        {
            if (!string.IsNullOrEmpty(obj))
            {
                return Convert.ToDouble(obj);
            }
            else
            {
                return 0;
            }
        }
        public static long ToLongOrZero(this string obj)
        {
            if (!string.IsNullOrEmpty(obj))
            {
                return long.Parse(obj);
            }
            else
            {
                return 0;
            }
        }
        public static List<long> ToListLong(this string obj, char split_key)
        {
            List<long> listLong = new List<long>();
            if (!string.IsNullOrEmpty(obj))
            {
                var list = obj.Split(split_key);
                if (list != null)
                {
                    foreach (var item in list)
                    {
                        if (!string.IsNullOrEmpty(item))
                        {
                            listLong.Add(long.Parse(item));
                        }
                    }
                }
            }
            return listLong;
        }
        public static List<int> ToListInt(this string obj, char split_key)
        {
            List<int> listInt = new List<int>();
            if (!string.IsNullOrEmpty(obj))
            {
                var list = obj.Split(split_key);
                if (list != null)
                {
                    foreach (var item in list)
                    {
                        if (!string.IsNullOrEmpty(item))
                        {
                            listInt.Add(int.Parse(item));
                        }
                    }
                }
            }
            return listInt;
        }

        public static List<short> ToListShort(this string obj, char split_key)
        {
            List<short> listInt = new List<short>();
            if (!string.IsNullOrEmpty(obj))
            {
                var list = obj.Split(split_key);
                if (list != null)
                {
                    foreach (var item in list)
                    {
                        if (!string.IsNullOrEmpty(item))
                        {
                            listInt.Add(short.Parse(item));
                        }
                    }
                }
            }
            return listInt;
        }

        public static string CollapseSpaces(this string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                return Regex.Replace(value, @"\s+", " ");
            }
            return string.Empty;
        }

        public static string HtmlEncoder(this string value)
        {
            StringBuilder sb = new StringBuilder(HttpUtility.HtmlEncode(value));
            // Selectively allow  and <i>
            sb.Replace("&lt;b&gt;", "<b>");
            sb.Replace("&lt;/b&gt;", "");
            sb.Replace("&lt;i&gt;", "<i>");
            sb.Replace("&lt;/i&gt;", "");
            return sb.ToString();
        }
        public static T FirstOrDefault<T>(this IEnumerable items) where T : class
        {
            var list = items.OfType<T>();
            if (list != null)
            {
                return list.FirstOrDefault();
            }

            return default(T);
        }
        public static string HtmlDecoder(this string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                return HttpUtility.HtmlDecode(value);
                String myEncodedString;
                // Encode the string.
                myEncodedString = HttpUtility.HtmlEncode(value);

                StringWriter myWriter = new StringWriter();
                // Decode the encoded string.
                HttpUtility.HtmlDecode(myEncodedString, myWriter);
                return myWriter.ToString();
                //return HttpUtility.HtmlDecode(value);
            }
            return value;
        }
        public static int GetIso8601WeekOfYear(DateTime time)
        {
            DayOfWeek day = CultureInfo.InvariantCulture.Calendar.GetDayOfWeek(time);
            if (day >= DayOfWeek.Monday && day <= DayOfWeek.Wednesday)
            {
                time = time.AddDays(3);
            }

            return CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(time, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
        }

        public static DateTime FirstDateOfWeek(int year, int weekOfYear, System.Globalization.CultureInfo ci)
        {
            DateTime jan1 = new DateTime(year, 1, 1);
            int daysOffset = (int)ci.DateTimeFormat.FirstDayOfWeek - (int)jan1.DayOfWeek;
            DateTime firstWeekDay = jan1.AddDays(daysOffset);
            int firstWeek = ci.Calendar.GetWeekOfYear(jan1, ci.DateTimeFormat.CalendarWeekRule, ci.DateTimeFormat.FirstDayOfWeek);
            if (firstWeek <= 1 || firstWeek > 50)
            {
                weekOfYear -= 1;
            }
            return firstWeekDay.AddDays(weekOfYear * 7);
        }
        public static DateTime GetStartOfWeek(int year, int month, int weekofmonth)
        {
            //lấy ngày bắt đầu của tuần trong tháng
            var day = weekofmonth * 7 - 6;
            var StartDate = new DateTime(year, month, day);
            var weekOfYear = GetIso8601WeekOfYear(StartDate);
            return FirstDateOfWeek(year, weekOfYear, CultureInfo.CurrentCulture);
        }

        public static DateTime GetEndOfWeek(DateTime startOfWeek)
        {
            return startOfWeek.AddDays(6);
        }
        private static double DaysInYear(this int iYear)
        {
            var startDate = new DateTime(iYear, 1, 1);
            var enddate = new DateTime(iYear, 12, 31);
            var totalDate = enddate - startDate;
            return totalDate.TotalDays;
        }

        /// <summary>
        /// Trả ra số tuần trong 1 năm
        /// </summary>
        /// <param name="iYear">Năm cần tính số tuần</param>
        /// <returns></returns>
        public static List<int> ListWeekOfYear(int iYear)
        {
            //lấy tổng số ngày trong năm
            double countDays = iYear.DaysInYear();
            List<int> arrWeeks = new List<int>();
            var j = 0;
            for (int i = 1; i <= countDays; i = i + 7)
            {
                j++;
                arrWeeks.Add(j);
            }
            //trả ra danh sách tuần theo năm
            return arrWeeks;
        }

        static readonly string[] Columns = new[] { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z", "AA", "AB", "AC", "AD", "AE", "AF", "AG", "AH", "AI", "AJ", "AK", "AL", "AM", "AN", "AO", "AP", "AQ", "AR", "AS", "AT", "AU", "AV", "AW", "AX", "AY", "AZ", "BA", "BB", "BC", "BD", "BE", "BF", "BG", "BH", "BI", "BJ", "BK", "BL", "BM", "BN", "BO", "BP", "BQ", "BR", "BS", "BT", "BU", "BV", "BW", "BX", "BY", "BZ", "CA", "CB", "CC", "CD", "CE", "CF", "CG", "CH", "CI", "CJ", "CK", "CL", "CM", "CN", "CO", "CP", "CQ", "CR", "CS", "CT", "CU", "CV", "CW", "CX", "CY", "CZ", "DA", "DB", "DC", "DD", "DE", "DF", "DG", "DH", "DI", "DJ", "DK", "DL", "DM", "DN", "DO", "DP", "DQ", "DR", "DS", "DT", "DU", "DV", "DW", "DX", "DY", "DZ" };
        public static string ToIndexToColumn(this int index)
        {
            if (index <= 0)
                throw new IndexOutOfRangeException("index must be a positive number");

            return Columns[index - 1];
        }

        public static string Truncate(string input, int length)
        {
            if (input.Length <= length)
            {
                return input;
            }
            else
            {
                return input.Substring(0, length) + "...";
            }
        }
        public static bool PhoneValidate(string strPhone)
        {
            bool isValid = false;
            string regexPhone = "^(0|84)(9|1)([0-9]{8,9})$";
            isValid = Regex.IsMatch(strPhone, regexPhone);
            return isValid;
        }
        public static bool PhoneValidate(string strPhone, out string cellar)
        {
            bool isValid = false;
            cellar = string.Empty;
            string regexVina = "^(0|84)(91|94|123|125|127|129|124|88)([0-9]{7})$";
            string regexViettel = "^(0|84)(96|97|98|162|163|164|165|166|167|168|169|86)([0-9]{7})$";
            string regexMobi = "^(0|84)(90|93|122|126|128|121|120|89)([0-9]{7})$";
            string regexVietNamMobile = "^(092|0188|0186)([x0-9]{7})$";
            // Check Phone number
            if (!string.IsNullOrEmpty(strPhone))
            {
                if (Regex.IsMatch(strPhone, regexVina))
                {
                    isValid = true;
                    cellar = "Vinaphone";
                }
                else if (Regex.IsMatch(strPhone, regexViettel))
                {
                    isValid = true;
                    cellar = "Viettel";
                }
                else if (Regex.IsMatch(strPhone, regexMobi))
                {
                    isValid = true;
                    cellar = "Mobifone";
                }
                else if (Regex.IsMatch(strPhone, regexVietNamMobile))
                {
                    isValid = true;
                    cellar = "Vietnamobile";
                }
            }
            else
            {
                isValid = false;
                cellar = string.Empty;
            }
            return isValid;
        }
        public static FileResult GetFile(string filePath)
        {
            var dir = @"C:\Users\Data\CardMedia\";
            var path = Path.Combine(dir, filePath);
            return new FileStreamResult(new FileStream(path, FileMode.Open), "image/jpeg");
        }
        public static string CreatePassword(int length)
        {
            const string valid = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            StringBuilder res = new StringBuilder();
            Random rnd = new Random();
            while (0 < length--)
            {
                res.Append(valid[rnd.Next(valid.Length)]);
            }
            return res.ToString();
        }

        public static string CreateTransId(int length)
        {
            const string valid = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            StringBuilder res = new StringBuilder();
            Random rnd = new Random();
            while (0 < length--)
            {
                res.Append(valid[rnd.Next(valid.Length)]);
            }
            return res.ToString();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public static string ConvertToCurrency(double number)
        {
            // number.ToString("N"); //12.345.00
            //return string.Format("{0:N}", number); // 12,345.00
            if (number == 0)
                return "0";
            return (number.ToString("#,###"));
        }
        public static string GetSha256(string text)
        {
            if (text == null)
            {
                return string.Empty;
            }

            byte[] message = System.Text.Encoding.ASCII.GetBytes(text);
            byte[] hashValue = GetSha256(message);

            string hashString = string.Empty;
            foreach (byte x in hashValue)
            {
                hashString += string.Format("{0:x2}", x);
            }

            return hashString;

        }

        private static byte[] GetSha256(byte[] message)
        {
            SHA256Managed hashString = new SHA256Managed();
            return hashString.ComputeHash(message);
        }

        //---------------------------------------------------------------------------------------------------------------------------------------------
        public static string key { get; set; } = "A!9HHhi%XjjYY4YP2@Nob009X";
        //public static string CurrentDate()
        //{
        //    return DateTime.Now.ToString("HH:mm:ss.ffff dd/MM/yyyy");
        //    //return string.Format("{dd/MM/yyyy}", DateTime.Now);
        //}
        //public static int ConvertToDay(int Day, int Month, int Year)
        //{
        //    int songay = 0;
        //    int Thu = 0;
        //    int[] a = { 31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31 };

        //    songay = ((Year - 1) % 7) * 365 + (Year - 1) / 4;
        //    /* Do so qua lon nen minh lay phan du luon o day
        //    khong lam sai thuat toan nhe*/
        //    if (Year % 4 == 0) a[1] = 29;
        //    for (int i = 0; i < (Month - 1); i++) songay += a[i];
        //    songay += Day;
        //    Thu = songay % 7;
        //    return Thu;
        //}
        //public static Random random = new Random((int)DateTime.Now.Ticks);
        //public static string RandomString(int size)
        //{
        //    StringBuilder builder = new StringBuilder();
        //    char ch;
        //    for (int i = 0; i < size; i++)
        //    {
        //        ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
        //        builder.Append(ch);
        //    }

        //    return builder.ToString();
        //}

        //public static string RandomStrings(DateTime date, int coso_id)
        //{
        //    string format = "{0}{1}{2}{3}{4}{5}";
        //    return string.Format(format, date.Year.ToString().Substring(2, 2), date.Month.ToString("00"), date.Day.ToString("00"), date.Hour.ToString("00"), date.Minute.ToString("00"), coso_id.ToString("0000"));
        //}
        //public static DateTime? ToDataTime(this string obj)
        //{
        //    if (!string.IsNullOrEmpty(obj))
        //    {
        //        var date = obj.Split('/');
        //        if (date != null)
        //        {
        //            if (!string.IsNullOrEmpty(date[0]) && !string.IsNullOrEmpty(date[1]) && !string.IsNullOrEmpty(date[2]))
        //            {
        //                var day = int.Parse(date[0]).ToString("00");
        //                var month = int.Parse(date[1]).ToString("00");
        //                var year = int.Parse(date[2]).ToString("0000");
        //                return DateTime.ParseExact(string.Format("{0}/{1}/{2}", day, month, year), "dd/MM/yyyy", null);
        //            }
        //        }
        //        return null;
        //    }
        //    else
        //    {
        //        return null;
        //    }
        //}
        //public static DateTime? ToDateTime(this string obj)
        //{
        //    if (!string.IsNullOrEmpty(obj))
        //    {
        //        var date = obj.Split('/');
        //        if (date != null)
        //        {
        //            if (!string.IsNullOrEmpty(date[0]) && !string.IsNullOrEmpty(date[1]) && !string.IsNullOrEmpty(date[2]))
        //            {
        //                var day = int.Parse(date[0]).ToString("00");
        //                var month = int.Parse(date[1]).ToString("00");
        //                var year = int.Parse(date[2]).ToString("0000");
        //                return DateTime.ParseExact(string.Format("{0}/{1}/{2}", day, month, year), "dd/MM/yyyy", null);
        //            }
        //        }
        //        return null;
        //    }
        //    else
        //    {
        //        return null;
        //    }
        //}
        //public static short? ToShortOrNULL(this string obj)
        //{
        //    if (!string.IsNullOrEmpty(obj))
        //    {
        //        return short.Parse(obj);
        //    }
        //    else
        //    {
        //        return null;
        //    }
        //}

        //public static int? ToIntOrNULL(this string obj)
        //{
        //    if (!string.IsNullOrEmpty(obj))
        //    {
        //        return int.Parse(obj);
        //    }
        //    else
        //    {
        //        return null;
        //    }
        //}
        //public static long? ToLongOrNULL(this string obj)
        //{
        //    if (!string.IsNullOrEmpty(obj))
        //    {
        //        return long.Parse(obj);
        //    }
        //    else
        //    {
        //        return null;
        //    }
        //}
        //public static short ToShortOrZero(this string obj)
        //{
        //    if (!string.IsNullOrEmpty(obj))
        //    {
        //        return short.Parse(obj);
        //    }
        //    else
        //    {
        //        return 0;
        //    }
        //}

        //public static int ToIntOrZero(this string obj)
        //{
        //    if (!string.IsNullOrEmpty(obj))
        //    {
        //        return int.Parse(obj);
        //    }
        //    else
        //    {
        //        return 0;
        //    }
        //}
        //public static double ToDoubleOrZero(this string obj)
        //{
        //    if (!string.IsNullOrEmpty(obj))
        //    {
        //        return Convert.ToDouble(obj);
        //    }
        //    else
        //    {
        //        return 0;
        //    }
        //}
        //public static long ToLongOrZero(this string obj)
        //{
        //    if (!string.IsNullOrEmpty(obj))
        //    {
        //        return long.Parse(obj);
        //    }
        //    else
        //    {
        //        return 0;
        //    }
        //}
        //public static List<long> ToListLong(this string obj, char split_key)
        //{
        //    List<long> listLong = new List<long>();
        //    if (!string.IsNullOrEmpty(obj))
        //    {
        //        var list = obj.Split(split_key);
        //        if (list != null)
        //        {
        //            foreach (var item in list)
        //            {
        //                if (!string.IsNullOrEmpty(item))
        //                {
        //                    listLong.Add(long.Parse(item));
        //                }
        //            }
        //        }
        //    }
        //    return listLong;
        //}
        //public static List<int> ToListInt(this string obj, char split_key)
        //{
        //    List<int> listInt = new List<int>();
        //    if (!string.IsNullOrEmpty(obj))
        //    {
        //        var list = obj.Split(split_key);
        //        if (list != null)
        //        {
        //            foreach (var item in list)
        //            {
        //                if (!string.IsNullOrEmpty(item))
        //                {
        //                    listInt.Add(int.Parse(item));
        //                }
        //            }
        //        }
        //    }
        //    return listInt;
        //}

        //public static List<short> ToListShort(this string obj, char split_key)
        //{
        //    List<short> listInt = new List<short>();
        //    if (!string.IsNullOrEmpty(obj))
        //    {
        //        var list = obj.Split(split_key);
        //        if (list != null)
        //        {
        //            foreach (var item in list)
        //            {
        //                if (!string.IsNullOrEmpty(item))
        //                {
        //                    listInt.Add(short.Parse(item));
        //                }
        //            }
        //        }
        //    }
        //    return listInt;
        //}

        //public static string CollapseSpaces(this string value)
        //{
        //    if (!string.IsNullOrEmpty(value))
        //    {
        //        return Regex.Replace(value, @"\s+", " ");
        //    }
        //    return string.Empty;
        //}

        //public static string HtmlEncoder(this string value)
        //{
        //    StringBuilder sb = new StringBuilder(HttpUtility.HtmlEncode(value));
        //    // Selectively allow  and <i>
        //    sb.Replace("&lt;b&gt;", "<b>");
        //    sb.Replace("&lt;/b&gt;", "");
        //    sb.Replace("&lt;i&gt;", "<i>");
        //    sb.Replace("&lt;/i&gt;", "");
        //    return sb.ToString();
        //}
        //public static T FirstOrDefault<T>(this IEnumerable items) where T : class
        //{
        //    var list = items.OfType<T>();
        //    if (list != null)
        //    {
        //        return list.FirstOrDefault();
        //    }

        //    return default(T);
        //}
        //public static string HtmlDecoder(this string value)
        //{
        //    if (!string.IsNullOrEmpty(value))
        //    {
        //        return HttpUtility.HtmlDecode(value);
        //        String myEncodedString;
        //        // Encode the string.
        //        myEncodedString = HttpUtility.HtmlEncode(value);

        //        StringWriter myWriter = new StringWriter();
        //        // Decode the encoded string.
        //        HttpUtility.HtmlDecode(myEncodedString, myWriter);
        //        return myWriter.ToString();
        //        //return HttpUtility.HtmlDecode(value);
        //    }
        //    return value;
        //}
        //public static int GetIso8601WeekOfYear(DateTime time)
        //{
        //    DayOfWeek day = CultureInfo.InvariantCulture.Calendar.GetDayOfWeek(time);
        //    if (day >= DayOfWeek.Monday && day <= DayOfWeek.Wednesday)
        //    {
        //        time = time.AddDays(3);
        //    }

        //    return CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(time, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
        //}

        //public static DateTime FirstDateOfWeek(int year, int weekOfYear, System.Globalization.CultureInfo ci)
        //{
        //    DateTime jan1 = new DateTime(year, 1, 1);
        //    int daysOffset = (int)ci.DateTimeFormat.FirstDayOfWeek - (int)jan1.DayOfWeek;
        //    DateTime firstWeekDay = jan1.AddDays(daysOffset);
        //    int firstWeek = ci.Calendar.GetWeekOfYear(jan1, ci.DateTimeFormat.CalendarWeekRule, ci.DateTimeFormat.FirstDayOfWeek);
        //    if (firstWeek <= 1 || firstWeek > 50)
        //    {
        //        weekOfYear -= 1;
        //    }
        //    return firstWeekDay.AddDays(weekOfYear * 7);
        //}
        //public static DateTime GetStartOfWeek(int year, int month, int weekofmonth)
        //{
        //    //lấy ngày bắt đầu của tuần trong tháng
        //    var day = weekofmonth * 7 - 6;
        //    var StartDate = new DateTime(year, month, day);
        //    var weekOfYear = GetIso8601WeekOfYear(StartDate);
        //    return FirstDateOfWeek(year, weekOfYear, CultureInfo.CurrentCulture);
        //}

        //public static DateTime GetEndOfWeek(DateTime startOfWeek)
        //{
        //    return startOfWeek.AddDays(6);
        //}
        //private static double DaysInYear(this int iYear)
        //{
        //    var startDate = new DateTime(iYear, 1, 1);
        //    var enddate = new DateTime(iYear, 12, 31);
        //    var totalDate = enddate - startDate;
        //    return totalDate.TotalDays;
        //}

        ///// <summary>
        ///// Trả ra số tuần trong 1 năm
        ///// </summary>
        ///// <param name="iYear">Năm cần tính số tuần</param>
        ///// <returns></returns>
        //public static List<int> ListWeekOfYear(int iYear)
        //{
        //    //lấy tổng số ngày trong năm
        //    double countDays = iYear.DaysInYear();
        //    List<int> arrWeeks = new List<int>();
        //    var j = 0;
        //    for (int i = 1; i <= countDays; i = i + 7)
        //    {
        //        j++;
        //        arrWeeks.Add(j);
        //    }
        //    //trả ra danh sách tuần theo năm
        //    return arrWeeks;
        //}

        //static readonly string[] Columns = new[] { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z", "AA", "AB", "AC", "AD", "AE", "AF", "AG", "AH", "AI", "AJ", "AK", "AL", "AM", "AN", "AO", "AP", "AQ", "AR", "AS", "AT", "AU", "AV", "AW", "AX", "AY", "AZ", "BA", "BB", "BC", "BD", "BE", "BF", "BG", "BH", "BI", "BJ", "BK", "BL", "BM", "BN", "BO", "BP", "BQ", "BR", "BS", "BT", "BU", "BV", "BW", "BX", "BY", "BZ", "CA", "CB", "CC", "CD", "CE", "CF", "CG", "CH", "CI", "CJ", "CK", "CL", "CM", "CN", "CO", "CP", "CQ", "CR", "CS", "CT", "CU", "CV", "CW", "CX", "CY", "CZ", "DA", "DB", "DC", "DD", "DE", "DF", "DG", "DH", "DI", "DJ", "DK", "DL", "DM", "DN", "DO", "DP", "DQ", "DR", "DS", "DT", "DU", "DV", "DW", "DX", "DY", "DZ" };
        //public static string ToIndexToColumn(this int index)
        //{
        //    if (index <= 0)
        //        throw new IndexOutOfRangeException("index must be a positive number");

        //    return Columns[index - 1];
        //}

        //public static string Truncate(string input, int length)
        //{
        //    if (input.Length <= length)
        //    {
        //        return input;
        //    }
        //    else
        //    {
        //        return input.Substring(0, length) + "...";
        //    }
        //}
        //public static bool PhoneValidate(string strPhone)
        //{
        //    bool isValid = false;
        //    string regexPhone = "^(0|84)(9|1)([0-9]{8,9})$";
        //    isValid = Regex.IsMatch(strPhone, regexPhone);
        //    return isValid;
        //}
        //public static bool PhoneValidate(string strPhone, out string cellar)
        //{
        //    bool isValid = false;
        //    cellar = string.Empty;
        //    string regexVina = "^(0|84)(91|94|123|125|127|129|124|88)([0-9]{7})$";
        //    string regexViettel = "^(0|84)(96|97|98|162|163|164|165|166|167|168|169|86)([0-9]{7})$";
        //    string regexMobi = "^(0|84)(90|93|122|126|128|121|120|89)([0-9]{7})$";
        //    string regexVietNamMobile = "^(092|0188|0186)([x0-9]{7})$";
        //    // Check Phone number
        //    if (!string.IsNullOrEmpty(strPhone))
        //    {
        //        if (Regex.IsMatch(strPhone, regexVina))
        //        {
        //            isValid = true;
        //            cellar = "Vinaphone";
        //        }
        //        else if (Regex.IsMatch(strPhone, regexViettel))
        //        {
        //            isValid = true;
        //            cellar = "Viettel";
        //        }
        //        else if (Regex.IsMatch(strPhone, regexMobi))
        //        {
        //            isValid = true;
        //            cellar = "Mobifone";
        //        }
        //        else if (Regex.IsMatch(strPhone, regexVietNamMobile))
        //        {
        //            isValid = true;
        //            cellar = "Vietnamobile";
        //        }
        //    }
        //    else
        //    {
        //        isValid = false;
        //        cellar = string.Empty;
        //    }
        //    return isValid;
        //}
        //public static FileResult GetFile(string filePath)
        //{
        //    var dir = @"C:\Users\Data\CardMedia\";
        //    var path = Path.Combine(dir, filePath);
        //    return new FileStreamResult(new FileStream(path, FileMode.Open), "image/jpeg");
        //}
        //public static string CreatePassword(int length)
        //{
        //    const string valid = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
        //    StringBuilder res = new StringBuilder();
        //    Random rnd = new Random();
        //    while (0 < length--)
        //    {
        //        res.Append(valid[rnd.Next(valid.Length)]);
        //    }
        //    return res.ToString();
        //}

        //public static string CreateTransId(int length)
        //{
        //    const string valid = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
        //    StringBuilder res = new StringBuilder();
        //    Random rnd = new Random();
        //    while (0 < length--)
        //    {
        //        res.Append(valid[rnd.Next(valid.Length)]);
        //    }
        //    return res.ToString();
        //}
        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="number"></param>
        ///// <returns></returns>
        //public static string ConvertToCurrency(double number)
        //{
        //    // number.ToString("N"); //12.345.00
        //    //return string.Format("{0:N}", number); // 12,345.00
        //    return (number.ToString("#,###"));
        //}
        public static string Encrypt(string toEncrypt, bool useHashing)
        {
            try
            {
                byte[] keyArray;
                byte[] toEncryptArray = UTF8Encoding.UTF8.GetBytes(toEncrypt);

                //System.Windows.Forms.MessageBox.Show(key);
                //If hashing use get hashcode regards to your key
                if (useHashing)
                {
                    MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
                    keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
                    //Always release the resources and flush data
                    // of the Cryptographic service provide. Best Practice

                    hashmd5.Clear();
                }
                else
                    keyArray = UTF8Encoding.UTF8.GetBytes(key);

                TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
                //set the secret key for the tripleDES algorithm
                tdes.Key = keyArray;
                //mode of operation. there are other 4 modes.
                //We choose ECB(Electronic code Book)
                tdes.Mode = CipherMode.ECB;
                //padding mode(if any extra byte added)

                tdes.Padding = PaddingMode.PKCS7;

                ICryptoTransform cTransform = tdes.CreateEncryptor();
                //transform the specified region of bytes array to resultArray
                byte[] resultArray =
                  cTransform.TransformFinalBlock(toEncryptArray, 0,
                  toEncryptArray.Length);
                //Release resources held by TripleDes Encryptor
                tdes.Clear();
                //Return the encrypted data into unreadable string format
                return Convert.ToBase64String(resultArray, 0, resultArray.Length);
            }
            catch (Exception)
            {
                return toEncrypt;
            }

        }

        public static string Decrypt(string cipherString, bool useHashing)
        {
            byte[] keyArray;
            //get the byte code of the string

            byte[] toEncryptArray = Convert.FromBase64String(cipherString);

            if (useHashing)
            {
                //if hashing was used get the hash code with regards to your key
                MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
                keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
                //release any resource held by the MD5CryptoServiceProvider

                hashmd5.Clear();
            }
            else
            {
                //if hashing was not implemented get the byte code of the key
                keyArray = UTF8Encoding.UTF8.GetBytes(key);
            }

            TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
            //set the secret key for the tripleDES algorithm
            tdes.Key = keyArray;
            //mode of operation. there are other 4 modes. 
            //We choose ECB(Electronic code Book)

            tdes.Mode = CipherMode.ECB;
            //padding mode(if any extra byte added)
            tdes.Padding = PaddingMode.PKCS7;

            ICryptoTransform cTransform = tdes.CreateDecryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(

                                 toEncryptArray, 0, toEncryptArray.Length);
            //Release resources held by TripleDes Encryptor                
            tdes.Clear();
            //return the Clear decrypted TEXT
            return UTF8Encoding.UTF8.GetString(resultArray);
        }

        public static string RemoveUnicodeCharactor(string text)
        {
            string result = "";
            try
            {
                for (int i = 33; i < 48; i++)
                {
                    if (i != 44)
                    {
                        text = text.Replace(((char)i).ToString(), "");
                    }
                }
                for (int i = 58; i < 65; i++)
                {
                    text = text.Replace(((char)i).ToString(), "");
                }
                for (int i = 91; i < 97; i++)
                {
                    text = text.Replace(((char)i).ToString(), "");
                }
                for (int i = 123; i < 127; i++)
                {
                    text = text.Replace(((char)i).ToString(), "");
                }
                Regex regex = new Regex(@"\p{IsCombiningDiacriticalMarks}+");
                string strFormD = text.Normalize(System.Text.NormalizationForm.FormD);
                result = regex.Replace(strFormD, String.Empty).Replace('\u0111', 'd').Replace('\u0110', 'D').Replace("(", "").Replace(")", "").Replace("\"", "").Replace("/", "").Replace("\\", "").Replace("'", "").Replace("“", "").Replace("”", "");
            }
            catch (Exception)
            {

            }
            return result;
        }
        public static SelectList StatusToList()
        {
            var status = new SelectListItem() { Text = "Phê duyệt", Value = ((int)CMS_STATUS.ACTIVE).ToString() };
            var lst = new List<SelectListItem>();
            lst.Add(status);
            status = new SelectListItem() { Text = "Chờ phê duyệt", Value = ((int)CMS_STATUS.PENDING).ToString() };
            lst.Add(status);
            status = new SelectListItem() { Text = "Đã xóa", Value = ((int)CMS_STATUS.DELETED).ToString() };
            lst.Add(status);
            return new SelectList(lst, "Value", "Text");
        }
        public static string ImagePath(string fileName, int type)
        {
            if (type == (int)CMS_IMAGE_TYPE.ARTICLE_TYPE)
            {
                return string.Format("/Uploads/TinTuc/{0}", fileName);
            }
            else
            {
                return string.Format("/Uploads/DuAn/{0}", fileName);
            }
        }
        public static string StatusToString(int status)
        {
            var strResult = string.Empty;
            switch (status)
            {
                case (int)CMS_STATUS.ACTIVE:
                    {
                        strResult = "Đã duyệt";
                        break;
                    }
                case (int)CMS_STATUS.PENDING:
                    {
                        strResult = "Chờ phê duyệt";
                        break;
                    }
                case (int)CMS_STATUS.DELETED:
                    {
                        strResult = "Đã xóa";
                        break;
                    }
                default: break;
            }
            return strResult;
        }

        public static string DatetimeToVN(DateTime dt)
        {
            if (dt == null)
            {
                return "";
            }
            else
            {
                return dt.ToString("dd/MM/yyyy");
            }

        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public static bool IsValidDigital(object o)
        {
            try
            {
                if (o != null)
                {
                    const string parent = @"[0-9]+";
                    var regex = new Regex(parent);
                    return regex.IsMatch(o.ToString());
                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public static bool IsValidEmail(object o)
        {
            try
            {
                if (o != null)
                {
                    const string parent = @"^[\w\.]+@[\w]+\.[\w]+";
                    var regex = new Regex(parent);
                    return regex.IsMatch(o.ToString());
                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public static bool IsValidDateTime(object o)
        {
            try
            {
                if (o != null)
                {
                    DateTime dateTime;
                    return DateTime.TryParse(o.ToString(), out dateTime);
                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dateTime1"></param>
        /// <param name="dateTime2"></param>
        /// <returns></returns>
        public static bool CompareDateTime(DateTime dateTime1, DateTime dateTime2)
        {
            try
            {
                if (dateTime1 > dateTime2)
                {
                    return true;
                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Convert date from dd/mm/yyyy to mm/dd/yyyy
        /// </summary>
        /// <param name="date"></param>
        /// <returns>DateTime</returns>
        public static string ConvertDateTime(string date)
        {
            string[] str = date.Split('/');

            return str[1] + "/" + str[0] + "/" + str[2];
        }

        /// <summary>
        /// Convert date from mm/dd/yyyy to yyyy/mm/dd
        /// </summary>
        /// <param name="date"></param>
        /// <returns>DateTime</returns>
        public static DateTime ConvertDateTimeyyyymmdd(string date)
        {
            string[] str = date.Split('/');
            var convertDate = new DateTime(Int32.Parse(str[2]), Int32.Parse(str[0]), Int32.Parse(str[1]));
            return convertDate;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="d1"></param>
        /// <param name="d2"></param>
        /// <returns></returns>
        public static int TotalDay(string d1, string d2)
        {
            DateTime date1 = ConvertDateTimeyyyymmdd(d1);
            DateTime date2 = DateTime.Parse(d2);
            TimeSpan timespan = date2.Subtract(date1);
            return timespan.Days;
        }

        public static string GetQueryStringFromUrl(string url, string queryStringName)
        {
            //http://stackoverflow.com/questions/35212714/url-rewrite-with-asp-net-query-string
            string value = "";
            try
            {
                if (url.Length > 0)
                {
                    string[] listString = url.Split('/');
                    if (listString.Length > 0)
                    {
                        for (int i = 0; i < listString.Length; i++)
                        {
                            if (queryStringName.Equals(listString[i])) // nếu querstring tồn tại trong url -> lấy giá trị ở mảng kế tiếp
                            {
                                value = listString[i + 1].Trim();
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {

            }
            return value;
        }

        public static string GetTitleRewriteFromUrl(string url)
        {
            string value = "";
            try
            {
                if (url.Length > 0)
                {
                    string[] listString = url.Split('/');
                    if (listString.Length > 0)
                    {
                        value = listString[listString.Length - 1].Trim();
                    }
                }
            }
            catch (Exception)
            {
            }
            return value;
        }

        public static string CreateFrendlyText(string str)
        {
            string frendlyText = "";
            try
            {
                frendlyText = ConvertUnicodeToNoneUnicode(str).Trim().Replace(" ", "-").ToLower();
            }
            catch (Exception)
            {
            }

            return frendlyText;
        }

        public static string ConvertUnicodeToNoneUnicode(string text)
        {
            string result = "";
            try
            {
                for (int i = 33; i < 48; i++)
                {
                    text = text.Replace(((char)i).ToString(), "");
                }
                for (int i = 58; i < 65; i++)
                {
                    text = text.Replace(((char)i).ToString(), "");
                }
                for (int i = 91; i < 97; i++)
                {
                    text = text.Replace(((char)i).ToString(), "");
                }
                for (int i = 123; i < 127; i++)
                {
                    text = text.Replace(((char)i).ToString(), "");
                }
                //text = text.Replace(" ", "-"); //Comment l?i d? không dua kho?ng tr?ng thành kư t? -
                Regex regex = new Regex(@"\p{IsCombiningDiacriticalMarks}+");
                string strFormD = text.Normalize(System.Text.NormalizationForm.FormD);
                result = regex.Replace(strFormD, String.Empty).Replace('\u0111', 'd').Replace('\u0110', 'D').Replace("(", "").Replace(")", "").Replace("\"", "").Replace("/", "").Replace("\\", "").Replace("'", "").Replace("“", "").Replace("”", "");
            }
            catch (Exception)
            {

            }
            return result;
        }

        public static bool CheckContainsSpecialChars(string value)
        {
            var list = new[] { "~", "`", "!", "@", "#", "$", "%", "^", "&", "*", "(", ")", "+", "=", "-", "{", "}", " ", "?", ".", "_", "\"" };
            return list.Any(value.Contains);
        }

        public static string GetAspxPageNameFromUrl(string url = "")
        {
            //"http://localhost:40455/Camera14/Admin/pagename.aspx";
            string value = "";
            try
            {
                if (url.Length > 0)
                {
                    string[] listString = url.Split('/');
                    if (listString.Length > 0)
                    {
                        for (int i = 0; i < listString.Length; i++)
                        {
                            if (listString[i].Contains(".aspx")) // nếu querstring tồn tại trong url -> lấy giá trị ở mảng kế tiếp
                            {
                                value = listString[i].Substring(0, listString[i].IndexOf(".aspx") + 5).Trim();
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
            }
            return value;
        }

        #region Upload and Compress image


        private static string GetRandomString(int size)
        {
            StringBuilder builder = new StringBuilder();
            char ch;
            for (int i = 0; i < size; i++)
            {
                ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                builder.Append(ch);
            }

            return builder.ToString();
        }

        /// <summary>
        /// upload ảnh và nén ảnh dùng trong trường hợp upload nhiều file dùng httpPost
        /// </summary>
        /// <param name="imagenameOut">tên file ảnh lưu db</param>
        /// <param name="FileUpload">FileUpload </param>
        /// <param name="folderName">tên folder upload ảnh</param>
        /// <param name="fileName">tên file ảnh</param>
        /// <returns>string</returns>
        public static string UploadAndCompressImageProduct(ref string imagenameOut, HttpPostedFile fUpload, string folderName = "", string fileName = "", int quanlity = 0)
        {
            string message = "";
            try
            {
                string fileComPressName = fileName.Split('.')[0].Replace(" ", "-") + "-" + GetRandomString(10) + "." + fileName.Split('.')[1];
                string filePathSrc = System.Web.HttpContext.Current.Server.MapPath("~/" + folderName + "/") + fileName;
                if (System.IO.File.Exists(filePathSrc))
                {
                    filePathSrc = System.Web.HttpContext.Current.Server.MapPath("~/" + folderName + "/") + fileName.Split('.')[0] + DateTime.Now.ToString("_mm_dd_yy") + "." + fileName.Split('.')[1];
                }

                // upload image
                if (fUpload.ContentLength > 0)
                {
                    fUpload.SaveAs(filePathSrc);
                }

                System.Drawing.Image myImage = System.Drawing.Image.FromFile(filePathSrc);
                System.Drawing.Image myImage1 = myImage;

                // Save the image with a quality
                string pahtdes = System.Web.HttpContext.Current.Server.MapPath("~/" + folderName + "/" + fileComPressName);
                try
                {
                    if (fUpload.ContentLength > 1024000)
                    {
                        SaveImageFile(pahtdes, myImage1, quanlity);
                    }
                    else
                    {
                        SaveImageFile(pahtdes, myImage1, 100);
                    }

                }
                finally
                {
                    // dispose image
                    myImage.Dispose();
                    myImage1.Dispose();
                    imagenameOut = fileComPressName;
                }

                // xóa file up
                if (System.IO.File.Exists(filePathSrc))
                {
                    System.IO.File.Delete(filePathSrc);
                }
            }
            catch (Exception ex)
            {
                return "Lỗi upload: " + ex.Message;
            }

            return message;
        }

        /// <summary>
        /// upload ảnh và nén ảnh
        /// </summary>
        /// <param name="imagenameOut">tên file ảnh lưu db</param>
        /// <param name="FileUpload">FileUpload </param>
        /// <param name="folderName">tên folder upload ảnh</param>
        /// <param name="fileName">tên file ảnh</param>
        /// <returns>string</returns>
        public static string UploadAndCompressImage(ref string imagenameOut, FileUpload fUpload, string folderName = "", string fileName = "", int quanlity = 0)
        {
            string message = "";
            try
            {
                if (!Directory.Exists(System.Web.HttpContext.Current.Server.MapPath("~/" + folderName)))
                {
                    Directory.CreateDirectory(System.Web.HttpContext.Current.Server.MapPath("~/" + folderName));
                }

                string fileComPressName = fileName.Split('.')[0].Replace(" ", "-") + DateTime.Now.ToString("_dd_mm_yyyy_hh_ss") + "." + fileName.Split('.')[1];
                string filePathSrc = System.Web.HttpContext.Current.Server.MapPath("~/" + folderName + "/") + fileName;
                if (System.IO.File.Exists(filePathSrc))
                {
                    filePathSrc = System.Web.HttpContext.Current.Server.MapPath("~/" + folderName + "/") + fileName.Split('.')[0] + DateTime.Now.ToString("_mm_dd_yy_hh_ss") + "." + fileName.Split('.')[1];
                }
                // upload image
                if (fUpload.HasFile)
                {
                    fUpload.SaveAs(filePathSrc);
                }

                System.Drawing.Image myImage = System.Drawing.Image.FromFile(filePathSrc);
                System.Drawing.Image myImage1 = myImage;

                // Save the image with a quality
                string pahtdes = System.Web.HttpContext.Current.Server.MapPath("~/" + folderName + "/" + fileComPressName);
                try
                {
                    SaveImageFile(pahtdes, myImage1, quanlity);
                }
                finally
                {
                    // dispose image
                    myImage.Dispose();
                    myImage1.Dispose();
                    imagenameOut = fileComPressName;
                }

                // xóa file up
                if (System.IO.File.Exists(filePathSrc))
                {
                    System.IO.File.Delete(filePathSrc);
                }
            }
            catch (Exception ex)
            {
                return "Lỗi upload: " + ex.Message;
            }

            return message;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <param name="img"></param>
        /// <param name="quality"></param>
        public static void SaveImageFile(string path, System.Drawing.Image img, int quality)
        {
            try
            {
                EncoderParameter qualityParam = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, quality);

                //ImageCodecInfo jpegCodec = GetEncoderInfo(@"images/jpeg");
                ImageCodecInfo jpegCodec = GetEncoderInfo(@"image/jpeg");

                EncoderParameters encoderParams = new EncoderParameters(1);

                encoderParams.Param[0] = qualityParam;

                System.IO.MemoryStream mss = new System.IO.MemoryStream();

                System.IO.FileStream fs = new System.IO.FileStream(path, System.IO.FileMode.Create, System.IO.FileAccess.ReadWrite);

                img.Save(mss, jpegCodec, encoderParams);
                byte[] matriz = mss.ToArray();
                fs.Write(matriz, 0, matriz.Length);

                mss.Close();
                fs.Close();
            }
            catch (Exception ex)
            {

            }
        }

        /// <summary> 
        /// Returns the image codec with the given mime type 
        /// </summary> 
        private static ImageCodecInfo GetEncoderInfo(string mimeType)
        {
            // Get image codecs for all image formats 
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageEncoders();

            // Find the correct image codec 
            for (int i = 0; i < codecs.Length; i++)
                if (codecs[i].MimeType == mimeType)
                {
                    return codecs[i];
                }
            return null;
        }
        #endregion

        /// <summary>
        /// Tạo icon điện thoại 
        /// </summary>
        /// <param name="phone">số điện thoại</param>
        /// <param name="title">tiêu đề khi chỉ chuột</param>
        /// <returns>chuỗi chứa số điện thoại</returns>
        public static string GetPhoneBlink(string phone, string title)
        {
            string strPhone = "";
            try
            {
                strPhone = //"<div class=\"quick-alo-phone quick-alo-green quick-alo-show\" id=\"quick-alo-phoneIcon\" style=\"right:0px;top:45%;\">"
                    "<div class=\"quick-alo-phone quick-alo-green quick-alo-show\" id=\"quick-alo-phoneIcon\">"
                              + "<a href=\"tel:" + phone + "\" title=" + phone + ">"
                              + "<div class=\"quick-alo-ph-circle\"></div>"
                              + "<div class=\"quick-alo-ph-circle-fill\"></div>"
                              + "<div class=\"quick-alo-ph-img-circle\"></div>"
                              + "</a>"
                              + "</div>";
            }
            catch (Exception)
            {
            }

            return strPhone;
        }

        public static string SendMail(string toEmail, string title, string content, string pathAttachFile = "")
        {
            string mess = "";
            try
            {
                GenericUnitOfWork _unitOfWork = new GenericUnitOfWork();
                SysSetting setting_email = _unitOfWork.GetRepositoryInstance<SysSetting>().GetFirstOrDefaultByParameter(o => o.SKey == "Email");
                SysSetting setting_password = _unitOfWork.GetRepositoryInstance<SysSetting>().GetFirstOrDefaultByParameter(o => o.SKey == "PassWordEmail");

                if (setting_email != null && setting_password != null)
                {
                    Fr_Email = setting_email.Value;
                    PassEmail = setting_password.Value;
                }

                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");
                SmtpServer.UseDefaultCredentials = true;
                mail.From = new MailAddress(Fr_Email);
                mail.IsBodyHtml = true;
                mail.To.Add(toEmail);
                mail.Subject = title;
                mail.Body = content;

                if (pathAttachFile != "")
                {
                    System.Net.Mail.Attachment attachment;
                    attachment = new System.Net.Mail.Attachment(pathAttachFile);
                    mail.Attachments.Add(attachment);
                }

                SmtpServer.Port = 587;
                SmtpServer.Credentials = new System.Net.NetworkCredential(Fr_Email, PassEmail);
                SmtpServer.EnableSsl = true;

                SmtpServer.Send(mail);

                #region save
                //// smtp settings
                //var smtp = new System.Net.Mail.SmtpClient();
                //{
                //    smtp.Host = "smtp.gmail.com";
                //    smtp.Port = 587;
                //    smtp.EnableSsl = true;
                //    smtp.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
                //    smtp.Credentials = new NetworkCredential(fromEmail, fromPassword);
                //    smtp.Timeout = 20000;
                //}
                //// Passing values to smtp object
                //smtp.Send(fromEmail, toEmail, title, content);
                #endregion
            }
            catch (Exception ex)
            {
                mess = "lỗi gửi email: " + ex.Message;
                ExceptionLogging.saveLog(mess);
            }

            return mess;
        }

        public static string FormatPhone(string phone)
        {
            if (phone != "")
            {
                int number = int.Parse(phone.Substring(1, phone.Length - 1));
                string s = number.ToString("000-000-000");
                string phoneOut = "0" + s.ToString();
                return phoneOut.Replace("-", ".");
            }
            else
            {
                return phone;
            }
        }

        public static string RemoveHtmlInStrip(string input)
        {
            return Regex.Replace(input, "<.*?>", String.Empty);
        }

        public static string RutGonTieuDeTin(string tin, int dodai)
        {
            try
            {
                if (tin != null && tin != "")
                {
                    if (tin.Length > dodai)
                    {
                        return tin.Substring(0, dodai) + "...";
                    }
                    else
                    {
                        return tin + "...";
                    }
                }
                else
                {
                    return "";
                }
            }
            catch (Exception ex)
            {
                return "";
            }
        }
        public static void FindAndReplace(Microsoft.Office.Interop.Word.Application wordApp, object findText, object replaceWithText)
        {
            object matchCase = true;
            object matchWholeWord = true;
            object matchWildCards = false;
            object matchSoundLike = false;
            object nmatchAllForms = false;
            object forward = true;
            object format = false;
            object matchKashida = false;
            object matchDiactitics = false;
            object matchAlefHamza = false;
            object matchControl = false;
            object read_only = false;
            object visible = true;
            object replace = 2;
            object wrap = 1;

            wordApp.Selection.Find.Execute(ref findText,
                        ref matchCase, ref matchWholeWord,
                        ref matchWildCards, ref matchSoundLike,
                        ref nmatchAllForms, ref forward,
                        ref wrap, ref format, ref replaceWithText,
                        ref replace, ref matchKashida,
                        ref matchDiactitics, ref matchAlefHamza,
                        ref matchControl);
        }
        public class Pdf
        {
            public static Boolean PrintPDFs(string pdfFileName)
            {
                try
                {
                    Process proc = new Process();
                    proc.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                    proc.StartInfo.Verb = "print";

                    //Define location of adobe reader/command line
                    //switches to launch adobe in "print" mode
                    proc.StartInfo.FileName =
                      @"C:\Program Files (x86)\Adobe\Reader 11.0\Reader\AcroRd32.exe";
                    proc.StartInfo.Arguments = String.Format(@"/p /h {0}", pdfFileName);
                    proc.StartInfo.UseShellExecute = false;
                    proc.StartInfo.CreateNoWindow = true;

                    proc.Start();
                    proc.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                    if (proc.HasExited == false)
                    {
                        proc.WaitForExit(10000);
                    }

                    proc.EnableRaisingEvents = true;

                    proc.Close();
                    KillAdobe("AcroRd32");
                    return true;
                }
                catch (Exception ex)
                {
                    return false;
                }
            }

            //For whatever reason, sometimes adobe likes to be a stage 5 clinger.
            //So here we kill it with fire.
            private static bool KillAdobe(string name)
            {
                try
                {
                    foreach (Process clsProcess in Process.GetProcesses().Where(
                             clsProcess => clsProcess.ProcessName.StartsWith(name)))
                    {
                        clsProcess.Kill();
                        return true;
                    }
                }
                catch (Exception ex)
                {

                }
                return false;
            }
        }//END Class
    }
}