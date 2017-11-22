using OpenNos.DAL.EF;
using OpenNos.Data;
using System;

namespace OpenNos.Mapper.Mappers
{
    public class MapTypeMapMapper
    {
        public MapTypeMapMapper()
        {

        }

        public void ToMapTypeMapDTO(MapTypeMap input, MapTypeMapDTO output)
        {
            output.MapId = input.MapId;
            output.MapTypeId = input.MapTypeId;
        }

        public void ToMapTypeMap(MapTypeMapDTO input, MapTypeMap output)
        {
            output.MapId = input.MapId;
            output.MapTypeId = input.MapTypeId;
        }
    }
}
