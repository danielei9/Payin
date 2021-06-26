using PayIn.Domain.Transport.Eige;
using Xp.Domain.Transport.MifareClassic;

namespace PayIn.Application.Transport.Scripts.Mobilis
{
	public interface ITransportCardGetCardTypeNameScript : IMifareClassicScript<EigeCard>
	{
	}
	public static class ITransportCardGetCardTypeNameScriptExtension
	{
		public static void AddRequest(this ITransportCardGetCardTypeNameScript that)
		{
			that.Read(that.Request, x => x.Tarjeta.Tipo);
			that.Read(that.Request, x => x.Tarjeta.Subtipo);
		}
	}
}
