using DAT.Entity;
using DAT.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BUS.Services
{
    public class ComboService : IComboService
    {
        private readonly IComboRepository _comboRepository;

        public ComboService(IComboRepository comboRepository)
        {
            _comboRepository = comboRepository;
        }

        public async Task<Product> GetComboWithDetailsAsync(int comboId)
            => await _comboRepository.GetComboWithDetailsAsync(comboId);

        public async Task<bool> ValidateComboSelectionAsync(int comboId, Dictionary<int, int> selectedOptionIds)
        {
            var combo = await _comboRepository.GetComboWithDetailsAsync(comboId);
            foreach (var group in combo.ComboOptionGroups)
            {
                if (selectedOptionIds.ContainsKey(group.OptionGroupId))
                {
                    int selectedCount = selectedOptionIds[group.OptionGroupId];
                    if (selectedCount < group.MinSelect || selectedCount > group.MaxSelect)
                        return false;
                }
                else if (group.MinSelect > 0)
                {
                    return false; // chưa chọn nhóm bắt buộc
                }
            }
            return true;
        }
    }
}
