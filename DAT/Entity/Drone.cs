using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAT.Entity
{
    [Table("Drone")]
    public class Drone
    {
        [Key]
        public int DroneID { get; set; }

        public int StationID { get; set; }

        [StringLength(100)]
        public string Model { get; set; }

        [Column(TypeName = "decimal(5, 2)")]
        public decimal? CurrentBattery { get; set; }

        [Column(TypeName = "decimal(5, 2)")]
        public decimal? MaxLoad { get; set; }

        [Column(TypeName = "decimal(9, 6)")]
        public decimal? CurrentLocation_Lat { get; set; }

        [Column(TypeName = "decimal(9, 6)")]
        public decimal? CurrentLocation_Lng { get; set; }

        public int StatusID { get; set; }

        // Navigation property
        [ForeignKey("StationID")]
        public virtual DroneStation DroneStation { get; set; }

        // Navigation property
        [ForeignKey("StatusID")]
        public virtual DroneStatus DroneStatus { get; set; }

        // Navigation property
        public virtual ICollection<Delivery> Deliveries { get; set; } = new List<Delivery>();
    }
}
