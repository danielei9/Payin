using System;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Xp.Domain.Transport.MifareClassic
{
	public class MifareClassicElement : BaseVM
	{
		protected MifareClassicCard Card;

		#region Constructors
		public MifareClassicElement(MifareClassicCard card)
		{
			Card = card;
		}
		#endregion Constructors

		#region Get
		protected T Get<T>([CallerMemberName] string name = null)
		{
			var property = this.GetType().GetProperty(name);
			var attribute = property.GetCustomAttributes(typeof(MifareClassicMemoryAttribute), true)
				.Cast<MifareClassicMemoryAttribute>()
				.FirstOrDefault();
			if (attribute == null)
				return default(T);

			var block = Card.Sectors[attribute.Sector].Blocks[attribute.Block];
			if (block == null || block.Value == null)
				throw new ApplicationException("El bloque " + attribute.Sector + "-" + attribute.Block + " no se ha cargado de la tarjeta");
			var result = block.Value
				.GetLittleEndian(attribute.StartBit - 1, attribute.EndBit - 1);

			return (T)Convert(typeof(T), result, attribute.EndBit - attribute.StartBit + 1);
		}
		#endregion Get

		#region Set
		protected void Set(byte[] value, [CallerMemberName] string name = null)
		{
			var property = this.GetType().GetProperty(name);
			var attribute = property.GetCustomAttributes(typeof(MifareClassicMemoryAttribute), true)
				.Cast<MifareClassicMemoryAttribute>()
				.FirstOrDefault();
			if (attribute == null)
				return;

			var block = Card.Sectors[attribute.Sector].Blocks[attribute.Block];
			if (block.Value.Count() != 16)
				throw new ApplicationException("Ha ocurrido un error en la escritura del bloque: " + attribute.Sector + "-" + attribute.Block + ", vuelva a leer la tarjeta");

			if (Card.Sectors[attribute.Sector].Blocks[attribute.Block].OldValue == null)
				Card.Sectors[attribute.Sector].Blocks[attribute.Block].OldValue = (byte[]) Card.Sectors[attribute.Sector].Blocks[attribute.Block].Value.Clone();

			Card.Sectors[attribute.Sector].Blocks[attribute.Block].Value
				.SetLittleEndian(attribute.StartBit - 1, attribute.EndBit - 1, value);

			Card.SetCrc(attribute.Sector, attribute.Block);
			Card.BlockBackup(attribute.Sector, attribute.Block);
		}
		#endregion Set

		#region Convert
		private object Convert(Type type, byte[] temp, int length)
		{
			var i = type;
			while (i != null)
			{
				if (i.IsGenericType && (i.GetGenericTypeDefinition() == typeof(GenericType<>)))
					return Activator.CreateInstance(type, new object[] { temp, length });
				i = i.BaseType;
			}

			if (type == typeof(byte[]))
				return temp;

			if (type == typeof(bool))
				return temp[0] != 0x00;

			if (type == typeof(DateTime))
			{
				var dia =
					temp[1] & 0x1F;
				var mes =
					(temp[1] >> 5) +
					((temp[0] & 0x01) << 3);
				var anyo = 2000 +
					temp[0] >> 1;

				return new DateTime(anyo, mes, dia);
			}

			if (type.IsEnum)
			{
				foreach (var value in Enum.GetValues(type))
					if ((int)value == temp[0])
						return value;
				return null;
			}

			if (type.GetGenericTypeDefinition() == typeof(Nullable<>))
				return Convert(type.GetGenericArguments()[0], temp, length);

			return null;
		}
		#endregion Convert
	}
}
