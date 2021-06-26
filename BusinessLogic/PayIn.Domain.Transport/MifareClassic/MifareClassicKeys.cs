namespace PayIn.Domain.Transport.MifareClassic
{
	public class MifareClassicKeys
	{
		#region A
		public string A { get; set; }
		#endregion A

		#region B
		public string B { get; set; }
		#endregion B

		#region Constructors
		public MifareClassicKeys(string a, string b)
		{
			A = a;
			B = b;
		}
		#endregion Constructors
	}
}
