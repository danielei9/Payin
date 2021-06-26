using System.ComponentModel.DataAnnotations;
using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Transport.Arguments.TransportCardSupportTitleCompatibility
{
	public class TransportCardSupportTitleCompatibilityUpdateArguments : IArgumentsBase
	{
		public int Id { get; set; }
		[Display(Name = "resources.transportCardSupportTitleCompatibility.transportCardSupportId")]
		[Required]
		public int TransportCardSupportId { get; set; }
		public int TransportTitleId { get; set; }
		[Display(Name = "resources.transportCardSupportTitleCompatibility.transportCardSupportId")]
		public string TransportCardSupportName { get; set; }

		#region Constructors
		public TransportCardSupportTitleCompatibilityUpdateArguments(int id, int transportTitleId, int transportCardSupportId, string transportCardSupportName)
		{
			Id = Id;
            TransportTitleId = transportTitleId;
			TransportCardSupportId = transportCardSupportId;
			TransportCardSupportName = transportCardSupportName;
		}
		#endregion Constructors
	}
}
