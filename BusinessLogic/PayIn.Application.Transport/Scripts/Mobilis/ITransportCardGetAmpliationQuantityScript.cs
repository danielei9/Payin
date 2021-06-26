using PayIn.Domain.Transport.Eige;
using Xp.Domain.Transport.MifareClassic;

namespace PayIn.Application.Transport.Scripts.Mobilis
{
	public interface ITransportCardGetAmpliationQuantityScript : IMifareClassicScript<EigeCard>
	{
	}
	public static class TransportCardGetAmpliationQuantityScriptExtension
	{
		public static void AddRequest(this ITransportCardGetAmpliationQuantityScript that)
		{
			that.Read(that.Request, x => x.Titulo.NumeroUnidadesValidezTemporal1);
			that.Read(that.Request, x => x.Titulo.NumeroUnidadesValidezTemporal2);
		}
	}
}
