using eLibrary.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace eLibrary.Controllers
{
    public class BookController : Controller
    {
        private ICodeService codeService { get; set; }
        private IBookService bookService { get; set; }

        void SetViewBagData()
        {
            ViewBag.BookTypeData = this.codeService.GetBookClass();
            ViewBag.BookKeeperData = this.codeService.GetBookKeeper();
            ViewBag.BookStatusData = this.codeService.GetBookStatus();
        }
        /// <summary>
        /// 員工資料查詢(顯示畫面)
        /// </summary>
        /// <returns></returns>
        [HttpGet()]
        public ActionResult Index()
        {
            SetViewBagData();
            return View();
        }

        /// <summary>
        /// 員工資料查詢(查詢)
        /// </summary>
        /// <returns></returns>
        [HttpPost()]
        public ActionResult Index(eLibrary.Model.BookSearchArg arg)
        {
            //if (arg.HireDateEnd == null)
            //    arg.HireDateEnd = DateTime.Now.ToShortDateString();
            ViewBag.SearchResult = bookService.GetBookByCondition(arg);
            SetViewBagData();
            return View("Index");
            //利用參數把資料傳回畫面
        }

        /// <summary>
        /// 新增員工畫面
        /// </summary>
        /// <returns></returns>
        [HttpGet()]
        public ActionResult InsertBook()
        {
            SetViewBagData();
            return View();
        }

        /// <summary>
        /// 新增員工
        /// </summary>
        /// <param name="book"></param>
        /// <returns></returns>
        [HttpPost()]
        public ActionResult InsertBook(eLibrary.Model.Books book)
        {
            SetViewBagData();
            //bookClassData
            if (ModelState.IsValid)
            {
                bookService.InsertBook(book);
                TempData["message"] = "新增成功";
            }
            else
            {
                TempData["message"] = "新增失敗";
            }
            return View(book);
        }

        /// <summary>
        /// 修改員工畫面
        /// </summary>
        /// <param name="employeeId"></param>
        /// <returns></returns>
        [HttpGet()]
        public ActionResult UpdateBook(string id)
        {
            SetViewBagData();

            eLibrary.Model.BookUpdateData temp = bookService.GetBookDataById(id);
            if (temp == null)
            {
                return RedirectToAction("Index");
            }
            return View(temp);

        }

        /// <summary>
        /// 修改員工畫面
        /// </summary>
        /// <param name="employeeId"></param>
        /// <returns></returns>
        [HttpPost()]
        public ActionResult UpdateBook(eLibrary.Model.BookUpdateData book)
        {
            SetViewBagData();

            if (ModelState.IsValid)
            {
                bookService.UpdateBookData(book);
                // Models.
                TempData["message"] = "編輯成功";
            }
            else
            {
                TempData["message"] = "編輯失敗";
            }
            return View(book);
        }

        /// <summary>
        /// 刪除員工
        /// </summary>
        /// <param name="employeeId"></param>
        /// <returns></returns>
        [HttpPost()]
        public JsonResult DeleteBook(string bookId)
        {
            try
            {
                bookService.DeleteBookById(bookId);
                return this.Json(true);
            }

            catch (Exception ex)
            {
                return this.Json(false);
            }
        }
    }
}