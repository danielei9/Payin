using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Arguments.ServiceCheckPoint
{
	public partial class ServiceCheckPointGetArguments : IArgumentsBase
  {
		public int Id { get; set; }

		#region Constructors
		public ServiceCheckPointGetArguments(int id)
		{
			Id = id;
		}
		#endregion Constructors
	}
}
