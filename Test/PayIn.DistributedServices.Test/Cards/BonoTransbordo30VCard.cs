﻿using System;

namespace PayIn.DistributedServices.Test.Cards
{
	public class BonoTransbordo30VCard : TestCard, ITestCard
	{
		


		protected static string[] Content { get; set; } = (

			"8EF989837D8804004659949765103308, 000000000000000000000000000000B1, 000000000000000000000000000000B1, FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF, " +
			"3110D06D000000000020000000004096, 110000001E42930454B7F410006E040F, 110000001E42930454B7F410006E040F, FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF, " +
			"8BE185BA973BCD17A8901418800CF051, 000000000000000000000000000000B1, 000000000000000000000000000000B1, FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF, " +
			"01C200A00740000000600CD06B0500E7, 627816C37BB9D37C812A4941182C01A1, 000000000000000000000000000000B1, FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF, " +
			"000000000000000000000000000000B1, 000000000000000000000000000000B1, 1AAC0F54B7748124B154F3000800001E, FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF, " +
			"000000000000000000000000000000B1, 000000000000000000000000000000B1, 1AAC0FCCCE428124B154F300080000BE, FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF, " +
			"000000000000000000000000000000B1, 000000000000000000000000000000B1, 000000000000000000000000000000B1, FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF, " +
			"1AAC0F2C1A298124B154F3000800006C, 1AAC0FCCAE4C8124B154F30008000064, 1AAC0FCCCE228124B154F3000800008D, FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF, " +
			"000000000000000000000000000000B1, 000000000000000000000000000000B1, 000000000000000000000000000000B1, FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF, " +
			"000000000000000000000000000000B1, 000000000000000000000000000000B1, 000000000000000000000000000000B1, FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF, " +
			"00000000000000000000000000000000, 00000000000000000000000000000000, 00000000000000000000000000000000, FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF, " +
			"4AAC0FCC1E6D8124B154730220000060, 1AAC0FCC2E2D8124B154F300080000D5, 2AAC0F54112F8124B154730110000034, FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF, " +
			"000000000000000000000000000000B1, 000000000000000000000000000000B1, 000000000000000000000000000000B1, FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF, " +
			"000000000000000000000000000000B1, 000000000000000000000000000000B1, 000000000000000000000000000000B1, FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF, " +
			"000000000000000000000000000000B1, 000000000000000000000000000000B1, 000000000000000000000000000000B1, FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF, " +
			"000000000000000000000000000000B1, 000000000000000000000000000000B1, 000000000000000000000000000000B1, FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF "
			).SplitString(",");

		#region Constructors
		public BonoTransbordo30VCard()
			: base("11223344", Content, null)
		{ }
		#endregion Constructors
	}
}
