using PayIn.Domain.Transport;
using Xp.Common;
using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Transport.Arguments.TransportSystem
{
	public class TransportSystemGetAllArguments : IArgumentsBase
	{
		public string Filter { get; set; }

		#region Constructors
		public TransportSystemGetAllArguments(string filter)
		{
			Filter = filter ?? "";
		}
		#endregion Constructors
	}
}
