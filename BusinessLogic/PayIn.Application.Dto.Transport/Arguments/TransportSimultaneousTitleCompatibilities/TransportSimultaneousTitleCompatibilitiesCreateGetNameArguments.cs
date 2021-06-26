using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Transport.Arguments.TransportSimultaneousTitleCompatibilities
{
	public class TransportSimultaneousTitleCompatibilitiesCreateGetNameArguments : IArgumentsBase
	{
		public int Id { get; set; }

		#region Constructors
		public TransportSimultaneousTitleCompatibilitiesCreateGetNameArguments(int id)
		{
			Id = Id;
		}
		#endregion Constructors
	}
}
