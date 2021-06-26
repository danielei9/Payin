using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Transport.Arguments.TransportCard
{
	public partial class TransportCardGetByDeviceAddressArguments : IArgumentsBase
	{
		public string DeviceAddress { get; set; }	
		
		#region Constructors
		public TransportCardGetByDeviceAddressArguments(string deviceAddress)
		{
			DeviceAddress = deviceAddress;
		}
		#endregion Constructors
	}
}