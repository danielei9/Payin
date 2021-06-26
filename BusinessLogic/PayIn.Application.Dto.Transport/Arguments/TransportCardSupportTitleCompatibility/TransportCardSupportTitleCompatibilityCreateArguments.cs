using System.ComponentModel.DataAnnotations;
using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Transport.Arguments.TransportCardSupportTitleCompatibility
{
	public class TransportCardSupportTitleCompatibilityCreateArguments : IArgumentsBase
	{
		[Display(Name = "resources.transportCardSupportTitleCompatibility.transportCardSupportId")]
		[Required]
		public int TransportCardSupportId { get; set; }
		public int TitleId { get; set; }

		#region Constructors
		public TransportCardSupportTitleCompatibilityCreateArguments(int transportCardSupportId, int titleId)
		{
			TransportCardSupportId = transportCardSupportId;
			TitleId = titleId;
		}
		#endregion Constructors
	}
}