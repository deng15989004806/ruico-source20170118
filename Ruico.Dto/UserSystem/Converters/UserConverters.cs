using AutoMapper;
using Ruico.Domain.UserSystemModule.Entities;

namespace Ruico.Dto.UserSystem.Converters
{
    public static class UserConverters
    {
        static UserConverters()
        {
            Mapper.CreateMap<User, UserDTO>();
            Mapper.CreateMap<UserDTO, User>();
        }

        public static User ToModel(this UserDTO dto)
        {
            return Mapper.Map<User>(dto);
        }

        public static UserDTO ToDto(this User model)
        {
            return Mapper.Map<UserDTO>(model);
        }
    }
}
