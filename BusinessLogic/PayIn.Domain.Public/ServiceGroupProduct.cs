using System.ComponentModel.DataAnnotations.Schema;
using Xp.Domain;

namespace PayIn.Domain.Public
{
    public class ServiceGroupProduct : Entity
    {
        public int ProductId { get; set; }

        #region Group
        public int? GroupId { get; set; }
        [ForeignKey("GroupId")]
        public ServiceGroup Group { get; set; }
        #endregion Group
    }
}
