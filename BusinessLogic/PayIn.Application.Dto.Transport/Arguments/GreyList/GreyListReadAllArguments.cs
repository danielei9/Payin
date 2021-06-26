using PayIn.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Transport.Arguments.GreyList
{
	public class GreyListReadAllArguments : IArgumentsBase
	{
		                                                        public long Id { get; private set; }
		[Display(Name = "resources.transportLog.readAll")] 		public bool ReadAllCard { get; private set; }

		#region Constructors
		public GreyListReadAllArguments(long id, bool readAllCard)
		{
			Id = id;
			ReadAllCard = readAllCard;
		}
		#endregion Constructors
	}
}
