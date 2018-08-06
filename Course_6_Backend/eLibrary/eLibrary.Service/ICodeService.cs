using System.Collections.Generic;
using System.Web.Mvc;

namespace eLibrary.Service
{
    public interface ICodeService
    {
        List<SelectListItem> GetBookClass();
        List<SelectListItem> GetBookKeeper();
        List<SelectListItem> GetBookStatus();
    }
}