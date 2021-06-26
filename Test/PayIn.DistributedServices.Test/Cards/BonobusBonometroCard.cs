﻿using System;

namespace PayIn.DistributedServices.Test.Cards
{
	public class BonobusBonometroCard : TestCard, ITestCard
	{
		protected static string[] Content { get; set; } = (
			"0CBCDD026F0804006263646566676869,000000000000000000000000000000B1,000000000000000000000000000000B1,000000000000FF07806986CCAAE576A2," +
			"31104C0E0000000000A02001000040AB,1300000008428D02BCBE8C10006EA03B,1300000008428D02BCBE8C10006EA03B,0000000000007E178869000000000000," +
			"0000008094924B03781C0018800C00FE,20202020202020202020202020200048,808080808000020000000000000010D0,00000000000078778869000000000000," +
			"030C0080040000000020FCCFF30400B2,7258BCE34029B93480C703AC0FB4000D,2020202020202020202020202020206B,00000000000078778869000000000000," +
			"627D00800020FCDFDF04000020010095,627D00800020FCDFDF04000020010095,1340D80BF40881BC200000000000006B,0000000000007F078869000000000000," +
			"00000000FFFFFFFF0000000000FF00FF,00000000FFFFFFFF0000000000FF00FF,188001BCCE948100106086003800001D,0000000000007F078869000000000000," +
			"00000000FFFFFFFF0000000000FF00FF,00000000FFFFFFFF0000000000FF00FF,000000000000000000000000000000B1,0000000000004C378B69000000000000," +
			"1840D88BE9C0818310588700080000C6,5A40183C9E7001A3100480022800006A,1880013C2EC1810010608600300000D3,0000000000007F078869000000000000," +
			"000000000000000000000000000000B1,000000000000000000000000000000B1,000000000000000000000000000000B1,00000000000078778869000000000000," +
			"000000000000000000000000000000B1,000000000000000000000000000000B1,000000000000000000000000000000B1,000000000000FF078069B0B1B2B3B4B5," +
			"000000000000000000000000000000B1,000000000000000000000000000000B1,000000000000000000000000000000B1,000000000000FF078069B0B1B2B3B4B5," +
			"1AAC0FBCBE0C01A31004800008000094,000000000000000000000000000000B1,000000000000000000000000000000B1,000000000000FF078069B0B1B2B3B4B5," +
			"000000000000000000000000000000B1,000000000000000000000000000000B1,000000000000000000000000000000B1,000000000000FF078069B0B1B2B3B4B5," +
			"000000000000000000000000000000B1,000000000000000000000000000000B1,000000000000000000000000000000B1,000000000000FF078069B0B1B2B3B4B5," +
			"000000000000000000000000000000B1,000000000000000000000000000000B1,000000000000000000000000000000B1,000000000000FF078069B0B1B2B3B4B5," +
			"000000000000000000000000000000B1,000000000000000000000000000000B1,000000000000000000000000000000B1,000000000000FF078069B0B1B2B3B4B5"
			).SplitString(",");

		#region Constructors
		public BonobusBonometroCard()
			: base("11223344", Content, null)
		{ }
		#endregion Constructors
	}
}