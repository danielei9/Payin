using System;

namespace Xp.Domain.Transport.MifareClassic
{
	[AttributeUsage(AttributeTargets.Property)]
	public class MifareClassicMemoryAttribute : Attribute
	{
		public string Name { get; set; }
		public byte Sector { get; set; }
		public byte Block { get; set; }
		public int StartBit { get; set; }
		public int EndBit { get; set; }

		#region Constructors
		public MifareClassicMemoryAttribute(string name, byte sector, byte block, int startBit, int endBit)
		{
			Name = name;
			Sector = sector;
			Block = block;
			StartBit = startBit;
			EndBit = endBit;
		}
		#endregion Constructors
	}
}
