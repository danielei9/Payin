using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xp.Application.Tsm.GlobalPlatform.Config
{
	/// <summary>
	/// Define the fact that the key is derivable.
	/// </summary>
	public interface ISCDerivableKey
	{
		/// <summary>
		/// Compute a set of key based on the mother key and the keydata (usually a random number sends by the other part).
		/// It returns an array of 3 keys: the encryption key, the MAC key and the data encryption key.
		/// </summary>
		/// <param name="keydata">data used to derivate mother key</param>
		/// <returns>an array of 3 keys (enc, mac, kek)</returns>
		SCGPKey[] DeriveKey(byte[] keydata);
	}
}
