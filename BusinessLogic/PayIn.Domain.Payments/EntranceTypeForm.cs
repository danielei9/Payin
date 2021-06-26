using System.ComponentModel.DataAnnotations.Schema;
using Xp.Domain;

namespace PayIn.Domain.Payments
{
	public class EntranceTypeForm : Entity
	{
		public int	FormId	{ get; set; }
		public int	Order	{ get; set; }

		#region EntranceType
		public int EntranceTypeId { get; set; }
		[ForeignKey("EntranceTypeId")]
		public EntranceType EntranceType { get; set; }
		#endregion EntranceType
	}
}
