﻿using System;

namespace PayIn.DistributedServices.Test.Cards
{
	/// <summary>
	/// Abono Transportes Joven ampliado - No debe dejar recargarlo
	/// </summary>
	public class AbonoTJoveAmpliadoCard : TestCard, ITestCard
	{
		protected static string[] Content { get; set; } = (
			"C4B6BD4F800804006263646566676869,000000000000000000000000000000B1,000000000000000000000000000000B1,000000000000FF07806986CCAAE576A2," +
			//"5210C4B8140B00000030000000004002,41881DE500628D02C48EF210206E201C,41881DE500628D02C48EF210206E201C,0000000000007E178869000000000000," +
			"5210C4B8140B00000030000000004002,41781EE500628D02C48EF210206E208A,41781EE500628D02C48EF210206E208A,0000000000007E178869000000000000," +
			"8BE185BA00000000000000000000008D,43494120202020202020202020205C83,82808080806072060000284B2C0123B0,00000000000078778869000000000000," +
			"01E400C2FF7500000020E801DC0000BD,627816C34B29B93480D801805CBC031F,53554152455A20202020202020202002,00000000000078778869000000000000," +
			"000000000000000000000000000000B1,000000000000000000000000000000B1,000000000000000000000000000000B1,0000000000007F078869000000000000," +
			"000000000000000000000000000000B1,000000000000000000000000000000B1,000000000000000000000000000000B1,0000000000007F078869000000000000," +
			"000000000000000000000000000000B1,000000000000000000000000000000B1,000000000000000000000000000000B1,0000000000004C378B69000000000000," +
			"1B801CC48E7201A31004000008000094,000000000000000000000000000000B1,000000000000000000000000000000B1,0000000000007F078869000000000000," +
			"000000000000000000000000000000B1,000000000000000000000000000000B1,000000000000000000000000000000B1,00000000000078778869000000000000," +
			"000000000000000000000000000000B1,000000000000000000000000000000B1,000000000000000000000000000000B1,000000000000FF078069B0B1B2B3B4B5," +
			"000000000000000000000000000000B1,000000000000000000000000000000B1,000000000000000000000000000000B1,000000000000FF078069B0B1B2B3B4B5," +
			"000000000000000000000000000000B1,000000000000000000000000000000B1,000000000000000000000000000000B1,000000000000FF078069B0B1B2B3B4B5," +
			"000000000000000000000000000000B1,000000000000000000000000000000B1,000000000000000000000000000000B1,000000000000FF078069B0B1B2B3B4B5," +
			"000000000000000000000000000000B1,000000000000000000000000000000B1,000000000000000000000000000000B1,000000000000FF078069B0B1B2B3B4B5," +
			"000000000000000000000000000000B1,000000000000000000000000000000B1,000000000000000000000000000000B1,000000000000FF078069B0B1B2B3B4B5," +
			"000000000000000000000000000000B1,000000000000000000000000000000B1,000000000000000000000000000000B1,000000000000FF078069B0B1B2B3B4B5"
		).SplitString(",");

		#region Constructors
		public AbonoTJoveAmpliadoCard()
			: base("11223344", Content, null)
		{ }
		#endregion Constructors
	}
}