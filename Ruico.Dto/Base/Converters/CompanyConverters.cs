using AutoMapper;
using Ruico.Domain.BaseModule.Entities;

namespace Ruico.Dto.Base.Converters
{
    public static class CompanyConverters
    {
        static CompanyConverters()
        {
            Mapper.CreateMap<Company, CompanyDTO>()
                .ForMember(x => x.Category, opt => opt.MapFrom(s => s.Category.ToDto()));
            Mapper.CreateMap<CompanyDTO, Company>()
                .ForMember(x => x.Category, opt => opt.MapFrom(s => s.Category.ToModel()));
        }

        public static Company ToModel(this CompanyDTO dto)
        {
            return Mapper.Map<Company>(dto);
        }

        public static CompanyDTO ToDto(this Company model)
        {
            return Mapper.Map<CompanyDTO>(model);
        }
    }
}
