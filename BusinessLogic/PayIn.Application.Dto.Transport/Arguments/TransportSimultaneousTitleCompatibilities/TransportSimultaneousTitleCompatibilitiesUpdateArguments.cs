using System.ComponentModel.DataAnnotations;
using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Transport.Arguments.TransportSimultaneousTitleCompatibilities
{
	public class TransportSimultaneousTitleCompatibilitiesUpdateArguments : IArgumentsBase
	{
		public int Id { get; set; }
		[Required]
		public int TransportTitle2Id { get; set; }
		public int TransportTitleId { get; set; }
		[Display(Name = "resources.transportSimultaneousTitleCompatibilities.titleName")]
		public string Name { get; set; }

		#region Constructors
		public TransportSimultaneousTitleCompatibilitiesUpdateArguments(int id, int transportTitleId, int transportTitle2Id, string name)
		{
			Id = Id;
            TransportTitleId = transportTitleId;
            TransportTitle2Id = transportTitle2Id;
			Name = name;
		}
		#endregion Constructors
	}
}
