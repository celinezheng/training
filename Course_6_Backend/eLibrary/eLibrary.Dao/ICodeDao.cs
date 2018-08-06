using System.Collections.Generic;
using System.Web.Mvc;

namespace eLibrary.Dao
{
    public interface ICodeDao
    {
        List<SelectListItem> GetBookClass();
        List<SelectListItem> GetBookKeeper();
        List<SelectListItem> GetBookStatus();
    }
}