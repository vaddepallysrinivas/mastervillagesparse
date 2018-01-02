using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1.classes
{
    public class CommodityInfo
    {
        public string Market { set; get; }
        public string ArrivalDate { set; get; }
        public string ArrivalNos { set; get; }
        public string Variety { set; get; }
        public string MinimumPrice { set; get; }
        public string MaximumPrice { set; get; }
        public string ModalPrice { set; get; }


    }

    public class CommodityInfo2
    {

        public string Name { set; get; }
        public List<string> objVarities{ set; get; }
        public List<string> prices { set; get; }


    }

    public class Varieties
    {

       
        public string date { set; get; }
        public string price { set; get; }

        
    }

    public class Item
    {


        public string id { set; get; }
        public string text { set; get; }


    }

    public class SelectedControl
    {


        public string Name { set; get; }
        public string Text { set; get; }


    }
}
