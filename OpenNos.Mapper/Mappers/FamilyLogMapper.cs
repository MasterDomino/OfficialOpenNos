using OpenNos.DAL.EF;
using OpenNos.Data;

namespace OpenNos.Mapper.Mappers
{
    public class FamilyLogMapper
    {
        public FamilyLogMapper()
        {

        }

        public void ToFamilyLogDTO(FamilyLog input, FamilyLogDTO output)
        {
            if (input == null)
            {
                output = null;
                return;
            }
            output.FamilyId = input.FamilyId;
            output.FamilyLogData = input.FamilyLogData;
            output.FamilyLogId = input.FamilyLogId;
            output.FamilyLogType = input.FamilyLogType;
            output.Timestamp = input.Timestamp;
        }

        public void ToFamilyLog(FamilyLogDTO input, FamilyLog output)
        {
            if (input == null)
            {
                output = null;
                return;
            }
            output.FamilyId = input.FamilyId;
            output.FamilyLogData = input.FamilyLogData;
            output.FamilyLogId = input.FamilyLogId;
            output.FamilyLogType = input.FamilyLogType;
            output.Timestamp = input.Timestamp;
        }
    }
}
