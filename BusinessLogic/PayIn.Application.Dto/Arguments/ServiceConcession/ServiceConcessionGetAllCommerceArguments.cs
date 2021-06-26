using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Arguments.ServiceConcession
{
	public class ServiceConcessionGetAllCommerceArguments : IArgumentsBase
	{
		public string Filter { get; set; }

		#region Constructors
		public ServiceConcessionGetAllCommerceArguments(string filter)
		{
			Filter = filter ?? "";
		}
		#endregion Constructors
	}
}
