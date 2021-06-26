using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Xp.Domain;

namespace PayIn.Domain.Public
{
    public class ServiceUserLink : Entity
	{
		[Required(AllowEmptyStrings = false)] public string Login { get; set; }

		#region Card
		public int? CardId { get; set; }
		[ForeignKey("CardId")]
		public ServiceCard Card { get; set; }
		#endregion Card
    }
}
