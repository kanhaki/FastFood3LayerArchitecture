using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.DTO
{
    public class ComboOptionGroupDTO
    {
        public string GroupName { get; set; }
        public int MinSelect { get; set; }
        public int MaxSelect { get; set; }
        public List<ComboOptionItemDTO> OptionItems { get; set; } = new List<ComboOptionItemDTO>();
    }
}
