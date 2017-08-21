using OpenNos.Data;
using System.Collections.Generic;

namespace OpenNos.DAL.Interface
{
    public interface IShellEffectGenerationDAO
    {
        #region Methods

        ShellEffectGenerationDTO InsertOrUpdate(ShellEffectGenerationDTO shelleffect);

        IEnumerable<ShellEffectGenerationDTO> LoadAll();

        #endregion
    }
}