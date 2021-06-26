using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xp.Common.Resources;

namespace Xp.Common.Exceptions
{
	public class XpEntityAlreadyExistsException : XpException
	{
		public XpEntityAlreadyExistsException(string entity)
			:base(GenericExceptionResources.EntityAlreadyExistsException.FormatString(entity))
		{
		}
	}
}
