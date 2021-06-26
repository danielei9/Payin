using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xp.Infrastructure
{
	public interface IMailService
	{
		void Send(string mail, string subject, string body);
	}
}
