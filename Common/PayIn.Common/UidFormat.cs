namespace PayIn.Common
{
    public enum UidFormat
    {
        Numeric = 0,
        LittleEndian = 1,
        BigEndian = 2,
        Mobilis = 3 // Numerico más dos digitos de CRC
    }
}
