using PayIn.Domain.Transport.Eige;
using Xp.Domain.Transport.MifareClassic;

namespace PayIn.Application.Transport.Scripts.Mobilis
{
	public interface ITransportCardGetTitleNameScript : IMifareClassicScript<EigeCard>
	{
	}
	public static class TransportCardGetTitleNameScriptExtension
	{
		public static void AddRequest(this ITransportCardGetTitleNameScript that)
		{
			that.Read(that.Request, x => x.Titulo.CodigoTitulo1);
			that.Read(that.Request, x => x.Titulo.CodigoTitulo2);
			that.Read(that.Request, x => x.Historico.CodigoTitulo1);
			that.Read(that.Request, x => x.Historico.CodigoTitulo2);
			that.Read(that.Request, x => x.Historico.CodigoTitulo3);
			that.Read(that.Request, x => x.Historico.CodigoTitulo4);
			that.Read(that.Request, x => x.Historico.CodigoTitulo5);
			that.Read(that.Request, x => x.Historico.CodigoTitulo6);
			that.Read(that.Request, x => x.Historico.CodigoTitulo7);
			that.Read(that.Request, x => x.Historico.CodigoTitulo8);
			that.Read(that.Request, x => x.Carga.CodigoTitulo1);
			that.Read(that.Request, x => x.Carga.CodigoTitulo2);
		}
	}
}
