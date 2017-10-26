using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace OpenNos.DAL.EF
{
    public class RecipeList
    {
        #region Instantiation

        public RecipeList()
        {
            Item = new HashSet<Item>();
            MapNpc = new HashSet<MapNpc>();
        }

        #endregion

        #region Properties

        public virtual ICollection<Item> Item { get; set; }

        public virtual ICollection<MapNpc> MapNpc { get; set; }

        public virtual Recipe Recipe { get; set; }

        public short RecipeId { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int RecipeListId { get; set; }

        #endregion
    }
}