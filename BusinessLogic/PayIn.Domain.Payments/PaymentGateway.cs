using System.ComponentModel.DataAnnotations;
using Xp.Domain;

namespace PayIn.Domain.Payments
{
	public class PaymentGateway : IEntity
	{
		                                      public int    Id   { get; set; }
		[Required(AllowEmptyStrings = false)] public string Name { get; set; }
	}
}
