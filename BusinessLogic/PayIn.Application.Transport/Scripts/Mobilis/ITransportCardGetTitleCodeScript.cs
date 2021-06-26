using PayIn.Domain.Transport.Eige;
using Xp.Domain.Transport.MifareClassic;

namespace PayIn.Application.Transport.Scripts.Mobilis
{
	public interface ITransportCardGetTitleCodeScript : IMifareClassicScript<EigeCard>
	{
	}
	public static class TransportCardGetTitleCodeScriptExtension
	{
		public static void AddRequest(this ITransportCardGetTitleCodeScript that)
		{
			that.Read(that.Request, x => x.Titulo.TitulosActivos);

			that.Read(that.Request, x => x.Titulo.CodigoTitulo1);
			that.Read(that.Request, x => x.Titulo.CodigoTitulo2);

			that.Read(that.Request, x => x.Tesc.Identificador1);
			that.Read(that.Request, x => x.Tesc.Identificador2);
		}
	}
}
