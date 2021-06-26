using Xp.Application.Tsm.GlobalPlatform.Commands;

namespace PayIn.Application.Tsm.Services.OpalExtensions
{
	public class M4MFileControlInformation: FileControlInformation
	{
		/*private TLV proprietaryData;
		*/
		public M4MFileControlInformation(byte[] data)
			:base(data)
		{
		}
		/*
		@Override
		public void setProprietaryData(byte[] proprietaryData) throws IOException
		{
			this.proprietaryData = new TLV((byte) 0xA5, proprietaryData);

			ByteBuffer data = ByteBuffer.wrap(proprietaryData).slice().asReadOnlyBuffer();

			while (data.hasRemaining()) {

				int tag = data.get() & 0xFF;
			byte length;
			byte[] value;

			switch (tag) {
				case 0x0073:
					length = data.get();
					value = new byte[length];
					data.get(value, 0, length);
					this.setSecurityDomainManagementData(value);
					break;
				case 0x009F:
					tag = data.get() & 0xFF;
					if (tag == 0x006E) {
						length = data.get();
						value = new byte[length];
						data.get(value, 0, length);
						this.setApplicationProductionLifeCycleData(value);
					} else if (tag == 0x0065) {
						length = data.get();
						value = new byte[length];
						data.get(value, 0, length);
						this.setMaximumLengthOfDataFieldInCommandMessage(value);
					} else if (tag == 0x0066) {
						// TODO: Do Something with the M4M Version Info Tag
						length = data.get();
						value = new byte[length];
						data.get(value, 0, length);
					} else {
						throw new IOException("Invalid tag after 0x9F (0x" + Integer.toHexString(tag) + ")");
					}
					break;
				default:
					throw new IOException("Invalid tag (0x" + Integer.toHexString(tag) + ")");
				}
			}
		}
		public TLV getProprietaryData()
		{
			return proprietaryData;
		}*/
	}
}
