using PayIn.Common;
using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Arguments.ServiceAddress
{
	public partial class ServiceAddressUpdateArguments : IArgumentsBase
	{
		public int	               Id     { get; set; }
		public int	               CityId { get; private set; }
		public string              Name   { get; private set; }
		public int?	               From   { get; private set; }
		public int?	               Until  { get; private set; }
		public ServiceAddressSide? Side   { get; private set; }

		#region Constructors
		public ServiceAddressUpdateArguments(int id, int cityId, string name, int? from, int? until, ServiceAddressSide? side)
		{
			Id = id;
			CityId = cityId;
			Name = name;
			From = from;
			Until = until;
			Side = side;
		}
		#endregion Constructors
	}
}
