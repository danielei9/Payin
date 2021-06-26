using System.Collections.Generic;
using System.Threading.Tasks;

namespace Xp.Domain.Transport.MifareClassic
{
	public interface IMifareClassicHsmService
	{
		// KeyName == KeyUri
		Task<byte[]> EncryptAsync(byte[] text, string keyName);
		Task<string> DecryptAsync(string text, string keyName);
		Task<string> GetKeyEncryptedAsync(int sector, MifareKeyType keyType);
		Task<string> GetKeysEncryptedAsync(CardSystem cardSystem, IEnumerable<MifareKeyId> keys, long uid, int aux);
		Task<string> SignAsync(string text, string keyName);
		Task<bool> ValidateAsync(string text, string sign, string keyName);
	}
}
