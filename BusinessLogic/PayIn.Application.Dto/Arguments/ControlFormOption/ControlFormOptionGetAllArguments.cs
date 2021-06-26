using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Arguments
{
	public partial class ControlFormOptionGetAllArguments : IArgumentsBase
	{
		//public string Filter { get; set; }
		public int ArgumentId { get; set; }
	
		#region Constructors
		public ControlFormOptionGetAllArguments(/*string filter,*/ int argumentId)
		{
			//Filter = filter ?? "";
			ArgumentId = argumentId;
		}
		#endregion Constructors
	}
}
