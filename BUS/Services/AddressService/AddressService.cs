using DAT.Entity;
using DAT.UnitOfWork;
using DTO.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BUS.Services.AddressService
{
    public class AddressService : IAddressService
    {
        private readonly IUnitOfWork _uow;

        public AddressService(IUnitOfWork unitOfWork)
        {
            _uow = unitOfWork;
        }

        public async Task<IEnumerable<AddressDTO>> GetAllForUserAsync(int userIdFromToken)
        {
            var entities = await _uow.Addresses.GetAllForUserAsync(userIdFromToken);
            return entities.Select(MapToDTO);
        }

        public async Task<AddressDTO?> GetByIdForUserAsync(int addressId, int userIdFromToken)
        {
            var entity = await _uow.Addresses.GetByIdForUserAsync(addressId, userIdFromToken);
            if (entity == null) return null;
            return MapToDTO(entity);
        }

        public async Task<AddressDTO> AddForUserAsync(AddressDTO dto, int userIdFromToken)
        {
            var entity = new Address
            {
                AdrsCustomerName = dto.AdrsCustomerName,
                AdrsLine = dto.AdrsLine,
                Phone = dto.Phone,
                IsDefault = dto.IsDefault,
                UserID = userIdFromToken
            };

            await _uow.Addresses.AddAsync(entity);
            await _uow.SaveChangesAsync();

            return MapToDTO(entity);
        }

        public async Task<bool> UpdateForUserAsync(int addressId, AddressDTO dto, int userIdFromToken)
        {
            // GetByIdForUserAsync ĐẢM BẢO user này chỉ sửa được địa chỉ CỦA HỌ
            var entity = await _uow.Addresses.GetByIdForUserAsync(addressId, userIdFromToken);
            if (entity == null)
                return false; // Không tìm thấy (hoặc cố sửa của người khác)

            entity.AdrsCustomerName = dto.AdrsCustomerName;
            entity.AdrsLine = dto.AdrsLine;
            entity.Phone = dto.Phone;
            entity.IsDefault = dto.IsDefault;

            _uow.Addresses.Update(entity);
            await _uow.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteForUserAsync(int addressId, int userIdFromToken)
        {
            var entity = await _uow.Addresses.GetByIdForUserAsync(addressId, userIdFromToken);
            if (entity == null)
                return false; // Không tìm thấy (hoặc cố xóa của người khác)

            _uow.Addresses.Remove(entity);
            await _uow.SaveChangesAsync();
            return true;
        }

        private AddressDTO MapToDTO(Address a)
        {
            return new AddressDTO
            {
                AdrsID = a.AdrsID,
                AdrsCustomerName = a.AdrsCustomerName,
                AdrsLine = a.AdrsLine,
                Phone = a.Phone,
                IsDefault = a.IsDefault.GetValueOrDefault() // Xử lý 'bit' (bool?)
            };
        }
    }
}
