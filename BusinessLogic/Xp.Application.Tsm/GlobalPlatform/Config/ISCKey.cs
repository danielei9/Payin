namespace Xp.Application.Tsm.GlobalPlatform.Config
{
	public interface ISCKey
	{
		// key type
		KeyType Type { get; }

		// key version
		byte Version { get; }

		// key id
		byte Id { get; }

		// value
		byte[] Value { get; }
	}
}
