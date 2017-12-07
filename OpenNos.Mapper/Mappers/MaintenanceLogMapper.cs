using OpenNos.DAL.EF;
using OpenNos.Data;

namespace OpenNos.Mapper.Mappers
{
    public class MaintenanceLogMapper
    {
        #region Methods

        public bool ToMaintenanceLog(MaintenanceLogDTO input, MaintenanceLog output)
        {
            if (input == null)
            {
                output = null;
                return false;
            }
            output.DateEnd = input.DateEnd;
            output.DateStart = input.DateStart;
            output.LogId = input.LogId;
            output.Reason = input.Reason;
            return true;
        }

        public bool ToMaintenanceLogDTO(MaintenanceLog input, MaintenanceLogDTO output)
        {
            if (input == null)
            {
                output = null;
                return false;
            }
            output.DateEnd = input.DateEnd;
            output.DateStart = input.DateStart;
            output.LogId = input.LogId;
            output.Reason = input.Reason;
            return true;
        }

        #endregion
    }
}