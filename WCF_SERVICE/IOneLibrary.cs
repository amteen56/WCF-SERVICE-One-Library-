using OnlineLibraryClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace WCF_SERVICE
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract]
    public interface IOneLibrary
    {
        [OperationContract]
        bool AddItem(OnlineLibData data);
        [OperationContract]
        bool UpdateItem(OnlineLibData data);
        [OperationContract]
        bool DeleteItem(int Itemid);
        [OperationContract]
        bool BorrowItem(int itemid,string returndate,string uname);
        [OperationContract]
        double ReturnItem(int itemid, string uname);
        [OperationContract]
        List<OnlineLibData> getData();
    }

}
