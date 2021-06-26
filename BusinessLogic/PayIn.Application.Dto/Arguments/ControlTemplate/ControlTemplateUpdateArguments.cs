using PayIn.Common.Resources;
using System.ComponentModel.DataAnnotations;
using Xp.Common;
using Xp.Common.Dto.Arguments;
using Xp.Common.Resources;

namespace PayIn.Application.Dto.Arguments.ControlTemplate
{
	public partial class ControlTemplateUpdateArguments : IArgumentsBase
	{
		                                                                   [Required] public int        Id            { get; private set; }
		[Display(Name="resources.controlTemplate.name")]                   [Required] public string     Name          { get; private set; }
		[Display(Name="resources.controlTemplate.observations")]                      public string     Observations  { get; private set; }
		[Display(Name="resources.controlTemplate.checkDuration")]                     public XpDuration CheckDuration { get; private set; }
		[Display(Name="resources.globalResources.monday")]                            public bool       Monday        { get; private set; }
		[Display(Name="resources.globalResources.tuesday")]                           public bool       Tuesday       { get; private set; }
		[Display(Name="resources.globalResources.wednesday")]                         public bool       Wednesday     { get; private set; }
		[Display(Name="resources.globalResources.thursday")]                          public bool       Thursday      { get; private set; }
		[Display(Name="resources.globalResources.friday")]                            public bool       Friday        { get; private set; }
		[Display(Name="resources.globalResources.saturday")]                          public bool       Saturday      { get; private set; }
		[Display(Name="resources.globalResources.sunday")]                            public bool       Sunday        { get; private set; }

		#region Constructors
		public ControlTemplateUpdateArguments(int id, string name, string observations, XpDuration checkDuration, bool monday, bool tuesday, bool wednesday, bool thursday, bool friday, bool saturday, bool sunday)
		{
			Id = id;
			Name = name ?? "";
			Observations = observations ?? "";
			CheckDuration = checkDuration;
			Monday = monday;
			Tuesday = tuesday;
			Wednesday = wednesday;
			Thursday = thursday;
			Friday = friday;
			Saturday = saturday;
			Sunday = sunday;
		}
		#endregion Constructors
	}
}
