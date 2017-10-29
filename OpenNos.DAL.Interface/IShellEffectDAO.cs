using OpenNos.Data;
using OpenNos.Data.Enums;
using System;
using System.Collections.Generic;

namespace OpenNos.DAL.Interface
{
    public interface IShellEffectDAO : IMappingBaseDAO
    {
        #region Methods

        DeleteResult DeleteByEquipmentSerialId(Guid id);

        ShellEffectDTO InsertOrUpdate(ShellEffectDTO shelleffect);

        IEnumerable<ShellEffectDTO> LoadByEquipmentSerialId(Guid id);

        #endregion
    }
}