using AutoMapper;
using Ruico.Domain.KaoQinModule.Entities;

namespace Ruico.Dto.KaoQin.Converters
{
    public static class ChuChaiConverters
    {
        static ChuChaiConverters()
        {
            Mapper.CreateMap<ChuChai, ChuChaiDTO>();
            Mapper.CreateMap<ChuChaiDTO, ChuChai>();
        }

        public static ChuChai ToModel(this ChuChaiDTO dto)
        {
            return Mapper.Map<ChuChai>(dto);
        }

        public static ChuChaiDTO ToDto(this ChuChai model)
        {
            return Mapper.Map<ChuChaiDTO>(model);
        }
    }
}
