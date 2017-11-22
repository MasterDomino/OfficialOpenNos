using OpenNos.DAL.EF;
using OpenNos.Data;
using System;

namespace OpenNos.Mapper.Mappers
{
    public class MapTypeMapper
    {
        public MapTypeMapper()
        {

        }

        public void ToMapTypeDTO(MapType input, MapTypeDTO output)
        {
            output.MapTypeId = input.MapTypeId;
            output.MapTypeName = input.MapTypeName;
            output.PotionDelay = input.PotionDelay;
            output.RespawnMapTypeId = input.RespawnMapTypeId;
            output.ReturnMapTypeId = input.ReturnMapTypeId;
        }

        public void ToMapType(MapTypeDTO input, MapType output)
        {
            output.MapTypeId = input.MapTypeId;
            output.MapTypeName = input.MapTypeName;
            output.PotionDelay = input.PotionDelay;
            output.RespawnMapTypeId = input.RespawnMapTypeId;
            output.ReturnMapTypeId = input.ReturnMapTypeId;
        }
    }
}
