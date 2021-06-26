using System.ComponentModel.DataAnnotations;
using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Transport.Arguments.TransportSimultaneousTitleCompatibilities
{
	public class TransportSimultaneousTitleCompatibilitiesCreateArguments : IArgumentsBase
	{
		[Display(Name = "resources.transportSimultaneousTitleCompatibilities.titleName")]
		[Required]
		public int TransportTitle2Id { get; set; }
		public int TitleId { get; set; }

		#region Constructors
		public TransportSimultaneousTitleCompatibilitiesCreateArguments(int transportTitle2Id, int titleId)
		{
			TransportTitle2Id = transportTitle2Id;
			TitleId = titleId;
		}
		#endregion Constructors
	}
}
