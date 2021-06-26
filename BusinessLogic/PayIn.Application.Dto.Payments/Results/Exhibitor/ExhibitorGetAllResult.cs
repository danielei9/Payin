using PayIn.Common;

namespace PayIn.Application.Dto.Payments.Results
{
	public class ExhibitorGetAllResult
    {
		public int              Id        { get; set; }
		public string           Name      { get; set; }
        public ExhibitorState   State     { get; set; }
    }
}
