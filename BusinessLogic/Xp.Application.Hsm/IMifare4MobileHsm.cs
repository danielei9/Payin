using System.Threading.Tasks;
using Xp.Domain.Transport.MifareClassic;

namespace Xp.Application.Hsm
{
	public interface IMifare4MobileHsm
	{
		Task<byte[]> GetPasswordAsync();
		Task<byte[]> GetPasswordAsync(byte sectorNumber);
		Task<byte[]> GetPasswordAsync(string keyA, string keyB);
		Task<byte[]> GetPasswordAsync(byte[] keyA, byte[] keyB);

		Task<byte[]> GetKeyAsync(MifareKeyType keyType);
		Task<byte[]> GetKeyAsync(byte sectorNumber, MifareKeyType keyType);
	}
}
