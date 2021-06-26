using Newtonsoft.Json;

namespace Xp.Domain.Transport.MifareClassic
{
	public class MifareClassicSector : BaseVM
	{
		#region Card
		[JsonIgnore]
		public MifareClassicCard Card { get; set; }
		#endregion Card

		#region Number
		public int Number { get; set; }
		#endregion Number

		#region Blocks
		public MifareClassicBlock[] Blocks { get; set; } 
		#endregion Blocks

		#region Constructors
		public MifareClassicSector(MifareClassicCard card, int number)
		{
			Blocks = new MifareClassicBlock[4];
			Card = card;
			Number = number;

			for (var i = 0; i < 4; i++)
				Blocks[i] = new MifareClassicBlock(this, i);
		}
		#endregion Constructors
	}
}
