using PayIn.Domain.Transport.Eige;
using Xp.Domain.Transport.MifareClassic;

namespace PayIn.Application.Transport.Scripts.Mobilis
{
	public interface ITransportCardGetValidationDataScript : IMifareClassicScript<EigeCard>
	{
	}
	public static class TransportCardGetValidationDataScriptExtension
	{
		public static void AddRequest(this ITransportCardGetExhaustedDateScript that)
		{
			// Titulo 1
			that.Read(that.Request, x => x.Titulo.SaldoViaje1);
			that.Read(that.Request, x => x.Titulo.FechaValidez1);
			that.Read(that.Request, x => x.Titulo.TipoUnidadesValidezTemporal1);
			that.Read(that.Request, x => x.Titulo.NumeroUnidadesValidezTemporal1);
			// Titulo 2
			that.Read(that.Request, x => x.Titulo.SaldoViaje2);
			that.Read(that.Request, x => x.Titulo.FechaValidez2);
			that.Read(that.Request, x => x.Titulo.TipoUnidadesValidezTemporal2);
			that.Read(that.Request, x => x.Titulo.NumeroUnidadesValidezTemporal2);
		}
	}
}
