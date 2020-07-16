using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineLibraryClass
{
    public class OnlineLibData
    {
        public int itemid { get; set; }
        public string itemtype { get; set; }
        public string itemtitle { get; set; }
        public int cost { get; set; }
        public int noofissue { get; set; }
        public string issuedate { get; set; }
        public string returndate { get; set; }
        public static double fine { get; set; }

        public OnlineLibData()
        {
            fine = 1.25;
        }
       
    }
}
