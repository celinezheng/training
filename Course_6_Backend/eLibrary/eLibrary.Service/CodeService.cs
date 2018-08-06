using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace eLibrary.Service
{
    public class CodeService : ICodeService
    {

        private eLibrary.Dao.ICodeDao codeDao { get; set; }
        /// <summary>
        /// 取得BOOK_CLASS的部分資料
        /// </summary>
        /// <returns></returns>
        public List<SelectListItem> GetBookClass()
        {
            return codeDao.GetBookClass();
        }

        /// <summary>
        /// 取得BOOK_KEEPER的部分資料
        /// </summary>
        /// <returns></returns>
        public List<SelectListItem> GetBookKeeper()
        {
            return codeDao.GetBookKeeper();
        }

        /// <summary>
        /// 取得BOOK_STATUS的部分資料
        /// </summary>
        /// <returns></returns>
        /// 可以把兩個欄位並起來，同樣的東西並起來(用別名)
        /// GetBookStatus 
        public List<SelectListItem> GetBookStatus()
        {
            return codeDao.GetBookStatus();
        }


    }
}
