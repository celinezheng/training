using Microsoft.VisualStudio.TestTools.UnitTesting;
using eLibrary.Dao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eLibrary.Model;

namespace eLibrary.Dao.Tests
{
    [TestClass()]
    public class BookDaoTests
    {
        List<Books> testList;
        eLibrary.Model.Books iBook;
        eLibrary.Model.BookSearchArg argEmpty,argI;
        eLibrary.Model.BookUpdateData bookUpdateData;
        eLibrary.Dao.BookDao test;
        
        //List<string> status;

        [TestInitialize()]
        public void Initializa()
        {
            test = new BookDao();            
            iBook = new Books()
            {
                //BOOK_ID = 0,
                BookName = "ibook",
                BookAuthor = "ia",
                BookPublisher = "ip",
                BookNote = "insert_test",
                BookBoughtDate = "2018-08-01",
                BookClassId = "BK"
            };

            argI = new BookSearchArg()
            {
                BookName = "ibook",
                BookClassId = "BK",
                BookKeeper = "",
                BookCodeId = "A"
            };

            argEmpty = new BookSearchArg()
            {
                BookName = "",
                BookClassId = "",
                BookKeeper = "",
                BookCodeId = ""
            };

            bookUpdateData = new BookUpdateData()
            {
                BookName = "ubook",
                BookAuthor = "ua",
                BookPublisher = "up",
                BookNote = "update_test",
                BookBoughtDate = "2018-08-01",
                BookCodeId = "A",
                BookKeeper = "0003"
            };



        }


        //[TestMethod()]
        //public void InsertBookTest()
        //{
        //    test.InsertBook(iBook);
        //    //Assert.Fail();
        //    Assert.AreEqual(0, 0);

        //}

        [TestMethod()]
        public void GetBookByConditionAndInsertTest()
        {
            //test
            testList = test.GetBookByCondition(argEmpty);
            test.InsertBook(iBook);
            var listNum = test.GetBookByCondition(argEmpty);
            Assert.AreEqual(testList.Count + 1, listNum.Count);

            //recover
            testList = test.GetBookByCondition(argI);
            var bookId = testList[0].BookId;
            test.DeleteBookById(bookId.ToString());

        }

        /// <summary>
        /// 結合findBook看有沒有確實刪除
        /// </summary>
        [TestMethod()]
        public void DeleteBookByIdTest()
        {
            testList = test.GetBookByCondition(argI);
            var bookId = testList[0].BookId;
            Assert.IsTrue(test.DeleteBookById(bookId.ToString()));
            var num = testList.Count;
            if(num==0)
            {
                test.InsertBook(iBook);
                Assert.AreEqual(testList[0].BookName, iBook.BookName);
                testList = test.GetBookByCondition(argI);
                Assert.AreEqual(testList.Count, 0);
            }
            else
            {
                testList = test.GetBookByCondition(argI);
                Assert.AreEqual(testList.Count, num-1);
            }
           
            
        }

        [TestMethod()]
        public void GetBookDataByIdTest()
        {
            //Assert.Fail();
            test.InsertBook(iBook);
            testList = test.GetBookByCondition(argI);
            var bookId = testList[0].BookId;
            var name = test.GetBookDataById(bookId.ToString()).BookName;
            Assert.AreEqual(name, iBook.BookName);
        }


        [TestMethod()]
        public void UpdateBookDataTest()
        {
            var bookId = test.InsertBook(iBook);
            bookUpdateData.BookId = bookId;
            bookUpdateData.BookClassId = iBook.BookClassId;
            Assert.IsTrue(test.UpdateBookData(bookUpdateData));
            var note = test.GetBookDataById(bookId.ToString()).BookNote;
            Assert.AreEqual(note, bookUpdateData.BookNote);
        }
    }
}