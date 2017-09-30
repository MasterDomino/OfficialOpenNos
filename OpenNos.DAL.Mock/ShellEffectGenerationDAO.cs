using OpenNos.DAL.Interface;
using OpenNos.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenNos.DAL.Mock
{
    public class ShellEffectGenerationDAO : BaseDAO<ShellEffectGenerationDTO>, IShellEffectGenerationDAO
    {
        public ShellEffectGenerationDTO InsertOrUpdate(ShellEffectGenerationDTO shelleffect) => throw new NotImplementedException();
    }
}
