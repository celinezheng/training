using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace eLibrary.Models
{
    public class CodeService
    {
        /// <summary>
        /// 取得DB連線字串
        /// </summary>
        /// <returns></returns>
        private string GetDBConnectionString()
        {
            return
                System.Configuration.ConfigurationManager.ConnectionStrings["DBConn"].ConnectionString.ToString();
        }
        /// <summary>
        /// 取得BOOK_CLASS的部分資料
        /// </summary>
        /// <returns></returns>
        public List<SelectListItem> GetBookClass()
        {
            DataTable dt = new DataTable();
            string sql = @"Select BOOK_CLASS_ID As CodeId, BOOK_CLASS_NAME As CodeName 
                           From dbo.BOOK_CLASS ";
            using (SqlConnection conn = new SqlConnection(this.GetDBConnectionString()))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                //cmd.Parameters.Add(new SqlParameter("@Type", type));
                SqlDataAdapter sqlAdapter = new SqlDataAdapter(cmd);
                sqlAdapter.Fill(dt);
                conn.Close();
            }
            return this.MapCodeData(dt);
        }

        /// <summary>
        /// 取得BOOK_KEEPER的部分資料
        /// </summary>
        /// <returns></returns>
        public List<SelectListItem> GetBookKeeper()
        {
            DataTable dt = new DataTable();
            string sql = @"Select USER_ENAME + '(' + USER_CNAME +')' As CodeName,
                                USER_ID As CodeId 
                           From dbo.MEMBER_M ";
            //因為userId不會一樣所以前面不用加Distinct
            using (SqlConnection conn = new SqlConnection(this.GetDBConnectionString()))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                //cmd.Parameters.Add(new SqlParameter("@Type", type));
                SqlDataAdapter sqlAdapter = new SqlDataAdapter(cmd);
                sqlAdapter.Fill(dt);
                conn.Close();
            }
            return this.MapCodeData(dt);
        }

        /// <summary>
        /// 取得BOOK_STATUS的部分資料
        /// </summary>
        /// <returns></returns>
        /// 可以把兩個欄位並起來，同樣的東西並起來(用別名)
        /// GetBookStatus 
        public List<SelectListItem> GetBookStatus()
        {
            DataTable dt = new DataTable();
            string sql = @"Select Distinct CODE_ID  As CodeId, CODE_NAME As CodeName
                           From dbo.BOOK_CODE ";
            //where STATUS TYPE='sTAtUS'


            using (SqlConnection conn = new SqlConnection(this.GetDBConnectionString()))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                //cmd.Parameters.Add(new SqlParameter("@Type", type));
                SqlDataAdapter sqlAdapter = new SqlDataAdapter(cmd);
                sqlAdapter.Fill(dt);
                conn.Close();
            }
            return this.MapCodeData(dt);

        }



        /// <summary>
        /// Maping Status資料
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        private List<SelectListItem> MapCodeData(DataTable dt)
        {
            List<SelectListItem> result = new List<SelectListItem>();
            foreach (DataRow row in dt.Rows)
            {
                result.Add(new SelectListItem()
                {
                    Text = row["CodeName"].ToString(),
                    Value = row["CodeId"].ToString()
                });
            }
            return result;
        }
    }
}