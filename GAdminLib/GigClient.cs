using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Xml;

namespace GAdminLib
{
    public class GigClient
    {
        #region Splitters
        static Regex SPSplitter = new Regex(@"<SP>", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        static Regex NLSplitter = new Regex(@"<NL>", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        static Regex GIGDSplitter = new Regex(@"<GIG_DATA>", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        #endregion
        public string CleanData(string d)
        {
            return  System.Web.HttpUtility.UrlEncode( d.Replace("\"", "&#34;").Replace("'", "&#39;"));
        }
        public string UnCleanData(string d)
        {
            return d.Replace( "&#34;","\"").Replace("&#39;","'");
        }
        public GigUser MyAccount;
        public string AccessToken = "";

        public string Host = "https://client.greedingames.com/";
        string HOST;
        public GigClient()
        {
            HOST = Host;
        }
        public GigClient(string host)
        {
            Host = host;
            HOST = host;
        }
        public string Username;
        public string ErrorMSG = "";
        public bool Login(string username, string password)
        {

            string passhash = Utils.MD5Hash(password);
            try
            {

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(HOST + "/Connect.php");
                request.Method = "POST";
                request.Accept = "gzip, deflate";
                request.Proxy = null;
                request.Timeout = 15000;
                request.KeepAlive = true;
                request.UserAgent = "GIG_CLIENT/UserAgent 1.0";
                request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
                string postData = "username=" + username + "&pass=" + passhash;
                byte[] byteArray = Encoding.UTF8.GetBytes(postData);
                // Set the ContentType property of the WebRequest.
                request.ContentType = "application/x-www-form-urlencoded";
                // Set the ContentLength property of the WebRequest.
                request.ContentLength = byteArray.Length;
                // Get the request stream.
                Stream dataStream = request.GetRequestStream();
                // Write the data to the request stream.
                dataStream.Write(byteArray, 0, byteArray.Length);
                // Close the Stream object.
                dataStream.Close();
                // Get the response.
                WebResponse sresponse = request.GetResponse();
                dataStream = sresponse.GetResponseStream();

                StreamReader reader = new StreamReader(dataStream);

                string responseFromServer = reader.ReadToEnd();
                if (responseFromServer.Contains("OK:"))
                {

                    reader.Close();
                    dataStream.Close();
                    sresponse.Close();
                    AccessToken = GIGDSplitter.Split(UnCleanData(responseFromServer).Replace("OK:", ""), 3)[1];
                    Username = username;
                    return true;



                }
                else if (responseFromServer.Contains("DENIED:"))
                {
                    ErrorMSG = GIGDSplitter.Split(UnCleanData(responseFromServer).Replace("DENIED:", ""), 3)[1];
                    return false;
                }
                else
                    return false;
            }
            catch
            {
                return false;
            }
        }
        public bool Logout()
        {
            try
            {

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(HOST + "/Disconnect.php");
                request.Method = "POST";
                request.Accept = "gzip, deflate";
                request.Proxy = null;
                request.Timeout = 15000;
                request.KeepAlive = true;
                request.UserAgent = "GIG_CLIENT/UserAgent 1.0";
                request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
                string postData = "AT=" + AccessToken;
                byte[] byteArray = Encoding.UTF8.GetBytes(postData);
                // Set the ContentType property of the WebRequest.
                request.ContentType = "application/x-www-form-urlencoded";
                // Set the ContentLength property of the WebRequest.
                request.ContentLength = byteArray.Length;
                // Get the request stream.
                Stream dataStream = request.GetRequestStream();
                // Write the data to the request stream.
                dataStream.Write(byteArray, 0, byteArray.Length);
                // Close the Stream object.
                dataStream.Close();
                // Get the response.
                WebResponse sresponse = request.GetResponse();
                dataStream = sresponse.GetResponseStream();

                StreamReader reader = new StreamReader(dataStream);

                string responseFromServer = reader.ReadToEnd();
                if (responseFromServer.Contains("OK:"))
                {

                    reader.Close();
                    dataStream.Close();
                    sresponse.Close();
                    AccessToken = "NO TOKEN";
                    return true;



                }
                else if (responseFromServer.Contains("DENIED:"))
                {
                    ErrorMSG = GIGDSplitter.Split(UnCleanData(responseFromServer).Replace("DENIED:", ""), 3)[1];
                    return false;
                }
                else
                    return false;
            }
            catch
            {
                return false;
            }
        }
        public GigUser GetUserInfo(string username)
        {
            GigUser user = new GigUser();
            try
            {

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(HOST + "/Answer.php");
                request.Method = "POST";
                request.Accept = "gzip, deflate";
                request.Proxy = null;
                request.Timeout = 15000;
                request.KeepAlive = true;
                request.UserAgent = "GIG_CLIENT/UserAgent 1.0";
                request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
                string postData = "token=GETUI&username=" + username + "&AT=" + AccessToken;
                byte[] byteArray = Encoding.UTF8.GetBytes(postData);
                // Set the ContentType property of the WebRequest.
                request.ContentType = "application/x-www-form-urlencoded";
                // Set the ContentLength property of the WebRequest.
                request.ContentLength = byteArray.Length;
                // Get the request stream.
                Stream dataStream = request.GetRequestStream();
                // Write the data to the request stream.
                dataStream.Write(byteArray, 0, byteArray.Length);
                // Close the Stream object.
                dataStream.Close();
                // Get the response.
                WebResponse sresponse = request.GetResponse();
                dataStream = sresponse.GetResponseStream();

                StreamReader reader = new StreamReader(dataStream);

                string responseFromServer = reader.ReadToEnd();
                if (responseFromServer.Contains("OK:"))
                {
                    reader.Close();
                    dataStream.Close();
                    sresponse.Close();
                    string p = GIGDSplitter.Split(UnCleanData(responseFromServer).Replace("OK:", ""), 3)[1];
                    string[] ln = NLSplitter.Split(p);
                    user.Username = ln[0];
                    user.Name = ln[1];
        
                    user.Email = ln[2];
             
                    user.Role = (GIGRoles)byte.Parse(ln[3]);
                    user.GIGP = int.Parse(ln[4]);
                    user.RegistrationDate = DateTime.Parse(ln[5]);

                    return user;



                }
                else if (responseFromServer.Contains("DENIED:"))
                {
                    ErrorMSG = GIGDSplitter.Split(UnCleanData(responseFromServer).Replace("DENIED:", ""), 3)[1];
                    return user;
                }
                else
                    return user;
            }
            catch
            {
                return user;
            }
        }
        public bool IsConnected()
        {
            try
            {

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(HOST + "/Online.php");
                request.Method = "POST";
                request.Accept = "gzip, deflate";
                request.Proxy = null;
                request.Timeout = 15000;
                request.KeepAlive = true;
                request.UserAgent = "GIG_CLIENT/UserAgent 1.0";
                request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
                string postData = "AT=" + AccessToken;
                byte[] byteArray = Encoding.UTF8.GetBytes(postData);
                // Set the ContentType property of the WebRequest.
                request.ContentType = "application/x-www-form-urlencoded";
                // Set the ContentLength property of the WebRequest.
                request.ContentLength = byteArray.Length;
                // Get the request stream.
                Stream dataStream = request.GetRequestStream();
                // Write the data to the request stream.
                dataStream.Write(byteArray, 0, byteArray.Length);
                // Close the Stream object.
                dataStream.Close();
                // Get the response.
                WebResponse sresponse = request.GetResponse();
                dataStream = sresponse.GetResponseStream();

                StreamReader reader = new StreamReader(dataStream);

                string responseFromServer = reader.ReadToEnd();
                if (responseFromServer.Contains("OK:"))
                {

                    reader.Close();
                    dataStream.Close();
                    sresponse.Close();

                    return true;



                }
                else if (responseFromServer.Contains("DENIED:"))
                {
                    ErrorMSG = GIGDSplitter.Split(UnCleanData(responseFromServer).Replace("DENIED:", ""), 3)[1];
                    return false;
                }
                else
                    return false;
            }
            catch
            {
                return false;
            }
        }
        public Dictionary<string, string> FindUser(string pattern)
        {
            Dictionary<string, string> users = new Dictionary<string, string>();
            try
            {

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(HOST + "/Answer.php");
                request.Method = "POST";
                request.Accept = "gzip, deflate";
                request.Proxy = null;
                request.Timeout = 15000;
                request.KeepAlive = true;
                request.UserAgent = "GIG_CLIENT/UserAgent 1.0";
                request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
                string postData = "token=FINDUSR&username=" + pattern + "&AT=" + AccessToken;
                byte[] byteArray = Encoding.UTF8.GetBytes(postData);
                // Set the ContentType property of the WebRequest.
                request.ContentType = "application/x-www-form-urlencoded";
                // Set the ContentLength property of the WebRequest.
                request.ContentLength = byteArray.Length;
                // Get the request stream.
                Stream dataStream = request.GetRequestStream();
                // Write the data to the request stream.
                dataStream.Write(byteArray, 0, byteArray.Length);
                // Close the Stream object.
                dataStream.Close();
                // Get the response.
                WebResponse sresponse = request.GetResponse();
                dataStream = sresponse.GetResponseStream();

                StreamReader reader = new StreamReader(dataStream);

                string responseFromServer = reader.ReadToEnd();
                if (responseFromServer.Contains("OK:"))
                {

                    reader.Close();
                    dataStream.Close();
                    sresponse.Close();
                    string p = GIGDSplitter.Split(UnCleanData(responseFromServer).Replace("OK:", ""), 3)[1];
                    if (p.Contains("<NL>"))
                    {
                        string[] ln = NLSplitter.Split(p);
                        for (int i = 0; i < ln.Length - 1; i++)
                        {
                            string found = ln[i];
                            string[] u = SPSplitter.Split(found);
                            if (!users.ContainsKey(u[0]))
                                users.Add(u[0], u[1]);
                        }

                    }

                    return users;



                }
                else if (responseFromServer.Contains("DENIED:"))
                {
                    ErrorMSG = GIGDSplitter.Split(UnCleanData(responseFromServer).Replace("DENIED:", ""), 3)[1];
                    return users;
                }
                else
                    return users;
            }
            catch
            {
                return users;
            }
        }
        public bool AddUser(string username, string msg)
        {

            try
            {

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(HOST + "/Answer.php");
                request.Method = "POST";
                request.Accept = "gzip, deflate";
                request.Proxy = null;
                request.Timeout = 15000;
                request.KeepAlive = true;
                request.UserAgent = "GIG_CLIENT/UserAgent 1.0";
                request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
                string postData = "token=ADDUSR&msg="+Utils.ToHex(msg)+"&username=" + MyAccount.Username + "&friend=" + username + "&AT=" + AccessToken;
                byte[] byteArray = Encoding.UTF8.GetBytes(postData);
                // Set the ContentType property of the WebRequest.
                request.ContentType = "application/x-www-form-urlencoded";
                // Set the ContentLength property of the WebRequest.
                request.ContentLength = byteArray.Length;
                // Get the request stream.
                Stream dataStream = request.GetRequestStream();
                // Write the data to the request stream.
                dataStream.Write(byteArray, 0, byteArray.Length);
                // Close the Stream object.
                dataStream.Close();
                // Get the response.
                WebResponse sresponse = request.GetResponse();
                dataStream = sresponse.GetResponseStream();

                StreamReader reader = new StreamReader(dataStream);

                string responseFromServer = reader.ReadToEnd();
                if (responseFromServer.Contains("OK:"))
                {

                    reader.Close();
                    dataStream.Close();
                    sresponse.Close();

                    return true;



                }
                else if (responseFromServer.Contains("DENIED:"))
                {
                    ErrorMSG = GIGDSplitter.Split(UnCleanData(responseFromServer).Replace("DENIED:", ""), 3)[1];
                    return false;
                }
                else
                    return false;
            }
            catch
            {
                return false;
            }
        }
        public bool AcceptUser(string friend)
        {

            try
            {

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(HOST + "/Answer.php");
                request.Method = "POST";
                request.Accept = "gzip, deflate";
                request.Proxy = null;
                request.Timeout = 15000;
                request.KeepAlive = true;
                request.UserAgent = "GIG_CLIENT/UserAgent 1.0";
                request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
                string postData = "token=ACCUSR&username=" + MyAccount.Username + "&friend=" + friend + "&AT=" + AccessToken;
                byte[] byteArray = Encoding.UTF8.GetBytes(postData);
                // Set the ContentType property of the WebRequest.
                request.ContentType = "application/x-www-form-urlencoded";
                // Set the ContentLength property of the WebRequest.
                request.ContentLength = byteArray.Length;
                // Get the request stream.
                Stream dataStream = request.GetRequestStream();
                // Write the data to the request stream.
                dataStream.Write(byteArray, 0, byteArray.Length);
                // Close the Stream object.
                dataStream.Close();
                // Get the response.
                WebResponse sresponse = request.GetResponse();
                dataStream = sresponse.GetResponseStream();

                StreamReader reader = new StreamReader(dataStream);

                string responseFromServer = reader.ReadToEnd();
                if (responseFromServer.Contains("OK:"))
                {

                    reader.Close();
                    dataStream.Close();
                    sresponse.Close();

                    return true;



                }
                else if (responseFromServer.Contains("DENIED:"))
                {
                    ErrorMSG = GIGDSplitter.Split(UnCleanData(responseFromServer).Replace("DENIED:", ""), 3)[1];
                    return false;
                }
                else
                    return false;
            }
            catch
            {
                return false;
            }
        }
        public bool RemoveUser(string username)
        {

            try
            {

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(HOST + "/Answer.php");
                request.Method = "POST";
                request.Accept = "gzip, deflate";
                request.Proxy = null;
                request.Timeout = 15000;
                request.KeepAlive = true;
                request.UserAgent = "GIG_CLIENT/UserAgent 1.0";
                request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
                string postData = "token=REMUSR&username=" + MyAccount.Username + "&friend=" + username + "&AT=" + AccessToken;
                byte[] byteArray = Encoding.UTF8.GetBytes(postData);
                // Set the ContentType property of the WebRequest.
                request.ContentType = "application/x-www-form-urlencoded";
                // Set the ContentLength property of the WebRequest.
                request.ContentLength = byteArray.Length;
                // Get the request stream.
                Stream dataStream = request.GetRequestStream();
                // Write the data to the request stream.
                dataStream.Write(byteArray, 0, byteArray.Length);
                // Close the Stream object.
                dataStream.Close();
                // Get the response.
                WebResponse sresponse = request.GetResponse();
                dataStream = sresponse.GetResponseStream();

                StreamReader reader = new StreamReader(dataStream);

                string responseFromServer = reader.ReadToEnd();
                if (responseFromServer.Contains("OK:"))
                {

                    reader.Close();
                    dataStream.Close();
                    sresponse.Close();

                    return true;



                }
                else if (responseFromServer.Contains("DENIED:"))
                {
                    ErrorMSG = GIGDSplitter.Split(UnCleanData(responseFromServer).Replace("DENIED:", ""), 3)[1];
                    return false;
                }
                else
                    return false;
            }
            catch
            {
                return false;
            }
        }
        public bool SendMessage(string from, string to, string message)
        {
            try
            {

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(HOST + "/Answer.php");
                request.Method = "POST";
                request.Accept = "gzip, deflate";
                request.Proxy = null;
                request.Timeout = 15000;
                request.KeepAlive = true;
                request.UserAgent = "GIG_CLIENT/UserAgent 1.0";
                request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
                string postData = "token=SENDMSG&from=" + from + "&to=" + to + "&msg=" + Utils.ToHex(message) + "&AT=" + AccessToken;
                byte[] byteArray = Encoding.UTF8.GetBytes(postData);
                // Set the ContentType property of the WebRequest.
                request.ContentType = "application/x-www-form-urlencoded";
                // Set the ContentLength property of the WebRequest.
                request.ContentLength = byteArray.Length;
                // Get the request stream.
                Stream dataStream = request.GetRequestStream();
                // Write the data to the request stream.
                dataStream.Write(byteArray, 0, byteArray.Length);
                // Close the Stream object.
                dataStream.Close();
                // Get the response.
                WebResponse sresponse = request.GetResponse();
                dataStream = sresponse.GetResponseStream();

                StreamReader reader = new StreamReader(dataStream);

                string responseFromServer = reader.ReadToEnd();
                if (responseFromServer.Contains("OK:SENT"))
                {

                    reader.Close();
                    dataStream.Close();
                    sresponse.Close();
                    return true;



                }
                else if (responseFromServer.Contains("DENIED:"))
                {
                    ErrorMSG = GIGDSplitter.Split(UnCleanData(responseFromServer).Replace("DENIED:", ""), 3)[1];
                    return false;
                }
                else
                    return false;
            }
            catch
            {
                return false;
            }
        }
        public List<GigMessage> GetMessages(string username)
        {
            List<GigMessage> pg = new List<GigMessage>();
            try
            {

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(HOST + "/Answer.php");
                request.Method = "POST";
                request.Accept = "gzip, deflate";
                request.Proxy = null;
                request.Timeout = 15000;
                request.KeepAlive = true;
                request.UserAgent = "GIG_CLIENT/UserAgent 1.0";
                request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
                string postData = "token=GETMSG&receiver=" + username + "&AT=" + AccessToken;
                byte[] byteArray = Encoding.UTF8.GetBytes(postData);
                // Set the ContentType property of the WebRequest.
                request.ContentType = "application/x-www-form-urlencoded";
                // Set the ContentLength property of the WebRequest.
                request.ContentLength = byteArray.Length;
                // Get the request stream.
                Stream dataStream = request.GetRequestStream();
                // Write the data to the request stream.
                dataStream.Write(byteArray, 0, byteArray.Length);
                // Close the Stream object.
                dataStream.Close();
                // Get the response.
                WebResponse sresponse = request.GetResponse();
                dataStream = sresponse.GetResponseStream();

                StreamReader reader = new StreamReader(dataStream);

                string responseFromServer = reader.ReadToEnd();
                if (responseFromServer.Contains("OK:"))
                {

                    reader.Close();
                    dataStream.Close();
                    sresponse.Close();
                    string p = GIGDSplitter.Split(UnCleanData(responseFromServer).Replace("OK:", ""), 3)[1];
                    File.WriteAllText(Application.StartupPath + @"\Data\MSG.dat", p);
                    string[] x = NLSplitter.Split(p);
                    foreach (string xa in x)
                    {
                        if (xa.Length > 0)
                        {
                            string[] a = SPSplitter.Split(xa, 3);
                            GigMessage msg = new GigMessage();
                            msg.Sender = a[0];
                            msg.Message = Utils.FromHex(a[1]);
                            msg.RD = DateTime.Parse(a[2]);
                            pg.Add(msg);
                        }
                    }

                    return pg;



                }
                else if (responseFromServer.Contains("DENIED:"))
                {
                    ErrorMSG = GIGDSplitter.Split(UnCleanData(responseFromServer).Replace("DENIED:", ""), 3)[1];
                    return pg;
                }
                else
                    return pg;
            }
            catch
            {
                return pg;
            }
        }

        public bool CleanNotifications()
        {

            try
            {

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(HOST + "/Answer.php");
                request.Method = "POST";
                request.Accept = "gzip, deflate";
                request.Proxy = null;
                request.Timeout = 15000;
                request.KeepAlive = true;
                request.UserAgent = "GIG_CLIENT/UserAgent 1.0";
                request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
                string postData = "token=GETNOTIF&clean=CLEAN&username=" + MyAccount.Username + "&AT=" + AccessToken;
                byte[] byteArray = Encoding.UTF8.GetBytes(postData);
                // Set the ContentType property of the WebRequest.
                request.ContentType = "application/x-www-form-urlencoded";
                // Set the ContentLength property of the WebRequest.
                request.ContentLength = byteArray.Length;
                // Get the request stream.
                Stream dataStream = request.GetRequestStream();
                // Write the data to the request stream.
                dataStream.Write(byteArray, 0, byteArray.Length);
                // Close the Stream object.
                dataStream.Close();
                // Get the response.
                WebResponse sresponse = request.GetResponse();
                dataStream = sresponse.GetResponseStream();

                StreamReader reader = new StreamReader(dataStream);

                string responseFromServer = reader.ReadToEnd();
                if (responseFromServer.Contains("OK:"))
                {

                    reader.Close();
                    dataStream.Close();
                    sresponse.Close();


                    return true;



                }
                else if (responseFromServer.Contains("DENIED:"))
                {
                    ErrorMSG = GIGDSplitter.Split(UnCleanData(responseFromServer).Replace("DENIED:", ""), 3)[1];
                    return false;
                }
                else
                    return false;
            }
            catch
            {
                return false;
            }
        }
        public bool CleanMessages()
        {

            try
            {

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(HOST + "/Answer.php");
                request.Method = "POST";
                request.Accept = "gzip, deflate";
                request.Proxy = null;
                request.Timeout = 15000;
                request.KeepAlive = true;
                request.UserAgent = "GIG_CLIENT/UserAgent 1.0";
                request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
                string postData = "token=GETMSG&clean=CLEAN&receiver=" + MyAccount.Username + "&AT=" + AccessToken;
                byte[] byteArray = Encoding.UTF8.GetBytes(postData);
                // Set the ContentType property of the WebRequest.
                request.ContentType = "application/x-www-form-urlencoded";
                // Set the ContentLength property of the WebRequest.
                request.ContentLength = byteArray.Length;
                // Get the request stream.
                Stream dataStream = request.GetRequestStream();
                // Write the data to the request stream.
                dataStream.Write(byteArray, 0, byteArray.Length);
                // Close the Stream object.
                dataStream.Close();
                // Get the response.
                WebResponse sresponse = request.GetResponse();
                dataStream = sresponse.GetResponseStream();

                StreamReader reader = new StreamReader(dataStream);

                string responseFromServer = reader.ReadToEnd();
                if (responseFromServer.Contains("OK:"))
                {

                    reader.Close();
                    dataStream.Close();
                    sresponse.Close();


                    return true;



                }
                else if (responseFromServer.Contains("DENIED:"))
                {
                    ErrorMSG = GIGDSplitter.Split(UnCleanData(responseFromServer).Replace("DENIED:", ""), 3)[1];
                    return false;
                }
                else
                    return false;
            }
            catch
            {
                return false;
            }
        }
        public bool CleanTransactions()
        {

            try
            {

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(HOST + "/Answer.php");
                request.Method = "POST";
                request.Accept = "gzip, deflate";
                request.Proxy = null;
                request.Timeout = 15000;
                request.KeepAlive = true;
                request.UserAgent = "GIG_CLIENT/UserAgent 1.0";
                request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
                string postData = "token=SHOWTRANS&action=CLEAR&AT=" + AccessToken;
                byte[] byteArray = Encoding.UTF8.GetBytes(postData);
                // Set the ContentType property of the WebRequest.
                request.ContentType = "application/x-www-form-urlencoded";
                // Set the ContentLength property of the WebRequest.
                request.ContentLength = byteArray.Length;
                // Get the request stream.
                Stream dataStream = request.GetRequestStream();
                // Write the data to the request stream.
                dataStream.Write(byteArray, 0, byteArray.Length);
                // Close the Stream object.
                dataStream.Close();
                // Get the response.
                WebResponse sresponse = request.GetResponse();
                dataStream = sresponse.GetResponseStream();

                StreamReader reader = new StreamReader(dataStream);

                string responseFromServer = reader.ReadToEnd();
                if (responseFromServer.Contains("OK:"))
                {

                    reader.Close();
                    dataStream.Close();
                    sresponse.Close();

                    return true;



                }
                else if (responseFromServer.Contains("DENIED:"))
                {
                    ErrorMSG = GIGDSplitter.Split(UnCleanData(responseFromServer).Replace("DENIED:", ""), 3)[1];
                    return false;
                }
                else
                    return false;
            }
            catch
            {
                return false;
            }
        }
        public List<GigTransaction> GetTransactions(bool admin)
        {
            List<GigTransaction> pg = new List<GigTransaction>();
            try
            {

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(HOST + "/Answer.php");
                request.Method = "POST";
                request.Accept = "gzip, deflate";
                request.Proxy = null;
                request.Timeout = 15000;
                request.KeepAlive = true;
                request.UserAgent = "GIG_CLIENT/UserAgent 1.0";
                request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
                string postData = "token=SHOWTRANS&action=SHOW&username=" + MyAccount.Username + "&AT=" + AccessToken;
                if (admin)
                    postData += "&admin=1";
                byte[] byteArray = Encoding.UTF8.GetBytes(postData);
                // Set the ContentType property of the WebRequest.
                request.ContentType = "application/x-www-form-urlencoded";
                // Set the ContentLength property of the WebRequest.
                request.ContentLength = byteArray.Length;
                // Get the request stream.
                Stream dataStream = request.GetRequestStream();
                // Write the data to the request stream.
                dataStream.Write(byteArray, 0, byteArray.Length);
                // Close the Stream object.
                dataStream.Close();
                // Get the response.
                WebResponse sresponse = request.GetResponse();
                dataStream = sresponse.GetResponseStream();

                StreamReader reader = new StreamReader(dataStream);

                string responseFromServer = reader.ReadToEnd();
                if (responseFromServer.Contains("OK:"))
                {

                    reader.Close();
                    dataStream.Close();
                    sresponse.Close();
                    string p = GIGDSplitter.Split(UnCleanData(responseFromServer).Replace("OK:", ""), 3)[1];
                    string[] x = NLSplitter.Split(p);

                    foreach (string veh in x)
                    {
                        if (veh.Length > 0)
                        {
                            string[] s = SPSplitter.Split(veh);
                            GigTransaction cmd = new GigTransaction();
                            cmd.Sender = s[0];
                            cmd.Receiver = s[1];
                            cmd.Amount = int.Parse(s[2]);
                            cmd.TimeStamp = DateTime.Parse(s[3]);


                            pg.Add(cmd);
                        }
                    }

                    return pg;



                }
                else if (responseFromServer.Contains("DENIED:"))
                {
                    ErrorMSG = GIGDSplitter.Split(UnCleanData(responseFromServer).Replace("DENIED:", ""), 3)[1];
                    return pg;
                }
                else
                    return pg;
            }
            catch
            {
                return pg;
            }
        }
        public List<string> GetFriends(string username)
        {
            List<string> fr = new List<string>();
            try
            {

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(HOST + "/Answer.php");
                request.Method = "POST";
                request.Accept = "gzip, deflate";
                request.Proxy = null;
                request.Timeout = 15000;
                request.KeepAlive = true;
                request.UserAgent = "GIG_CLIENT/UserAgent 1.0";
                request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
                string postData = "token=GETF&action=SHOW&AT=" + AccessToken+"&username="+username;
                byte[] byteArray = Encoding.UTF8.GetBytes(postData);
                // Set the ContentType property of the WebRequest.
                request.ContentType = "application/x-www-form-urlencoded";
                // Set the ContentLength property of the WebRequest.
                request.ContentLength = byteArray.Length;
                // Get the request stream.
                Stream dataStream = request.GetRequestStream();
                // Write the data to the request stream.
                dataStream.Write(byteArray, 0, byteArray.Length);
                // Close the Stream object.
                dataStream.Close();
                // Get the response.
                WebResponse sresponse = request.GetResponse();
                dataStream = sresponse.GetResponseStream();

                StreamReader reader = new StreamReader(dataStream);

                string responseFromServer = reader.ReadToEnd();
                if (responseFromServer.Contains("OK:"))
                {

                    reader.Close();
                    dataStream.Close();
                    sresponse.Close();
                    string p = GIGDSplitter.Split(UnCleanData(responseFromServer).Replace("OK:", ""), 3)[1];
                    string[] x = NLSplitter.Split(p);

                    foreach (string veh in x)
                    {
                        if (veh.Length > 0)
                        {
                            fr.Add(veh);
                        }
                    }

                    return fr;



                }
                else if (responseFromServer.Contains("DENIED:"))
                {
                    ErrorMSG = GIGDSplitter.Split(UnCleanData(responseFromServer).Replace("DENIED:", ""), 3)[1];
                    return fr;
                }
                else
                    return fr;
            }
            catch
            {
                return fr;
            }
        }
        public List<string> GetChars()
        {
            List<string> fr = new List<string>();
            try
            {

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(HOST + "/Answer.php");
                request.Method = "POST";
                request.Accept = "gzip, deflate";
                request.Proxy = null;
                request.Timeout = 15000;
                request.KeepAlive = true;
                request.UserAgent = "GIG_CLIENT/UserAgent 1.0";
                request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
                string postData = "token=GETCL&username="+MyAccount.Username+"&AT=" + AccessToken;
                byte[] byteArray = Encoding.UTF8.GetBytes(postData);
                // Set the ContentType property of the WebRequest.
                request.ContentType = "application/x-www-form-urlencoded";
                // Set the ContentLength property of the WebRequest.
                request.ContentLength = byteArray.Length;
                // Get the request stream.
                Stream dataStream = request.GetRequestStream();
                // Write the data to the request stream.
                dataStream.Write(byteArray, 0, byteArray.Length);
                // Close the Stream object.
                dataStream.Close();
                // Get the response.
                WebResponse sresponse = request.GetResponse();
                dataStream = sresponse.GetResponseStream();

                StreamReader reader = new StreamReader(dataStream);

                string responseFromServer = reader.ReadToEnd();
                if (responseFromServer.Contains("OK:"))
                {

                    reader.Close();
                    dataStream.Close();
                    sresponse.Close();
                    string p = GIGDSplitter.Split(UnCleanData(responseFromServer).Replace("OK:", ""), 3)[1];
                    string[] x = NLSplitter.Split(p);

                    foreach (string veh in x)
                    {
                        if (veh.Length > 0)
                        {
                            fr.Add(veh);
                        }
                    }

                    return fr;



                }
                else if (responseFromServer.Contains("DENIED:"))
                {
                    ErrorMSG = GIGDSplitter.Split(UnCleanData(responseFromServer).Replace("DENIED:", ""), 3)[1];
                    return fr;
                }
                else
                    return fr;
            }
            catch
            {
                return fr;
            }
        }
        public List<GFriendRequests> GetFriendRequests(string user)
        {
            List<GFriendRequests> pg = new List<GFriendRequests>();
            try
            {

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(HOST + "/Answer.php");
                request.Method = "POST";
                request.Accept = "gzip, deflate";
                request.Proxy = null;
                request.Timeout = 15000;
                request.KeepAlive = true;
                request.UserAgent = "GIG_CLIENT/UserAgent 1.0";
                request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
                string postData = "token=GETFREQ&username=" + user + "&AT=" + AccessToken;
                byte[] byteArray = Encoding.UTF8.GetBytes(postData);
                // Set the ContentType property of the WebRequest.
                request.ContentType = "application/x-www-form-urlencoded";
                // Set the ContentLength property of the WebRequest.
                request.ContentLength = byteArray.Length;
                // Get the request stream.
                Stream dataStream = request.GetRequestStream();
                // Write the data to the request stream.
                dataStream.Write(byteArray, 0, byteArray.Length);
                // Close the Stream object.
                dataStream.Close();
                // Get the response.
                WebResponse sresponse = request.GetResponse();
                dataStream = sresponse.GetResponseStream();

                StreamReader reader = new StreamReader(dataStream);

                string responseFromServer = reader.ReadToEnd();
                if (responseFromServer.Contains("OK:"))
                {

                    reader.Close();
                    dataStream.Close();
                    sresponse.Close();
                    string p = GIGDSplitter.Split(UnCleanData(responseFromServer).Replace("OK:", ""), 3)[1];
                    string[] x = NLSplitter.Split(p);
                    foreach (string xa in x)
                    {
                        if (xa.Length > 0)
                        {
                            string[] a = SPSplitter.Split(xa, 3);
                            GFriendRequests fr = new GFriendRequests();
                            fr.Friend = a[0];
                            fr.Message = Utils.FromHex(a[1]);
                       
                            pg.Add(fr);
                        }
                    }

                    return pg;



                }
                else if (responseFromServer.Contains("DENIED:"))
                {
                    ErrorMSG = GIGDSplitter.Split(UnCleanData(responseFromServer).Replace("DENIED:", ""), 3)[1];
                    return pg;
                }
                else
                    return pg;
            }
            catch
            {
                return pg;
            }
        }
        public List<GLog> GetLogs(string user)
        {
            List<GLog> pg = new List<GLog>();
            try
            {

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(HOST + "/Admin.php");
                request.Method = "POST";
                request.Accept = "gzip, deflate";
                request.Proxy = null;
                request.Timeout = 15000;
                request.KeepAlive = true;
                request.UserAgent = "GIG_CLIENT/UserAgent 1.0";
                request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
                string postData = "token=SHOWLOG&username=" + user + "&AT=" + AccessToken;
                byte[] byteArray = Encoding.UTF8.GetBytes(postData);
                // Set the ContentType property of the WebRequest.
                request.ContentType = "application/x-www-form-urlencoded";
                // Set the ContentLength property of the WebRequest.
                request.ContentLength = byteArray.Length;
                // Get the request stream.
                Stream dataStream = request.GetRequestStream();
                // Write the data to the request stream.
                dataStream.Write(byteArray, 0, byteArray.Length);
                // Close the Stream object.
                dataStream.Close();
                // Get the response.
                WebResponse sresponse = request.GetResponse();
                dataStream = sresponse.GetResponseStream();

                StreamReader reader = new StreamReader(dataStream);

                string responseFromServer = reader.ReadToEnd();
                if (responseFromServer.Contains("OK:"))
                {

                    reader.Close();
                    dataStream.Close();
                    sresponse.Close();
                    string p = GIGDSplitter.Split(UnCleanData(responseFromServer).Replace("OK:", ""), 3)[1];
                    string[] x = NLSplitter.Split(p);
                    foreach (string xa in x)
                    {
                        if (xa.Length > 0)
                        {
                            string[] a = SPSplitter.Split(xa, 3);
                            GLog fr = new GLog();
                            fr.LogText = a[0];
                            fr.LogDate =DateTime.Parse(a[1]);

                            pg.Add(fr);
                        }
                    }

                    return pg;



                }
                else if (responseFromServer.Contains("DENIED:"))
                {
                    ErrorMSG = GIGDSplitter.Split(UnCleanData(responseFromServer).Replace("DENIED:", ""), 3)[1];
                    return pg;
                }
                else
                    return pg;
            }
            catch
            {
                return pg;
            }
        }
        public List<GigCommand> GetCommands(string type)
        {
            List<GigCommand> pg = new List<GigCommand>();
            try
            {

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(HOST + "/Admin.php");
                request.Method = "POST";
                request.Accept = "gzip, deflate";
                request.Proxy = null;
                request.Timeout = 15000;
                request.KeepAlive = true;
                request.UserAgent = "GIG_CLIENT/UserAgent 1.0";
                request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
                string postData = "token=SHOWCMD&cmdt=" + type + "&username="+Username+"&AT=" + AccessToken;
                byte[] byteArray = Encoding.UTF8.GetBytes(postData);
                // Set the ContentType property of the WebRequest.
                request.ContentType = "application/x-www-form-urlencoded";
                // Set the ContentLength property of the WebRequest.
                request.ContentLength = byteArray.Length;
                // Get the request stream.
                Stream dataStream = request.GetRequestStream();
                // Write the data to the request stream.
                dataStream.Write(byteArray, 0, byteArray.Length);
                // Close the Stream object.
                dataStream.Close();
                // Get the response.
                WebResponse sresponse = request.GetResponse();
                dataStream = sresponse.GetResponseStream();

                StreamReader reader = new StreamReader(dataStream);

                string responseFromServer = reader.ReadToEnd();
                if (responseFromServer.Contains("OK:"))
                {

                    reader.Close();
                    dataStream.Close();
                    sresponse.Close();
                    string p = GIGDSplitter.Split(UnCleanData(responseFromServer).Replace("OK:", ""), 3)[1];
                    string[] x = NLSplitter.Split(p);

                    foreach (string veh in x)
                    {
                        if (veh.Length > 0)
                        {

                            string[] s = SPSplitter.Split(veh);
                            GigCommand cmd = new GigCommand();
                            cmd.SID = s[0];
                            cmd.Username = s[1];
                            cmd.GTAUsername = s[2];
                            cmd.Email = s[3];
                            cmd.ContactEmail = s[4];
                            cmd.TimeStamp = DateTime.Parse(s[5]);
                            cmd.Pack = s[6];
                            cmd.Name = s[7];
                            cmd.Option = s[8];
                            if(s[9] == "1") cmd.Status = CommandStatus.VALIDATED;
                            cmd.Price = int.Parse(s[10]);
                            pg.Add(cmd);
                        }
                    }

                    return pg;



                }
                else if (responseFromServer.Contains("DENIED:"))
                {
                    ErrorMSG = GIGDSplitter.Split(UnCleanData(responseFromServer).Replace("DENIED:", ""), 3)[1];
                    return pg;
                }
                else
                    return pg;
            }
            catch
            {
                return pg;
            }
        }
        public GCharacter GetCharInfo(string username)
        {
            GCharacter user = new GCharacter();
            try
            {

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(HOST + "/Answer.php");
                request.Method = "POST";
                request.Accept = "gzip, deflate";
                request.Proxy = null;
                request.Timeout = 15000;
                request.KeepAlive = true;
                request.UserAgent = "GIG_CLIENT/UserAgent 1.0";
                request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
                string postData = "token=GETCI&gtaun=" + username + "&AT=" + AccessToken;
                byte[] byteArray = Encoding.UTF8.GetBytes(postData);
                // Set the ContentType property of the WebRequest.
                request.ContentType = "application/x-www-form-urlencoded";
                // Set the ContentLength property of the WebRequest.
                request.ContentLength = byteArray.Length;
                // Get the request stream.
                Stream dataStream = request.GetRequestStream();
                // Write the data to the request stream.
                dataStream.Write(byteArray, 0, byteArray.Length);
                // Close the Stream object.
                dataStream.Close();
                // Get the response.
                WebResponse sresponse = request.GetResponse();
                dataStream = sresponse.GetResponseStream();

                StreamReader reader = new StreamReader(dataStream);

                string responseFromServer = reader.ReadToEnd();
                if (responseFromServer.Contains("OK:"))
                {
                    reader.Close();
                    dataStream.Close();
                    sresponse.Close();
                    string p = GIGDSplitter.Split(UnCleanData(responseFromServer).Replace("OK:", ""), 3)[1];
                    string[] ln = NLSplitter.Split(p);
     
                    user.Name = ln[0];
                    user.Firstname = ln[1];
                    user.Skin = ushort.Parse(ln[2]);

                    user.Job = ushort.Parse(ln[3]);
                    user.PayDay = ulong.Parse(ln[4]);
                    user.Faction =ushort.Parse(ln[5]);
                    user.Rank = ushort.Parse(ln[6]);
                    user.Level = byte.Parse(ln[7]);
          

 
                    return user;



                }
                else if (responseFromServer.Contains("DENIED:"))
                {
                    ErrorMSG = GIGDSplitter.Split(UnCleanData(responseFromServer).Replace("DENIED:", ""), 3)[1];
                    return user;
                }
                else
                    return user;
            }
            catch
            {
                return user;
            }
        }
        public List<string> GetNotifications(string user)
        {
            List<string> pg = new List<string>();
            try
            {

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(HOST + "/Answer.php");
                request.Method = "POST";
                request.Accept = "gzip, deflate";
                request.Proxy = null;
                request.Timeout = 15000;
                request.KeepAlive = true;
                request.UserAgent = "GIG_CLIENT/UserAgent 1.0";
                request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
                string postData = "token=GETNOTIF&username=" + user + "&AT=" + AccessToken;
                byte[] byteArray = Encoding.UTF8.GetBytes(postData);
                // Set the ContentType property of the WebRequest.
                request.ContentType = "application/x-www-form-urlencoded";
                // Set the ContentLength property of the WebRequest.
                request.ContentLength = byteArray.Length;
                // Get the request stream.
                Stream dataStream = request.GetRequestStream();
                // Write the data to the request stream.
                dataStream.Write(byteArray, 0, byteArray.Length);
                // Close the Stream object.
                dataStream.Close();
                // Get the response.
                WebResponse sresponse = request.GetResponse();
                dataStream = sresponse.GetResponseStream();

                StreamReader reader = new StreamReader(dataStream);

                string responseFromServer = reader.ReadToEnd();
                if (responseFromServer.Contains("OK:"))
                {

                    reader.Close();
                    dataStream.Close();
                    sresponse.Close();
                    string p = GIGDSplitter.Split(UnCleanData(responseFromServer).Replace("OK:", ""), 3)[1];
                    string[] x = NLSplitter.Split(p);

                    foreach (string veh in x)
                    {
                        if (veh.Length > 0)
                        {
                            pg.Add(veh);
                        }
                    }

                    return pg;



                }
                else if (responseFromServer.Contains("DENIED:"))
                {
                    ErrorMSG = GIGDSplitter.Split(UnCleanData(responseFromServer).Replace("DENIED:", ""), 3)[1];
                    return pg;
                }
                else
                    return pg;
            }
            catch
            {
                return pg;
            }
        }
        // Admins
        public bool Notify(string username, string notif)
        {

            try
            {

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(HOST + "/Admin.php");
                request.Method = "POST";
                request.Accept = "gzip, deflate";
                request.Proxy = null;
                request.Timeout = 15000;
                request.KeepAlive = true;
                request.UserAgent = "GIG_CLIENT/UserAgent 1.0";
                request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
                string postData = "token=NOTIF&notif=" + notif + "&username=" + username + "&AT=" + AccessToken;
                byte[] byteArray = Encoding.UTF8.GetBytes(postData);
                // Set the ContentType property of the WebRequest.
                request.ContentType = "application/x-www-form-urlencoded";
                // Set the ContentLength property of the WebRequest.
                request.ContentLength = byteArray.Length;
                // Get the request stream.
                Stream dataStream = request.GetRequestStream();
                // Write the data to the request stream.
                dataStream.Write(byteArray, 0, byteArray.Length);
                // Close the Stream object.
                dataStream.Close();
                // Get the response.
                WebResponse sresponse = request.GetResponse();
                dataStream = sresponse.GetResponseStream();

                StreamReader reader = new StreamReader(dataStream);

                string responseFromServer = reader.ReadToEnd();
                if (responseFromServer.Contains("OK:"))
                {

                    reader.Close();
                    dataStream.Close();
                    sresponse.Close();

                    return true;



                }
                else if (responseFromServer.Contains("DENIED:"))
                {
                    ErrorMSG = GIGDSplitter.Split(UnCleanData(responseFromServer).Replace("DENIED:", ""), 3)[1];
                    return false;
                }
                else
                    return false;
            }
            catch
            {
                return false;
            }
        }
    
        // Commands

        public bool RemoveCommand(string sid,string tab)
        {

            try
            {

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(HOST + "/Admin.php");
                request.Method = "POST";
                request.Accept = "gzip, deflate";
                request.Proxy = null;
                request.Timeout = 15000;
                request.KeepAlive = true;
                request.UserAgent = "GIG_CLIENT/UserAgent 1.0";
                request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
                string postData = "token=REMCMD&sid=" + sid+"&cmdt="+tab + "&AT=" + AccessToken;
                byte[] byteArray = Encoding.UTF8.GetBytes(postData);
                // Set the ContentType property of the WebRequest.
                request.ContentType = "application/x-www-form-urlencoded";
                // Set the ContentLength property of the WebRequest.
                request.ContentLength = byteArray.Length;
                // Get the request stream.
                Stream dataStream = request.GetRequestStream();
                // Write the data to the request stream.
                dataStream.Write(byteArray, 0, byteArray.Length);
                // Close the Stream object.
                dataStream.Close();
                // Get the response.
                WebResponse sresponse = request.GetResponse();
                dataStream = sresponse.GetResponseStream();

                StreamReader reader = new StreamReader(dataStream);

                string responseFromServer = reader.ReadToEnd();
                if (responseFromServer.Contains("OK:"))
                {

                    reader.Close();
                    dataStream.Close();
                    sresponse.Close();

                    return true;



                }
                else if (responseFromServer.Contains("DENIED:"))
                {
                    ErrorMSG = GIGDSplitter.Split(UnCleanData(responseFromServer).Replace("DENIED:", ""), 3)[1];
                    return false;
                }
                else
                    return false;
            }
            catch
            {
                return false;
            }
        }
        public bool ValidateCommand(string sid, string tab)
        {

            try
            {

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(HOST + "/Admin.php");
                request.Method = "POST";
                request.Accept = "gzip, deflate";
                request.Proxy = null;
                request.Timeout = 15000;
                request.KeepAlive = true;
                request.UserAgent = "GIG_CLIENT/UserAgent 1.0";
                request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
                string postData = "token=VALCMD&sid=" + sid + "&cmdt=" + tab + "&AT=" + AccessToken;
                byte[] byteArray = Encoding.UTF8.GetBytes(postData);
                // Set the ContentType property of the WebRequest.
                request.ContentType = "application/x-www-form-urlencoded";
                // Set the ContentLength property of the WebRequest.
                request.ContentLength = byteArray.Length;
                // Get the request stream.
                Stream dataStream = request.GetRequestStream();
                // Write the data to the request stream.
                dataStream.Write(byteArray, 0, byteArray.Length);
                // Close the Stream object.
                dataStream.Close();
                // Get the response.
                WebResponse sresponse = request.GetResponse();
                dataStream = sresponse.GetResponseStream();

                StreamReader reader = new StreamReader(dataStream);

                string responseFromServer = reader.ReadToEnd();
                if (responseFromServer.Contains("OK:"))
                {

                    reader.Close();
                    dataStream.Close();
                    sresponse.Close();

                    return true;



                }
                else if (responseFromServer.Contains("DENIED:"))
                {
                    ErrorMSG = GIGDSplitter.Split(UnCleanData(responseFromServer).Replace("DENIED:", ""), 3)[1];
                    return false;
                }
                else
                    return false;
            }
            catch
            {
                return false;
            }
        }
        public bool RemoveAllCommand(string tab)
        {

            try
            {

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(HOST + "/Admin.php");
                request.Method = "POST";
                request.Accept = "gzip, deflate";
                request.Proxy = null;
                request.Timeout = 15000;
                request.KeepAlive = true;
                request.UserAgent = "GIG_CLIENT/UserAgent 1.0";
                request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
                string postData = "token=REMACMD&cmdt=" + tab + "&AT=" + AccessToken;
                byte[] byteArray = Encoding.UTF8.GetBytes(postData);
                // Set the ContentType property of the WebRequest.
                request.ContentType = "application/x-www-form-urlencoded";
                // Set the ContentLength property of the WebRequest.
                request.ContentLength = byteArray.Length;
                // Get the request stream.
                Stream dataStream = request.GetRequestStream();
                // Write the data to the request stream.
                dataStream.Write(byteArray, 0, byteArray.Length);
                // Close the Stream object.
                dataStream.Close();
                // Get the response.
                WebResponse sresponse = request.GetResponse();
                dataStream = sresponse.GetResponseStream();

                StreamReader reader = new StreamReader(dataStream);

                string responseFromServer = reader.ReadToEnd();
                if (responseFromServer.Contains("OK:"))
                {

                    reader.Close();
                    dataStream.Close();
                    sresponse.Close();

                    return true;



                }
                else if (responseFromServer.Contains("DENIED:"))
                {
                    ErrorMSG = GIGDSplitter.Split(UnCleanData(responseFromServer).Replace("DENIED:", ""), 3)[1];
                    return false;
                }
                else
                    return false;
            }
            catch
            {
                return false;
            }
        }
        public bool RemovAllValidatedCommand(string tab)
        {

            try
            {

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(HOST + "/Admin.php");
                request.Method = "POST";
                request.Accept = "gzip, deflate";
                request.Proxy = null;
                request.Timeout = 15000;
                request.KeepAlive = true;
                request.UserAgent = "GIG_CLIENT/UserAgent 1.0";
                request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
                string postData = "token=REMAVCMD&cmdt=" + tab + "&AT=" + AccessToken;
                byte[] byteArray = Encoding.UTF8.GetBytes(postData);
                // Set the ContentType property of the WebRequest.
                request.ContentType = "application/x-www-form-urlencoded";
                // Set the ContentLength property of the WebRequest.
                request.ContentLength = byteArray.Length;
                // Get the request stream.
                Stream dataStream = request.GetRequestStream();
                // Write the data to the request stream.
                dataStream.Write(byteArray, 0, byteArray.Length);
                // Close the Stream object.
                dataStream.Close();
                // Get the response.
                WebResponse sresponse = request.GetResponse();
                dataStream = sresponse.GetResponseStream();

                StreamReader reader = new StreamReader(dataStream);

                string responseFromServer = reader.ReadToEnd();
                if (responseFromServer.Contains("OK:"))
                {

                    reader.Close();
                    dataStream.Close();
                    sresponse.Close();

                    return true;



                }
                else if (responseFromServer.Contains("DENIED:"))
                {
                    ErrorMSG = GIGDSplitter.Split(UnCleanData(responseFromServer).Replace("DENIED:", ""), 3)[1];
                    return false;
                }
                else
                    return false;
            }
            catch
            {
                return false;
            }
        }
      // Products

        public bool AddProduct(string name, string desc, int qte, ulong prix, byte type)
        {

            try
            {

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(HOST + "/Admin.php");
                request.Method = "POST";
                request.Accept = "gzip, deflate";
                request.Proxy = null;
                request.Timeout = 15000;
                request.KeepAlive = true;
                request.UserAgent = "GIG_CLIENT/UserAgent 1.0";
                request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
                string postData = "token=ADDPROD&name=" + name + "&qte=" + qte.ToString() +  "&prix=" + prix.ToString() + "&type=" + type.ToString() + "&desc=" +CleanData(desc) + "&AT=" + AccessToken;
                byte[] byteArray = Encoding.UTF8.GetBytes(postData);
                // Set the ContentType property of the WebRequest.
                request.ContentType = "application/x-www-form-urlencoded";
                // Set the ContentLength property of the WebRequest.
                request.ContentLength = byteArray.Length;
                // Get the request stream.
                Stream dataStream = request.GetRequestStream();
                // Write the data to the request stream.
                dataStream.Write(byteArray, 0, byteArray.Length);
                // Close the Stream object.
                dataStream.Close();
                // Get the response.
                WebResponse sresponse = request.GetResponse();
                dataStream = sresponse.GetResponseStream();

                StreamReader reader = new StreamReader(dataStream);

                string responseFromServer = reader.ReadToEnd();
                if (responseFromServer.Contains("OK:"))
                {

                    reader.Close();
                    dataStream.Close();
                    sresponse.Close();

                    return true;



                }
                else if (responseFromServer.Contains("DENIED:"))
                {
                    ErrorMSG = GIGDSplitter.Split(UnCleanData(responseFromServer).Replace("DENIED:", ""), 3)[1];
                    return false;
                }
                else
                    return false;
            }
            catch
            {
                return false;
            }
        }
        public bool AddPictures(string pid, string img,string img1,string img2)
        {

            try
            {

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(HOST + "/Admin.php");
                request.Method = "POST";
                request.Accept = "gzip, deflate";
                request.Proxy = null;
                request.Timeout = 15000;
                request.KeepAlive = true;
                request.UserAgent = "GIG_CLIENT/UserAgent 1.0";
                request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
                string postData = "token=ADDPICS&pid=" + pid + "&img=" + CleanData(img) + "&img1=" + CleanData(img1) + "&img2=" + CleanData(img2) + "&AT=" + AccessToken;
                byte[] byteArray = Encoding.UTF8.GetBytes(postData);
                // Set the ContentType property of the WebRequest.
                request.ContentType = "application/x-www-form-urlencoded";
                // Set the ContentLength property of the WebRequest.
                request.ContentLength = byteArray.Length;
                // Get the request stream.
                Stream dataStream = request.GetRequestStream();
                // Write the data to the request stream.
                dataStream.Write(byteArray, 0, byteArray.Length);
                // Close the Stream object.
                dataStream.Close();
                // Get the response.
                WebResponse sresponse = request.GetResponse();
                dataStream = sresponse.GetResponseStream();

                StreamReader reader = new StreamReader(dataStream);

                string responseFromServer = reader.ReadToEnd();
                if (responseFromServer.Contains("OK:"))
                {

                    reader.Close();
                    dataStream.Close();
                    sresponse.Close();

                    return true;



                }
                else if (responseFromServer.Contains("DENIED:"))
                {
                    ErrorMSG = GIGDSplitter.Split(UnCleanData(responseFromServer).Replace("DENIED:", ""), 3)[1];
                    return false;
                }
                else
                    return false;
            }
            catch
            {
                return false;
            }
        }
        public bool RemoveProduct(string pid)
        {

            try
            {

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(HOST + "/Admin.php");
                request.Method = "POST";
                request.Accept = "gzip, deflate";
                request.Proxy = null;
                request.Timeout = 15000;
                request.KeepAlive = true;
                request.UserAgent = "GIG_CLIENT/UserAgent 1.0";
                request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
                string postData = "token=REMPROD&pid=" + pid + "&AT=" + AccessToken;
                byte[] byteArray = Encoding.UTF8.GetBytes(postData);
                // Set the ContentType property of the WebRequest.
                request.ContentType = "application/x-www-form-urlencoded";
                // Set the ContentLength property of the WebRequest.
                request.ContentLength = byteArray.Length;
                // Get the request stream.
                Stream dataStream = request.GetRequestStream();
                // Write the data to the request stream.
                dataStream.Write(byteArray, 0, byteArray.Length);
                // Close the Stream object.
                dataStream.Close();
                // Get the response.
                WebResponse sresponse = request.GetResponse();
                dataStream = sresponse.GetResponseStream();

                StreamReader reader = new StreamReader(dataStream);

                string responseFromServer = reader.ReadToEnd();
                if (responseFromServer.Contains("OK:"))
                {

                    reader.Close();
                    dataStream.Close();
                    sresponse.Close();

                    return true;



                }
                else if (responseFromServer.Contains("DENIED:"))
                {
                    ErrorMSG = GIGDSplitter.Split(UnCleanData(responseFromServer).Replace("DENIED:", ""), 3)[1];
                    return false;
                }
                else
                    return false;
            }
            catch
            {
                return false;
            }
        }
        public bool ModifyProduct(string pid,string name, string desc, int qte, ulong prix, byte type)
        {

            try
            {

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(HOST + "/Admin.php");
                request.Method = "POST";
                request.Accept = "gzip, deflate";
                request.Proxy = null;
                request.Timeout = 15000;
                request.KeepAlive = true;
                request.UserAgent = "GIG_CLIENT/UserAgent 1.0";
                request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
                string postData = "token=MODPROD&pid="+pid+"&name=" + name + "&qte=" + qte.ToString() + "&prix=" + prix.ToString() + "&type=" + type.ToString() + "&desc=" + CleanData(desc) + "&AT=" + AccessToken;
                byte[] byteArray = Encoding.UTF8.GetBytes(postData);
                // Set the ContentType property of the WebRequest.
                request.ContentType = "application/x-www-form-urlencoded";
                // Set the ContentLength property of the WebRequest.
                request.ContentLength = byteArray.Length;
                // Get the request stream.
                Stream dataStream = request.GetRequestStream();
                // Write the data to the request stream.
                dataStream.Write(byteArray, 0, byteArray.Length);
                // Close the Stream object.
                dataStream.Close();
                // Get the response.
                WebResponse sresponse = request.GetResponse();
                dataStream = sresponse.GetResponseStream();

                StreamReader reader = new StreamReader(dataStream);

                string responseFromServer = reader.ReadToEnd();
                if (responseFromServer.Contains("OK:"))
                {

                    reader.Close();
                    dataStream.Close();
                    sresponse.Close();

                    return true;



                }
                else if (responseFromServer.Contains("DENIED:"))
                {
                    ErrorMSG = GIGDSplitter.Split(UnCleanData(responseFromServer).Replace("DENIED:", ""), 3)[1];
                    return false;
                }
                else
                    return false;
            }
            catch
            {
                return false;
            }
        }
        public bool ModifyPictures(string pid, string img, string img1, string img2)
        {

            try
            {

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(HOST + "/Admin.php");
                request.Method = "POST";
                request.Accept = "gzip, deflate";
                request.Proxy = null;
                request.Timeout = 15000;
                request.KeepAlive = true;
                request.UserAgent = "GIG_CLIENT/UserAgent 1.0";
                request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
                string postData = "token=MODPICS&pid=" + pid + "&img=" + CleanData(img) + "&img1=" + CleanData(img1) + "&img2=" + CleanData(img2) + "&AT=" + AccessToken;
                byte[] byteArray = Encoding.UTF8.GetBytes(postData);
                // Set the ContentType property of the WebRequest.
                request.ContentType = "application/x-www-form-urlencoded";
                // Set the ContentLength property of the WebRequest.
                request.ContentLength = byteArray.Length;
                // Get the request stream.
                Stream dataStream = request.GetRequestStream();
                // Write the data to the request stream.
                dataStream.Write(byteArray, 0, byteArray.Length);
                // Close the Stream object.
                dataStream.Close();
                // Get the response.
                WebResponse sresponse = request.GetResponse();
                dataStream = sresponse.GetResponseStream();

                StreamReader reader = new StreamReader(dataStream);

                string responseFromServer = reader.ReadToEnd();
                if (responseFromServer.Contains("OK:"))
                {

                    reader.Close();
                    dataStream.Close();
                    sresponse.Close();

                    return true;



                }
                else if (responseFromServer.Contains("DENIED:"))
                {
                    ErrorMSG = GIGDSplitter.Split(UnCleanData(responseFromServer).Replace("DENIED:", ""), 3)[1];
                    return false;
                }
                else
                    return false;
            }
            catch
            {
                return false;
            }
        }


        public bool AddVehicle(string pid, string Color1, string Color2, byte Sale, byte Featured, ushort Speed, ushort Fuel, byte Places, byte Tuning, ulong PrixIG, byte Water, string Category)
        {

            try
            {

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(HOST + "/Admin.php");
                request.Method = "POST";
                request.Accept = "gzip, deflate";
                request.Proxy = null;
                request.Timeout = 15000;
                request.KeepAlive = true;
                request.UserAgent = "GIG_CLIENT/UserAgent 1.0";
                request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
                string postData = "token=ADDVEH&AT=" + AccessToken + "&pid=" + pid + "&color=" + Color1 + "&color2=" + Color2 + "&sale=" + Sale.ToString() + "&featured=" + Featured.ToString() + "&speed=" + Speed.ToString() + "&fuel=" + Fuel.ToString() + "&places=" + Places.ToString() + "&tuning=" + Tuning.ToString() + "&prixig=" + PrixIG.ToString() + "&water=" + Water.ToString() + "&category=" + Category;
                byte[] byteArray = Encoding.UTF8.GetBytes(postData);
                // Set the ContentType property of the WebRequest.
                request.ContentType = "application/x-www-form-urlencoded";
                // Set the ContentLength property of the WebRequest.
                request.ContentLength = byteArray.Length;
                // Get the request stream.
                Stream dataStream = request.GetRequestStream();
                // Write the data to the request stream.
                dataStream.Write(byteArray, 0, byteArray.Length);
                // Close the Stream object.
                dataStream.Close();
                // Get the response.
                WebResponse sresponse = request.GetResponse();
                dataStream = sresponse.GetResponseStream();

                StreamReader reader = new StreamReader(dataStream);

                string responseFromServer = reader.ReadToEnd();
                if (responseFromServer.Contains("OK:"))
                {

                    reader.Close();
                    dataStream.Close();
                    sresponse.Close();

                    return true;



                }
                else if (responseFromServer.Contains("DENIED:"))
                {
                    ErrorMSG = GIGDSplitter.Split(UnCleanData(responseFromServer).Replace("DENIED:", ""), 3)[1];
                    return false;
                }
                else
                    return false;
            }
            catch
            {
                return false;
            }
        }
        public bool AddHouse(string pid,string Pieces, string IdInt, string Sale, byte Featured, string Ville, ushort Popularity, string Garage, string GarageMap, string Wall, string WallMap, ulong PrixIG, string Jardin, string Piscine, string Category)
        {

            try
            {

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(HOST + "/Admin.php");
                request.Method = "POST";
                request.Accept = "gzip, deflate";
                request.Proxy = null;
                request.Timeout = 15000;
                request.KeepAlive = true;
                request.UserAgent = "GIG_CLIENT/UserAgent 1.0";
                request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
                string postData = "token=ADDHOU&AT=" + AccessToken + "&pid=" + pid + "&pieces=" + Pieces + "&idint=" + IdInt + "&sale=" + Sale + "&featured=" + Featured.ToString() + "&ville=" + Ville + "&popularity=" + Popularity.ToString() + "&garage=" + Garage + "&garagemap=" + GarageMap + "&wall=" + Wall + "&wallmap=" + WallMap + "&jardin=" + Jardin + "&piscine=" + Piscine + "&prixig=" + PrixIG.ToString() +  "&category=" + Category;
                byte[] byteArray = Encoding.UTF8.GetBytes(postData);
                // Set the ContentType property of the WebRequest.
                request.ContentType = "application/x-www-form-urlencoded";
                // Set the ContentLength property of the WebRequest.
                request.ContentLength = byteArray.Length;
                // Get the request stream.
                Stream dataStream = request.GetRequestStream();
                // Write the data to the request stream.
                dataStream.Write(byteArray, 0, byteArray.Length);
                // Close the Stream object.
                dataStream.Close();
                // Get the response.
                WebResponse sresponse = request.GetResponse();
                dataStream = sresponse.GetResponseStream();

                StreamReader reader = new StreamReader(dataStream);

                string responseFromServer = reader.ReadToEnd();
                if (responseFromServer.Contains("OK:"))
                {

                    reader.Close();
                    dataStream.Close();
                    sresponse.Close();

                    return true;



                }
                else if (responseFromServer.Contains("DENIED:"))
                {
                    ErrorMSG = GIGDSplitter.Split(UnCleanData(responseFromServer).Replace("DENIED:", ""), 3)[1];
                    return false;
                }
                else
                    return false;
            }
            catch
            {
                return false;
            }
        }
        public bool AddBizz(string pid, string Stock, string Sale, byte Featured, string Ville, ushort Popularity, string Depot, string DepotMap, ulong PrixIG, string Category)
        {

            try
            {

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(HOST + "/Admin.php");
                request.Method = "POST";
                request.Accept = "gzip, deflate";
                request.Proxy = null;
                request.Timeout = 15000;
                request.KeepAlive = true;
                request.UserAgent = "GIG_CLIENT/UserAgent 1.0";
                request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
                string postData = "token=ADDBIZ&AT=" + AccessToken + "&pid=" + pid + "&stock=" + Stock +  "&sale=" + Sale + "&featured=" + Featured.ToString() + "&ville=" + Ville + "&popularity=" + Popularity.ToString() + "&depot=" + Depot + "&depotmap=" + DepotMap + "&prixig=" + PrixIG.ToString() + "&category=" + Category;
                byte[] byteArray = Encoding.UTF8.GetBytes(postData);
                // Set the ContentType property of the WebRequest.
                request.ContentType = "application/x-www-form-urlencoded";
                // Set the ContentLength property of the WebRequest.
                request.ContentLength = byteArray.Length;
                // Get the request stream.
                Stream dataStream = request.GetRequestStream();
                // Write the data to the request stream.
                dataStream.Write(byteArray, 0, byteArray.Length);
                // Close the Stream object.
                dataStream.Close();
                // Get the response.
                WebResponse sresponse = request.GetResponse();
                dataStream = sresponse.GetResponseStream();

                StreamReader reader = new StreamReader(dataStream);

                string responseFromServer = reader.ReadToEnd();
                if (responseFromServer.Contains("OK:"))
                {

                    reader.Close();
                    dataStream.Close();
                    sresponse.Close();

                    return true;



                }
                else if (responseFromServer.Contains("DENIED:"))
                {
                    ErrorMSG = GIGDSplitter.Split(UnCleanData(responseFromServer).Replace("DENIED:", ""), 3)[1];
                    return false;
                }
                else
                    return false;
            }
            catch
            {
                return false;
            }
        }
        public List<GProduct> GetProducts(string limit)
        {
            List<GProduct> pg = new List<GProduct>();
            try
            {

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(HOST + "/Admin.php");
                request.Method = "POST";
                request.Accept = "gzip, deflate";
                request.Proxy = null;
                request.Timeout = 15000;
                request.KeepAlive = true;
                request.UserAgent = "GIG_CLIENT/UserAgent 1.0";
                request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
                string postData = "token=SHOWPROD&lim="+limit+ "&AT=" + AccessToken;
                byte[] byteArray = Encoding.UTF8.GetBytes(postData);
                // Set the ContentType property of the WebRequest.
                request.ContentType = "application/x-www-form-urlencoded";
                // Set the ContentLength property of the WebRequest.
                request.ContentLength = byteArray.Length;
                // Get the request stream.
                Stream dataStream = request.GetRequestStream();
                // Write the data to the request stream.
                dataStream.Write(byteArray, 0, byteArray.Length);
                // Close the Stream object.
                dataStream.Close();
                // Get the response.
                WebResponse sresponse = request.GetResponse();
                dataStream = sresponse.GetResponseStream();

                StreamReader reader = new StreamReader(dataStream);

                string responseFromServer = reader.ReadToEnd();
                if (responseFromServer.Contains("OK:"))
                {

                    reader.Close();
                    dataStream.Close();
                    sresponse.Close();
                    string p = GIGDSplitter.Split(UnCleanData(responseFromServer).Replace("OK:", ""), 3)[1];
                    string[] x = NLSplitter.Split(p);

                    foreach (string veh in x)
                    {
                        if (veh.Length > 0)
                        {

                            string[] s = SPSplitter.Split(veh);
                            GProduct cmd = new GProduct();
        
                            cmd.ProductID = ulong.Parse(s[0]);
                            cmd.Name = s[1];
                            cmd.Description = s[2];
                            cmd.Quantity = ushort.Parse(s[3]);
                            cmd.Price = ulong.Parse(s[4]);
                            cmd.ProductType = (GProductType)byte.Parse(s[5]);
                            cmd.ProductDate = DateTime.Parse(s[6]);

                      
                            pg.Add(cmd);
                        }
                    }

                    return pg;



                }
                else if (responseFromServer.Contains("DENIED:"))
                {
                    ErrorMSG = GIGDSplitter.Split(UnCleanData(responseFromServer).Replace("DENIED:", ""), 3)[1];
                    return pg;
                }
                else
                    return pg;
            }
            catch
            {
                return pg;
            }
        }


        public bool ModifyUser(string username, string name, string email, string role)
        {

            try
            {

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(HOST + "/Admin.php");
                request.Method = "POST";
                request.Accept = "gzip, deflate";
                request.Proxy = null;
                request.Timeout = 15000;
                request.KeepAlive = true;
                request.UserAgent = "GIG_CLIENT/UserAgent 1.0";
                request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
                string postData = "token=MODUSR&username=" + username + "&name=" + name + "&email=" + email + "&role=" + role + "&AT=" + AccessToken;
                byte[] byteArray = Encoding.UTF8.GetBytes(postData);
                // Set the ContentType property of the WebRequest.
                request.ContentType = "application/x-www-form-urlencoded";
                // Set the ContentLength property of the WebRequest.
                request.ContentLength = byteArray.Length;
                // Get the request stream.
                Stream dataStream = request.GetRequestStream();
                // Write the data to the request stream.
                dataStream.Write(byteArray, 0, byteArray.Length);
                // Close the Stream object.
                dataStream.Close();
                // Get the response.
                WebResponse sresponse = request.GetResponse();
                dataStream = sresponse.GetResponseStream();

                StreamReader reader = new StreamReader(dataStream);

                string responseFromServer = reader.ReadToEnd();
                if (responseFromServer.Contains("OK:"))
                {

                    reader.Close();
                    dataStream.Close();
                    sresponse.Close();

                    return true;



                }
                else if (responseFromServer.Contains("DENIED:"))
                {
                    ErrorMSG = GIGDSplitter.Split(UnCleanData(responseFromServer).Replace("DENIED:", ""), 3)[1];
                    return false;
                }
                else
                    return false;
            }
            catch
            {
                return false;
            }
        }
        public bool RemoveUsers(string username)
        {

            try
            {

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(HOST + "/Admin.php");
                request.Method = "POST";
                request.Accept = "gzip, deflate";
                request.Proxy = null;
                request.Timeout = 15000;
                request.KeepAlive = true;
                request.UserAgent = "GIG_CLIENT/UserAgent 1.0";
                request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
                string postData = "token=REMUSR&username=" + username + "&AT=" + AccessToken;
                byte[] byteArray = Encoding.UTF8.GetBytes(postData);
                // Set the ContentType property of the WebRequest.
                request.ContentType = "application/x-www-form-urlencoded";
                // Set the ContentLength property of the WebRequest.
                request.ContentLength = byteArray.Length;
                // Get the request stream.
                Stream dataStream = request.GetRequestStream();
                // Write the data to the request stream.
                dataStream.Write(byteArray, 0, byteArray.Length);
                // Close the Stream object.
                dataStream.Close();
                // Get the response.
                WebResponse sresponse = request.GetResponse();
                dataStream = sresponse.GetResponseStream();

                StreamReader reader = new StreamReader(dataStream);

                string responseFromServer = reader.ReadToEnd();
                if (responseFromServer.Contains("OK:"))
                {

                    reader.Close();
                    dataStream.Close();
                    sresponse.Close();

                    return true;



                }
                else if (responseFromServer.Contains("DENIED:"))
                {
                    ErrorMSG = GIGDSplitter.Split(UnCleanData(responseFromServer).Replace("DENIED:", ""), 3)[1];
                    return false;
                }
                else
                    return false;
            }
            catch
            {
                return false;
            }
        }
        public bool CreateChar(string username, string gtaun)
        {

            try
            {

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(HOST + "/Admin.php");
                request.Method = "POST";
                request.Accept = "gzip, deflate";
                request.Proxy = null;
                request.Timeout = 15000;
                request.KeepAlive = true;
                request.UserAgent = "GIG_CLIENT/UserAgent 1.0";
                request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
                string postData = "token=CRECHAR&username=" + username + "&gtaun=" + gtaun + "&AT=" + AccessToken;
                byte[] byteArray = Encoding.UTF8.GetBytes(postData);
                // Set the ContentType property of the WebRequest.
                request.ContentType = "application/x-www-form-urlencoded";
                // Set the ContentLength property of the WebRequest.
                request.ContentLength = byteArray.Length;
                // Get the request stream.
                Stream dataStream = request.GetRequestStream();
                // Write the data to the request stream.
                dataStream.Write(byteArray, 0, byteArray.Length);
                // Close the Stream object.
                dataStream.Close();
                // Get the response.
                WebResponse sresponse = request.GetResponse();
                dataStream = sresponse.GetResponseStream();

                StreamReader reader = new StreamReader(dataStream);

                string responseFromServer = reader.ReadToEnd();
                if (responseFromServer.Contains("OK:"))
                {

                    reader.Close();
                    dataStream.Close();
                    sresponse.Close();

                    return true;



                }
                else if (responseFromServer.Contains("DENIED:"))
                {
                    ErrorMSG = GIGDSplitter.Split(UnCleanData(responseFromServer).Replace("DENIED:", ""), 3)[1];
                    return false;
                }
                else
                    return false;
            }
            catch
            {
                return false;
            }
        }


        public List<string> GetAllUsers(int page)
        {
            List<string> pg = new List<string>();
            try
            {

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(HOST + "/Admin.php");
                request.Method = "POST";
                request.Accept = "gzip, deflate";
                request.Proxy = null;
                request.Timeout = 15000;
                request.KeepAlive = true;
                request.UserAgent = "GIG_CLIENT/UserAgent 1.0";
                request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
                string postData = "token=GALLUSR&page=" + page.ToString() + "&AT=" + AccessToken;
                byte[] byteArray = Encoding.UTF8.GetBytes(postData);
                // Set the ContentType property of the WebRequest.
                request.ContentType = "application/x-www-form-urlencoded";
                // Set the ContentLength property of the WebRequest.
                request.ContentLength = byteArray.Length;
                // Get the request stream.
                Stream dataStream = request.GetRequestStream();
                // Write the data to the request stream.
                dataStream.Write(byteArray, 0, byteArray.Length);
                // Close the Stream object.
                dataStream.Close();
                // Get the response.
                WebResponse sresponse = request.GetResponse();
                dataStream = sresponse.GetResponseStream();

                StreamReader reader = new StreamReader(dataStream);

                string responseFromServer = reader.ReadToEnd();
                if (responseFromServer.Contains("OK:"))
                {

                    reader.Close();
                    dataStream.Close();
                    sresponse.Close();
                    string p = GIGDSplitter.Split(UnCleanData(responseFromServer).Replace("OK:", ""), 3)[1];
                    string[] x = NLSplitter.Split(p);

                    foreach (string veh in x)
                    {
                        if (veh.Length > 0)
                        {
                            pg.Add(veh);
                        }
                    }

                    return pg;



                }
                else if (responseFromServer.Contains("DENIED:"))
                {
                    ErrorMSG = GIGDSplitter.Split(UnCleanData(responseFromServer).Replace("DENIED:", ""), 3)[1];
                    return pg;
                }
                else
                    return pg;
            }
            catch
            {
                return pg;
            }
        }


    }
}
