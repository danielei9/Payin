using Microsoft.Azure.KeyVault;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using Xp.Common;
using Xp.Domain.Transport.MifareClassic;

namespace Xp.Application.Hsm.Payin
{
	public class Mifare4MobileHsm : IMifare4MobileHsm
	{
		private class EigeMifareKey
		{
			public EigeMifareKeyId Key { get; set; }
			public string Value { get; set; }
		}
		private class EigeMifareKeyId
		{
			public byte Sector { get; set; }
			public string Type { get; set; }
		}

		private static KeyVaultClient keyVaultClient { get; set; }

		#region Constructors
		public Mifare4MobileHsm()
		{
			keyVaultClient = new KeyVaultClient(new KeyVaultClient.AuthenticationCallback(GetADTokenAsync));
		}
		#endregion Constructors

		#region GetADTokenAsync
		private async Task<string> GetADTokenAsync(string authority, string resource, string scope)
		{
			var authContext = new AuthenticationContext(authority);
			var clientCred = new ClientCredential(
				ConfigurationManager.AppSettings["HSMClientId"],
				ConfigurationManager.AppSettings["HSMClientSecret"]
			);
			var result = await authContext.AcquireTokenAsync(resource, clientCred);

			if (result == null)
				throw new InvalidOperationException("Failed to obtain the JWT token");

			return result.AccessToken;
		}
		#endregion GetADTokenAsync

		#region GetKeysAsync
		private static IEnumerable<EigeMifareKey> KeysCache;
		private async Task<IEnumerable<EigeMifareKey>> GetKeysAsync()
		{
			if (KeysCache == null)
			{
				var secretUri = ConfigurationManager.AppSettings["Secret_Eige"];
				if (secretUri == null)
					throw new Exception("secretUri error");

				var secret = await keyVaultClient.GetSecretAsync(secretUri);
				KeysCache = secret.Value.FromJson<IEnumerable<EigeMifareKey>>();
			}
			return KeysCache;
		}
		#endregion GetKeysAsync

		#region GetPasswordAsync
		public async Task<byte[]> GetPasswordAsync()
		{
			var keyA = await GetKeyAsync(MifareKeyType.A);
			var keyB = await GetKeyAsync(MifareKeyType.B);

			return await GetPasswordAsync(keyA, keyB);
		}
		public async Task<byte[]> GetPasswordAsync(byte sectorNumber)
		{
			return await GetPasswordAsync(
				await GetKeyAsync(sectorNumber, MifareKeyType.A),
				await GetKeyAsync(sectorNumber, MifareKeyType.B)
			);
		}
		public async Task<byte[]> GetPasswordAsync(string keyA, string keyB)
		{
			return await GetPasswordAsync(
				keyA.FromHexadecimal(),
				keyB.FromHexadecimal()
			);
		}
		public async Task<byte[]> GetPasswordAsync(byte[] keyA, byte[] keyB)
		{
			return await Task.Run(() =>
			{
				// Derive key B
				var dKeyB = new byte[8];
				dKeyB[7] = (byte)((keyB[5] << 1) & 0xFF);
				dKeyB[6] = (byte)((keyB[4] << 1) & 0xFF);
				dKeyB[5] = (byte)((keyB[3] << 1) & 0xFF);
				dKeyB[4] = (byte)((keyB[2] << 1) & 0xFF);
				dKeyB[3] = (byte)((keyB[1] << 1) & 0xFF);
				dKeyB[2] = (byte)((keyB[0] << 1) & 0xFF);
				dKeyB[1] = (byte)(
					((keyB[5] & 0x80) >> 1) |
					((keyB[4] & 0x80) >> 2) |
					((keyB[3] & 0x80) >> 3) |
					((keyB[2] & 0x80) >> 4) |
					((keyB[1] & 0x80) >> 5) |
					((keyB[0] & 0x80) >> 6)
				);
				dKeyB[0] = 0x00; // 00 7E FE FE FE FE FE FE

				// Derive key A
				var dKeyA = new byte[8];
				dKeyA[0] = (byte)((keyA[0] << 1) & 0xFF);
				dKeyA[1] = (byte)((keyA[1] << 1) & 0xFF);
				dKeyA[2] = (byte)((keyA[2] << 1) & 0xFF);
				dKeyA[3] = (byte)((keyA[3] << 1) & 0xFF);
				dKeyA[4] = (byte)((keyA[4] << 1) & 0xFF);
				dKeyA[5] = (byte)((keyA[5] << 1) & 0xFF);
				dKeyA[6] = (byte)(
					((keyA[0] & 0x80) >> 1) |
					((keyA[1] & 0x80) >> 2) |
					((keyA[2] & 0x80) >> 3) |
					((keyA[3] & 0x80) >> 4) |
					((keyA[4] & 0x80) >> 5) |
					((keyA[5] & 0x80) >> 6)
				);
				dKeyA[7] = 0x00; // FE FE FE FE FE FE 7E 00

				// Join keys
				var dKey = new byte[16];
				dKey.Copy(dKeyB);
				dKey.Copy(dKeyA, targetOffset: 8);

				var password = XpSecurity.Cypher_DESede_ECB_NoPadding(dKey.Reverse(), new byte[8]).Reverse(); // 0B 54 57 07 45 FE 3A E7
				return password;
			});
		}
		#endregion GetPasswordAsync

		#region GetKeyAsync
		public async Task<byte[]> GetKeyAsync(MifareKeyType keyType)
		{
			return await Task.Run(() => {
				if (keyType == MifareKeyType.A)
					return "AAAAAAAAAAAA".FromHexadecimal();
				else
					return "313131313131".FromHexadecimal();
			});
		}
		public async Task<byte[]> GetKeyAsync(byte sectorNumber, MifareKeyType keyType)
		{
			var key = (await GetKeysAsync())
				.Where(x =>
					x.Key.Sector == sectorNumber &&
					x.Key.Type == keyType.ToString()
				)
				.FirstOrDefault();

			return key.Value.FromHexadecimal();
		}
		#endregion GetKeyAsync
	}
}
