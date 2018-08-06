using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eLibrary.Model;


using System.Web.Mvc;

namespace eLibrary.Service
{
    public class BookService : IBookService
    {
        private eLibrary.Dao.IBookDao bookDao { get; set; }
        /// <summary>
        /// 依照條件取得客戶資料
        /// </summary>
        /// <returns></returns>
        public List<Books> GetBookByCondition(BookSearchArg arg)
        {
            
            return bookDao.GetBookByCondition(arg);
        }

        /// <summary>
        /// 新增員工
        /// </summary>
        /// <param name="book"></param>
        /// <returns>員工編號</returns>
        public int InsertBook(eLibrary.Model.Books book)
        {
            return bookDao.InsertBook(book);
        }

        public eLibrary.Model.BookUpdateData GetBookDataById(string id)
        {
            return bookDao.GetBookDataById(id);
        }

        /// <summary>
        /// 刪除書籍
        /// </summary>
        /// <param name="book"></param>
        /// <returns></returns>
        public void DeleteBookById(string bookId)
        {
            bookDao.DeleteBookById(bookId);
        }

        /// <summary>
        /// 編輯書籍
        /// </summary>
        /// <param name="book"></param>
        /// <returns></returns>
        public bool UpdateBookData(eLibrary.Model.BookUpdateData book)
        {
            return bookDao.UpdateBookData(book);
        }


    }
}
