using PayIn.Domain.Transport.Eige.Enums;
using PayIn.Domain.Transport.Eige.Types;
using Xp.Domain.Transport.MifareClassic;

namespace PayIn.Domain.Transport.Eige
{
	public class EigeTarjeta : MifareClassicElement
	{
		[MifareClassicMemory("TNS",  0, 0,   1,  32)] public EigeInt32 Serial { get { return Get<EigeInt32>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("TNT",  0, 1,   1,  40)] public EigeBytes Numero { get { return Get<EigeBytes>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("TCET", 0, 1,  41,  48)] public GenericEnum<EigeCodigoEntornoTarjetaEnum> CodigoEntorno { get { return Get<GenericEnum<EigeCodigoEntornoTarjetaEnum>>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("TT",   1, 0,   1,   4)] public GenericEnum<EigeTipoTarjetaEnum> Tipo { get { return Get<GenericEnum<EigeTipoTarjetaEnum>>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("TST",  1, 0,   5,   8)] public GenericEnum<EigeSubtipoTarjetaEnum> Subtipo { get { return Get<GenericEnum<EigeSubtipoTarjetaEnum>>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("TV",   1, 0,   9,  12)] public EigeInt8 Version { get { return Get<EigeInt8>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("TEP",  1, 0,  13,  17)] public EigeInt8 EmpresaPropietaria { get { return Get<EigeInt8>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("TBGF", 1, 0,  18,  18)] public EigeBool BitGrupo { get { return Get<EigeBool>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("TFC",  1, 0,  19,  32)] public EigeDate Caducidad { get { return Get<EigeDate>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("ATBS", 1, 0, 120, 120)] public EigeBool BitSeguimiento { get { return Get<EigeBool>(); } set { Set(value.Raw); } }

		#region Constructors
		public EigeTarjeta(MifareClassicCard card)
			: base(card)
		{
		}
		#endregion Constructors
	}
}
