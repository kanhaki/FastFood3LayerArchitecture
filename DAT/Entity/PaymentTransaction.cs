using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAT.Entity
{
    [Table("PaymentTransaction")]
    public class PaymentTransaction
    {
        [Key]
        public int TransactionID { get; set; }

        [StringLength(100)]
        public string vnp_TransactionNo { get; set; }

        public int OrderID { get; set; } // Đây là Unique, EF Core tự biết qua quan hệ 1-1

        [StringLength(255)]
        public string OrderInfo { get; set; }

        public int StatusID { get; set; }

        public DateTime? PaymentDate { get; set; }

        [StringLength(50)]
        public string BankCode { get; set; }

        [StringLength(10)]
        public string ResponseCode { get; set; }

        [Required]
        public int Amount { get; set; } // Đã là INT

        // Navigation property (Quan hệ 1-1)
        [ForeignKey("OrderID")]
        public virtual Order Order { get; set; }

        // Navigation property
        [ForeignKey("StatusID")]
        public virtual PaymentTransactionStatus PaymentTransactionStatus { get; set; }
    }
}
