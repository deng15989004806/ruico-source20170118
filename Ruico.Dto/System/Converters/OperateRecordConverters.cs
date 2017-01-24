using AutoMapper;
using Ruico.Domain.SystemModule.Entities;

namespace Ruico.Dto.System.Converters
{
    public static class OperateRecordConverters
    {
        static OperateRecordConverters()
        {
            Mapper.CreateMap<OperateRecord, OperateRecordDTO>();
            Mapper.CreateMap<OperateRecordDTO, OperateRecord>();

            Mapper.CreateMap<OperateRecordArchive, OperateRecordDTO>();
            Mapper.CreateMap<OperateRecordExtend, OperateRecordExtendDTO>();
        }

        public static OperateRecord ToModel(this OperateRecordDTO dto)
        {
            return Mapper.Map<OperateRecord>(dto);
        }

        public static OperateRecordDTO ToDto(this OperateRecord model)
        {
            return Mapper.Map<OperateRecordDTO>(model);
        }

        public static OperateRecordDTO ToDto(this OperateRecordArchive model)
        {
            return Mapper.Map<OperateRecordDTO>(model);
        }

        public static OperateRecordExtendDTO ToDto(this OperateRecordExtend model)
        {
            return Mapper.Map<OperateRecordExtendDTO>(model);
        }
    }
}
