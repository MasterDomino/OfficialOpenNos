using OpenNos.DAL.EF;
using OpenNos.Data;

namespace OpenNos.Mapper.Mappers
{
    public class PenaltyLogMapper
    {
        public PenaltyLogMapper()
        {
        }

        public void ToPenaltyLogDTO(PenaltyLog input, PenaltyLogDTO output)
        {
            output.AccountId = input.AccountId;
            output.AdminName = input.AdminName;
            output.DateEnd = input.DateEnd;
            output.DateStart = input.DateStart;
            output.Penalty = input.Penalty;
            output.PenaltyLogId = input.PenaltyLogId;
            output.Reason = input.Reason;
        }

        public void ToPenaltyLog(PenaltyLogDTO input, PenaltyLog output)
        {
            output.AccountId = input.AccountId;
            output.AdminName = input.AdminName;
            output.DateEnd = input.DateEnd;
            output.DateStart = input.DateStart;
            output.Penalty = input.Penalty;
            output.PenaltyLogId = input.PenaltyLogId;
            output.Reason = input.Reason;
        }
    }
}
