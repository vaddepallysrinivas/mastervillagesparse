using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Configuration;
using System.Text.RegularExpressions;
using System.Web;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Chrome;
using Mono.Web;
using ConsoleApplication1.classes;



namespace ConsoleApplication1
{
    public abstract class SearchBase
    {
        internal static bool IS_IP_CHECKED = false;
        string TestUrl = "http://www.agmarknet.nic.in/agnew/NationalBEnglish/DatewiseCommodityReport.aspx";
        internal string Accept = "text/html, application/xhtml+xml, */*";
        internal const int BUFF_SIZE = 1024;
        internal int RetrySleepMultiplier = 5000;
        internal int Timeout = 20000;
        internal string DOMAIN_URL = "";
        internal string DOMAIN = "";
        internal string _lastResponsePayload = "";
        internal string _lastUrl = "";
        internal string Referer = "";
        internal string LastResponseEncoding = "";
        internal HttpStatusCode LastResponseStatusCode = HttpStatusCode.OK;
        internal Uri LastResponseUri = null;
        internal WebHeaderCollection LastResponseHeaders = null;
        internal bool ClearCookieJarOnTimeout = false;
        internal bool Compression = false;
        internal bool RetryIfRedirected = false;
        internal bool XmlHttpRequest = false;
        internal bool XmlWebService = false;
        private CookieCollection _lastResponseCookies = null;

        internal int _sleepInSecondsHigh = 120;
        internal int _sleepInSecondsLow = 30;

        internal int _sleepHigh = 120000;
        internal int _sleepLow = 30000;

        public SearchBase()
        {

        }

        internal CookieContainer _cookieJar = new CookieContainer();

        internal string GetWebPageNoCookies(string webPageUrl)
        {
            if (!IS_IP_CHECKED)
            {
                IS_IP_CHECKED = true;
                string testPage = GetWebPage(TestUrl, false, null);
                Console.WriteLine(testPage);
                //WriteLog(testPage);
            }

            return GetWebPage(webPageUrl, false, null);
        }



        public string GetProxy()
        {
            var proxies = new string[] { "115.178.97.159:8080", "165.98.68.220:8080", "186.119.136.203:8080" };
            Random rd = new Random();
            return proxies[rd.Next(proxies.Length - 1)];
        }
        public string GetProxyBaseWebPage(string webPageUrl)
        {
            WebClient wc = new WebClient();
            wc.Proxy = new WebProxy(GetProxy());
            wc.Headers.Add(HttpRequestHeader.UserAgent, "Mozilla/5.0 (Windows NT 6.2; WOW64; rv:29.0) Gecko/20100101 Firefox/29.0");
            //wc.Headers.Add("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)");
            //string URL1 = "http://www.yelp.com/search?find_loc={ZIPCODE}&cflt={CATEGORY}";
            //string URL2 = "http://www.yelp.com/search?find_loc={ZIPCODE}&cflt={CATEGORY}&request_origin=user&start={PAGENUMBER}";
            //int totalBusinessCount = 10;
            //int businessCount = 0;
            //int DataPageSize = 10;

            //var url1 = URL1.Replace("{CATEGORY}", "DENTISTS").Replace("{ZIPCODE}", "60803");
            //var url2 = URL2.Replace("{CATEGORY}", "DENTISTS").Replace("{ZIPCODE}", "60803");
            //string datSourceURL = url1;
            //datSourceURL = datSourceURL.Replace("/search?", "/search/snippet?");
            //datSourceURL = datSourceURL.Replace("find_desc=", "find_desc");
            ////datSourceURL = datSourceURL + "&parent_request_id=1e051cfb63e343a7";
            return wc.DownloadString(webPageUrl);

        }


        public string GetWebPage(string webPageUrl, byte[] payLoad)
        {


            if (!IS_IP_CHECKED)
            {
                IS_IP_CHECKED = true;
                string testPage = GetWebPage(TestUrl, false, null);
                Console.WriteLine(testPage);
                // WriteLog(testPage);
            }



            return GetWebPage(webPageUrl, true, payLoad);

        }
        public string GetWebPage(string webPageUrl)
        {


            if (!IS_IP_CHECKED)
            {
                IS_IP_CHECKED = true;
                string testPage = GetWebPage(TestUrl, false, null);
                Console.WriteLine(testPage);
                // WriteLog(testPage);
            }



            return GetWebPage(webPageUrl, true, null);

        }

        public string PostSubmit(string webPageUrl)
        {

            var webPage = string.Empty;
            PostSubmitter post1 = new PostSubmitter();
            post1.Url = webPageUrl;
            post1.PostItems.Add("html", webPage);
            post1.Type = PostSubmitter.PostTypeEnum.Post;
            webPage = post1.Post();


            return webPage;

        }



        public string GetWebPageSelenium(string webPageUrl, List<SelectedControl> objSelecedControls, bool isSubmit)
        {

            var webPage = string.Empty;
            using (var driver = new  ChromeDriver(@"D:\GIT\from nagendra\Mastermandal\ConsoleApplication1\packages\Selenium.WebDriver.ChromeDriver.2.34.0\driver\win32"))
            {
                // Go to the home page
                driver.Navigate().GoToUrl(webPageUrl);

                // Get the page elements
                //var cboYear = driver.FindElementById("cboYear");
               // var cboMonth = driver.FindElementById("cboMonth");
              //  IWebElement cboMonth = driver.FindElement(By.Id("cboMonth"));
              //  List<IWebElement> dateOfBirthOptions = (List<IWebElement>)cboMonth.FindElement(By.TagName("option"));
               // var cboState = driver.FindElementById("cboState");
               // var cboCommodity = driver.FindElementById("cboCommodity");
               

                //var userPasswordField = driver.FindElementById("pwd");
               // var loginButton = driver.FindElementByXPath("//input[@value='Login']");

                // Type user name and password
                //driver.FindElement(By.Id("cboYear")).FindElement(By.XPath(".//option[contains(text(),'2017')]")).Click();
                //driver.FindElement(By.Id("cboMonth")).FindElement(By.XPath(".//option[contains(text(),'June')]")).Click();
                //driver.FindElement(By.Id("cboMonth")).FindElement(By.XPath(".//option[contains(text(),'June')]")).Click();
               // driver.FindElement(By.Id("cboState")).FindElement(By.XPath(".//option[contains(text(),'Telangana')]")).Click();
                //driver.FindElement(By.Id("cboCommodity")).FindElement(By.XPath(".//option[contains(text(),'Banana')]")).Click();
                foreach (var obj in objSelecedControls)
                {
                    driver.FindElement(By.Id(obj.Name)).FindElement(By.XPath(".//option[contains(text(),'"+obj.Text+"')]")).Click();
                }


                if (isSubmit)
                {
                    var loginButton = driver.FindElementByXPath("//input[@value='Submit']");
                    loginButton.Click();
                }

                webPage = driver.PageSource;
               // cboYear.SendKeys("2017");
                //cboMonth.SendKeys("3");

                // and click the login button
                //loginButton.Click();

                // Extract the text and save it into result.txt
              //  var result = driver.FindElementByXPath("//div[@id='case_login']/h3").Text;
                //File.WriteAllText("result.txt", result);

                // Take a screenshot and save it into screen.png
               // driver.GetScreenshot().SaveAsFile(@"screen.png", ImageFormat.Png);
            }
           return Mono.Web.HttpUtility.HtmlDecode(webPage).ToString();
        }
        //public string GetWebPageSelenium(string webPageUrl)
        //{

        //    var webPage = string.Empty;
        //   // IWebDriver driver = new FirefoxDriver();
        //    IWebDriver driver = new ChromeDriver();
        //    driver.Navigate().GoToUrl(webPageUrl);
        //    System.Threading.Thread.Sleep(5000);
        //    driver.Manage().Window.Maximize();
        //    System.Threading.Thread.Sleep(5000);

        //    webPage = driver.PageSource;
        //    if (FindBlocked(webPage))
        //    {

        //        //MailService.SendEmail("Quickr Site Blocked ASAP Respond" + DateTime.Now);
        //        //DialogResult dns = MessageBox.Show("Please Enter Captcha Image...");
        //        //if (dns == DialogResult.OK)
        //        //{
        //        //    System.Threading.Thread.Sleep(15000);
        //        //}
        //    }



        //    webPage = driver.PageSource;
        //    driver.Close();
        //    return Mono.Web.HttpUtility.HtmlDecode(webPage).ToString();
        //}



        bool FindBlocked(string webPage)
        {


            var doc = new HtmlAgilityPack.HtmlDocument();
            doc.LoadHtml(webPage);

            if (doc.DocumentNode.SelectSingleNode("//body[@class='block-page']") != null)
            {
                return true;
            }

            return false;
        }



        public string GetResponseURL(string webPageUrl, string postData)
        {
            if (!IS_IP_CHECKED)
            {
                IS_IP_CHECKED = true;
                string testPage = GetWebPage(TestUrl, false, null);
                Console.WriteLine(testPage);
                // WriteLog(testPage);
            }

            return GetWebPage(webPageUrl, true, null);
        }


        internal string GetWebPage(string webPageUrl, bool useCookieJar, byte[] payload)
        {
            int retries = 0;
            HttpWebResponse resp = null;
            HttpWebRequest req = null;
        RETRY:
            _lastUrl = webPageUrl;

            try
            {
                //string urlEncoded = System.Web.HttpUtility.UrlEncode(webPageUrl);
                Uri url = new Uri(webPageUrl);
                req = (HttpWebRequest)WebRequest.Create(url);
                //if (!string.IsNullOrEmpty(OutboundIp))
                //    req.ServicePoint.BindIPEndPointDelegate = new BindIPEndPoint(Bind);
                //req.KeepAlive = true;
                if (XmlWebService)
                {
                    req.Accept = "application/xml";
                }
                else if (XmlHttpRequest)
                {
                    req.Accept = "text/javascript, text/html, application/xml, text/xml, */*";
                    req.Headers.Add("X-Requested-With", "XMLHttpRequest");
                }
                else
                {
                    req.Accept = Accept;
                }


                req.CookieContainer = _cookieJar;
                req.AllowAutoRedirect = true;
                req.KeepAlive = true;
               // req.Referer = HttpUtility.UrlEncode(referer);
                req.UserAgent = "Mozilla/5.0 (Windows; U; Windows NT 5.1; en-US; rv:1.9.0.8) Gecko/2009021910 Firefox/3.0.7 (.NET CLR 3.5.30729)";
                req.Headers.Add("Pragma", "no-cache");
                req.Timeout = 40000;


                //  //req.UserAgent = "Mozilla/4.0 (compatible; MSIE 8.0; Windows NT 6.0; WOW64; Trident/4.0; SLCC1; .NET CLR 2.0.50727; Media Center PC 5.0; .NET CLR 3.5.21022; .NET CLR 3.5.30729; OfficeLiveConnector.1.5; OfficeLivePatch.1.3; .NET4.0C; .NET CLR 3.0.30729; Creative AutoUpdate v1.40.01)";
                //  req.UserAgent = "Mozilla/5.0 (compatible; MSIE 9.0; Windows NT 6.1; WOW64; Trident/5.0)";
                //  req.Headers.Add("Accept-Language", "en-gb,en;q=0.5");
                ////  req.Headers.Add("Accept-Language", "en-us");
                //  if (Compression)
                //      req.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
                //  if (Referer.Any())
                //      req.Referer = Referer;
                //  req.ProtocolVersion = HttpVersion.Version11;
                //  if (useCookieJar)
                //      req.CookieContainer = _cookieJar;
                //  req.Timeout = Timeout;

                if (payload != null)
                {

                    req.Method = "POST";
                    req.ContentType = "application/x-www-form-urlencoded";
                    req.ContentLength = payload.Length;
                    Stream payloadStream = req.GetRequestStream();
                    payloadStream.Write(payload, 0, payload.Length);
                    payloadStream.Close();
                }

                //  WriteLog("START Web Rq " + webPageUrl);
                Console.WriteLine(DateTime.Now + " " + webPageUrl);
                resp = (HttpWebResponse)req.GetResponse();
            }
            catch (WebException ex)
            {


                if (retries == 1)
                {
                    return string.Empty;
                    //Console.WriteLine("trying post submitter with other method of getting webpage..........");
                    //PostSubmitter post = new PostSubmitter();
                    //post.Url = webPageUrl;
                    //post.Type = PostSubmitter.PostTypeEnum.Post;
                    //string result = post.Post();
                    //return result;

                }

                //TODO: Test with PLOS URLs
                if (!SearchBase.isValidUrl(webPageUrl))
                {
                    InvalidURLException appExc = new InvalidURLException("Url Is Not Valid: " + webPageUrl, ex);
                    // ConsoleError(DateTime.Now + " Url Is Not Valid: " + webPageUrl);
                    throw appExc;
                }

                if (ex.Status == WebExceptionStatus.Timeout)
                {
                    if (retries > 5)
                        throw;

                    retries++;
                    if (retries == 5)
                        System.Threading.Thread.Sleep(4 * 60 * 60 * 1000);
                    else
                        System.Threading.Thread.Sleep(RetrySleepMultiplier * retries);
                    // ConsoleError(DateTime.Now + " RETRY (TO) " + webPageUrl);
                    //  WriteWarning("RETRY(WE-TO) " + webPageUrl);
                    if (useCookieJar && ClearCookieJarOnTimeout)
                        _cookieJar = new CookieContainer();
                    goto RETRY;
                }
                else if (ex.Status == WebExceptionStatus.ProtocolError)
                {
                    if (retries >= 1)
                    {
                        throw;
                    }


                    retries++;
                    resp = (HttpWebResponse)ex.Response;

                    //Common errors - least to most serious
                    if (resp.StatusCode == HttpStatusCode.NotFound)
                    {
                        System.Threading.Thread.Sleep(60 * 1000);
                        // ConsoleError(DateTime.Now + " RETRY (PE-NF) " + webPageUrl);
                        //  WriteWarning("RETRY(WE-PE-NF) " + webPageUrl);
                    }
                    else if (resp.StatusCode == HttpStatusCode.GatewayTimeout)
                    {
                        if (retries == 2)
                            System.Threading.Thread.Sleep(4 * 60 * 60 * 1000);
                        else
                            System.Threading.Thread.Sleep(RetrySleepMultiplier * retries);
                        //  ConsoleError(DateTime.Now + " RETRY (PE-GT) " + webPageUrl);
                        //WriteWarning("RETRY(WE-PE-GT) " + webPageUrl);
                    }
                    else if (resp.StatusCode == HttpStatusCode.InternalServerError)
                    {
                        if (retries == 2)
                            System.Threading.Thread.Sleep(4 * 60 * 60 * 1000);
                        else
                            System.Threading.Thread.Sleep(RetrySleepMultiplier * retries);
                        // ConsoleError(DateTime.Now + " RETRY (PE-IS) " + webPageUrl);
                        //WriteWarning("RETRY(WE-PE-IS) " + webPageUrl);
                    }
                    else if (resp.StatusCode == HttpStatusCode.ServiceUnavailable)
                    {
                        if (retries == 2)
                            System.Threading.Thread.Sleep(4 * 60 * 60 * 1000);
                        else
                            System.Threading.Thread.Sleep(RetrySleepMultiplier * retries);
                        // ConsoleError(DateTime.Now + " RETRY (PE-SU) " + webPageUrl);
                        // WriteWarning("RETRY(WE-PE-SU) " + webPageUrl);
                    }
                    else if (resp.StatusCode == HttpStatusCode.Forbidden)
                    {
                        if (retries == 2)
                            System.Threading.Thread.Sleep(24 * 60 * 60 * 1000);
                        else
                            System.Threading.Thread.Sleep(RetrySleepMultiplier * retries);
                        // ConsoleError(DateTime.Now + " RETRY (PE-FO) " + webPageUrl);
                        // WriteWarning("RETRY(WE-PE-FO) " + webPageUrl);
                    }
                    else
                    {
                        if (retries == 2)
                            System.Threading.Thread.Sleep(4 * 60 * 60 * 1000);
                        else
                            System.Threading.Thread.Sleep(RetrySleepMultiplier * retries);
                        // ConsoleError(DateTime.Now + " RETRY (PE) " + webPageUrl);
                        // WriteWarning("RETRY(WE-PE) " + webPageUrl);
                    }
                    goto RETRY;
                }
                else if (ex.Status == WebExceptionStatus.ConnectFailure)
                {
                    if (retries > 2)
                        throw;

                    retries++;
                    if (retries == 2)
                        System.Threading.Thread.Sleep(4 * 60 * 60 * 1000);
                    else
                        System.Threading.Thread.Sleep(RetrySleepMultiplier * retries);
                    //  ConsoleError(DateTime.Now + " RETRY (CF) " + webPageUrl);
                    //WriteWarning("RETRY(WE-CF) " + webPageUrl);
                    goto RETRY;
                }
                else if (ex.Status == WebExceptionStatus.ReceiveFailure)
                {
                    if (retries > 2)
                        throw;

                    retries++;
                    if (retries == 2)
                        System.Threading.Thread.Sleep(4 * 60 * 60 * 1000);
                    else
                        System.Threading.Thread.Sleep(RetrySleepMultiplier * retries);
                    // ConsoleError(DateTime.Now + " RETRY (RF) " + webPageUrl);
                    //WriteWarning("RETRY(WE-RF) " + webPageUrl);
                    goto RETRY;
                }
                else
                {
                    if (retries > 1)
                        throw;

                    retries++;
                    System.Threading.Thread.Sleep(4 * 60 * 60 * 1000);
                    //   ConsoleError(DateTime.Now + " RETRY (OE) " + webPageUrl);
                    //  WriteWarning("RETRY(WE-OE) " + webPageUrl);
                    goto RETRY;
                }
            }
            catch (UriFormatException uriEx)
            {
                // UpdateConsoleTitle(CrawlerName + " - " + uriEx.Message);
                var appExc = new InvalidURLException("Url Is Not Valid: " + webPageUrl, uriEx);
                // ConsoleError(DateTime.Now + " Url Is Not Valid: " + webPageUrl);
                throw appExc;
            }

            if (resp != null)
            {
                LastResponseHeaders = resp.Headers;
                _lastResponseCookies = resp.Cookies;
                foreach (System.Net.Cookie c in _lastResponseCookies)
                {
                    //WriteLog("cookie: " + c);
                }

                LastResponseStatusCode = resp.StatusCode;
                // UpdateConsoleTitle(CrawlerName + " - " + LastResponseStatusCode);
                LastResponseUri = resp.ResponseUri;

                //CharacterSet defaults to "ISO-8859-1" - Elsevier can have a null CharacterSet
                if (!string.IsNullOrEmpty(resp.CharacterSet))
                    LastResponseEncoding = resp.CharacterSet.ToUpper();
            }
            if (resp.ResponseUri.AbsoluteUri != req.RequestUri.AbsoluteUri)
            {
                if (retries == 0 && RetryIfRedirected)
                {
                    //  WriteLog("RetryIfRedirected " + webPageUrl);
                    Console.WriteLine(DateTime.Now + " RetryIfRedirected");
                    System.Threading.Thread.Sleep(2000);
                    retries++;
                    goto RETRY;
                }
                else
                {
                    //   WriteLog("Redirected To " + resp.ResponseUri.AbsoluteUri);
                    Console.WriteLine(DateTime.Now + " Redirected");
                }
            }

            //  WriteLog("END Web Rq");

            //http://link.aip.org/link/ASMECP/v2005/i41855a/p103/s1&Agg=doi

            MemoryStream localStream = new MemoryStream();
            byte[] serverBytes = new byte[BUFF_SIZE];
            Stream serverFile = null;

            try
            {

                //if (resp.Headers["set-cookie"] != null)
                //{
                //    string rawHeader = resp.Headers["Set-Cookie"];

                //}
                //DateTime modifiedDate = resp.LastModified;
                serverFile = resp.GetResponseStream();

                int bytesRead = 1;

                do
                {
                    bytesRead = serverFile.Read(serverBytes, 0, serverBytes.Length);
                    if (bytesRead == 0)
                    {
                        break;
                    }

                    localStream.Write(serverBytes, 0, bytesRead);

                } while (bytesRead > 0);

            }
            catch (IOException ex)
            {
                //UpdateConsoleTitle(CrawlerName + " - " + ex.Message);
                //WriteWarning(DateTime.Now + " " + ex);
                // WriteWarning(DateTime.Now + " " + webPageUrl);

                if (retries > 5)
                    throw;

                retries++;
                System.Threading.Thread.Sleep(RetrySleepMultiplier * retries);
                // WriteWarning("RETRY(IO) " + webPageUrl);
                goto RETRY;

            }
            catch (WebException ex)
            {
                // UpdateConsoleTitle(CrawlerName + " - " + ex.Message);
                // WriteWarning(DateTime.Now + " " + ex);
                // WriteWarning(DateTime.Now + " " + webPageUrl);
                // WriteWarning("WebException.Status: " + ex.Status);

                if (ex.Status == WebExceptionStatus.Timeout)
                {
                    if (retries > 5)
                        throw;

                    retries++;
                    if (retries == 5)
                        System.Threading.Thread.Sleep(4 * 60 * 60 * 1000);
                    else
                        System.Threading.Thread.Sleep(RetrySleepMultiplier * retries);
                    //ConsoleError(DateTime.Now + " RETRY (TO) " + webPageUrl);
                    //WriteWarning("RETRY(WE-TO) " + webPageUrl);
                    if (useCookieJar && ClearCookieJarOnTimeout)
                        _cookieJar = new CookieContainer();
                    goto RETRY;
                }
                else
                {
                    if (retries > 1)
                        throw;

                    retries++;
                    System.Threading.Thread.Sleep(4 * 60 * 60 * 1000);
                    //ConsoleError(DateTime.Now + " RETRY (OE) " + webPageUrl);
                    // WriteWarning("RETRY(WE-OE) " + webPageUrl);
                    goto RETRY;
                }
            }
            finally
            {
                if (serverFile != null) serverFile.Close();
                if (localStream != null) localStream.Close();
                if (resp != null) resp.Close();
            }

            //File.SetLastWriteTime(localFile, modifiedDate);

            //This assumes LastResponseEncoding (HttpWebResponse.CharacterSet) is an acceptable character set
            _lastResponsePayload = Encoding.GetEncoding(LastResponseEncoding).GetString(localStream.ToArray());

            //Look inside content to override encoding
            //UTF-16 is untested
            //<meta charset="utf-8"/>
            //<meta content="text/html; charset=UTF-8" http-equiv="Content-Type">
            //<meta content='text/html; charset=UTF-8' http-equiv='Content-Type' />
            var matchCollection = Regex.Matches(_lastResponsePayload, @"[;\s]charset=([^/>\s]+)", RegexOptions.Multiline);
            if (matchCollection.Count > 0 && matchCollection[0].Groups.Count > 1)
            {
                var charset = matchCollection[0].Groups[1].Value.Replace("\"", "").Replace("'", "").ToUpper();
                if (String.CompareOrdinal(LastResponseEncoding, charset) != 0)
                {
                    if (String.CompareOrdinal(charset, "ISO-8859-1") == 0 || String.CompareOrdinal(charset, "US-ASCII") == 0
                        || String.CompareOrdinal(charset, "UTF-8") == 0 || String.CompareOrdinal(charset, "UTF-16") == 0)
                    {
                        LastResponseEncoding = charset;
                        _lastResponsePayload = Encoding.GetEncoding(LastResponseEncoding).GetString(localStream.ToArray());
                    }
                    else
                    {
                        //  WriteWarning("Ignoring Unknown Character Set: " + charset);
                    }
                }
            }
            // return _lastResponsePayload;

            //  Reparing all tags------------------------------
            //  PostSubmitter post1 = new PostSubmitter();
            // post1.Url = "http://fixmyhtml.com/";

            _lastResponsePayload = Regex.Replace(_lastResponsePayload, @"<!.*", "");
            // post1.PostItems.Add("html", _lastResponsePayload);


            //post1.Type = PostSubmitter.PostTypeEnum.Post;
            // string result1 = post1.Post();
            // reapiring all tags
            //   string a= HttpContext.Current.Server.HtmlEncode(_lastResponsePayload);
            //return _lastResponsePayload;
            //_lastResponsePayload = _lastResponsePayload.Replace("&#150;", "-");
            //return Mono.Web.HttpUtility.HtmlDecode(result1).ToString();


            return Mono.Web.HttpUtility.HtmlDecode(_lastResponsePayload).ToString();
        }


        //public override string CrawlerName
        //{
        //    get { return "SpiderBase"; }
        //}



        /// <summary>
        /// method for validating a url with regular expressions
        /// </summary>
        /// <param name="url">url we're validating</param>
        /// <returns>true if valid, otherwise false</returns>
        public static bool isValidUrl(string url)
        {
            string pattern = @"^(http|https|ftp)\://[a-zA-Z0-9\-\.]+\.[a-zA-Z]{2,3}(:[a-zA-Z0-9]*)?/?([a-zA-Z0-9\-\._\?\,\'/\\\+&amp;%\$#\=~])*[^\.\,\)\(\s]$";
            Regex reg = new Regex(pattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);
            return reg.IsMatch(url);
        }

        public string BuildUrl(string url, bool htmlDecode = true)
        {
            //TODO: Do we still need DOMAIN_URL?
            url = Regex.Replace(url, "#.*", "");
            if (!url.Contains("http") && !url.ToLower().Contains(DOMAIN_URL.ToLower()))
            {
                url = DOMAIN_URL + url;

                Uri uri = new Uri("http://" + url);
                return uri.AbsoluteUri;
            }
            else
            {
                return url;
            }
           
        }

        public void SetDomain(string url)
        {
            var uri = new Uri(url);
             DOMAIN_URL = uri.Host;
            //DOMAIN_URL = String.Format("{0}:{1}", uri.Scheme, uri.Host);
        }
    }

    public class InvalidURLException : ApplicationException
    {
        public InvalidURLException() { }

        public InvalidURLException(string message, Exception innerException)
            : base(message, innerException)
        {

        }
    }

    //internal class WebPage
    //{
    //    public string Cookie = string.Empty;
    //    public string Html = string.Empty;
    //}

}
