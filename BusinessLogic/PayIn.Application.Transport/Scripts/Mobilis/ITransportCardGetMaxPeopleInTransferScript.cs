using PayIn.Domain.Transport.Eige;
using Xp.Domain.Transport.MifareClassic;

namespace PayIn.Application.Transport.Scripts.Mobilis
{
	public interface ITransportCardGetMaxPeopleInTransferScript : IMifareClassicScript<EigeCard>
	{
	}
	public static class TransportCardGetMaxPeopleInTransferScriptExtension
	{
		public static void AddRequest(this ITransportCardGetMaxPeopleInTransferScript that)
		{
			that.Read(that.Request, x => x.Titulo.MaxPersonasEnTransbordo1);
			that.Read(that.Request, x => x.Titulo.MaxPersonasEnTransbordo2);
		}
	}
}
