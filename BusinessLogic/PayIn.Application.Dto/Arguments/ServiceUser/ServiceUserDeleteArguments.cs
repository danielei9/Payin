using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Arguments
{
	public partial class ServiceUserDeleteArguments : IArgumentsBase
	{
		public int Id			{ get; set; }

		#region Constructors
		public ServiceUserDeleteArguments(int id)
		{
			Id = id;
		}
		#endregion Constructors
	}
}
