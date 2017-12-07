using OpenNos.DAL.EF;
using OpenNos.Data;

namespace OpenNos.Mapper.Mappers
{
    public class MapTypeMapMapper
    {
        #region Methods

        public bool ToMapTypeMap(MapTypeMapDTO input, MapTypeMap output)
        {
            if (input == null)
            {
                output = null;
                return false;
            }
            output.MapId = input.MapId;
            output.MapTypeId = input.MapTypeId;
            return true;
        }

        public bool ToMapTypeMapDTO(MapTypeMap input, MapTypeMapDTO output)
        {
            if (input == null)
            {
                output = null;
                return false;
            }
            output.MapId = input.MapId;
            output.MapTypeId = input.MapTypeId;
            return true;
        }

        #endregion
    }
}