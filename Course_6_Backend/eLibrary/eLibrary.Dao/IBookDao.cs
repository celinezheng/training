using System.Collections.Generic;
using eLibrary.Model;

namespace eLibrary.Dao
{
    public interface IBookDao
    {
        bool DeleteBookById(string bookId);
        List<Books> GetBookByCondition(BookSearchArg arg);
        BookUpdateData GetBookDataById(string id);
        int InsertBook(Books book);
        bool UpdateBookData(BookUpdateData book);
    }
}