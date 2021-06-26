using PayIn.Common;
using System.ComponentModel.DataAnnotations;
using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Arguments.ServiceAddressName
{
	public partial class ServiceAddressNameCreateArguments : IArgumentsBase
	{
		[Required] public int             AddressId   { get; set; }
		           public ProviderMapType ProviderMap { get; private set; }
		[Required] public string          Name        { get; private set; }

		#region Constructors
		public ServiceAddressNameCreateArguments(int addressId, ProviderMapType providerMap, string name)
		{
			AddressId = addressId;
			ProviderMap = providerMap;
			Name = name;
		}
		#endregion Constructors
	}
}
