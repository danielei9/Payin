using PayIn.Common;
using System.Threading.Tasks;

namespace Xp.Domain.Transport.MifareClassic.Operaons
{
	public class MifareAutenticateOperation : IMifareOperation
	{
		public MifareOperationType Operation { get; set; }
		public MifareKeyType KeyType { get; set; }
		public byte Sector { get; set; }
		public DiversifyType? DiversifyType { get; set; }
		public KeyVersionType KeyVersionType { get; set; }

		#region Constructors
		public static async Task<MifareAutenticateOperation> CreateAsync(byte sector, MifareKeyType keyType)
		{
			return await Task.Run(() =>
			{
				return new MifareAutenticateOperation
				{
					Operation = MifareOperationType.Autenticate,
					KeyType = keyType,
					Sector = sector
					//DiversifyType = diversifyType,
					//KeyVersionType = keyVersionType
				};
			});
		}
		#endregion Constructors
	}
}
