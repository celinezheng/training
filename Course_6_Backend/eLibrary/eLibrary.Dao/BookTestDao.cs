using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eLibrary.Model;

namespace eLibrary.Dao
{
    public class BookTestDao : IBookDao
    {
        public bool DeleteBookById(string bookId)
        {
            throw new NotImplementedException();
        }

        public List<Books> GetBookByCondition(BookSearchArg arg)
        {
            throw new NotImplementedException();
        }

        public BookUpdateData GetBookDataById(string id)
        {
            throw new NotImplementedException();
        }

        public int InsertBook(Books book)
        {
            throw new NotImplementedException();
        }

        public bool UpdateBookData(BookUpdateData book)
        {
            throw new NotImplementedException();
        }
    }
}
