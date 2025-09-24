using DAT.Entity;
using DTO.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BUS.Services
{
    public interface IComboService
    {
        Task<ProductDTO> GetComboWithDetailsAsync(int comboId);
        Task<bool> ValidateComboSelectionAsync(int comboId, Dictionary<int, int> selectedOptionIds);
    }
}
