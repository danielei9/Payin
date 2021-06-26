using System;
using System.Collections.Generic;

namespace PayIn.Web
{
    public class ActionLink
	{
		public enum ActionLinkType
		{
			Panel,
			Popup,
			Csv
		}

		public ActionLinkType          Type           { get; private set; }
		public string                  Source         { get; private set; }
		public string                  Icon           { get; private set; }
		public string                  Show           { get; private set; }
		public string                  Id             { get; private set; }
		public string                  Arguments      { get; private set; }
		public string                  Class          { get; private set; }
		public string                  Text           { get; private set; }
		public string                  TextTranslate  { get; private set; }
		public IEnumerable<ActionLink> Items          { get; private set; }

		#region Constructors
		public ActionLink(ActionLinkType type, string source, string icon = "", string id = "", string arguments = "", string show = "", IEnumerable<string> roles = null, string class_ = "", string text = "", string textTranslate = "")
		{
			Type = type;
			Source = source;
			Icon = icon;
			Id = id;
			Arguments = arguments;
			Class = class_;
			Text = text;
			TextTranslate = textTranslate;

			Show = show;
			if (roles != null)
				foreach (var role in roles)
				{
					if (!Show.IsNullOrEmpty())
						Show = Show + " || ";
					Show = Show + "authentication.hasRole(\"" + role + "\")";
				}
		}
		public ActionLink(IEnumerable<ActionLink> items, string icon = "", string show = "", IEnumerable<string> roles = null)
		{
			Icon = icon;
			Items = items;


			Show = show;
			if (roles != null)
				foreach (var role in roles)
				{
					if (!Show.IsNullOrEmpty())
						Show = Show + " || ";
					Show = Show + "authentication.hasRole(\"" + role + "\")";
				}
		}
		#endregion Constructors
	}
}
