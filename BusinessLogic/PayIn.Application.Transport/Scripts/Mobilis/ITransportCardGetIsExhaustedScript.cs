using PayIn.Domain.Transport.Eige;
using Xp.Domain.Transport.MifareClassic;

namespace PayIn.Application.Transport.Scripts.Mobilis
{
	public interface ITransportCardGetIsExhaustedScript : IMifareClassicScript<EigeCard>
	{
	}
	public static class TransportCardGetIsExhaustedScriptExtension
	{
		public static void AddRequest(this ITransportCardGetIsExhaustedScript that)
		{
			that.Read(that.Request, x => x.Titulo.NumeroUnidadesValidezTemporal1);
			that.Read(that.Request, x => x.Titulo.NumeroUnidadesValidezTemporal2);
		}
	}
}
