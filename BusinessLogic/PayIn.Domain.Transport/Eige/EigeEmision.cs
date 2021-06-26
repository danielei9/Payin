using PayIn.Domain.Transport.Eige.Enums;
using PayIn.Domain.Transport.Eige.Types;
using Xp.Domain.Transport.MifareClassic;

namespace PayIn.Domain.Transport.Eige
{
	public class EigeEmision : MifareClassicElement
	{
		[MifareClassicMemory("EEE", 3, 1, 18, 22)] public GenericEnum<EigeEmpresaEmisoraEnum> EmpresaEmisora { get { return Get<GenericEnum<EigeEmpresaEmisoraEnum>>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("EFE", 3, 1, 23, 36)] public EigeDate FechaEmision { get { return Get<EigeDate>(); } set { Set(value.Raw); } }

		#region Constructors
		public EigeEmision(MifareClassicCard card)
			: base(card)
		{
		}
		#endregion Constructors
	}
}
