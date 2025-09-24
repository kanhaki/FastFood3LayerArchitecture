using DAT.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BUS.Services
{
    public interface IComboService
    {
        Task<Product> GetComboWithDetailsAsync(int comboId);
        Task<bool> ValidateComboSelectionAsync(int comboId, Dictionary<int, int> selectedOptionIds);
    }
}
