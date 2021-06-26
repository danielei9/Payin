using PayIn.Domain.Transport.Eige;
using Xp.Domain.Transport.MifareClassic;

namespace PayIn.Application.Transport.Scripts.Mobilis
{
	public interface ITransportCardGetBalanceScript : IMifareClassicScript<EigeCard>
	{
	}
	public static class ITransportCardGetBalanceScriptExtension
	{
		public static void AddRequest(this ITransportCardGetBalanceScript that)
		{
			that.Read(that.Request, x => x.Titulo.CodigoTitulo1);
			that.Read(that.Request, x => x.Titulo.SaldoViaje1);
			that.Read(that.Request, x => x.Titulo.CodigoTitulo2);
			that.Read(that.Request, x => x.Titulo.SaldoViaje2);
			that.Read(that.Request, x => x.TituloTuiN.SaldoViaje);
			that.Read(that.Request, x => x.TituloTuiN.CobradoPrimerViajero);
			that.Read(that.Request, x => x.TituloTuiN.BonificadoPrimerViajero);
		}
	}
}
