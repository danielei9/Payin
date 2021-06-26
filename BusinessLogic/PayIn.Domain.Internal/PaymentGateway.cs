using System.ComponentModel.DataAnnotations;
using Xp.Domain;

namespace PayIn.Domain.Internal
{
	public class PaymentGateway : IEntity
	{
		                            public int    Id    { get; set; }
		[Required]	[MinLength(1)]	public string	Name	{ get; set; }
	}
}
