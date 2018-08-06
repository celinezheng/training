using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLibrary.Model
{
    public class Books
    {
        /// <summary>
        /// 書籍編號
        /// </summary>
        ///[MaxLength(5)]
        [DisplayName("書籍編號")]
        public int BookId { get; set; }


        /// <summary>
        /// 員工編號
        /// </summary>
        ///[MaxLength(5)]
        [DisplayName("書名")]
        [Required(ErrorMessage = "此欄位必填")]
        public string BookName { get; set; }

        /// <summary>
        /// 作者(BOOK_ARTHOR)
        /// </summary>
        [DisplayName("作者")]
        [Required(ErrorMessage = "此欄位必填")]
        public string BookAuthor { get; set; }

        /// <summary>
        /// 員工姓名(Last Name)
        /// </summary>
        [DisplayName("出版商")]
        [Required(ErrorMessage = "此欄位必填")]
        public string BookPublisher { get; set; }

        /// <summary>
        /// 職稱
        /// </summary>
        [DisplayName("內容簡介")]
        [Required(ErrorMessage = "此欄位必填")]
        public string BookNote { get; set; }
    
        [DisplayName("購書日期")]
        [Required(ErrorMessage = "此欄位必填")]
        public string BookBoughtDate { get; set; }

        /// <summary>
        /// 類別代碼
        /// </summary>
        [DisplayName("圖書類別")]
        [Required(ErrorMessage = "此欄位必填")]
        public string BookClassId { get; set; }



        /// <summary>
        /// 類別
        /// </summary>
        [DisplayName("圖書類別")]
        //[Required(ErrorMessage = "此欄位必填")]
        public string BookClassName { get; set; }

        /// <summary>
        /// 借閱狀態
        /// </summary>
        [DisplayName("借閱狀態")]
        //[Required(ErrorMessage = "此欄位必填")]
        public string BookCodeName { get; set; }

        /// <summary>
        /// 借閱狀態碼
        /// </summary>
        [DisplayName("借閱狀態碼")]
        //[Required(ErrorMessage = "此欄位必填")]
        public string BookCodeId { get; set; }

        /// <summary>
        /// 借閱人
        /// </summary>
        [DisplayName("借閱人")]
        //[Required(ErrorMessage = "此欄位必填")]
        public string BookKeeper { get; set; }
    }
}
