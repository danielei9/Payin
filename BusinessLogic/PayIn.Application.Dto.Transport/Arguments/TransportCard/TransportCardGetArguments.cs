using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Transport.Arguments.TransportCard
{
	public partial class TransportCardGetArguments : IArgumentsBase
	{		
		public int? DeviceEntry { get; set; }	
		public string DeviceAddress { get; set; }	
		
		#region Constructors
		public TransportCardGetArguments(int? deviceEntry, string deviceAddress)
		{
			DeviceEntry = deviceEntry;			
			DeviceAddress = deviceAddress;
		}
		#endregion Constructors
	}
}