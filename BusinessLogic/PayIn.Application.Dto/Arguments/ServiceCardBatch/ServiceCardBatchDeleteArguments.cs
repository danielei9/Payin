using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Arguments
{
	public partial class ServiceCardBatchDeleteArguments : IArgumentsBase
	{
		public int Id			{ get; set; }

		#region Constructors
		public ServiceCardBatchDeleteArguments(int id)
		{
			Id = id;
		}
		#endregion Constructors
	}
}
