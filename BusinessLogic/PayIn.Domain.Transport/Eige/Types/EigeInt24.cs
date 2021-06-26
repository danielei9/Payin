using System;
using Xp.Domain.Transport.MifareClassic;

namespace PayIn.Domain.Transport.Eige.Types
{
    public class EigeInt24 : GenericType<long>
    {
        #region Value
        public override long Value
        {
            get
            {
                return Raw.ToInt24() ?? base.Value;
            }
        }
        #endregion Value

        #region Constructors
        public EigeInt24(byte[] value, int length)
            : base(value, length)
        { }
        public EigeInt24(Int32 value)
            : base(new byte[] {
                (byte)value,
                (byte)(value >> 8),
                (byte)(value >> 16)
            }, 24)
        { }
        #endregion Constructors
    }
}