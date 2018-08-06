using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace eLibrary.Models
{
    public class BookService
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
        /// 依照條件取得客戶資料
        /// </summary>
        /// <returns></returns>
        public List<Models.Books> GetBookByCondition(Models.BookSearchArg arg)
        {
            //LEFT JOIN dbo.BOOK_CODE as BCD

            //                   ON(BD.BOOK_STATUS = BCD.CODE_ID)
            //                and BCD.CODE_TYPE = 'STATUS'
            DataTable dt = new DataTable();
            string sql = @"SELECT BD.BOOK_ID As BookId , BD.BOOK_CLASS_ID As BookClassId, BD.BOOK_NAME As BookName,
                                  CONVERT( varchar(12), BD.BOOK_BOUGHT_DATE, 111) AS BookBoughtDate, 
                                  BCD.CODE_NAME AS BookStatusName, M.USER_ENAME AS BookKeeperEName, 
                                  M.USER_CNAME As BookKeeperCName, BCL.BOOK_CLASS_NAME As BookClassName  
                           FROM  dbo.BOOK_DATA as BD 
	                       JOIN dbo.BOOK_CODE as BCD
	                           ON (BD.BOOK_STATUS = BCD.CODE_ID) And BCD.CODE_TYPE = 'BOOK_STATUS'
                           JOIN dbo.BOOK_CLASS as BCL
                               ON (BD.BOOK_CLASS_ID = BCL.BOOK_CLASS_ID)
                           LEFT JOIN dbo.MEMBER_M as M
	                           ON (BD.BOOK_KEEPER = M.USER_ID)
                           Where (BD.BOOK_CLASS_ID = @BookClassId Or @BookClassId = '') AND
                                 (BD.BOOK_NAME = @BookName Or @BookName = '')
                           ORDER BY BookBoughtDate DESC";

            using (SqlConnection conn = new SqlConnection(this.GetDBConnectionString()))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.Add(new SqlParameter("@BookClassId", arg.BOOK_CLASS_ID == null ? string.Empty : arg.BOOK_CLASS_ID));
                cmd.Parameters.Add(new SqlParameter("@BookName", arg.BOOK_NAME == null ? string.Empty : arg.BOOK_NAME));
                cmd.Parameters.Add(new SqlParameter("@BookKeeper", arg.BOOK_KEEPER == null ? string.Empty : arg.BOOK_KEEPER));
                cmd.Parameters.Add(new SqlParameter("@BookStatus", arg.BOOK_CODE_ID == null ? string.Empty : arg.BOOK_CODE_ID));
                SqlDataAdapter sqlAdapter = new SqlDataAdapter(cmd);
                sqlAdapter.Fill(dt);
                conn.Close();
            }
            //the above are search conditions
            return this.MapBookDataToList(dt);

        }


        /// <summary>
        /// 新增員工
        /// </summary>
        /// <param name="book"></param>
        /// <returns>員工編號</returns>
        public int InsertBook(Models.Books book)
        {
            string sql = @"
                         BEGIN TRY
                            BEGIN TRANSACTION
                                 INSERT INTO BOOK_DATA
						         (
							         BOOK_NAME, BOOK_AUTHOR, BOOK_PUBLISHER, 
                                     BOOK_NOTE, BOOK_BOUGHT_DATE, BOOK_CLASS_ID,
                                     BOOK_STATUS                          
						         )
						        VALUES
						        (
							        @BOOK_NAME, @BOOK_AUTHOR, @BOOK_PUBLISHER,
                                    @BOOK_NOTE, @BOOK_BOUGHT_DATE, @BOOK_CLASS_ID,
                                    @BOOK_STATUS
						        )
						        Select SCOPE_IDENTITY()
                            COMMIT TRANSACTION
                          END TRY
                          BEGIN CATCH
                             ROlLBACK TRANSACTION
                          END CATCH";
            int bookId;
            //區域變數：bookId
            //not sure the usage of BookId：直接接修改，或檢查新增成功(bookId!=0)
            using (SqlConnection conn = new SqlConnection(this.GetDBConnectionString()))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.Add(new SqlParameter("@BOOK_NAME", book.BOOK_NAME));
                cmd.Parameters.Add(new SqlParameter("@BOOK_AUTHOR", book.BOOK_AUTHOR));
                cmd.Parameters.Add(new SqlParameter("@BOOK_PUBLISHER", book.BOOK_PUBLISHER));
                cmd.Parameters.Add(new SqlParameter("@BOOK_NOTE", book.BOOK_NOTE));
                cmd.Parameters.Add(new SqlParameter("@BOOK_BOUGHT_DATE", book.BOOK_BOUGHT_DATE));
                cmd.Parameters.Add(new SqlParameter("@BOOK_CLASS_ID", book.BOOK_CLASS_ID));
                cmd.Parameters.Add(new SqlParameter("@BOOK_STATUS", "A"));

                bookId = Convert.ToInt32(cmd.ExecuteScalar());
                //字串轉數字都是32(長度差別
                conn.Close();
            }
            return bookId;
        }

        public Models.BookUpdateData GetBookDataById(string id)
        {
            Models.BookUpdateData BookData;

            DataTable dt = new DataTable();
            string sql = @"SELECT BD.BOOK_NAME As BookName , BD.BOOK_AUTHOR As BookAuthor,
                                  BD.BOOK_PUBLISHER As BookPublisher , BD.BOOK_NOTE As BookNote,
                                  CONVERT( varchar(12), BD.BOOK_BOUGHT_DATE, 111) AS BookBoughtDate, 
                                  BD.BOOK_CLASS_ID As BookClassId, BD.BOOK_STATUS As BookStatus,
                                  BD.BOOK_KEEPER As BookKeeper, BD.BOOK_ID As BookId 
                           FROM  dbo.BOOK_DATA as BD 
	                       Where (BD.BOOK_ID = @BookId)";

            using (SqlConnection conn = new SqlConnection(this.GetDBConnectionString()))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.Add(new SqlParameter("@BookId", id));
                SqlDataAdapter sqlAdapter = new SqlDataAdapter(cmd);
                sqlAdapter.Fill(dt);
                conn.Close();

                if (dt.Rows.Count == 0)
                {
                    return null;
                }

                BookData = new BookUpdateData()
                {
                    BOOK_ID = (int)dt.Rows[0]["BookId"],
                    BOOK_NAME = dt.Rows[0]["BookName"].ToString(),
                    BOOK_AUTHOR = dt.Rows[0]["BookAuthor"].ToString(),
                    BOOK_PUBLISHER = dt.Rows[0]["BookPublisher"].ToString(),
                    BOOK_NOTE = dt.Rows[0]["BookNote"].ToString(),
                    BOOK_BOUGHT_DATE = dt.Rows[0]["BookBoughtDate"].ToString(),
                    BOOK_CLASS_ID = dt.Rows[0]["BookClassId"].ToString(),
                    BOOK_CODE_ID = dt.Rows[0]["BookStatus"].ToString(),
                    BOOK_KEEPER = dt.Rows[0]["BookKeeper"].ToString()

                };
            }
            return BookData;
        }


        /// <summary>
        /// 刪除書籍
        /// </summary>
        /// <param name="book"></param>
        /// <returns></returns>
        public void DeleteBookById(string bookId)
        {
            string sql = @"BEGIN TRY
                            BEGIN TRANSACTION
                                  DELETE FROM dbo.BOOK_DATA
                                  WHERE BOOK_ID = @BookId AND
                                        (BOOK_STATUS = 'A' OR BOOK_STATUS = 'U')
                            COMMIT TRANSACTION
                          END TRY
                          BEGIN CATCH
                             ROlLBACK TRANSACTION
                          END CATCH";
            using (SqlConnection conn = new SqlConnection(this.GetDBConnectionString()))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.Add(new SqlParameter("@BookId", bookId));
                SqlDataAdapter sqlAdapter = new SqlDataAdapter(cmd);
                cmd.ExecuteNonQuery();

                conn.Close();
            }

        }


        /// <summary>
        /// 編輯書籍
        /// </summary>
        /// <param name="book"></param>
        /// <returns></returns>
        public void UpdateBookData(Models.BookUpdateData book)
        {
            string sql = @"
                         BEGIN TRY
		                    BEGIN TRANSACTION

                         UPDATE dbo.BOOK_DATA
                         SET BOOK_NAME = @BookName, BOOK_AUTHOR = @BookAuthor, 
                         BOOK_PUBLISHER = @BookPublisher, BOOK_NOTE = @BookNote, 
                         BOOK_BOUGHT_DATE = @BookBoughtDate, BOOK_CLASS_ID = @BookClass,
                         BOOK_STATUS = @BookStatus, BOOK_KEEPER = @BookKeeper
                         WHERE BOOK_ID = @BookId
                            COMMIT TRANSACTION
                         END TRY
                         BEGIN CATCH
		                    ROLLBACK TRANSACTION
	                     END CATCH";
            //not sure the usage of BookId
            using (SqlConnection conn = new SqlConnection(this.GetDBConnectionString()))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.Add(new SqlParameter("@BookId", book.BOOK_ID));
                cmd.Parameters.Add(new SqlParameter("@BookName", book.BOOK_NAME));
                cmd.Parameters.Add(new SqlParameter("@BookAuthor", book.BOOK_AUTHOR));
                cmd.Parameters.Add(new SqlParameter("@BookPublisher", book.BOOK_PUBLISHER));
                cmd.Parameters.Add(new SqlParameter("@BookNote", book.BOOK_NOTE));
                cmd.Parameters.Add(new SqlParameter("@BookBoughtDate", book.BOOK_BOUGHT_DATE));
                cmd.Parameters.Add(new SqlParameter("@BookClass", book.BOOK_CLASS_ID));
                cmd.Parameters.Add(new SqlParameter("@BookStatus", book.BOOK_CODE_ID));
                cmd.Parameters.Add(new SqlParameter("@BookKeeper", book.BOOK_KEEPER == null ? string.Empty : book.BOOK_KEEPER));
                cmd.ExecuteNonQuery();
                conn.Close();
            }
        }



        /// <summary>
        /// test
        /// </summary>
        /// <param name="bookData"></param>
        /// <returns></returns>
        private List<Models.Books> MapBookDataToList(DataTable bookData)
        {
            List<Models.Books> result = new List<Books>();
            foreach (DataRow row in bookData.Rows)
            {
                result.Add(new Books()
                {
                    BOOK_ID = (int)row["BookId"],
                    BOOK_CLASS_NAME = row["BookClassName"].ToString(),
                    BOOK_NAME = row["BookName"].ToString(),
                    BOOK_BOUGHT_DATE = row["BookBoughtDate"].ToString(),
                    BOOK_CODE_NAME = row["BookStatusName"].ToString(),
                    BOOK_KEEPER = row["BookKeeperEName"].ToString() == "" ? string.Empty : row["BookKeeperEName"].ToString() + '(' + row["BookKeeperCName"].ToString() + ')'
                });
            }
            return result;
        }
    }
}