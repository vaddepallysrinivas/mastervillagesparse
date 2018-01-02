using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Text.RegularExpressions;
using System.Collections;
using System.Threading;
using ConsoleApplication1;
using ConsoleApplication1.classes;

namespace SandsCaptal.ClassesParse
{
    public class AgmarknetParse
    {
        string webPage = string.Empty;
        string nextUrl = string.Empty;


        public List<CommodityInfo> parse(string webPage)
        {
            var commodityList = new List<CommodityInfo>();
            var obj = new CommodityInfo();
            try
            {
                var market = "";
                var doc = new HtmlAgilityPack.HtmlDocument();
                doc.LoadHtml(webPage);
                var linktrs = doc.DocumentNode.SelectNodes("//table[@id='gridRecords']//tr");

                if (linktrs != null)
                {
                    foreach (HtmlNode tr in linktrs)
                    {
                        var doc1 = new HtmlAgilityPack.HtmlDocument();
                        doc1.LoadHtml(tr.InnerHtml);
                        var linktds = doc1.DocumentNode.SelectNodes("//td");
                        var i = 0;
                        obj = new CommodityInfo();
                        if (linktds != null)
                        {
                            foreach (HtmlNode td in linktds)
                            {
                                var tdValue = td.InnerText;
                                switch (i)
                                {
                                    case 0:
                                        if (market == string.Empty)
                                        {
                                            market = td.InnerText;
                                        };
                                        obj.Market = market;
                                        break;
                                    case 1: obj.ArrivalDate = tdValue; break;
                                    case 2: obj.ArrivalNos = tdValue; break;
                                    case 3: obj.Variety = tdValue; break;
                                    case 4: obj.MinimumPrice = tdValue; break;
                                    case 5: obj.MaximumPrice = tdValue; break;
                                    case 6: obj.ModalPrice = tdValue; break;
                                    default: obj.ArrivalDate = tdValue; break;

                                }
                                i++;
                            }
                        }
                        commodityList.Add(obj);
                    }
                }
            }
            catch (Exception ex) { }
            return commodityList;
        }

        public List<Item> parseOptions(string webPage, string controlName)
        {

            List<Item> objItems = new List<Item>();
            var objItem = new Item();

            try
            {

                var doc = new HtmlAgilityPack.HtmlDocument();
                doc.LoadHtml(webPage);
                var linktrs = doc.DocumentNode.SelectNodes("//select[@id='" + controlName + "']//option");
                //select[@class='required-entry super-attribute-select']/option/following-sibling::text()
                if (linktrs != null)
                {
                    foreach (HtmlNode option in linktrs)
                    {

                        string value = option.Attributes["value"].Value;
                        string text = option.NextSibling.InnerText;
                        objItem = new Item() { id = value, text = text };
                        objItems.Add(objItem);

                    }
                }

            }
            catch (Exception ex) { }
            return objItems;
        }

        public List<CommodityInfo2> parseData(string webPage)
        {
            var commidity = "";
            var commodityList = new List<CommodityInfo2>();
            var objVarities = new List<string>();
            var prices = new List<string>();

            var obj = new CommodityInfo2();
            try
            {

                var doc = new HtmlAgilityPack.HtmlDocument();
                doc.LoadHtml(webPage);
                var linktrs = doc.DocumentNode.SelectNodes("//table[@id='gridRecords']//tr");

                if (linktrs != null)
                {
                    foreach (HtmlNode tr in linktrs)
                    {
                        var doc1 = new HtmlAgilityPack.HtmlDocument();
                        doc1.LoadHtml(tr.InnerHtml);
                        //commidty node
                        var linktdCommidity = doc1.DocumentNode.SelectSingleNode("//td[@style='color:#266606;background-color:#ffd747;border-color:#266606;font-weight:bold;']");

                        if (linktdCommidity != null)
                        {

                            obj = new CommodityInfo2();
                            objVarities = new List<string>();
                            prices = new List<string>();
                            commidity = linktdCommidity.InnerText;
                            obj.Name = commidity;

                            continue;
                        }

                        if (commidity != string.Empty)
                        {
                            var linktds = doc1.DocumentNode.SelectNodes("//td");
                            var i = 0;
                            foreach (HtmlNode td in linktds)
                            {

                                var d = td.Attributes["style"].Value.ToString();
                                if (d == "color:#266606;background-color:#ffffe7;border-color:#266606;font-size:11pt;font-weight:bold;width:180px;"||d=="color:#266606;background-color:#ffffe7;border-color:#266606;font-size:11pt;width:80px;")
                                {
                                    objVarities.Add(td.InnerText);
                                }
                                else if (d == "border-color:#266606;width:180px;" || d == "border-color:#266606;width:80px;")
                                {
                                    prices.Add(td.InnerText);
                                    i = 1;
                                }

                                
                            }

                            if (i == 1)
                            {
                                obj.objVarities = objVarities;
                                obj.prices = prices;
                                commodityList.Add(obj);
                            }
                        }

                        
                    }
                }
            }
            catch (Exception ex) { }
            return commodityList;
        }


    }


}
