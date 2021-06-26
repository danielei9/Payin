using PayIn.Domain.Transport.Eige;
using Xp.Domain.Transport.MifareClassic;

namespace PayIn.Application.Transport.Scripts.Mobilis
{
	public interface ITransportCardGetAmpliationScript : IMifareClassicScript<EigeCard>
	{
	}
	public static class TransportCardGetAmpliationScriptExtension
	{
		public static void AddRequest(this ITransportCardGetAmpliationScript that)
		{
			//that.Read(that.Request, x => x.Titulo.NumeroUnidadesValidezTemporal1);
			//that.Read(that.Request, x => x.Titulo.NumeroUnidadesValidezTemporal2);
			//that.Read(that.Request, x => x.Titulo.TipoUnidadesValidezTemporal1);
			//that.Read(that.Request, x => x.Titulo.TipoUnidadesValidezTemporal2);
			that.Read(that.Request, x => x.Titulo.TituloEnAmpliacion1);
			that.Read(that.Request, x => x.Titulo.TituloEnAmpliacion2);
			that.Read(that.Request, x => x.Titulo.FechaValidez1);
			that.Read(that.Request, x => x.Titulo.FechaValidez2);
		}
	}
}
