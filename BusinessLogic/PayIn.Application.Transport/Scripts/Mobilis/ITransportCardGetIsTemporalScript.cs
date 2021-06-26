using PayIn.Domain.Transport.Eige;
using Xp.Domain.Transport.MifareClassic;

namespace PayIn.Application.Transport.Scripts.Mobilis
{
	public interface ITransportCardGetIsTemporalScript : IMifareClassicScript<EigeCard>
	{
	}
	public static class TransportCardGetIsTemporalScriptExtension
	{
		public static void AddRequest(this ITransportCardGetIsTemporalScript that)
		{
			that.Read(that.Request, x => x.Titulo.NumeroUnidadesValidezTemporal1);
			that.Read(that.Request, x => x.Titulo.NumeroUnidadesValidezTemporal2);
		}
	}
}
