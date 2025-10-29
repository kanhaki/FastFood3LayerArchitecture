using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAT.Entity
{
    [Table("Delivery")]
    public class Delivery
    {
        [Key]
        public int DeliveryID { get; set; }

        public int OrderID { get; set; } // Đây là Unique, EF Core tự biết qua quan hệ 1-1
        public int DroneID { get; set; }

        public DateTime? EstimatedPickupTime { get; set; }
        public DateTime? ActualPickupTime { get; set; }
        public DateTime? EstimatedDropoffTime { get; set; }
        public DateTime? ActualDropoffTime { get; set; }

        public int StatusID { get; set; }

        // Navigation property (Quan hệ 1-1)
        [ForeignKey("OrderID")]
        public virtual Order Order { get; set; }

        // Navigation property
        [ForeignKey("DroneID")]
        public virtual Drone Drone { get; set; }

        // Navigation property
        [ForeignKey("StatusID")]
        public virtual DeliveryStatus DeliveryStatus { get; set; }
    }
}
