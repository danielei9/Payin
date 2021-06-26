using PayIn.Domain.Transport.Eige.Enums;
using PayIn.Domain.Transport.Eige.Types;
using Xp.Domain.Transport.MifareClassic;

namespace PayIn.Domain.Transport.Eige
{
	public class EigeCarga : MifareClassicElement
	{
		[MifareClassicMemory("CRP",   2, 0,  32,  32)] public GenericEnum<EigePosicionUltimaCargaEnum> PosicionUltima { get { return Get<GenericEnum<EigePosicionUltimaCargaEnum>>(); } set { Set(value.Raw); } }
		// Carga 1
		[MifareClassicMemory("C1E",   2, 0,  33,  37)] public EigeInt8 Empresa1 { get { return Get<EigeInt8>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("C1TEC", 2, 0,  38,  39)] public GenericEnum<EigeTipoEquipoCargaEnum> TipoEquipo1 { get { return Get<GenericEnum<EigeTipoEquipoCargaEnum>>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("C1LE",  2, 0,  40,  63)] public EigeInt24 Expendedor1 { get { return Get<EigeInt24>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("C1FC",  2, 0,  64,  77)] public EigeDate Fecha1 { get { return Get<EigeDate>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("C1TO",  2, 0,  78,  82)] public EigeBytes TipoOperacion1 { get { return Get<EigeBytes>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("",      2, 0,  78,  79)] public GenericEnum<EigeTipoOperacionCarga_OpcionEnum> TipoOperacion1_Opcion { get { return Get<GenericEnum<EigeTipoOperacionCarga_OpcionEnum>>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("",      2, 0,  80,  82)] public GenericEnum<EigeTipoOperacionCarga_OperacionEnum> TipoOperacion1_Operacion { get { return Get<GenericEnum<EigeTipoOperacionCarga_OperacionEnum>>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("C1FP",  2, 0,  83,  86)] public GenericEnum<EigeFormaPagoEnum> FormaPago1 { get { return Get<GenericEnum<EigeFormaPagoEnum>>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("C1CT",  2, 0,  87,  98)] public EigeInt16 CodigoTitulo1 { get { return Get<EigeInt16>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("C1I",   2, 0,  99, 116)] public EigeCurrency Importe1 { get { return Get<EigeCurrency>(); } set { Set(value.Raw); } }
		// Carga 
		[MifareClassicMemory("C2E",   3, 1,  37,  41)] public EigeInt8 Empresa2 { get { return Get<EigeInt8>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("C2TEC", 3, 1,  42,  43)] public GenericEnum<EigeTipoEquipoCargaEnum> TipoEquipo2 { get { return Get<GenericEnum<EigeTipoEquipoCargaEnum>>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("C2LE",  3, 1,  44,  67)] public EigeInt24 Expendedor2 { get { return Get<EigeInt24>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("C2FC",  3, 1,  68,  81)] public EigeDate Fecha2 { get { return Get<EigeDate>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("C2TO",  3, 1,  82,  86)] public EigeBytes TipoOperacion2 { get { return Get<EigeBytes> (); } set { Set(value.Raw); } }
		[MifareClassicMemory("",      3, 1,  82,  83)] public GenericEnum<EigeTipoOperacionCarga_OpcionEnum> TipoOperacion2_Opcion { get { return Get<GenericEnum<EigeTipoOperacionCarga_OpcionEnum>>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("",      3, 1,  84,  86)] public GenericEnum<EigeTipoOperacionCarga_OperacionEnum> TipoOperacion2_Operacion { get { return Get<GenericEnum<EigeTipoOperacionCarga_OperacionEnum>>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("C2FP",  3, 1,  87,  90)] public GenericEnum<EigeFormaPagoEnum> FormaPago2 { get { return Get<GenericEnum<EigeFormaPagoEnum>>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("C2CT",  3, 1,  91, 102)] public EigeInt16 CodigoTitulo2 { get { return Get<EigeInt16>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("C2I",   3, 1, 103, 120)] public EigeCurrency Importe2 { get { return Get<EigeCurrency>(); } set { Set(value.Raw); } }

		#region Constructors
		public EigeCarga(MifareClassicCard card)
			: base(card)
		{
		}
		#endregion Constructors
	}
}
