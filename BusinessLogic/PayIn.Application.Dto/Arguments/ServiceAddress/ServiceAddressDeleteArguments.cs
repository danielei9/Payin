using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Arguments.ServiceAddress
{
	public partial class ServiceAddressDeleteArguments : IArgumentsBase
	{
		public int Id { get; set; }

		#region Constructors
		public ServiceAddressDeleteArguments(int id)
		{
			Id = id;
		}
		#endregion Constructors
	}
}
