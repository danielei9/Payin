using PayIn.Domain.Transport;
using System.ComponentModel.DataAnnotations;
using Xp.Common;
using Xp.Common.Dto.Arguments;
using Xp.Domain.Transport;

namespace PayIn.Application.Dto.Transport.Arguments.TransportSystem
{
	public class TransportSystemCreateArguments : IArgumentsBase
	{
		
		[Display(Name= "resources.transportSystem.name")]     [Required]  public string Name { get; set; }
		[Display(Name= "resources.transportSystem.cardType")] [Required]  public CardType CardType { get; set; }

		#region Constructors
		public TransportSystemCreateArguments(string name, CardType cardType)
		{
			Name = name;
			CardType = cardType;
		}
		#endregion Constructors
	}
}
