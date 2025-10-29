using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAT.Entity
{
    [Table("DroneStation")]
    public class DroneStation
    {
        [Key]
        public int StationID { get; set; }

        [StringLength(255)]
        public string Name { get; set; }

        [StringLength(500)]
        public string Address { get; set; }

        [Column(TypeName = "decimal(9, 6)")]
        public decimal? Location_Lat { get; set; }

        [Column(TypeName = "decimal(9, 6)")]
        public decimal? Location_Lng { get; set; }

        // Navigation property
        public virtual ICollection<Drone> Drones { get; set; } = new List<Drone>();
    }
}
