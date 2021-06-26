using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Arguments.ServiceCheckPoint
{
	public partial class ServiceCheckPointGetItemChecksArguments : IArgumentsBase
	{
		public string Filter { get; set; }
		public int    Id { get; set; }

		#region Constructors
		public ServiceCheckPointGetItemChecksArguments(int id, string filter)
		{
			Filter = filter ?? "";
			Id = id;
		}
		#endregion Constructors
	}
}
