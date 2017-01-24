using AutoMapper;
using Ruico.Domain.BaseModule.Entities;

namespace Ruico.Dto.Base.Converters
{
    public static class SerialNumberRuleConverters
    {
        static SerialNumberRuleConverters()
        {
            Mapper.CreateMap<SerialNumberRule, SerialNumberRuleDTO>();
            Mapper.CreateMap<SerialNumberRuleDTO, SerialNumberRule>();
        }

        public static SerialNumberRule ToModel(this SerialNumberRuleDTO dto)
        {
            return Mapper.Map<SerialNumberRule>(dto);
        }

        public static SerialNumberRuleDTO ToDto(this SerialNumberRule model)
        {
            return Mapper.Map<SerialNumberRuleDTO>(model);
        }
    }
}
