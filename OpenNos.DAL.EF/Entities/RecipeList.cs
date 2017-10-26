using System.ComponentModel.DataAnnotations.Schema;

namespace OpenNos.DAL.EF
{
    public class RecipeList
    {
        #region Properties

        public virtual Item Item { get; set; }

        public virtual MapNpc MapNpc { get; set; }

        public virtual Recipe Recipe { get; set; }

        public short? ItemVNum { get; set; }

        public int? MapNpcId { get; set; }

        public short RecipeId { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int RecipeListId { get; set; }

        #endregion
    }
}