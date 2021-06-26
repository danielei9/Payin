using PayIn.Domain.Transport.Eige.Enums;
using PayIn.Domain.Transport.Eige.Types;
using Xp.Domain.Transport.MifareClassic;

namespace PayIn.Domain.Transport.Eige
{
	public class EigeProduccion : MifareClassicElement
	{
		[MifareClassicMemory("PEF", 3, 1, 1,  3)] public GenericEnum<EigeEmpresaFabricanteEnum> EmpresaFabricante { get { return Get<GenericEnum<EigeEmpresaFabricanteEnum>>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("PFF", 3, 1, 4, 17)] public EigeDate FechaFabricacion { get { return Get<EigeDate>(); } set { Set(value.Raw); } }

		#region Constructors
		public EigeProduccion(MifareClassicCard card)
			: base(card)
		{
		}
		#endregion Constructors
	}
}
