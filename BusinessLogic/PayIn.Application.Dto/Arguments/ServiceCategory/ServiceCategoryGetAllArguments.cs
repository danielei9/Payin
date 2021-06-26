using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Arguments
{
	public class ServiceCategoryGetAllArguments : IArgumentsBase
	{
		public string Filter { get; set; }

		#region Constructors
		public ServiceCategoryGetAllArguments(string filter)
		{
			Filter = filter ?? "";
		}
		#endregion Constructors 
	}
}
