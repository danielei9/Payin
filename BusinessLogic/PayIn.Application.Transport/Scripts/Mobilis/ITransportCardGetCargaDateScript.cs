using PayIn.Domain.Transport.Eige;
using Xp.Domain.Transport.MifareClassic;

namespace PayIn.Application.Transport.Scripts.Mobilis
{
	public interface ITransportCardGetCargaDateScript : IMifareClassicScript<EigeCard>
	{
	}
	public static class TransportCardGetCargaDateScriptExtension
	{
		public static void AddRequest(this ITransportCardGetCargaDateScript that)
		{
			that.Read(that.Request, x => x.Carga.Fecha1);
			that.Read(that.Request, x => x.Carga.Fecha2);
		}
	}
}
