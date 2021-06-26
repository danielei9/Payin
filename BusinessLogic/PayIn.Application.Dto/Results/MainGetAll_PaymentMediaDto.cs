
namespace PayIn.Application.Dto.Results
{
	public partial class MainGetAll_PaymentMediaDto
	{
		#region Id
		public virtual int Id { get; set; }
		#endregion Id

		#region Name
		public virtual string Name { get; set; }
		#endregion Name

		#region PaymentMediaType
		public virtual string PaymentMediaType { get; set; }
		#endregion PaymentMediaType

		#region IsFavorite
		public virtual bool IsFavorite { get; set; }
		#endregion IsFavorite

		#region Constructors
		public MainGetAll_PaymentMediaDto(string name, string paymentMediaType, bool isFavorite = false)
		{
			Name = name;
			PaymentMediaType = paymentMediaType;
			IsFavorite = isFavorite;
		}
		#endregion Constructors
	}
}
