using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Arguments.ServiceCheckPoint
{
	public partial class ServiceCheckPointDeleteArguments : IArgumentsBase
	{
		public int Id { get; private set; }

		#region Constructors
		public ServiceCheckPointDeleteArguments(int id)
		{
			Id = id;
		}
		#endregion Constructors
	}
}
