using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenNos.DAL.EF
{
    public class Quest
    {
        #region Instantiation

        public Quest()
        {
            QuestProgress = new HashSet<QuestProgress>();
        }

        #endregion

        #region Properties

        public long QuestId { get; set; }

        public string QuestData { get; set; }

        public virtual ICollection<QuestProgress> QuestProgress { get; set; }


        #endregion
    }
}
