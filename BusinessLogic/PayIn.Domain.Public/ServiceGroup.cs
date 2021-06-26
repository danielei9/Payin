using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Xp.Domain;

namespace PayIn.Domain.Public
{
    public class ServiceGroup : Entity
    {
        [Required(AllowEmptyStrings = false)]
        public string Name { get; set; }
        public bool AllProduct { get; set; }
        public bool AllEntranceType { get; set; }

		#region DefaultGroupWhenEmitCategories
		[InverseProperty("DefaultGroupWhenEmit")]
		public ICollection<ServiceCategory> DefaultGroupWhenEmitCategories { get; set; }
		#endregion DefaultGroupWhenEmitCategories

		#region Category
		public int? CategoryId { get; set; }
		[ForeignKey("CategoryId")]
		public ServiceCategory Category { get; set; }
		#endregion Category

		#region Users
		[InverseProperty("Groups")]
        public ICollection<ServiceUser> Users { get; set; } = new List<ServiceUser>();
        #endregion Users

		#region Products
		[InverseProperty("Group")]
        public ICollection<ServiceGroupProduct> Products { get; set; }
		#endregion Products

		#region EntranceTypes
		[InverseProperty("Group")]
        public ICollection<ServiceGroupEntranceType> EntranceTypes { get; set; }
        #endregion EntranceTypes

        #region Cards
        [InverseProperty("Groups")]
        public ICollection<ServiceCard> Cards { get; set; } = new List<ServiceCard>();
        #endregion Cards
    }
}
