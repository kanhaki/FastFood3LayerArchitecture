using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAT.Entity
{
    public class ComboOptionItem
    {
        public int OptionItemId { get; set; }
        public int OptionGroupId { get; set; }
        public int ProductId { get; set; }

        public ComboOptionGroup OptionGroup { get; set; }
        public Product Product { get; set; }
    }
}
