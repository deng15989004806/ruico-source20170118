using AutoMapper;
using Ruico.Domain.WeixinModule.Entities;

namespace Ruico.Dto.Weixin.Converters
{
    public static class AppMenuConverters
    {
        static AppMenuConverters()
        {
            Mapper.CreateMap<AppMenu, AppMenuDTO>()
                .ForMember(x => x.Parent, opt => opt.MapFrom(s => s.Parent.ToDto()));
            Mapper.CreateMap<AppMenuDTO, AppMenu>()
                .ForMember(x => x.Parent, opt => opt.MapFrom(s => s.Parent.ToModel()));
        }

        public static AppMenu ToModel(this AppMenuDTO dto)
        {
            return Mapper.Map<AppMenu>(dto);
        }

        public static AppMenuDTO ToDto(this AppMenu model)
        {
            return Mapper.Map<AppMenuDTO>(model);
        }
    }
}
