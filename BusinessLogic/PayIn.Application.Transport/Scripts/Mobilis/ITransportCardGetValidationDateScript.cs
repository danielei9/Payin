using PayIn.Domain.Transport.Eige;
using Xp.Domain.Transport.MifareClassic;

namespace PayIn.Application.Transport.Scripts.Mobilis
{
	public interface ITransportCardGetValidationDateScript : IMifareClassicScript<EigeCard>
	{
	}
	public static class TransportCardGetValidationDateScriptExtension
	{
		public static void AddRequest(this ITransportCardGetValidationDateScript that)
		{
			that.Read(that.Request, x => x.Historico.FechaHora1);
			that.Read(that.Request, x => x.Historico.FechaHora2);
			that.Read(that.Request, x => x.Historico.FechaHora3);
			that.Read(that.Request, x => x.Historico.FechaHora4);
			that.Read(that.Request, x => x.Historico.FechaHora5);
			that.Read(that.Request, x => x.Historico.FechaHora6);
			that.Read(that.Request, x => x.Historico.FechaHora7);
			that.Read(that.Request, x => x.Historico.FechaHora8);
			that.Read(that.Request, x => x.Validacion.FechaValidacion);
			that.Read(that.Request, x => x.TituloTuiN.UltimaValidacionFechaHora);
		}
	}
}
