using System.Threading.Tasks;

namespace Xp.Application.Tsm.Mifare4Mobile.Commands
{
	public abstract class M4mCommand
	{
		public const byte UpdateMifareClassicSector = 0x06;
		public const byte ReadMifareClassicSector = 0x07;
		public const byte UpdateSmMetadataSector = 0x20;
		public const byte AddAndUpdateMdacCommand = 0x21;

		public int Command { get; set; }

		#region Constructors
		public M4mCommand(byte command)
		{
			Command = command;
		}
		#endregion Constructors

		#region ToHexadecimalAsync
		public abstract Task<string> ToHexadecimalAsync();
		#endregion ToHexadecimalAsync
	}
}
