using PayIn.Common;
using System;
using System.Collections.Generic;
using Xp.Common;

namespace PayIn.Application.Dto.Payments.Results
{
	public partial class EventGetResult
	{
		public class EventTranslation
		{
			public int Id;
			public LanguageEnum Language;
		}

		public int Id							{ get; set; }
		public decimal? Longitude				{ get; set; }
		public decimal? Latitude				{ get; set; }
		public string Place						{ get; set; }
		public string Name						{ get; set; }
		public string Description				{ get; set; }
		public string PhotoUrl					{ get; set; }
		public string PhotoMenuUrl				{ get; set; }
		public int? Capacity					{ get; set; }
		public XpDateTime EventStart			{ get; set; }
		public XpDateTime EventEnd				{ get; set; }
		public XpDateTime CheckInStart			{ get; set; }
		public XpDateTime CheckInEnd			{ get; set; }
		public EventState State					{ get; set; }
		public int EntranceSystemId				{ get; set; }
		public string EntranceSystemName		{ get; set; }
		public bool IsVisible					{ get; set; }
        public long? Code						{ get; set; }
        public string ShortDescription			{ get; set; }
        public string Conditions				{ get; set; }
		public EventVisibility Visibility		{ get; set; }
        public IEnumerable<object> EventImages  { get; set; }
		public int? ProfileId					{ get; set; }
		public string ProfileName				{ get; set; }
        public string MapUrl					{ get; set; }
		public int? MaxEntrancesPerCard			{ get; set; }
		public decimal? MaxAmountToSpend	    { get; set; }
		public IEnumerable<EventTranslation> TranslationsName				{ get; set; }
		public IEnumerable<EventTranslation> TranslationsDescription		{ get; set; }
		public IEnumerable<EventTranslation> TranslationsShortDescription	{ get; set; }
		public IEnumerable<EventTranslation> TranslationsPlace				{ get; set; }
		public IEnumerable<EventTranslation> TranslationsConditions			{ get; set; }
	}
}
