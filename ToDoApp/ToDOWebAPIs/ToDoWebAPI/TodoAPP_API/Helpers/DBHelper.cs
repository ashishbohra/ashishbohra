using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using TodoAPP_API.Models;

namespace TodoAPP_API.Helpers
{
    public class DBHelper
    {
        //Function use to open sql connection and interact with server to fetch list data 
        public static DataSet GetOrAddListToDoDB(ListRequestBody dolistbody, string Procedure,string constr)
        {
            /*
             whole block is in try catch statement to handle exceptions 
             */

            DataSet ds = new DataSet();
            //Define connection
            SqlConnection con = new SqlConnection(constr);
            try
            {
                //open connection and other properties like sp and his parameter
                con.Open();
                SqlCommand com = new SqlCommand(Procedure, con);
                com.CommandType = CommandType.StoredProcedure;
                com.CommandTimeout = 1000;
                com.Parameters.AddWithValue("@DataXML", dolistbody.xmlString);
                com.Parameters.AddWithValue("@type", dolistbody.type);

                //execute cuery and get result
                SqlDataAdapter adap = new SqlDataAdapter(com);
                adap.Fill(ds);
                con.Close();
            }
            catch (Exception e)
            {
            }
            finally
            {
                //dispose connection 
                con.Dispose();
            }
            return ds;
        }

        //Function use to open sql connection and interact with server to fetch list item fort list name data 
        public static DataSet GetOrAddListItemToDoDB(ListItemRequestBOdy dolistbody, string Procedure, string constr)
        {
            DataSet ds = new DataSet();
            //Define connection
            SqlConnection con = new SqlConnection(constr);
            try
            {
                con.Open();
                //open connection and other properties like sp and his parameter
                SqlCommand com = new SqlCommand(Procedure, con);
                com.CommandType = CommandType.StoredProcedure;
                com.CommandTimeout = 1000;
                com.Parameters.AddWithValue("@DataXML", dolistbody.xmlString);
                com.Parameters.AddWithValue("@listName", dolistbody.ListName);
                com.Parameters.AddWithValue("@type", dolistbody.type);
                //open connection and other properties like sp and his parameter
                SqlDataAdapter adap = new SqlDataAdapter(com);
                adap.Fill(ds);
                con.Close();
            }
            catch (Exception e)
            {
            }
            finally
            {
                //open connection and other properties like sp and his parameter
                con.Dispose();
            }
            return ds;
        }
    }
}
