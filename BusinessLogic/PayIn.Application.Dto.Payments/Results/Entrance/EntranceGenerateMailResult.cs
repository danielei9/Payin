using PayIn.Common;
using Xp.Common;

namespace PayIn.Application.Dto.Payments.Results
{
	public partial class EntranceGenerateMailResult
	{
       //Event
        public string       EventName                   { get; set; }
        public XpDateTime   EventDate                   { get; set; } 
        public string       EventPhotoUrl               { get; set; }
        public string       EventImageUrl               { get; set; }
        public string       EventLocation               { get; set; }

        //EntranceType & Entrance
        public string       EntranceTypeName            { get; set; }
        public decimal      EntrancePrice               { get; set; }
        public long         EntranceCode                { get; set; }
        public string       EntranceShortDescription    { get; set; }
        public string       EntranceConditions          { get; set; }

        //User
        public string       UserName                    { get; set; }
        public string       UserVatnumber               { get; set; }

        //Concession
        public string       ConcessionName              { get; set; }
        public string       ConcessionVatNumber         { get; set; }

        public XpDateTime   EntranceSellTime            { get; set; }
    }
}
