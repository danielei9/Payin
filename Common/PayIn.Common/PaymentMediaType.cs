using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using Xp.Common;

namespace PayIn.Common
{
	public enum PaymentMediaType
	{
		Visa = 1,
		MasterCard = 2,
		AmericanExpress = 3,
		WebCard = 4,
		Purse = 5 //Monedero
	}
}
