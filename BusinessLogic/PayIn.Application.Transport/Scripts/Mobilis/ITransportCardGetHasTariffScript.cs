using PayIn.Domain.Transport.Eige;
using Xp.Domain.Transport.MifareClassic;

namespace PayIn.Application.Transport.Scripts.Mobilis
{
	public interface ITransportCardGetHasTariffScript : IMifareClassicScript<EigeCard>
	{
	}
	public static class TransportCardGetHasTariffScriptExtension
	{
		public static void AddRequest(this ITransportCardGetHasTariffScript that)
		{
			that.Read(that.Request, x => x.Titulo.TipoTarifa1);
			that.Read(that.Request, x => x.Titulo.TipoTarifa2);
		}
	}
}
