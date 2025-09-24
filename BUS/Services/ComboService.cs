using AutoMapper;
using DAT.Entity;
using DAT.Repository;
using DTO.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BUS.Services
{
    public class ComboService : IComboService
    {
        private readonly IComboRepository _comboRepo;
        private readonly IMapper _mapper;

        public ComboService(IComboRepository comboRepo, IMapper mapper)
        {
            _comboRepo = comboRepo;
            _mapper = mapper;
        }

        // Lấy combo kèm chi tiết (ComboDetails + OptionGroups + OptionItems)
        public async Task<ProductDTO> GetComboWithDetailsAsync(int comboId)
        {
            var combo = await _comboRepo.GetComboWithDetailsAsync(comboId);
            if (combo == null) return null;

            // Map entity -> DTO
            return _mapper.Map<ProductDTO>(combo);
        }

        // Kiểm tra lựa chọn combo có hợp lệ
        // selectedOptionIds: key = OptionGroupId, value = OptionItemId được chọn
        public async Task<bool> ValidateComboSelectionAsync(int comboId, Dictionary<int, int> selectedOptionIds)
        {
            var combo = await _comboRepo.GetComboWithDetailsAsync(comboId);
            if (combo == null) return false;

            foreach (var group in combo.ComboOptionGroups)
            {
                if (!selectedOptionIds.ContainsKey(group.OptionGroupId))
                    return false; // chưa chọn nhóm bắt buộc

                var selectedItemId = selectedOptionIds[group.OptionGroupId];
                // kiểm tra item có tồn tại trong nhóm không
                if (!group.ComboOptionItems.Any(i => i.OptionItemId == selectedItemId))
                    return false;
            }

            return true; // tất cả hợp lệ
        }
    }
}
