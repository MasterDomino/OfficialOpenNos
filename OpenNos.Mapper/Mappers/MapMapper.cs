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
            output.Data = input.Data;
            output.MapId = input.MapId;
            output.Music = input.Music;
            output.Name = input.Name;
            output.ShopAllowed = input.ShopAllowed;
        }

        public void ToMap(MapDTO input, Map output)
        {
            output.Data = input.Data;
            output.MapId = input.MapId;
            output.Music = input.Music;
            output.Name = input.Name;
            output.ShopAllowed = input.ShopAllowed;
        }
    }
}
