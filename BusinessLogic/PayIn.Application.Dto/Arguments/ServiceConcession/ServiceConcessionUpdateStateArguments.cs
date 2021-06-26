using PayIn.Common;
using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Arguments.ServiceConcession
{
	public partial class ServiceConcessionUpdateStateArguments : IArgumentsBase
	{
		public int             Id      { get; private set;}
		public ConcessionState State   { get; private set;}

		#region Constructors
		public ServiceConcessionUpdateStateArguments(int id, ConcessionState state) 
		{
			Id = id;
			State = state;
		}
		#endregion Constructors
	}
}
