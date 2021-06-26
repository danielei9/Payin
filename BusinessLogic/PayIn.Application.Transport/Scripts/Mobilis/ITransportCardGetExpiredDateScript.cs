using PayIn.Domain.Transport.Eige;
using Xp.Domain.Transport.MifareClassic;

namespace PayIn.Application.Transport.Scripts.Mobilis
{
	public interface ITransportCardGetExpiredDateScript : IMifareClassicScript<EigeCard>
	{
	}
	public static class TransportCardGetExpiredDateScriptExtension
	{
		public static void AddRequest(this ITransportCardGetExpiredDateScript that)
		{
			that.Read(that.Request, x => x.Tarjeta.Caducidad);
		}
	}
}
