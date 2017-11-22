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
            if (input == null)
            {
                output = null;
                return;
            }
            output.MapId = input.MapId;
            output.MapTypeId = input.MapTypeId;
        }

        public void ToMapTypeMap(MapTypeMapDTO input, MapTypeMap output)
        {
            if (input == null)
            {
                output = null;
                return;
            }
            output.MapId = input.MapId;
            output.MapTypeId = input.MapTypeId;
        }
    }
}
