using PayIn.Common;
using Xp.Common;

namespace PayIn.Application.Dto.Payments.Results
{
	public class MobileExhibitorGetAllResult
	{
        public int Id { get; set; }
        public string Name { get; set; }
        public ExhibitorState State { get; set; }
    }
}
