using PayIn.Application.Transport.Scripts.Mobilis;
using PayIn.Domain.Transport.Eige;
using PayIn.Domain.Transport.MifareClassic.Operations;
using System.Collections.Generic;

namespace PayIn.Application.Transport.Scripts
{
	public class TransportCardCheckExhaustedScript : MifareClassicScript<EigeCard>,
		ITransportCardCheckExhaustedScript
	{
		#region Constructors
		public TransportCardCheckExhaustedScript(string userName)
			:base(new EigeCard(userName))
		{
			Read(Request, x => x.Titulo.SaldoViaje1);
			Read(Request, x => x.Titulo.FechaValidez1);
			Read(Request, x => x.Titulo.NumeroUnidadesValidezTemporal1);
			Read(Request, x => x.Titulo.TipoUnidadesValidezTemporal1);
			Read(Request, x => x.Titulo.SaldoViaje2);
			Read(Request, x => x.Titulo.FechaValidez2);
			Read(Request, x => x.Titulo.NumeroUnidadesValidezTemporal2);
			Read(Request, x => x.Titulo.TipoUnidadesValidezTemporal2);

			// Response in handler
		}
		public TransportCardCheckExhaustedScript(string userName, IEnumerable<MifareOperationResultArguments> values)
			: this(userName)
		{
			this.Load(values);
		}
		#endregion Constructors
	}
}
