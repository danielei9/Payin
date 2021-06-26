using PayIn.Domain.Transport.Eige;
using Xp.Domain.Transport.MifareClassic;

namespace PayIn.Application.Transport.Scripts.Mobilis
{
	public interface ITransportCardGetHasHourValidityScript : IMifareClassicScript<EigeCard>
	{
	}
	public static class TransportCardGetHasHourValidityScriptExtension
	{
		public static void AddRequest(this ITransportCardGetHasHourValidityScript that)
		{
			that.Read(that.Request, x => x.Titulo.ValidezHoraria1);
			that.Read(that.Request, x => x.Titulo.ValidezHoraria2);
		}
	}
}
