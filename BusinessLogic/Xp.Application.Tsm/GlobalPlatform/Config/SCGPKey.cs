using System;

namespace Xp.Application.Tsm.GlobalPlatform.Config
{
	public class SCGPKey: ISCKey
	{
		// key type
		public KeyType Type { get; set; }

		// key version
		public byte Version { get; set; }

		// key id
		public byte Id { get; set; }
		
		// key value
		public byte[] Value { get; set; }

		public SCGPKey(byte version, byte id, KeyType type, byte[] value)
		{
			if (value == null)
				throw new ArgumentException("data must be not null");

			// TODO: test data key size

			Version = version;
			Id = id;
			Type = type;
			Value = value;
		}
		
		public override string ToString()
		{
			return 
				"SCGPKey(setVersion: " + Version +
				", id:" + Id +
				", type:" + Type +
				", data:" + Value.ToHexadecimal() +
				")";
		}
	}
}
