using PayIn.Domain.Transport.Eige;
using Xp.Domain.Transport.MifareClassic;

namespace PayIn.Application.Transport.Scripts.Mobilis
{
	public interface ITransportCardIsRechargableScript : IMifareClassicScript<EigeCard>,
		ITransportCardGetExpiredDateScript,
		ITransportCardGetCardOwnerNameScript
	{
	}
	public static class TransportCardIsRechargableScriptExtension
	{
		public static void AddRequest(this ITransportCardIsRechargableScript that)
		{
			// Otros
			(that as ITransportCardGetExpiredDateScript).AddRequest();
			(that as ITransportCardGetCardOwnerNameScript).AddRequest();
		}
	}
}
