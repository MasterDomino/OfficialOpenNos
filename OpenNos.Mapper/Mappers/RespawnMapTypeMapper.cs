using OpenNos.DAL.EF;
using OpenNos.Data;

namespace OpenNos.Mapper.Mappers
{
    public class RespawnMapTypeMapper
    {
        public RespawnMapTypeMapper()
        {
        }

        public void ToRespawnMapTypeDTO(RespawnMapType input, RespawnMapTypeDTO output)
        {
            output.DefaultMapId = input.DefaultMapId;
            output.DefaultX = input.DefaultX;
            output.DefaultY = input.DefaultY;
            output.Name = input.Name;
            output.RespawnMapTypeId = input.RespawnMapTypeId;
        }

        public void ToRespawnMapType(RespawnMapTypeDTO input, RespawnMapType output)
        {
            output.DefaultMapId = input.DefaultMapId;
            output.DefaultX = input.DefaultX;
            output.DefaultY = input.DefaultY;
            output.Name = input.Name;
            output.RespawnMapTypeId = input.RespawnMapTypeId;
        }
    }
}

