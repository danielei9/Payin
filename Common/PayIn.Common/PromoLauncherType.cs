using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayIn.Common
{
	public enum PromoLauncherType
	{
		Normal = -1, //Se aplica al realizar una recarga
		Recharge = 0, //Realizar automáticamente si es la primera recarga
		Birthday = 1,
		Instant = 2 //Realizar automáticamente sin hacer recarga
	}
}
