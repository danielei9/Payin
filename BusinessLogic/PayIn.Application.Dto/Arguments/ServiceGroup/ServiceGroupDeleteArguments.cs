using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Arguments
{
	public partial class ServiceGroupDeleteArguments : IArgumentsBase
	{
		public int Id			{ get; set; }

		#region Constructors
		public ServiceGroupDeleteArguments(int id)
		{
			Id = id;
		}
		#endregion Constructors
	}
}
