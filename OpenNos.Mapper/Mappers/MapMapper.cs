using OpenNos.DAL.EF;
using OpenNos.Data;

namespace OpenNos.Mapper.Mappers
{
    public class MapMapper
    {
        #region Methods

        public bool ToMap(MapDTO input, Map output)
        {
            if (input == null)
            {
                output = null;
                return false;
            }
            output.Data = input.Data;
            output.MapId = input.MapId;
            output.Music = input.Music;
            output.Name = input.Name;
            output.ShopAllowed = input.ShopAllowed;
            return true;
        }

        public bool ToMapDTO(Map input, MapDTO output)
        {
            if (input == null)
            {
                output = null;
                return false;
            }
            output.Data = input.Data;
            output.MapId = input.MapId;
            output.Music = input.Music;
            output.Name = input.Name;
            output.ShopAllowed = input.ShopAllowed;
            return true;
        }

        #endregion
    }
}