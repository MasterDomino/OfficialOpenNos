using OpenNos.DAL.Interface;
using OpenNos.Data;
using System;

namespace OpenNos.DAL.Mock
{
    public class ShellEffectGenerationDAO : BaseDAO<ShellEffectGenerationDTO>, IShellEffectGenerationDAO
    {
        public ShellEffectGenerationDTO InsertOrUpdate(ShellEffectGenerationDTO shelleffect) => throw new NotImplementedException();
    }
}
