using PayIn.Common;
using Xp.Common.Dto.Arguments;
using Xp.Domain.Transport.MifareClassic;

namespace PayIn.Application.Dto.Transport.Arguments.TransportCardApplication
{
	public partial class TransportCardApplicationGetAllArguments : IArgumentsBase
	{		
		public string ApplicationId { get; set; }	
		public KeyVersionType KeyVersion { get; set; }
		public MifareKeyType? ReadKey { get; set; }
		public MifareKeyType? WriteKey { get; set; }
		
		#region Constructors
		public TransportCardApplicationGetAllArguments(string applicationId, KeyVersionType keyVersion, MifareKeyType readKey, MifareKeyType writeKey)
		{
			ApplicationId = applicationId;
			KeyVersion = keyVersion;
			ReadKey = readKey;
			WriteKey = WriteKey;
		}
		#endregion Constructors
	}
}