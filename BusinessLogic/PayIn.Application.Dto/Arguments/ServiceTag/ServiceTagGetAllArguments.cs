using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Arguments.ServiceTag
{
	public partial class ServiceTagGetAllArguments : IArgumentsBase
	{
		public string   Filter       { get; set; }

		#region Constructors
		public ServiceTagGetAllArguments(string filter)
		{
			Filter = filter ?? "";
		}
		#endregion Constructors
	}
}
