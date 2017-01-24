using AutoMapper;
using Ruico.Domain.UserSystemModule.Entities;

namespace Ruico.Dto.UserSystem.Converters
{
    public static class RoleConverters
    {
        static RoleConverters()
        {
            Mapper.CreateMap<Role, RoleDTO>();

            Mapper.CreateMap<RoleDTO, Role>();
        }

        public static Role ToModel(this RoleDTO dto)
        {
            return Mapper.Map<Role>(dto);
        }

        public static RoleDTO ToDto(this Role model)
        {
            return Mapper.Map<RoleDTO>(model);
        }
    }
}
