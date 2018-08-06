using eLibrary.Dao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLibrary.Service
{
    class BookFactory
    {
        public IBookDao GetEmployeeDao()
        {
            IBookDao result;

            switch (Common.ConfigTool.GetAppsetting("DaoInTest"))
            {
                case "Y":
                    result = new eLibrary.Dao.BookTestDao();
                    break;
                case "N":
                    result = new eLibrary.Dao.BookDao();
                    break;
                default:
                    result = new eLibrary.Dao.BookTestDao();
                    break;
            }
            return result;
        }
    }
}
