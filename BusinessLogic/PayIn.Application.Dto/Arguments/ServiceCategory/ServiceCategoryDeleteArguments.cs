using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Arguments
{
	public partial class ServiceCategoryDeleteArguments : IArgumentsBase
	{
		public int Id			{ get; set; }

		#region Constructors
		public ServiceCategoryDeleteArguments(int id)
		{
			Id = id;
		}
		#endregion Constructors
	}
}
