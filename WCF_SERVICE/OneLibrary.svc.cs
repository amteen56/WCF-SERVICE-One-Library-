﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using OnlineLibraryClass;

namespace WCF_SERVICE
{

    public class OneLibrary : IOneLibrary
    {
        public bool AddItem(OnlineLibData data)
        {
            string query =
           @"INSERT INTO Items (ItemType, cost, noOfIssue,itemTitle) VALUES
                 ('{0}', '{1}', '{2}', '{3}')";
            int rowadded = DBUtl.ExecSQL(query, data.itemtype,data.cost,data.noofissue,data.itemtitle);
            if (rowadded > 0)
                return true;
            else
                return false;
        }

        public bool BorrowItem(int itemid, string returndate, string uname)
        {
            string query =
           @"INSERT INTO IssueDetails (ItemId, IssueDate, ReturnDate, uname, IsReturned) VALUES
                 ('{0}',convert(datetime, '{1}',103), convert(datetime, '{2}', 103),'{3}', 0)";
            int rowadded = DBUtl.ExecSQL(query, itemid, DateTime.Now.ToShortDateString(), returndate, uname);
            if (rowadded > 0)
            {
                query =
          @"SELECT noOfIssue FROM Items WHERE Id = '{0}'";
                var dt = DBUtl.GetTable(query, itemid);
                double noOfIssue = Convert.ToDouble(dt.Rows[0]["noOfIssue"].ToString());
                if(noOfIssue>0)
                {
                    query =
          @"UPDATE Items SET noOfIssue = noOfIssue-1 WHERE Id = '{0}'";
                    rowadded = DBUtl.ExecSQL(query, itemid);
                    if (rowadded > 0)
                        return true;
                    else
                        return false;
                }
                else
                    return false;
            }
            else
                return false;
        }

        public bool DeleteItem(int Itemid)
        {
            string query =
            @"DELETE FROM Items WHERE Id = '{0}'";
            int roweffected = DBUtl.ExecSQL(query, Itemid);
            if (roweffected > 0)
                return true;
            else
                return false;
        }

        public List<OnlineLibData> getData()
        {
            string query =
            @"SELECT * FROM Items";
            var dlist = DBUtl.GetTable(query);
            var list = new List<OnlineLibData>();
            foreach (DataRow row in dlist.Rows)
            {
                OnlineLibData obj = new OnlineLibData();
                obj.itemid = Convert.ToInt32(row["Id"].ToString());
                obj.cost = Convert.ToInt32(row["cost"].ToString());
                obj.itemtype = row["ItemType"].ToString();
                obj.itemtitle = row["itemTitle"].ToString();
                obj.noofissue = Convert.ToInt32(row["noOfIssue"].ToString());
                list.Add(obj);
            }
            return list;
        }

        public double ReturnItem(int itemid,string uname)
        {
            string query =
           @"SELECT * FROM IssueDetails WHERE ItemId = '{0}' AND IsReturned = 0 AND uname = '{1}'";
            var dt = DBUtl.GetTable(query,itemid,uname);
            if (dt.Rows.Count > 0)
            {
                DateTime date = Convert.ToDateTime(dt.Rows[0]["ReturnDate"].ToString());
                int days = 0;
                if (date > DateTime.Now)
                   days = date.Subtract(DateTime.Now).Days;
                query =
           @"SELECT cost FROM Items WHERE Id = '{0}'";
                dt = DBUtl.GetTable(query,itemid);
             double cost = Convert.ToDouble(dt.Rows[0]["cost"].ToString());
                double fine = days * OnlineLibData.fine;
                query =
         @"UPDATE Items SET noOfIssue = noOfIssue+1 WHERE Id = '{0}'";
                int rowadded = DBUtl.ExecSQL(query, itemid);
                if (rowadded > 0)
                {
                    query = @"UPDATE IssueDetails SET IsReturned = 1 WHERE ItemId = '{0}' AND uname = '{1}'";
                    rowadded = DBUtl.ExecSQL(query, itemid,uname);
                    if (rowadded > 0)
                        return cost + fine;
                    else
                        return -1;
                    return cost + fine;

                }
                else
                    return -1;
            }
            return -1;
        }

        public bool UpdateItem(OnlineLibData data)
        {
            string query =
           @"UPDATE Items SET ItemType = '{1}', cost = '{2}', noOfIssue = '{3}',itemTitle='{4}' WHERE Id = '{0}'";
            int roweffected = DBUtl.ExecSQL(query, data.itemid, data.itemtype, data.cost, data.noofissue, data.itemtitle);
            if (roweffected > 0)
                return true;
            else
                return false;
        }
    }
}
