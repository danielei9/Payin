using System.ComponentModel.DataAnnotations;
using Xp.Domain;

namespace PayIn.Domain.Payments
{
	public class Gateway : Entity
	{
		[Required(AllowEmptyStrings = false)] public string Name { get; set; }
	}
}
