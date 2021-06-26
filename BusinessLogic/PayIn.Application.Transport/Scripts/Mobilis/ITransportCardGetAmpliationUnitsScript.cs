using PayIn.Domain.Transport.Eige;
using Xp.Domain.Transport.MifareClassic;

namespace PayIn.Application.Transport.Scripts.Mobilis
{
	public interface ITransportCardGetAmpliationUnitsScript : IMifareClassicScript<EigeCard>
	{
	}
	public static class TransportCardGetAmpliationUnitsScriptExtension
	{
		public static void AddRequest(this ITransportCardGetAmpliationUnitsScript that)
		{
			that.Read(that.Request, x => x.Titulo.TipoUnidadesValidezTemporal1);
			that.Read(that.Request, x => x.Titulo.TipoUnidadesValidezTemporal2);
		}
	}
}
