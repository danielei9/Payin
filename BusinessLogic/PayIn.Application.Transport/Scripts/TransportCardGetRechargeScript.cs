using PayIn.Application.Transport.Scripts.Mobilis;
using PayIn.Domain.Transport.Eige;
using PayIn.Domain.Transport.MifareClassic.Operations;
using System.Collections.Generic;
using Xp.Domain.Transport.MifareClassic;

namespace PayIn.Application.Transport.Scripts
{
	public class TransportCardGetRechargeScript : MifareClassicScript<EigeCard>,
		ITransportCardCheckBlackListScript
	{
		#region Constructors
		public TransportCardGetRechargeScript(string userName, IMifareClassicHsmService hsm)
			:base(new EigeCard(userName, hsm))
		{
			Read(Request, x => x.Validacion.ListaNegra);
			Read(Request, x => x.Titulo.TitulosActivos);
			Read(Request, x => x.Titulo.ControlTarifa1);
			Read(Request, x => x.Titulo.ControlTarifa2);
			Read(Request, x => x.Titulo.SaldoViaje1);
			Read(Request, x => x.Titulo.SaldoViaje2);
			// Para poder comprobar lista negra
			(this as ITransportCardCheckBlackListScript).AddRequest();

			// Response in handler
		}
		public TransportCardGetRechargeScript(string userName, IMifareClassicHsmService hsm, IEnumerable<MifareOperationResultArguments> values)
			: this(userName, hsm)
		{
			this.Load(values);
		}
		#endregion Constructors
	}
}
