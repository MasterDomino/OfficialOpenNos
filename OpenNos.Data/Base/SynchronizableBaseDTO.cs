using System;

namespace OpenNos.Data
{
    public abstract class SynchronizableBaseDTO : MappingBaseDTO
    {
        #region Instantiation

        protected SynchronizableBaseDTO() => Id = Guid.NewGuid(); //make unique

        #endregion

        #region Properties

        public Guid Id { get; set; }

        #endregion

        #region Methods

        public override bool Equals(object obj) => ((SynchronizableBaseDTO)obj).Id == Id;

        public override int GetHashCode() => Id.GetHashCode();

        #endregion
    }
}