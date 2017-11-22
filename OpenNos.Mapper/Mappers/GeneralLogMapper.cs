using OpenNos.DAL.EF;
using OpenNos.Data;

namespace OpenNos.Mapper.Mappers
{
    public class GeneralLogMapper
    {
        public GeneralLogMapper()
        {

        }

        public void ToGeneralLogDTO(GeneralLog input, GeneralLogDTO output)
        {
            if (input == null)
            {
                output = null;
                return;
            }
            output.AccountId = input.AccountId;
            output.CharacterId = input.CharacterId;
            output.IpAddress = input.IpAddress;
            output.LogData = input.LogData;
            output.LogId = input.LogId;
            output.LogType = input.LogType;
            output.Timestamp = input.Timestamp;
        }

        public void ToGeneralLog(GeneralLogDTO input, GeneralLog output)
        {
            if (input == null)
            {
                output = null;
                return;
            }
            output.AccountId = input.AccountId;
            output.CharacterId = input.CharacterId;
            output.IpAddress = input.IpAddress;
            output.LogData = input.LogData;
            output.LogId = input.LogId;
            output.LogType = input.LogType;
            output.Timestamp = input.Timestamp;
        }
    }
}
