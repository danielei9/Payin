using PayIn.Domain.Transport.Eige.Enums;
using PayIn.Domain.Transport.Eige.Types;
using Xp.Domain.Transport.MifareClassic;

namespace PayIn.Domain.Transport.Eige
{
	public class EigePersonalizacion : MifareClassicElement
	{
		[MifareClassicMemory("PREP", 2, 0,  1,  5)] public EigeString EmpresaPersonalizadora { get { return Get<EigeString>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("PRFP", 2, 0,  6, 19)] public EigeDate FechaPersonalizacion { get { return Get<EigeDate>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("",     2, 0,  6, 19)] public GenericEnum<EigeFechaPersonalizacion_DispositivoEnum> FechaPersonalizacion_Dispositivo { get { return Get<GenericEnum<EigeFechaPersonalizacion_DispositivoEnum>>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("PRTO", 2, 0, 20, 31)] public GenericEnum<EigeTipoOperacionEnum> TipoOperacion { get { return Get<GenericEnum<EigeTipoOperacionEnum>>(); } set { Set(value.Raw); } }

		#region Constructors
		public EigePersonalizacion(MifareClassicCard card)
			: base(card)
		{
		}
		#endregion Constructors
	}
}
