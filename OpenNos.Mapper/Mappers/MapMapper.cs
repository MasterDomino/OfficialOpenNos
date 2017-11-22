using OpenNos.DAL.EF;
using OpenNos.Data;
using System;

namespace OpenNos.Mapper.Mappers
{
    public class MapMapper
    {
        public MapMapper()
        {

        }

        public void ToMapDTO(Map input, MapDTO output)
        {
            if (input == null)
            {
                output = null;
                return;
            }
            output.Data = input.Data;
            output.MapId = input.MapId;
            output.Music = input.Music;
            output.Name = input.Name;
            output.ShopAllowed = input.ShopAllowed;
        }

        public void ToMap(MapDTO input, Map output)
        {
            if (input == null)
            {
                output = null;
                return;
            }
            output.Data = input.Data;
            output.MapId = input.MapId;
            output.Music = input.Music;
            output.Name = input.Name;
            output.ShopAllowed = input.ShopAllowed;
        }
    }
}
