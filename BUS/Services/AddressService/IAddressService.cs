using DTO.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BUS.Services.AddressService
{
    public interface IAddressService
    {
        Task<IEnumerable<AddressDTO>> GetAllForUserAsync(int userIdFromToken);
        Task<AddressDTO?> GetByIdForUserAsync(int addressId, int userIdFromToken);
        Task<AddressDTO> AddForUserAsync(AddressDTO dto, int userIdFromToken);
        Task<bool> UpdateForUserAsync(int addressId, AddressDTO dto, int userIdFromToken);
        Task<bool> DeleteForUserAsync(int addressId, int userIdFromToken);
    }
}
