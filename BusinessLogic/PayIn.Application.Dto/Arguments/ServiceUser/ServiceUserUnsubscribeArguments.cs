using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Arguments
{
	public partial class ServiceUserUnsubscribeArguments : IArgumentsBase
	{
		public int Id	{ get; set; }

		#region Constructors
		public ServiceUserUnsubscribeArguments (int id)
		{
			Id = id;
		}
		#endregion Constructors
	}
}

