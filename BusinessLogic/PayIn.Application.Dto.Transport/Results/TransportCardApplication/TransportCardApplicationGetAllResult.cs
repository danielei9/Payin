using PayIn.Common;
using Xp.Domain.Transport.MifareClassic;

namespace PayIn.Application.Dto.Transport.Results.TransportCardApplication
{
	public class TransportCardApplicationGetAllResult
	{
		public int Id { get; set; }
		public string ApplicationId { get; set; }
		public KeyVersionType KeyVersion { get; set; }
		public string Content { get; set; }
		public string AccessCondition { get; set; }
		public MifareKeyType? ReadKey { get; set; }
		public MifareKeyType? WriteKey { get; set; }
	}
}
