using System;

namespace PayIn.Application.Dto.Arguments
{
	public class TokenArguments<TArgs>
		where TArgs : class
	{
		public string Version { get; set; }
		public int Type { get; set; }
		public string Content { get; set; }

		#region Constructors
		public TokenArguments()
		{
			Version = "1.0";
			Type = 1;
		}
		#endregion Constructors

		#region GetContent
		public TArgs GetContent()
		{
			var content = Content.FromJson<TArgs>();
			return content;
		}
		#endregion GetContent

		#region SetContent
		public TokenArguments<TArgs> SetContent(TArgs content)
		{
			Content = content.ToJson();
			return this;
		}
		#endregion SetContent
	}
}
