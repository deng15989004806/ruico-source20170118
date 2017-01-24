using AutoMapper;
using Ruico.Domain.BaseModule.Entities;

namespace Ruico.Dto.Base.Converters
{
    public static class CargoConverters
    {
        static CargoConverters()
        {
            Mapper.CreateMap<Cargo, CargoDTO>()
                .ForMember(x => x.Category, opt => opt.MapFrom(s => s.Category.ToDto()));
            Mapper.CreateMap<CargoDTO, Cargo>()
                .ForMember(x => x.Category, opt => opt.MapFrom(s => s.Category.ToModel()));
        }

        public static Cargo ToModel(this CargoDTO dto)
        {
            return Mapper.Map<Cargo>(dto);
        }

        public static CargoDTO ToDto(this Cargo model)
        {
            return Mapper.Map<CargoDTO>(model);
        }
    }
}
