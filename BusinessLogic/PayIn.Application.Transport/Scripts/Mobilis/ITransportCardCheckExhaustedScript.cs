using PayIn.Domain.Transport.Eige;
using Xp.Domain.Transport.MifareClassic;

namespace PayIn.Application.Transport.Scripts.Mobilis
{
	public interface ITransportCardCheckExhaustedScript : IMifareClassicScript<EigeCard>,
		ITransportCardGetIsTemporalScript,
		ITransportCardGetHasBalanceScript
	{
	}
	public static class TransportCardCheckExhaustedScriptExtension
	{
		public static void AddRequest(this ITransportCardCheckExhaustedScript that)
		{
			that.Read(that.Request, x => x.TituloTuiN.SaldoViaje_Sign);
			that.Read(that.Request, x => x.TituloTuiN.SaldoViaje_Value);
			// Titulo 1
			that.Read(that.Request, x => x.Titulo.SaldoViaje1);
			that.Read(that.Request, x => x.Titulo.FechaValidez1);
			that.Read(that.Request, x => x.Titulo.NumeroUnidadesValidezTemporal1);
			that.Read(that.Request, x => x.Titulo.TipoUnidadesValidezTemporal1);
			that.Read(that.Request, x => x.Titulo.ControlTarifa1);
			that.Read(that.Request, x => x.Titulo.ValidezZonal1);
			that.Read(that.Request, x => x.Titulo.CodigoTitulo1);
			// Titulo 2
			that.Read(that.Request, x => x.Titulo.SaldoViaje2);
			that.Read(that.Request, x => x.Titulo.FechaValidez2);
			that.Read(that.Request, x => x.Titulo.NumeroUnidadesValidezTemporal2);
			that.Read(that.Request, x => x.Titulo.TipoUnidadesValidezTemporal2);
			that.Read(that.Request, x => x.Titulo.ControlTarifa2);
			that.Read(that.Request, x => x.Titulo.ValidezZonal2);
			that.Read(that.Request, x => x.Titulo.CodigoTitulo2);
			// Monedero
			that.Read(that.Request, x => x.Titulo.SaldoMonedero);
			// Bonus
			that.Read(that.Request, x => x.Titulo.SaldoBonus);
			// Otros
			(that as ITransportCardGetIsTemporalScript).AddRequest();
			(that as ITransportCardGetHasBalanceScript).AddRequest();
		}
	}
}
