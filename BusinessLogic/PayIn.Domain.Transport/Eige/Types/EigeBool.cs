using Xp.Domain.Transport.MifareClassic;

namespace PayIn.Domain.Transport.Eige.Types
{
	public class EigeBool : GenericType<bool>
	{
		#region Value
		public override bool Value
		{
			get
			{
				return (Raw != null) && (Raw.Length > 0) && (Raw[0] != 0x00);
			}
		}
		#endregion Value

		#region Constructors
		public EigeBool(byte[] value, int length)
			: base(value, length)
		{ }
		public EigeBool(bool value)
			: base(value ? 
				new byte[] { 0x01 } :
				new byte[] { 0x00 },
				1
			)
		{ }
		#endregion Constructors
	}
}
