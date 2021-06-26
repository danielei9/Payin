using System;
using Xp.Domain.Transport.MifareClassic;

namespace PayIn.Domain.Transport.Eige.Types
{
    public class EigeInt32 : GenericType<long>
    {
        #region Value
        public override long Value
        {
            get
            {
                return Raw.ToInt32() ?? base.Value;
            }
        }
        #endregion Value

        #region Constructors
        public EigeInt32(byte[] value, int length)
            : base(value, length)
        { }
        public EigeInt32(int value)
            : base(new byte[] {
                (byte)value,
                (byte)(value >> 8),
                (byte)(value >> 16),
                (byte)(value >> 24)
            }, 32)
        { }
        #endregion Constructors
    }
}