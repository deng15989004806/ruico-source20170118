using System.Linq;
using AutoMapper;
using Ruico.Domain.UserSystemModule.Entities;

namespace Ruico.Dto.UserSystem.Converters
{
    public static class MenuConverters
    {
        static MenuConverters()
        {
            Mapper.CreateMap<Menu, MenuDTO>()
                .ForMember(x => x.Module, opt => opt.MapFrom(s => s.Module.ToDto()))
                .ForMember(x => x.Parent, opt => opt.MapFrom(s => s.Parent.ToDto()))
                .ForMember(x => x.Permissions, opt => opt.MapFrom(s => s.Permissions.Select(x=>x.ToDto()).ToList()));
            Mapper.CreateMap<MenuDTO, Menu>()
                .ForMember(x => x.Module, opt => opt.MapFrom(s => s.Module.ToModel()))
                .ForMember(x => x.Parent, opt => opt.MapFrom(s => s.Parent.ToModel()))
                .ForMember(x => x.Permissions, opt => opt.MapFrom(s => s.Permissions.Select(x => x.ToModel()).ToList()));

            Mapper.CreateMap<Permission, PermissionDTO>();
            Mapper.CreateMap<PermissionDTO, Permission>();
        }

        public static Menu ToModel(this MenuDTO dto)
        {
            return Mapper.Map<Menu>(dto);
        }

        public static MenuDTO ToDto(this Menu model)
        {
            return Mapper.Map<MenuDTO>(model);
        }

        public static Permission ToModel(this PermissionDTO dto)
        {
            return Mapper.Map<Permission>(dto);
        }

        public static PermissionDTO ToDto(this Permission model)
        {
            return Mapper.Map<PermissionDTO>(model);
        }
    }
}
