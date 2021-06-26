using PayIn.Common;
using System.ComponentModel.DataAnnotations;
using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Arguments.ServiceAddressName
{
	public partial class ServiceAddressNameUpdateArguments : IArgumentsBase
	{
		[Required] public int             Id          { get; set; }
		[Required] public string          Name        { get; set; }
		           public ProviderMapType ProviderMap { get; set; }

		#region Constructors
		public ServiceAddressNameUpdateArguments(int id, string name, ProviderMapType providerMap)
		{
			Id = id;
			Name = name;
			ProviderMap = providerMap;
		}
		#endregion Constructors
	}
}
