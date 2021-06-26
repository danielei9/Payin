namespace PayIn.Application.Tsm.Services.CardConfiguration
{
	public class CardConfig
	{
		public string Label { get; set; }
		public string Keys { get; set; }
		public string AccessCondition { get; set; }
		public string ConfigUrl { get; set; }
		public bool Valid { get; set; }

		public CardConfig(string label, string keys, string accessCondition, string configUrl)
		{
			Label = label;
			Keys = keys;
			AccessCondition = accessCondition;
			ConfigUrl = configUrl;

			Valid = false;
		}

		public void checkSyntax()
		{
			Valid = false;

			if (Label == null || Label.Length > 64)
				return;

			if (Keys == null)
				return;

			if (AccessCondition == null || AccessCondition.Length > (255 * 2))
				return;

			if (ConfigUrl != null && ConfigUrl.Length > 127 * 2)
				return;

			Valid = true;
		}
	}
}
