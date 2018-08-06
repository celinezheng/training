using System.Collections.Generic;
using eLibrary.Model;

namespace eLibrary.Service
{
    public interface IBookService
    {
        void DeleteBookById(string bookId);
        List<Books> GetBookByCondition(BookSearchArg arg);
        BookUpdateData GetBookDataById(string id);
        int InsertBook(Books book);
        bool UpdateBookData(BookUpdateData book);
    }
}