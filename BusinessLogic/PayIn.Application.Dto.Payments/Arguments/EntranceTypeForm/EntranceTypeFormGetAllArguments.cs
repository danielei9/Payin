using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Payments.Arguments
{
	public partial class EntranceTypeFormGetAllArguments : IArgumentsBase
	{
		//public string Filter { get; set; }
		public int EntranceTypeId { get; set; }
		public int FormId { get; set; }

		#region Constructors
		public EntranceTypeFormGetAllArguments(/*string filter,*/ int entranceTypeId, int formId)
		{
			//Filter = filter ?? "";
			EntranceTypeId = entranceTypeId;
			FormId = formId;
		}
		#endregion Constructors
	}
}
