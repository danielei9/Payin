using PayIn.Common.Resources;
using System.ComponentModel.DataAnnotations;
using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Payments.Arguments.Log
{
	public class LogUndoArguments : IArgumentsBase
	{
		public int Id { get; set; }


		#region Constructors
		public LogUndoArguments(int id)
		{
			Id = id;			
		}
		#endregion Constructors
	}
}