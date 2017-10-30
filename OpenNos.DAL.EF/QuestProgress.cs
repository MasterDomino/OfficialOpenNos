using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenNos.DAL.EF
{
    public class QuestProgress
    {
        #region Instantiation

        public QuestProgress()
        {
        }

        #endregion

        #region Properties

        public long QuestProgressId { get; set; }

        public virtual Quest Quest { get; set; }

        public long QuestId { get; set; }

        public string QuestData { get; set; }

        public virtual Character Character { get; set; }

        public long CharacterId { get; set; }

        public bool IsFinished { get; set; }

        #endregion
    }
}
