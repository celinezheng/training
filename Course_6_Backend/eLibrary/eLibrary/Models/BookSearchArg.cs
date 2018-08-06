using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace eLibrary.Models
{
    public class BookSearchArg
    {
        [DisplayName("書名")]
        public string BOOK_NAME { get; set; }//EmployeeId:BOOK_NAME
        [DisplayName("圖書類別")]
        public string BOOK_CLASS_NAME { get; set; }//EmployeeName:BOOK_CLASS_NAME
        [DisplayName("圖書類別代碼")]
        public string BOOK_CLASS_ID { get; set; }
        [DisplayName("借閱人")]
        public string BOOK_KEEPER { get; set; }//JobTitleId:BOOK_KEEPER(in form of id)
        [DisplayName("借閱狀態")]
        public string BOOK_CODE_ID { get; set; }
    }
}