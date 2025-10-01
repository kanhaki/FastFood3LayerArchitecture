using DTO.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BUS.Services
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryDTO>> GetAllAsync();
        Task<CategoryDTO?> GetByIdAsync(long id);
        Task AddAsync(CategoryDTO dto);
        Task UpdateAsync(CategoryDTO dto);
        Task<bool> DeleteAsync(long id);
    }
}
