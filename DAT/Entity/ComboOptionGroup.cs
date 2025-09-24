using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAT.Entity
{
    public class ComboOptionGroup
    {
        public int OptionGroupId { get; set; }
        public int ComboProductId { get; set; }
        public string GroupName { get; set; }
        public int MinSelect { get; set; }
        public int MaxSelect { get; set; }

        public Product ComboProduct { get; set; }
        public ICollection<ComboOptionItem> ComboOptionItems { get; set; }
    }
}
