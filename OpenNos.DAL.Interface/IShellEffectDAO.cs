using OpenNos.Data;
using OpenNos.Data.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenNos.DAL.Interface
{
    public interface IShellEffectDAO : IMappingBaseDAO
    {
        DeleteResult DeleteByItemInstanceId(Guid id);

        ShellEffectDTO InsertOrUpdate(ShellEffectDTO shelleffect);

        IEnumerable<ShellEffectDTO> LoadByItemInstanceId(Guid id);
    }
}
