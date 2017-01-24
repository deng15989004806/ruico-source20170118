using AutoMapper;
using Ruico.Domain.SystemModule.Entities;

namespace Ruico.Dto.System.Converters
{
    public static class SerialNumberConverters
    {
        static SerialNumberConverters()
        {
            Mapper.CreateMap<SerialNumber, SerialNumberDTO>();
            Mapper.CreateMap<SerialNumberDTO, SerialNumber>();
        }

        public static SerialNumber ToModel(this SerialNumberDTO dto)
        {
            return Mapper.Map<SerialNumber>(dto);
        }

        public static SerialNumberDTO ToDto(this SerialNumber model)
        {
            return Mapper.Map<SerialNumberDTO>(model);
        }
    }
}
