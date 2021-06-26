using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Payments.Arguments
{
	public class EventDeleteImageGalleryArguments : IArgumentsBase
	{
		public int Id { set; get; }

        #region Constructor
        public EventDeleteImageGalleryArguments(int id)
		{
			Id = id;
        }
		#endregion Constructor
	}
}
