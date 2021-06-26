using System;
using System.Web.Mvc;

namespace PayIn.Web.Helpers
{
	public class GenericDisposableWrapper : IDisposable
	{
		private readonly HtmlHelper _helper;
		private readonly Func<string> _end;

		#region Constructors
		public GenericDisposableWrapper(HtmlHelper helper, Func<string> begin, Func<string> end)
		{
			_helper = helper;
			_end = end;

			if (begin != null)
				_helper.ViewContext.Writer.Write(begin());
		}
		#endregion Constructors

		#region Dispose
		public void Dispose()
		{
			if (_end() != null)
				_helper.ViewContext.Writer.Write(_end());
		}
		#endregion Dispose
	}
}