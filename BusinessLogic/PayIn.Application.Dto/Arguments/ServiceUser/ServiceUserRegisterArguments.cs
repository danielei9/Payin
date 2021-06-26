using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Arguments
{
	public partial class ServiceUserRegisterArguments : IArgumentsBase
	{
		public int Id	{ get; set; }

		#region Constructors
		public ServiceUserRegisterArguments(int id)
		{
			Id = id;
		}
		#endregion Constructors
	}
}

