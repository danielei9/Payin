using PayIn.Common;
using System.ComponentModel.DataAnnotations;
using Xp.Domain;

namespace PayIn.Domain.Public
{
	public class ServiceAddressName : IEntity
	{
		                                      public int             Id          { get; set; }
		[Required(AllowEmptyStrings = false)] public string          Name        { get; set; }
		                                      public ProviderMapType ProviderMap { get; set; }

		#region ServiceAddress
		public int AddressId { get; set; }
		public ServiceAddress Address { get; set; }
		#endregion
	}
}
