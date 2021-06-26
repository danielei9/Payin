using Xp.Common.Dto.Arguments;


namespace PayIn.Application.Dto.Arguments.ControlTemplateCheck
{
    public class ControlTemplateCheckDeleteArguments  : IArgumentsBase
	{
		public int Id { get; private set; }

		#region Constructors
		public ControlTemplateCheckDeleteArguments(int id)
		{
			Id = id;
		}
		#endregion Constructors
	}
}
