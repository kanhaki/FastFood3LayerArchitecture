using DAT.Entity;
using DAT.UnitOfWork;
using DTO.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BUS.Services.RestaurantService
{
    public class RestaurantService : IRestaurantService
    {
        private readonly IUnitOfWork _uow;

        public RestaurantService(IUnitOfWork unitOfWork)
        {
            _uow = unitOfWork;
        }

        public async Task<IEnumerable<RestaurantDTO>> GetAllAsync()
        {
            var entities = await _uow.Restaurants.GetAllWithStatusAsync();
            return entities.Select(MapToDTO);
        }

        public async Task<RestaurantDTO?> GetByIdAsync(int id)
        {
            var entity = await _uow.Restaurants.GetByIdWithStatusAsync(id);
            if (entity == null) return null;
            return MapToDTO(entity);
        }

        // Hàm helper (D.R.Y)
        private RestaurantDTO MapToDTO(Restaurant r)
        {
            return new RestaurantDTO
            {
                RestaurantID = r.RestaurantID,
                Name = r.Name,
                Address = r.Address,
                PhoneNumber = r.PhoneNumber,
                OpeningHours = r.OpeningHours,
                Location_Lat = r.Location_Lat,
                Location_Lng = r.Location_Lng,
                StatusID = r.StatusID,
                StatusName = r.RestaurantStatus?.StatusName
            };
        }
    }
}
