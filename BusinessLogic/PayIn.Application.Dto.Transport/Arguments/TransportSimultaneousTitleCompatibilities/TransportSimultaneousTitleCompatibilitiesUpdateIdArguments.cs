using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Transport.Arguments.TransportSimultaneousTitleCompatibilities
{
	public class TransportSimultaneousTitleCompatibilitiesUpdateIdArguments : IArgumentsBase
	{
		public int Id { get; set; }

		#region Constructors
		public TransportSimultaneousTitleCompatibilitiesUpdateIdArguments(int id)
		{
			Id = Id;
		}
		#endregion Constructors
	}
}
