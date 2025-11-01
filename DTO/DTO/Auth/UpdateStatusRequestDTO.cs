using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.DTO
{
    public class UpdateStatusRequestDTO
    {
        [System.ComponentModel.DataAnnotations.Required]
        public string Status { get; set; }
    }
}
