using PayIn.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PayIn.Application.Payments.Services
{
	public class PromotionService
	{
		//string temp = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijkmnopqrstuvwxyz23456789_-&@#";
		//var result = "";
		//var random = new Random(74);
		//	while (temp.Length > 0)
		//	{
		//		var val = random.Next(temp.Length);
		//var item = temp[val];
		//temp = temp.Substring(0, val) + temp.Substring(val + 1);

		//		if (!result.Contains(item))
		//			result = result + item;
		//	}
		private string AlphanumericPattern = "YifVpNDnIm4Ph@2Q&7t-qAc6_Ggd#ra8CyX9RvB5OMZkz3WeSEFHUoJbKujswTxL";
		private int AlphanumericBits = 6;
		private long MultiplicadorPromotion = 1000000;

		#region ClearCode
		public string ClearCode(string code)
		{
			return
				code.Substring(0, 1)
				.Replace('O', '0')
				.Replace('l', '1')
				.Replace('I', '1') + 
				code
				.Substring(1)
				.Replace('0', 'O')
				.Replace('l', 'I')
				.Replace('1', 'I')
				.ToString();
		}
		#endregion ClearCode

		#region CheckCode
		public bool CheckCode(string code, int promotionId, int codeNum)
		{
			if (code.Substring(0, 1) == "0")
			{
				// Caracteres confusos:
				// 0 => O
				// l => I
				// 1 => I
				IEnumerable<char> temp = code
					.Substring(1)
					.Replace('0', 'O')
					.Replace('l', 'I')
					.Replace('1', 'I');

				long oldValue = 0;
				long result = 0;
				int iteration = 0;
				while (temp.Count() > 0)
				{
					var char_ = temp.First();
					temp = temp.Skip(1);

					var value = AlphanumericPattern.IndexOf(char_) ^ oldValue;
					oldValue = AlphanumericPattern.IndexOf(char_);

					result = (value << (AlphanumericBits * iteration)) + result;

					iteration++;
				}

				var resto = result % MultiplicadorPromotion;
				var div = result / MultiplicadorPromotion;

				return
					codeNum == resto &&
					promotionId == div;
			}

			throw new NotImplementedException();
		}
		#endregion CheckCode

		#region CreateCode
		public string CreateCode(PromotionExecutionType type, int promotionId, int codeNum)
		{
			if (type == PromotionExecutionType.AlphaNumeric)
			{
				// Caracteres confusos:
				// 0 => O
				// l => I
				// 1 => I

				long temp = promotionId * MultiplicadorPromotion + codeNum;
				int value = 0;
				var result = new StringBuilder();
				while (temp > 0)
				{
					var nextValue = temp >> AlphanumericBits;
					value = (temp - (nextValue << AlphanumericBits)).ConvertTo<int>() ^ value;
					// Se hace xor con el valor anterior para que en todos los caracteres haya cambios

					result.Append(AlphanumericPattern[value]);

					temp = nextValue;
				}

				return "0" + result.ToString();
			}

			throw new NotImplementedException();
		}
		#endregion CreateCode
	}
}