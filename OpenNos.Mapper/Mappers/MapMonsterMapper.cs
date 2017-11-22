using OpenNos.DAL.EF;
using OpenNos.Data;
using System;

namespace OpenNos.Mapper.Mappers
{
    public class MapMonsterMapper
    {
        public MapMonsterMapper()
        {

        }

        public void ToMapMonsterDTO(MapMonster input, MapMonsterDTO output)
        {
            if (input == null)
            {
                output = null;
                return;
            }
            output.IsDisabled = input.IsDisabled;
            output.IsMoving = input.IsMoving;
            output.MapId = input.MapId;
            output.MapMonsterId = input.MapMonsterId;
            output.MapX = input.MapX;
            output.MapY = input.MapY;
            output.MonsterVNum = input.MonsterVNum;
            output.Position = input.Position;
        }

        public void ToMapMonster(MapMonsterDTO input, MapMonster output)
        {
            if (input == null)
            {
                output = null;
                return;
            }
            output.IsDisabled = input.IsDisabled;
            output.IsMoving = input.IsMoving;
            output.MapId = input.MapId;
            output.MapMonsterId = input.MapMonsterId;
            output.MapX = input.MapX;
            output.MapY = input.MapY;
            output.MonsterVNum = input.MonsterVNum;
            output.Position = input.Position;
        }
    }
}
