using System;
using Xp.Domain.Transport.MifareClassic;

namespace PayIn.Domain.Transport.Eige.Types
{
    public class EigeInt64 : GenericType<long>
    {
        #region Value
        public override long Value
        {
            get
            {
                return Raw.ToInt64() ?? base.Value;
            }
        }
        #endregion Value

        #region Constructors
        public EigeInt64(byte[] value, int length)
            : base(value, length)
        { }
        public EigeInt64(long value)
            : base(new byte[] {
                (byte)value,
                (byte)(value >> 8),
                (byte)(value >> 16),
                (byte)(value >> 24),
				(byte)(value >> 32),
				(byte)(value >> 40),
				(byte)(value >> 48),
				(byte)(value >> 56),
			}, 64)
        { }
        #endregion Constructors
    }
}