using AutoMapper;
using Ruico.Domain.UserSystemModule.Entities;

namespace Ruico.Dto.UserSystem.Converters
{
    public static class ModuleConverters
    {
        static ModuleConverters()
        {
            Mapper.CreateMap<Module, ModuleDTO>();
            Mapper.CreateMap<ModuleDTO, Module>();
        }

        public static Module ToModel(this ModuleDTO dto)
        {
            return Mapper.Map<Module>(dto);
        }

        public static ModuleDTO ToDto(this Module model)
        {
            return Mapper.Map<ModuleDTO>(model);
        }
    }
}
