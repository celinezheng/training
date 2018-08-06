using eLibrary.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLibrary.Dao
{
    public class BookDao : IBookDao
    {
        /// <summary>
        /// 取得DB連線字串
        /// </summary>
        /// <returns></returns>
        private string GetDBConnectionString()
        {
            return eLibrary.Common.ConfigTool.GetDBConnectionString("DBConn");
        }

        /// <summary>
        /// 依照條件取得客戶資料
        /// </summary>
        /// <returns></returns>
        public List<eLibrary.Model.Books> GetBookByCondition(BookSearchArg arg)
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
                                 (BD.BOOK_NAME = @BookName Or @BookName = '')AND
                                 (BD.BOOK_STATUS = @BookStatus Or @BookStatus = '')AND
                                 (BD.BOOK_KEEPER = @BookKeeper Or @BookKeeper = '')
                           ORDER BY BookBoughtDate DESC";

            using (SqlConnection conn = new SqlConnection(this.GetDBConnectionString()))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.Add(new SqlParameter("@BookClassId", arg.BookClassId == null ? string.Empty : arg.BookClassId));
                cmd.Parameters.Add(new SqlParameter("@BookName", arg.BookName == null ? string.Empty : arg.BookName));
                cmd.Parameters.Add(new SqlParameter("@BookKeeper", arg.BookKeeper == null ? string.Empty : arg.BookKeeper));
                cmd.Parameters.Add(new SqlParameter("@BookStatus", arg.BookCodeId == null ? string.Empty : arg.BookCodeId));
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
        public int InsertBook(Books book)
        {
            string sql = @"
                         BEGIN TRY
                            BEGIN TRANSACTION
                                 INSERT INTO BOOK_DATA
						         (
							         BOOK_NAME, BOOK_AUTHOR, BOOK_PUBLISHER, 
                                     BOOK_STATUS,BOOK_NOTE, BOOK_BOUGHT_DATE, BOOK_CLASS_ID                          
						         )
						        VALUES
						        (
							        @BOOK_NAME, @BOOK_AUTHOR, @BOOK_PUBLISHER,
                                    @BOOK_STATUS, 
                                    @BOOK_NOTE, CONVERT( varchar(12), @BOOK_BOUGHT_DATE, 111), @BOOK_CLASS_ID
                                    
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
                cmd.Parameters.Add(new SqlParameter("@BOOK_NAME", book.BookName));
                cmd.Parameters.Add(new SqlParameter("@BOOK_AUTHOR", book.BookAuthor));
                cmd.Parameters.Add(new SqlParameter("@BOOK_PUBLISHER", book.BookPublisher));
                cmd.Parameters.Add(new SqlParameter("@BOOK_NOTE", book.BookNote));
                cmd.Parameters.Add(new SqlParameter("@BOOK_BOUGHT_DATE", book.BookBoughtDate));
                cmd.Parameters.Add(new SqlParameter("@BOOK_CLASS_ID", book.BookClassId));
                cmd.Parameters.Add(new SqlParameter("@BOOK_STATUS", "A"));

                bookId = Convert.ToInt32(cmd.ExecuteScalar());
                //字串轉數字都是32(長度差別
                conn.Close();
            }
            return bookId;
        }

        public eLibrary.Model.BookUpdateData GetBookDataById(string id)
        {
            eLibrary.Model.BookUpdateData BookData;

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
                    BookId = (int)dt.Rows[0]["BookId"],
                    BookName = dt.Rows[0]["BookName"].ToString(),
                    BookAuthor = dt.Rows[0]["BookAuthor"].ToString(),
                    BookPublisher = dt.Rows[0]["BookPublisher"].ToString(),
                    BookNote = dt.Rows[0]["BookNote"].ToString(),
                    BookBoughtDate = dt.Rows[0]["BookBoughtDate"].ToString(),
                    BookClassId = dt.Rows[0]["BookClassId"].ToString(),
                    BookCodeId = dt.Rows[0]["BookStatus"].ToString(),
                    BookKeeper = dt.Rows[0]["BookKeeper"].ToString()

                };
            }
            return BookData;
        }


        /// <summary>
        /// 刪除書籍
        /// </summary>
        /// <param name="book"></param>
        /// <returns></returns>
        public bool DeleteBookById(string bookId)
        {
            try
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
                    var effected = cmd.ExecuteNonQuery();
                    conn.Close();

                    return (effected == 1);
                }
            }            
            catch (Exception ex)
            {
                eLibrary.Common.Logger.Write(eLibrary.Common.Logger.LogCategoryEnum.Error,ex.ToString());
                throw;
            }

        }


        /// <summary>
        /// 編輯書籍
        /// </summary>
        /// <param name="book"></param>
        /// <returns></returns>
        public bool UpdateBookData(eLibrary.Model.BookUpdateData book)
        {
            string sql = @"
                         BEGIN TRY
		                    BEGIN TRANSACTION

                         UPDATE dbo.BOOK_DATA
                         SET BOOK_NAME = @BookName, BOOK_AUTHOR = @BookAuthor, 
                         BOOK_PUBLISHER = @BookPublisher, BOOK_NOTE = @BookNote, 
                         BOOK_BOUGHT_DATE =CONVERT( varchar(12), @BookBoughtDate, 111) , BOOK_CLASS_ID = @BookClass,
                         BOOK_STATUS = @BookStatus, BOOK_KEEPER = @BookKeeper
                         WHERE BOOK_ID = @BookId
                            COMMIT TRANSACTION
                         END TRY
                         BEGIN CATCH
		                    ROLLBACK TRANSACTION
	                     END CATCH";
            //not sure the usage of BookId
            var result = 0;
            using (SqlConnection conn = new SqlConnection(this.GetDBConnectionString()))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.Add(new SqlParameter("@BookId", book.BookId));
                cmd.Parameters.Add(new SqlParameter("@BookName", book.BookName));
                cmd.Parameters.Add(new SqlParameter("@BookAuthor", book.BookAuthor));
                cmd.Parameters.Add(new SqlParameter("@BookPublisher", book.BookPublisher));
                cmd.Parameters.Add(new SqlParameter("@BookNote", book.BookNote));
                cmd.Parameters.Add(new SqlParameter("@BookBoughtDate", book.BookBoughtDate));
                cmd.Parameters.Add(new SqlParameter("@BookClass", book.BookClassId));
                cmd.Parameters.Add(new SqlParameter("@BookStatus", book.BookCodeId));
                cmd.Parameters.Add(new SqlParameter("@BookKeeper", book.BookKeeper == null ? string.Empty : book.BookKeeper));
                result = cmd.ExecuteNonQuery();
                conn.Close();
            }
            return (result == 1);
        }



        /// <summary>
        /// test
        /// </summary>
        /// <param name="bookData"></param>
        /// <returns></returns>
        private List<eLibrary.Model.Books> MapBookDataToList(DataTable bookData)
        {
            List<eLibrary.Model.Books> result = new List<Books>();
            foreach (DataRow row in bookData.Rows)
            {
                result.Add(new Books()
                {
                    BookId = (int)row["BookId"],
                    BookClassName = row["BookClassName"].ToString(),
                    BookName = row["BookName"].ToString(),
                    BookBoughtDate = row["BookBoughtDate"].ToString(),
                    BookCodeName = row["BookStatusName"].ToString(),
                    BookKeeper = row["BookKeeperEName"].ToString() == "" ? string.Empty : row["BookKeeperEName"].ToString() + '(' + row["BookKeeperCName"].ToString() + ')'
                });
            }
            return result;
        }
    }
}
