using AutoMapper;
using Ruico.Domain.UserSystemModule.Entities;

namespace Ruico.Dto.UserSystem.Converters
{
    public static class RoleGroupConverters
    {
        static RoleGroupConverters()
        {
            Mapper.CreateMap<RoleGroup, RoleGroupDTO>();
            Mapper.CreateMap<RoleGroupDTO, RoleGroup>();
        }

        public static RoleGroup ToModel(this RoleGroupDTO dto)
        {
            return Mapper.Map<RoleGroup>(dto);
        }

        public static RoleGroupDTO ToDto(this RoleGroup model)
        {
            return Mapper.Map<RoleGroupDTO>(model);
        }
    }
}
