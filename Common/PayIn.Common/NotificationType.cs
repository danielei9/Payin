using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using Xp.Common;

namespace PayIn.Common
{
	public enum NotificationType
	{
		ConcessionVinculation			    = 1,
		ConcessionVinculationAccepted       = 2,
		PaymentSucceed                      = 3,
		PaymentError                        = 4,
		RefundSucceed                       = 5,
		RefundError						    = 6,
		PaymentMediaCreateSucceed		    = 7,
		PaymentMediaCreateError			    = 8,
		ConcessionVinculationRefused	    = 9,
		ConcessionDissociation			    = 10,
		PaymentWorkerConcessionDissociation = 11,
		PaymentUserConcessionDissociation	= 12,
		AcceptConcessionVinculation			= 13,
		TicketActive						= 14,
		TicketActiveToday					= 15,
 		PaymentConcessionCampaign           = 16,
		PurseDeleted						= 17,
		PaymentMediaLastDay					= 18,
		PaymentConcessionPurse              = 19,
		TicketLastDay						= 20,
		ServiceNotificationCreate           = 21,
		Personalized                        = 22,
		SendPromotionCode                   = 23,
		OrderChangeState                    = 24,
		ChatSend                            = 25,
		ValidateEntrance                    = 26,
		CardVinculated                      = 27,
		CardDesvinculated                   = 28,
		NoticeCreate						= 29,
		NoticeUpdate						= 30,
		EdictCreate							= 31,
		EdictUpdate						= 32,
		PageCreate							= 33,
		PageUpdate							= 34,
		ServiceIncidenceCreate				= 35
	}
}
