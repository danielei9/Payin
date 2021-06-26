using PayIn.Domain.Transport.Eige;
using Xp.Domain.Transport.MifareClassic;

namespace PayIn.Application.Transport.Scripts.Mobilis
{
	public interface ITransportCardGetBalanceUnitsScript : IMifareClassicScript<EigeCard>
	{
	}
	public static class ITransportCardGetBalanceUnitsScriptExtension
	{
		public static void AddRequest(this ITransportCardGetBalanceUnitsScript that)
		{
			that.Read(that.Request, x => x.Titulo.CodigoTitulo1);
			that.Read(that.Request, x => x.Titulo.CodigoTitulo2);
		}
	}
}
