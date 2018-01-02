using ConsoleApplication1.DBAccesLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleApplication1.Classes;
using System.Web.Script.Serialization;
using System.Net;
using System.Collections.Specialized;
using SandsCaptal.ClassesParse;
using ConsoleApplication1.classes;


namespace ConsoleApplication1.parse
{
    public class MasterData : IMasterData
    {

        public void Go()
        {
            //parse  states
            //runStates();

            //parse districts 
            //runDisticts();
            //parse talukas 
            //runTaluka();
            //parse villages 
            // runVillages();
            getMarkets();
        }

        public void runVillages()
        {
            IList<tbl_taluka> talukaList = null;
            using (var context = new MasterDataEntities())
            {

                talukaList = context.tbl_taluka.ToList<tbl_taluka>();
            }


            foreach (var districtObj in talukaList)
            {

                GenericClass obj = new GenericClass();

                var nextpageurl = "http://www.mapsofindia.com/villages/data.php?state=" + districtObj.stateId + "&district=" + districtObj.districtId + "&tehsil=" + districtObj.talukaid + "";
                var webpage = obj.GetWebPage(nextpageurl);


                IList<MasterDataJson> persons = new JavaScriptSerializer().Deserialize<IList<MasterDataJson>>(webpage);

                for (int i = 0; i < persons.Count; i++)
                {
                    using (var context = new MasterDataEntities())
                    {

                        var t = new tbl_village
                        {
                            villageId = Convert.ToInt32(persons[i].value),
                            villageName = persons[i + 1].text,
                            stateId = districtObj.stateId,
                            districtId = districtObj.districtId,
                            talukaid = districtObj.talukaid
                        };
                        context.tbl_village.Add(t);
                        context.SaveChanges();
                    }

                    i = i + 1;
                }

            }

        }
        public void runTaluka()
        {

            IList<tbl_district> distinctList = null;
            using (var context = new MasterDataEntities())
            {

                distinctList = context.tbl_district.ToList<tbl_district>();
            }


            foreach (var districtObj in distinctList)
            {

                GenericClass obj = new GenericClass();

                var nextpageurl = "http://www.mapsofindia.com/villages/data.php?state=" + districtObj.stateId + "&district=" + districtObj.districtId + "";
                var webpage = obj.GetWebPage(nextpageurl);


                IList<MasterDataJson> persons = new JavaScriptSerializer().Deserialize<IList<MasterDataJson>>(webpage);

                for (int i = 0; i < persons.Count; i++)
                {
                    using (var context = new MasterDataEntities())
                    {

                        var t = new tbl_taluka
                        {
                            talukaid = Convert.ToInt16(persons[i].value),
                            talukaName = persons[i + 1].text,
                            stateId = districtObj.stateId,
                            districtId = districtObj.districtId
                        };
                        context.tbl_taluka.Add(t);
                        context.SaveChanges();
                    }

                    i = i + 1;
                }

            }
        }
        public void runDisticts()
        {

            IList<tbl_state> statesList = null;
            using (var context = new MasterDataEntities())
            {

                statesList = context.tbl_state.ToList<tbl_state>();
            }


            foreach (var stateObj in statesList)
            {

                GenericClass obj = new GenericClass();

                var nextpageurl = "http://www.mapsofindia.com/villages/data.php?state=" + stateObj.stateId + "";
                var webpage = obj.GetWebPage(nextpageurl);


                IList<MasterDataJson> persons = new JavaScriptSerializer().Deserialize<IList<MasterDataJson>>(webpage);

                for (int i = 0; i < persons.Count; i++)
                {
                    using (var context = new MasterDataEntities())
                    {

                        var t = new tbl_district
                        {
                            districtId = Convert.ToInt16(persons[i].value),
                            districtName = persons[i + 1].text,
                            stateId = stateObj.stateId
                        };
                        context.tbl_district.Add(t);
                        context.SaveChanges();
                    }

                    i = i + 1;
                }

            }
        }
        public void runStates()
        {


            GenericClass obj = new GenericClass();

            var nextpageurl = "http://www.mapsofindia.com/villages/data.php?state=all";
            var webpage = obj.GetWebPage(nextpageurl);


            IList<MasterDataJson> persons = new JavaScriptSerializer().Deserialize<IList<MasterDataJson>>(webpage);

            for (int i = 0; i < persons.Count; i++)
            {
                using (var context = new MasterDataEntities())
                {

                    var t = new tbl_state
                    {
                        stateId = Convert.ToInt16(persons[i].value),
                        stateName = persons[i + 1].text
                    };
                    context.tbl_state.Add(t);
                    context.SaveChanges();
                }

                i = i + 1;
            }

        }


        //public void sampleRun1()
        //{


        //    var url1 = "http://www.agmarknet.nic.in/agnew/NationalBEnglish/DatewiseCommodityReport2.aspx";

        //    var m_values = new NameValueCollection() {

        //         //{"__EVENTTARGET",""},
        //         // {"__EVENTARGUMENT",""},
        //         //  {"__LASTFOCUS",""},
        //         //   {"__VIEWSTATE","/wEPDwUJODM1ODc5NTEzD2QWAgIDD2QWEAIIDxBkEBUTES0tLVNlbGVjdCBZZWFyLS0tBDIwMDAEMjAwMQQyMDAyBDIwMDMEMjAwNAQyMDA1BDIwMDYEMjAwNwQyMDA4BDIwMDkEMjAxMAQyMDExBDIwMTIEMjAxMwQyMDE0BDIwMTUEMjAxNgQyMDE3FRMRLS0tU2VsZWN0IFllYXItLS0EMjAwMAQyMDAxBDIwMDIEMjAwMwQyMDA0BDIwMDUEMjAwNgQyMDA3BDIwMDgEMjAwOQQyMDEwBDIwMTEEMjAxMgQyMDEzBDIwMTQEMjAxNQQyMDE2BDIwMTcUKwMTZ2dnZ2dnZ2dnZ2dnZ2dnZ2dnZxYBAglkAgwPEGQPFg1mAgECAgIDAgQCBQIGAgcCCAIJAgoCCwIMFg0QBRAtLVNlbGVjdCBNb250aC0tBRAtLVNlbGVjdCBNb250aC0tZxAFB0phbnVhcnkFB0phbnVhcnlnEAUIRmVicnVhcnkFCEZlYnJ1YXJ5ZxAFBU1hcmNoBQVNYXJjaGcQBQVBcHJpbAUFQXByaWxnEAUDTWF5BQNNYXlnEAUESnVuZQUESnVuZWcQBQRKdWx5BQRKdWx5ZxAFBkF1Z3VzdAUGQXVndXN0ZxAFCVNlcHRlbWJlcgUJU2VwdGVtYmVyZxAFB09jdG9iZXIFB09jdG9iZXJnEAUITm92ZW1iZXIFCE5vdmVtYmVyZxAFCERlY2VtYmVyBQhEZWNlbWJlcmcWAQIBZAIODw8WBB4EVGV4dAUHU3RhdGUgOh4HVmlzaWJsZWdkZAIQDxAPFgIfAWdkEBUcEy0tLVNlbGVjdCBTdGF0ZS0tLS0OQW5kaHJhIFByYWRlc2gFQXNzYW0FQmloYXILQ2hhdHRpc2dhcmgDR29hB0d1amFyYXQHSGFyeWFuYRBIaW1hY2hhbCBQcmFkZXNoEUphbW11IGFuZCBLYXNobWlyCUpoYXJraGFuZAlLYXJuYXRha2EGS2VyYWxhDk1hZGh5YSBQcmFkZXNoC01haGFyYXNodHJhB01hbmlwdXIJTWVnaGFsYXlhDE5DVCBvZiBEZWxoaQZPcmlzc2ELUG9uZGljaGVycnkGUHVuamFiCVJhamFzdGhhbgpUYW1pbCBOYWR1CVRlbGFuZ2FuYQdUcmlwdXJhDVV0dGFyIFByYWRlc2gKVXR0cmFraGFuZAtXZXN0IEJlbmdhbBUcEy0tLVNlbGVjdCBTdGF0ZS0tLS0OQW5kaHJhIFByYWRlc2gFQXNzYW0FQmloYXILQ2hhdHRpc2dhcmgDR29hB0d1amFyYXQHSGFyeWFuYRBIaW1hY2hhbCBQcmFkZXNoEUphbW11IGFuZCBLYXNobWlyCUpoYXJraGFuZAlLYXJuYXRha2EGS2VyYWxhDk1hZGh5YSBQcmFkZXNoC01haGFyYXNodHJhB01hbmlwdXIJTWVnaGFsYXlhDE5DVCBvZiBEZWxoaQZPcmlzc2ELUG9uZGljaGVycnkGUHVuamFiCVJhamFzdGhhbgpUYW1pbCBOYWR1CVRlbGFuZ2FuYQdUcmlwdXJhDVV0dGFyIFByYWRlc2gKVXR0cmFraGFuZAtXZXN0IEJlbmdhbBQrAxxnZ2dnZ2dnZ2dnZ2dnZ2dnZ2dnZ2dnZ2dnZ2dnFgECAWQCEg8PFgQfAAULQ29tbW9kaXR5IDofAWdkZAIUDxAPFgQeC18hRGF0YUJvdW5kZx8BZ2QQFU0ULS1TZWxlY3QgQ29tbW9kaXR5LS0FQWp3YW4FQXBwbGUZQmFqcmEoUGVhcmwgTWlsbGV0L0N1bWJ1KQZCYW5hbmEOQmFuYW5hIC0gR3JlZW4FQmVhbnMIQmVldHJvb3QRQmVuZ2FsIEdyYW0oR3JhbSkVQmhpbmRpKExhZGllcyBGaW5nZXIpDEJpdHRlciBnb3VyZBZCbGFjayBHcmFtIChVcmQgQmVhbnMpDEJvdHRsZSBnb3VyZAdCcmluamFsC0J1bmNoIEJlYW5zB0NhYmJhZ2UGQ2Fycm90CkNhc2hld251dHMLQ2FzdG9yIFNlZWQLQ2F1bGlmbG93ZXINQ2x1c3RlciBiZWFucwdDb2NvbnV0BUNvcHJhEUNvcmlhbmRlcihMZWF2ZXMpBkNvdHRvbgtDb3R0b24gU2VlZBBDdWN1bWJhcihLaGVlcmEpBURhbGRhCURydW1zdGljawxEcnkgQ2hpbGxpZXMJRmllbGQgUGVhF0ZyZW5jaCBCZWFucyAoRnJhc2JlYW4pC0dpbmdlcihEcnkpDEdyZWVuIENoaWxsaRJHcmVlbiBHcmFtIChNb29uZykKR3JlZW4gUGVhcw9Hcm91bmQgTnV0IFNlZWQJR3JvdW5kbnV0EUdyb3VuZG51dCAoU3BsaXQpFEdyb3VuZG51dCBwb2RzIChyYXcpBEd1YXIMR3VyKEphZ2dlcnkpDkpvd2FyKFNvcmdodW0pBEp1dGUGS2lubm93Ekt1bHRoaShIb3JzZSBHcmFtKQ9MZWFmeSBWZWdldGFibGUFTGVtb24ETGltZQVNYWl6ZRNOaWdlciBTZWVkIChSYW10aWwpBU9uaW9uC09uaW9uIEdyZWVuBk9yYW5nZQtQYWRkeShEaGFuKQZQYXBheWEIUGVhcyBjb2QIUGVhcyBXZXQGUG90YXRvB1B1bXBraW4HUmFkZGlzaBRSYWdpIChGaW5nZXIgTWlsbGV0KQhSZWQgR3JhbQRSaWNlEFJpZGdlZ3VhcmQoVG9yaSkJU2FmZmxvd2VyClNuYWtlZ3VhcmQJU3VuZmxvd2VyDlN1bmZsb3dlciBTZWVkDFN3ZWV0IFBvdGF0bw5UYW1hcmluZCBGcnVpdA1UYW1hcmluZCBTZWVkBlRvbWF0bwhUdXJtZXJpYwVXaGVhdARXb29kA1lhbRVNFC0tU2VsZWN0IENvbW1vZGl0eS0tAzEzNwIxNwIyOAIxOQI5MAI5NAMxNTcBNgI4NQI4MQE4AjgyAjM1AzIyNAMxNTQDMTUzAjM2AzEyMwIzNAI4MAMxMzgDMTI5AjQzAjE1Ajk5AzE1OQMyNzMDMTY4AzEzMgI2NAMyOTgCMjcCODcBOQI1MAMyNjgCMTADMzE0AzMxMgI3NQI3NAE1AjE2AzMzNgMxMTQDMTcxAzMxMAMxODABNAI5OAIyMwMzNTgCMTgBMgI3MgMzMDgDMTc0AjI0Ajg0AzE2MQIzMAE3ATMDMTYwAjU5AzE1NgIxNAMyODUDMTUyAzI2MQMyMDgCNzgCMzkBMQMyMDMDMjQ0FCsDTWdnZ2dnZ2dnZ2dnZ2dnZ2dnZ2dnZ2dnZ2dnZ2dnZ2dnZ2dnZ2dnZ2dnZ2dnZ2dnZ2dnZ2dnZ2dnZ2dnZ2dnZ2dnZ2dnZ2dnZ2dnZ2dnFgECBGQCGg8PFgIfAWdkZAIbDw8WAh8BZ2RkGAEFHl9fQ29udHJvbHNSZXF1aXJlUG9zdEJhY2tLZXlfXxYDBQxJbWFnZUJ1dHRvbjIFDEltYWdlQnV0dG9uMQULQ2FsYV9CdXR0b272YIcRSOLcLhNROmLaIYlL3fDjHA=="},
        //         //    {"__VIEWSTATEGENERATOR","64F0D178"},
        //         //    {"__PREVIOUSPAGE","afFCp7-XxsElibX1oqVLLeWPqGN7k1mqQtltwz-9IFCObo4qTb-q_077Oj8uFb97kpEASsLy0cEC_oxSjaLyCt0dZJjjCxThJu6EmrnUD147BhR66iiTJbIDWD6PvgTaE5IPMA2"},
        //         //      {"__EVENTVALIDATION","/wEWmwEC2viJ9AsC0sLV5AIC0sKZ0wgCzPT7pg8C29HrBALltPS/CgLz/6YDAvP/0qgHAvP/ztUOAvP/+vIFAvP/lp4NAvP/grsEAvP/vuALAvP/qo0DAvP/huQFAvP/soENApjGgJgKApjGvMUBApjGqOIIApjGxA8CmMbwtAcCmMbs0Q4CmMaY/QUCmMa0mg0Ch6SLoA0CrIHzsgkC5967iwUCqPHfiQ0C1oqkEQL+ibDyCQKHpY/4BAK9hsmTBwKD9L2hBwKo67LZAQK+nIDBDgKk+7fFDwLbqfu6AwLIgcWbAQKApJvuBwKjkJPjBQLR9tCaBgKVs7blDgK0h7GBDALj7MbNAQL++cy7CgK3oeC6BgKWj6PICAKyvbq/CwKAz7ntAgKC5svyAwKDzpqIDAKXitiICQLY3Z1mAvWA/q4KApG97/sEAuL/zOcFAsz9yK4HAvOpm+UHAvSU6aoPAvm7mpwCAsKdkOgCAvzc45AOArWf5ZMNApXFvOkMAv+m6rMIAr6HiecFAomh/58JAs/ExzYCnLH7rwECqcqV+AICwKuJ2wwCwavN2AwCwKvB2AwC2Kut2wwC2Kud2wwCqcqN+AICxavt2AwC16uR2wwC16uh2wwC16vt2AwC16ul2wwCwquR2wwC25e7uAQC2pe3uAQCta2VzwkCwquV2wwCta2ZzwkCwqud2wwC16ut2wwCnqDVpgoCo4nzkwQCw6uZ2wwCwKuR2wwC2KvB2AwCo4nvkwQCtq2NzwkCnqDBpgoCkMT60g8Cxaud2wwCn6D9pgoCwauJ2wwC16uJ2wwC2Kvt2AwCxKut2wwCn6DBpgoCwKut2wwC3JfHuQQCksSC0g8CxquR2wwCxqud2wwCxKvt2AwCwKuV2wwChuHzjwgC2pfHuQQCi9PI+QUC6OrOjAsC5urqjAsCw6vt2AwC2KvN2AwCwauZ2wwCgKDNpgoCwKvN2AwCwavt2AwCxqul2wwCgKDZpgoC2pevuAQCwaud2wwC16ud2wwCi9PU+QUCwqut2wwCxqvt2AwCwqvt2AwC5uqyjwsCxKvB2AwChOHrjwgCwKud2wwC4PiFkg4CkMTy0g8CjNPU+QUCn6DZpgoCxqvN2AwCwqvB2AwCwKvt2AwCtq2hzwkC25ezuAQCsdqGzQsCwova3gMCoLew/QoCmaCGmQQCm6Ce2QIC2PnM0wsCvqyxkgcC97HUnQoC56u5lAoC3fO2dHIwQKQGa192nY03MaSDu2RLwMyQ"},

        //            { "cboYear","2017"},
        //            { "cboMonth","April"},
        //            { "cboState","Telangana"},
        //            {"cboCommodity","19"},
        //            {"btnSubmit","Submit"},
        //            {"hidCommName","Banana"},
        //            {"hidCommCode","19"},
        //            {"hidStateCode","TL"},
        //            {"hidStateName","Telangana"},
        //            {"hidYear","2017"},
        //            {"hidMonthName","April"},
        //            {"hidGroupCode","5"}
                    
            
        //    };
        //   // PostSubmitter obj = new PostSubmitter(url1, mvalues);
        //   // var result = obj.Post();

        //    StringBuilder parameters = new StringBuilder();
        //    for (int i = 0; i < m_values.Count; i++)
        //    {
        //        EncodeAndAddItem(ref parameters, m_values.GetKey(i), m_values[i]);
        //    }
        //    byte[] bytes = Encoding.UTF8.GetBytes(parameters.ToString());
        //    GenericClass obj = new GenericClass();
           
        //    var webpage = obj.GetWebPage(url1,bytes);


        //}

        //public void sampleRun()
        //{
        //    var url1 = "http://www.agmarknet.nic.in/agnew/NationalBEnglish/CommodityPricesWeeklyReport.aspx";
        //       GenericClass obj = new GenericClass();
        //      var webpage= obj.GetWebPageSelenium(url1);
        //      var obj1= new AgmarknetParse();
        //      obj1.parse(webpage);

        //}


        public void getMarkets()
        {
            var url1 = "http://www.agmarknet.nic.in/agnew/NationalBEnglish/CommodityPricesWeeklyReport.aspx";
            GenericClass obj = new GenericClass();

            List<SelectedControl> objList = new List<SelectedControl>();

            //---------------------------geting markets
            var objSelectedControl = new SelectedControl() { Name = "cboState", Text = "Telangana" };
            objList.Add(objSelectedControl);
            var webpage = obj.GetWebPageSelenium(url1, objList, false);
            var obj1 = new AgmarknetParse();
            obj1.parseOptions(webpage, "cboMarket");

            //----------------------------getting commidity
            //var objSelectedState = new SelectedControl() { Name = "cboState", Text = "Telangana" };
            //var objSelectedMarket = new SelectedControl() { Name = "cboMarket", Text = "Gajwel" };
            //objList.Add(objSelectedState);
            //objList.Add(objSelectedMarket);
            //var webpage = obj.GetWebPageSelenium(url1, objList, false);
            //var obj1 = new AgmarknetParse();
            //obj1.parseOptions(webpage, "cboCommodity");


            //var objSelectedState = new SelectedControl() { Name = "cboState", Text = "Telangana" };
            //var objSelectedMarket = new SelectedControl() { Name = "cboMarket", Text = "Gajwel" };
            //var objSelectedCommidity = new SelectedControl() { Name = "cboCommodity", Text = "All" };
            //objList.Add(objSelectedState);
            //objList.Add(objSelectedMarket);
            //objList.Add(objSelectedCommidity);
            //var webpage = obj.GetWebPageSelenium(url1, objList, true);
            //var obj1 = new AgmarknetParse();
            //obj1.parseData(webpage);

        }



        private void EncodeAndAddItem(ref StringBuilder baseRequest, string key, string dataItem)
        {
            if (baseRequest == null)
            {
                baseRequest = new StringBuilder();
            }
            if (baseRequest.Length != 0)
            {
                baseRequest.Append("&");
            }
            baseRequest.Append(key);
            baseRequest.Append("=");
            baseRequest.Append(System.Net.WebUtility.UrlEncode(dataItem));
        }
    }
}
