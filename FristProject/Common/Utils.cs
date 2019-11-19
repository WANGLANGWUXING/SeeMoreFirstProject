using System;
using System.Configuration;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Web;

namespace FristProject.Common
{
    public class Utils
    {
        private static bool RemoteCertificateValidate(object sender, X509Certificate cert, X509Chain chain, SslPolicyErrors error)
        {
            return true;
        }

        public static bool posturl(string yuming)
        {
            bool result = false;
            string absoluteUri = HttpContext.Current.Request.UrlReferrer.AbsoluteUri;
            if (absoluteUri.Substring(0, 24) == yuming)
            {
                result = true;
            }
            return result;
        }

        public static string md5str16(string password)
        {
            MD5CryptoServiceProvider mD5CryptoServiceProvider = new MD5CryptoServiceProvider();
            return BitConverter.ToString(mD5CryptoServiceProvider.ComputeHash(Encoding.Default.GetBytes(password))).Replace("-", "").ToLower().Substring(8, 16);
        }

        public static string md5str32(string password)
        {
            MD5CryptoServiceProvider mD5CryptoServiceProvider = new MD5CryptoServiceProvider();
            return BitConverter.ToString(mD5CryptoServiceProvider.ComputeHash(Encoding.Default.GetBytes(password))).Replace("-", "").ToLower();
        }

        public static string MD5str16(string password)
        {
            MD5CryptoServiceProvider mD5CryptoServiceProvider = new MD5CryptoServiceProvider();
            return BitConverter.ToString(mD5CryptoServiceProvider.ComputeHash(Encoding.Default.GetBytes(password))).Replace("-", "").ToUpper().Substring(8, 16);
        }

        public static string MD5str32(string password)
        {
            MD5CryptoServiceProvider mD5CryptoServiceProvider = new MD5CryptoServiceProvider();
            return BitConverter.ToString(mD5CryptoServiceProvider.ComputeHash(Encoding.Default.GetBytes(password))).Replace("-", "").ToUpper();
        }

        public bool GetMultimedia(string ACCESS_TOKEN, string MEDIA_ID, string filename)
        {
            string empty = string.Empty;
            string empty2 = string.Empty;
            string address = string.Empty;
            string empty3 = string.Empty;
            string requestUriString = "http://file.api.weixin.qq.com/cgi-bin/media/get?access_token=" + ACCESS_TOKEN + "&media_id=" + MEDIA_ID;
            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(requestUriString);
            httpWebRequest.Method = "GET";
            WebResponse response = httpWebRequest.GetResponse();
            HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            address = httpWebResponse.ResponseUri.ToString();
            WebClient webClient = new WebClient();
            webClient.DownloadFile(address, filename);
            return true;
        }

        public static string getjson(string url, string PostString)
        {
            ServicePointManager.ServerCertificateValidationCallback = (RemoteCertificateValidationCallback)Delegate.Combine(ServicePointManager.ServerCertificateValidationCallback, new RemoteCertificateValidationCallback(RemoteCertificateValidate));
            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            httpWebRequest.Method = "POST";
            byte[] bytes = Encoding.UTF8.GetBytes(PostString);
            httpWebRequest.ContentType = "application/x-www-form-urlencoded";
            httpWebRequest.ContentLength = (long)bytes.Length;
            Stream requestStream = httpWebRequest.GetRequestStream();
            requestStream.Write(bytes, 0, bytes.Length);
            requestStream.Close();
            HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            Stream responseStream = httpWebResponse.GetResponseStream();
            StreamReader streamReader = new StreamReader(responseStream);
            string result = streamReader.ReadToEnd();
            httpWebResponse.Close();
            return result;
        }

        public static int ConvertDateTimeInt(DateTime time)
        {
            DateTime d = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            return (int)(time - d).TotalSeconds;
        }

        public static DateTime ConvertIntDateTime(string timeStamp)
        {
            DateTime dateTime = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            long ticks = long.Parse(timeStamp + "0000000");
            TimeSpan value = new TimeSpan(ticks);
            return dateTime.Add(value);
        }

        public static string GetSHA(string Pwd)
        {
            byte[] bytes = Encoding.Default.GetBytes(Pwd);
            SHA1 sHA = new SHA1CryptoServiceProvider();
            byte[] value = sHA.ComputeHash(bytes);
            return BitConverter.ToString(value).Replace("-", "");
        }

        public static string PostPage(string posturl, string postData, string title)
        {
            Encoding uTF = Encoding.UTF8;
            byte[] bytes = uTF.GetBytes(postData);
            string fileName = ConfigurationManager.AppSettings["certPath_" + title].ToString();
            string password = ConfigurationManager.AppSettings["password_" + title].ToString();
            X509Certificate2 value = new X509Certificate2(fileName, password, X509KeyStorageFlags.MachineKeySet);
            ServicePointManager.ServerCertificateValidationCallback = (RemoteCertificateValidationCallback)Delegate.Combine(ServicePointManager.ServerCertificateValidationCallback, new RemoteCertificateValidationCallback(RemoteCertificateValidate));
            HttpWebRequest httpWebRequest = WebRequest.Create(posturl) as HttpWebRequest;
            CookieContainer cookieContainer = new CookieContainer();
            httpWebRequest.CookieContainer = cookieContainer;
            httpWebRequest.AllowAutoRedirect = true;
            httpWebRequest.Method = "POST";
            httpWebRequest.ContentType = "text/xml";
            httpWebRequest.ContentLength = (long)bytes.Length;
            httpWebRequest.ClientCertificates.Add(value);
            Stream requestStream = httpWebRequest.GetRequestStream();
            requestStream.Write(bytes, 0, bytes.Length);
            requestStream.Close();
            HttpWebResponse httpWebResponse = httpWebRequest.GetResponse() as HttpWebResponse;
            Stream responseStream = httpWebResponse.GetResponseStream();
            StreamReader streamReader = new StreamReader(responseStream, uTF);
            string result = streamReader.ReadToEnd();
            string empty = string.Empty;
            return result;
        }

        public static string Cutechar(string parStr)
        {
            parStr = parStr == null ? "" : parStr;
            parStr = parStr.Trim();
            parStr = parStr.Replace("--", "－－");
            parStr = parStr.Replace("'", "''");
            string[] arrErrorStr =
            {
                "exec,ＥＸＥＣ",
                "select,ＳＥＬＥＣＴ",
                "update,ＵＰＤＡＴＥ",
                "insert,ＩＮＳＥＲＴ",
                "delete,ＤＥＬＥＴＥ",
                "script,ＳＣＲＩＰＴ",
                "iframe,ＩＦＲＡＭＥ",
                "frame,ＦＲＡＭＥ"
            };
            for (int i = 0; i < arrErrorStr.Length; i++)
            {
                string[] arrTemp = arrErrorStr[i].Split(',');
                while (parStr.ToLower().Contains(arrTemp[0]))
                {
                    parStr = parStr.ToLower().Replace(arrTemp[0], arrTemp[1]);
                }
            }
            return parStr;
        }
    }
}