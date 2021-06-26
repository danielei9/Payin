using PayIn.Domain.Transport.Eige;
using Xp.Domain.Transport.MifareClassic;

namespace PayIn.Application.Transport.Scripts.Mobilis
{
	public interface ITransportCardGetHasDayValidityScript : IMifareClassicScript<EigeCard>
	{
	}
	public static class TransportCardGetHasDayValidityScriptExtension
	{
		public static void AddRequest(this ITransportCardGetHasDayValidityScript that)
		{
			that.Read(that.Request, x => x.Titulo.ValidezDiaria1);
			that.Read(that.Request, x => x.Titulo.ValidezDiaria2);
		}
	}
}
