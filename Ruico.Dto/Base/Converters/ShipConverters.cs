using AutoMapper;
using Ruico.Domain.BaseModule.Entities;

namespace Ruico.Dto.Base.Converters
{
    public static class ShipConverters
    {
        static ShipConverters()
        {
            Mapper.CreateMap<Ship, ShipDTO>();
            Mapper.CreateMap<ShipDTO, Ship>();
        }

        public static Ship ToModel(this ShipDTO dto)
        {
            return Mapper.Map<Ship>(dto);
        }

        public static ShipDTO ToDto(this Ship model)
        {
            return Mapper.Map<ShipDTO>(model);
        }
    }
}
