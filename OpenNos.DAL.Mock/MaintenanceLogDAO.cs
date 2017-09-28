using System;
using OpenNos.DAL.Interface;
using OpenNos.Data;

namespace OpenNos.DAL.Mock
{
    public class MaintenanceLogDAO : BaseDAO<MaintenanceLogDTO>, IMaintenanceLogDAO
    {
        public MaintenanceLogDTO LoadFirst() => throw new NotImplementedException();
    }
}