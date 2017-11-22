using OpenNos.DAL.EF;
using OpenNos.Data;

namespace OpenNos.Mapper.Mappers
{
    public class ScriptedInstanceMapper
    {
        public ScriptedInstanceMapper()
        {
        }

        public void ToScriptedInstanceDTO(ScriptedInstance input, ScriptedInstanceDTO output)
        {
            output.MapId = input.MapId;
            output.PositionX = input.PositionX;
            output.PositionY = input.PositionY;
            output.Script = input.Script;
            output.ScriptedInstanceId = input.ScriptedInstanceId;
            output.Type = input.Type;
        }

        public void ToScriptedInstance(ScriptedInstanceDTO input, ScriptedInstance output)
        {
            output.MapId = input.MapId;
            output.PositionX = input.PositionX;
            output.PositionY = input.PositionY;
            output.Script = input.Script;
            output.ScriptedInstanceId = input.ScriptedInstanceId;
            output.Type = input.Type;
        }
    }
}

