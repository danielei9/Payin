using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xp.Application.Tsm.GlobalPlatform.Utilities
{
	/// TLV - Type Length Value.
	/// -0----1-2------3-3--------LENGTH-
	/// | tag | LENGTH |     VALUE     |
	/// ---------------------------------
	/// </summary>
	public class Tlv
	{
		/// The tag of TLV
		public byte Tag
		{
			get
			{
				return (byte)(_Tag & 0xFF);
			}
			set
			{
				_Tag = value;
			}
		}
		public byte _Tag;

		/// The length of the TLV
		public byte Length
		{
			get
			{
				return (byte)(_Length & 0xFF);
			}
			set
			{
				_Length = value;
			}
		}
		private byte _Length;

		/// The value of the TLV
		public byte[] Value
		{
			get
			{
				return _Value.ToArray();
			}
			set
			{
				Length = (byte)value.Length;
				_Value = value;
			}
		}
		private byte[] _Value;

		/// <summary>
		/// Creates a new TLV.
		/// </summary>
		/// <param name="data">data all data of TLV</param>
		public Tlv(byte[] data)
		{
			SetTlv(data);
		}
		/// <summary>
		/// Creates a new TLV.
		/// </summary>
		/// <param name="tag">type TLV tag</param>
		/// <param name="value">value TLV value</param>
		public Tlv(byte tag, byte[] value)
		{
			if (value.Length > 0x00FF)
				throw new ApplicationException("Value is too long (" + value.Length + ")");

			Tag = tag;
			Length = (byte) value.Length;
			Value = value.ToArray();
		}
		/// <summary>
		/// Set TLV with a array byte
		/// </summary>
		/// <param name="data">data TLV data</param>
		public void SetTlv(byte[] data)
		{
			if (
				(data.Length < 3) || // Minimal size of a TLV
				((data [1] & 0xFF) != (data.Length - 2)) // Check Length value
			)
				throw new ApplicationException("Invalid TLV length");

			Tag = data [0];
			Length = data [1];
			Value = data.Skip(2).Take(Length).ToArray();
		}
		
		/// <summary>
		/// A byte array containing the representation of this TLV instance.
		/// </summary>
		/// <returns>A byte array containing the representation of this TLV instance</returns>
		public byte[] toBinary()
		{
			byte[] ret = new byte[Length + 2];

			ret[0] = (byte)(Tag & 0xFF);
			ret[1] = (byte)(Length & 0xFF);
			ret.Copy(Value, 0, Length, 2);

			return ret.CloneArray();
		}

		/// <summary>
		/// Return a string representation of this TLV.
		/// </summary>
		/// <returns>the string representation of this TLV</returns>
		public String toString()
		{
			var sb = new StringBuilder();

			sb.Append("TLV [ 0x");
			sb.Append(Tag.ToHexadecimal());
			sb.Append(", ");
			sb.Append(Length);
			sb.Append(", ");

			if (Value == null)
				sb.Append("null");
			else
				sb.Append(Value.ToHexadecimal());

			sb.Append("]");

			return sb.ToString();
		}
	}
}
