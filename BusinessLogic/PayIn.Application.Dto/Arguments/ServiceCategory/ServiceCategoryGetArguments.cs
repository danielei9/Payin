using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Arguments
{
	public class ServiceCategoryGetArguments : IArgumentsBase
	{
		public int Id { get; set; }

		#region Constructors
		public ServiceCategoryGetArguments(int id)
		{
			Id = id;
		}
		#endregion Constructors
	}
}
