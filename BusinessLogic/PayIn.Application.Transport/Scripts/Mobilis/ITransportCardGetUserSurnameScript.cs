using PayIn.Domain.Transport.Eige;
using Xp.Domain.Transport.MifareClassic;

namespace PayIn.Application.Transport.Scripts.Mobilis
{
	public interface ITransportCardGetUserSurnameScript : IMifareClassicScript<EigeCard>
	{
	}
	public static class ITransportCardGetUserSurnameScriptExtension
	{
		public static void AddRequest(this ITransportCardGetUserSurnameScript that)
		{
			that.Read(that.Request, x => x.Usuario.Apellidos);
		}
	}
}
