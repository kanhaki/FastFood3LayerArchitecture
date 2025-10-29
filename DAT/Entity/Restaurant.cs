using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAT.Entity
{
    [Table("Restaurant")]
    public class Restaurant
    {
        [Key]
        public int RestaurantID { get; set; }

        public int ManagerID { get; set; }

        [Required]
        [StringLength(255)]
        public string Name { get; set; }

        [StringLength(500)]
        public string Address { get; set; }

        [StringLength(20)]
        public string PhoneNumber { get; set; }

        [StringLength(100)]
        public string OpeningHours { get; set; }

        [Column(TypeName = "decimal(9, 6)")]
        public decimal? Location_Lat { get; set; }

        [Column(TypeName = "decimal(9, 6)")]
        public decimal? Location_Lng { get; set; }

        public int StatusID { get; set; }

        // Navigation property: Mối quan hệ với User (Manager)
        [ForeignKey("ManagerID")]
        public virtual User Manager { get; set; }

        // Navigation property: Mối quan hệ với RestaurantStatus
        [ForeignKey("StatusID")]
        public virtual RestaurantStatus RestaurantStatus { get; set; }

        // Navigation property
        public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
    }
}
