using AutoMapper;
using Ruico.Domain.BaseModule.Entities;

namespace Ruico.Dto.Base.Converters
{
    public static class CategoryConverters
    {
        static CategoryConverters()
        {
            Mapper.CreateMap<Category, CategoryDTO>()
                .ForMember(x => x.Parent, opt => opt.MapFrom(s => s.Parent.ToDto()));
            Mapper.CreateMap<CategoryDTO, Category>()
                .ForMember(x => x.Parent, opt => opt.MapFrom(s => s.Parent.ToModel()));
        }

        public static Category ToModel(this CategoryDTO dto)
        {
            return Mapper.Map<Category>(dto);
        }

        public static CategoryDTO ToDto(this Category model)
        {
            return Mapper.Map<CategoryDTO>(model);
        }
    }
}
