using Newtonsoft.Json;

namespace Xp.Domain.Transport.MifareClassic
{
	public class MifareClassicBlock : BaseVM
	{
		#region Sector
		[JsonIgnore]
		public MifareClassicSector Sector { get; set; }
		#endregion Sector

		#region Number
		public int Number { get; set; }
		#endregion Number

		#region Value
		private byte[] _Value = null;
		public byte[] Value
		{
			get
			{
				return _Value;
			}
			set
			{
				Set(ref _Value, value);
			}
		}
		#endregion Value

		#region OldValue
		private byte[] _OldValue = null;
		public byte[] OldValue
		{
			get
			{
				return _OldValue;
			}
			set
			{
				Set(ref _OldValue, value);
			}
		}
		#endregion OldValue

		#region Constructors
		public MifareClassicBlock(MifareClassicSector sector, int number)
		{
			Sector = sector;
			Number = number;
			Value = new byte[0];
		}
		#endregion Constructors

		#region Set
		public void Set(byte[] value)
		{
			if (OldValue == null)
				OldValue = Value;

			Value = value;
		}
		#endregion Set
	}
}
