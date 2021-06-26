namespace Xp.Domain.Transport.MifareClassic
{
	public abstract class GenericType<T>
	{
		#region Raw
		public byte[] Raw { get; set; }
		#endregion Raw

		#region Length
		public int Length { get; set; }
		#endregion Length

		#region Value
		public virtual T Value
		{
			get
			{
				return default(T);
			}
		}
		#endregion Value

		#region ToString
		public override string ToString()
		{
			return Value.ToString();
		}
		#endregion ToString

		#region Constructors
		public GenericType(byte[] raw, int length)
		{
			Raw = raw;
			Length = length;
		}
		#endregion Constructors
	}
}
