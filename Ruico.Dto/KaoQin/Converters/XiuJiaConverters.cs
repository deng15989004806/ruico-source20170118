using AutoMapper;
using Ruico.Domain.KaoQinModule.Entities;

namespace Ruico.Dto.KaoQin.Converters
{
    public static class XiuJiaConverters
    {
        static XiuJiaConverters()
        {
            Mapper.CreateMap<XiuJia, XiuJiaDTO>();
            Mapper.CreateMap<XiuJiaDTO, XiuJia>();
        }

        public static XiuJia ToModel(this XiuJiaDTO dto)
        {
            return Mapper.Map<XiuJia>(dto);
        }

        public static XiuJiaDTO ToDto(this XiuJia model)
        {
            return Mapper.Map<XiuJiaDTO>(model);
        }
    }
}
