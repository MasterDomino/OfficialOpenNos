using OpenNos.DAL.Interface;
using OpenNos.Data;
using System;
using System.Collections.Generic;
using OpenNos.Data.Enums;

namespace OpenNos.DAL.Mock
{
    public class ShellEffectDAO : BaseDAO<ShellEffectDTO>, IShellEffectDAO
    {
        public DeleteResult DeleteByItemInstanceId(Guid id) => throw new NotImplementedException();

        public ShellEffectDTO InsertOrUpdate(ShellEffectDTO shelleffect) => throw new NotImplementedException();

        public IEnumerable<ShellEffectDTO> LoadByItemInstanceId(Guid id) => throw new NotImplementedException();
    }
}
