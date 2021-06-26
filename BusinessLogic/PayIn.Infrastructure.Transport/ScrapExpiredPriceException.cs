using System;

namespace PayIn.Infrastructure.Transport
{
	public class ScrapExpiredPriceException : ApplicationException
	{
		public ScrapExpiredPriceException(string title, string zone, string fecha = null)
			: base("Tarifa caducada para el \n" + title + " zona " + zone + ".\n" + 
				fecha == null ?
					"Actualice la tarifa en cualquier Centro de Atención al Cliente para poder consultar el saldo." :
					"Actualice la tarifa en cualquier Centro de Atención al Cliente antes de " + fecha + " para poder consultar el saldo."
			)
		{
		}
	}
}
