using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Transport.Arguments.TransportSimultaneousTitleCompatibilities
{
	public class TransportSimultaneousTitleCompatibilitiesDeleteArguments : IArgumentsBase
	{
		public int Id { get; set; }

		#region Constructors
		public TransportSimultaneousTitleCompatibilitiesDeleteArguments(int id)
		{
			Id = id;
		}
		#endregion Constructors
	}
}
