﻿ using System;

namespace PayIn.DistributedServices.Test.Cards
{
	public class Bonometro20VCard : TestCard, ITestCard
	{
		protected static string[] Content { get; set; } = (
			"76DFDCD3A6880400468E44194D202204,000000000000000000000000000000B1,000000000000000000000000000000B1,000000000000FF07806986CCAAE576A2,"+
			"31104C0E000000000020A00000000032,0300000008030300BC9EF4180078004E,0300000008030300BC9EF4180078004E,0000000000007E178869000000000000,"+
			"0000000017000000B8A2D4FA400B000D,20202020202020202020202020200048,808080808000020000000000000010D0,00000000000078778869000000000000,"+
			"030C0080040000000020FCCFF30400B2,7258BCE370914C8980294AAD0FB400D5,2020202020202020202020202020206B,00000000000078778869000000000000,"+
			"627D00800020F00FDC04000080020038,627D00800020F00FDC04000080020038,188001BC9EF4810010608600380000D2,0000000000007F078869000000000000,"+
			"00000000FFFFFFFF0000000000FF00FF,00000000FFFFFFFF0000000000FF00FF,000000000000000000000000000000B1,0000000000007F078869000000000000,"+
			"00000000FFFFFFFF0000000000FF00FF,00000000FFFFFFFF0000000000FF00FF,000000000000000000000000000000B1,0000000000004C378B69000000000000,"+
			"1840D88BE9C0818310588700080000C6,5A40183C9E7001A3100480022800006A,1880013C2EC1810010608600300000D3,0000000000007F078869000000000000,"+
			"000000000000000000000000000000B1,000000000000000000000000000000B1,000000000000000000000000000000B1,00000000000078778869000000000000,"+
			"000000000000000000000000000000B1,000000000000000000000000000000B1,000000000000000000000000000000B1,000000000000FF078069B0B1B2B3B4B5,"+
			"000000000000000000000000000000B1,000000000000000000000000000000B1,000000000000000000000000000000B1,000000000000FF078069B0B1B2B3B4B5,"+
			"000000000000000000000000000000B1,000000000000000000000000000000B1,000000000000000000000000000000B1,000000000000FF078069B0B1B2B3B4B5,"+
			"000000000000000000000000000000B1,000000000000000000000000000000B1,000000000000000000000000000000B1,000000000000FF078069B0B1B2B3B4B5,"+
			"000000000000000000000000000000B1,000000000000000000000000000000B1,000000000000000000000000000000B1,000000000000FF078069B0B1B2B3B4B5,"+
			"000000000000000000000000000000B1,000000000000000000000000000000B1,000000000000000000000000000000B1,000000000000FF078069B0B1B2B3B4B5,"+
			"000000000000000000000000000000B1,000000000000000000000000000000B1,000000000000000000000000000000B1,000000000000FF078069B0B1B2B3B4B5"
			).SplitString(",");

		#region Constructors
		public Bonometro20VCard()
			: base("11223344", Content, null)
		{ }
		#endregion Constructors
	}
}