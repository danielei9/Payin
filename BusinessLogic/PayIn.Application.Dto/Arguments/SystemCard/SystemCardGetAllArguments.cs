using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Arguments.SystemCard
{
	public partial class SystemCardGetAllArguments : IArgumentsBase
	{
		public string Filter { get; set; }

		#region Constructors
		public SystemCardGetAllArguments(string filter)
		{
			Filter = filter ?? "";
		}
		#endregion Constructors 
	}
}
