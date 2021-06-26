using PayIn.Common;
using Xp.Domain;
using Xp.Domain.Transport.MifareClassic;

namespace PayIn.Domain.Transport
{
	public class TransportCardApplication : Entity
	{
		public string ApplicationId { get; set; }
		public KeyVersionType KeyVersion { get; set; } 
		public string Content { get; set;}
		public string AccessCondition { get; set; }
		public MifareKeyType? ReadKey { get; set; }	
		public MifareKeyType? WriteKey { get; set; }												

		#region TransportSystem
		public int TransportSystemId { get; set; }
		public TransportSystem TransportSystem { get; set; }
		#endregion TransportSystem
	}
}
