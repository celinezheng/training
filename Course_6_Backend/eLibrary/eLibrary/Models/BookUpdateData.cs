using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace eLibrary.Models
{
    public class BookUpdateData
    {
        /// <summary>
        /// 書籍編號
        /// </summary>
        ///[MaxLength(5)]
        [DisplayName("書籍編號")]
        public int BOOK_ID { get; set; }//EmployeeId


        /// <summary>
        /// 員工編號
        /// </summary>
        ///[MaxLength(5)]
        [DisplayName("書名")]
        [Required(ErrorMessage = "此欄位必填")]
        public string BOOK_NAME { get; set; }//EmployeeId

        /// <summary>
        /// 作者(BOOK_ARTHOR)
        /// </summary>
        [DisplayName("作者")]
        [Required(ErrorMessage = "此欄位必填")]
        public string BOOK_AUTHOR { get; set; }//EmployeeFirstName

        /// <summary>
        /// 員工姓名(Last Name)
        /// </summary>
        [DisplayName("出版商")]
        [Required(ErrorMessage = "此欄位必填")]
        public string BOOK_PUBLISHER { get; set; }//EmployeeLastName

        /// <summary>
        /// 職稱
        /// </summary>
        [DisplayName("內容簡介")]
        [Required(ErrorMessage = "此欄位必填")]
        public string BOOK_NOTE { get; set; }//JobTitle

        [DisplayName("購書日期")]
        [Required(ErrorMessage = "此欄位必填")]
        public string BOOK_BOUGHT_DATE { get; set; }//JobTitleId

        /// <summary>
        /// 類別
        /// </summary>
        [DisplayName("圖書類別")]
        [Required(ErrorMessage = "此欄位必填")]
        public string BOOK_CLASS_ID { get; set; }//TitleOfCourtesy

        /// <summary>
        /// 借閱狀態碼
        /// </summary>
        [DisplayName("借閱狀態碼")]
        //[Required(ErrorMessage = "此欄位必填")]
        public string BOOK_CODE_ID { get; set; }//not sure if it is CODE_id

        /// <summary>
        /// 借閱人
        /// </summary>
        [DisplayName("借閱人")]
        //[Required(ErrorMessage = "此欄位必填")]
        public string BOOK_KEEPER { get; set; }//not sire if it is user name

    }
}