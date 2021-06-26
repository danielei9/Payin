using Microsoft.Practices.Unity;
using PayIn.BusinessLogic.Common;
using PayIn.Domain.JustMoney;
using PayIn.Infrastructure.JustMoney.Arguments;
using PayIn.Infrastructure.JustMoney.Db;
using PayIn.Infrastructure.JustMoney.Enums;
using PayIn.Infrastructure.JustMoney.Results;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Xp.Domain;

namespace PayIn.Infrastructure.JustMoney.Server
{
	public class PfsServer
	{
		[Dependency] public ISessionData SessionData { get; set; }

		private readonly string PfsUrl = "https://staging.prepaidfinancialservices.com/accountapiv2/service.asmx/Process";
		//private readonly string PfsUrl = "https://www.prepaidfinancialservices.com/accountapiv2/service.asmx ";
		private readonly string PfsUser = "JustMoneyextapiusr";
		private readonly string PfsPassword = "SnP6VltB*LzZ";
		private readonly string PfsSubbin = "59991198";
		private readonly string PfsVendorId = "10000809";

		private readonly string TpvUrl = "https://staging.prepaidfinancialservices.com/AcquiringTunnel/request.ashx";
		//private readonly string TpvUrl = "https://www.prepaidfinancialservices.com/AcquiringTunnel/request.ashx";

#if JUSTMONEY
		private readonly string ConfimUrl = @"https://justmoney.pay-in.es/#/PrepaidCard/RechargedCard";
		private readonly string ErrorUrl  = @"https://justmoney.pay-in.es/#/PrepaidCard/RechargedCardError";
#else
		private readonly string ConfimUrl = @"http://localhost:9090/#/PrepaidCard/RechargedCard";
		private readonly string ErrorUrl = @"http://localhost:9090/#/PrepaidCard/RechargedCardError";
#endif
		
		#region CreateLog
		protected async Task<Log> CreateLogAsync(string relatedMethod, string relatedId, TimeSpan elapsed, object handler, object arguments, object result, Exception exception = null)
		{
			using (var context = new JustMoneyContext())
			{
				var message = "";
				if (exception != null)
				{
					var ex = exception;
					while (ex != null)
					{
						message += "\n" + ex.Message;
						ex = ex.InnerException;
					}
				}

				var log = new Log
				{
					DateTime = DateTime.Now.ToUTC(),
					Duration = elapsed,
					Login = SessionData.Login ?? "",
					ClientId = SessionData.ClientId,
					RelatedClass = "PfsService",
					RelatedMethod = relatedMethod,
					RelatedId = 0,
					Error = exception == null ? "" : message + "\n" + exception.StackTrace
				};
				//await LogRepository.AddAsync(log);

				context.Log.Add(log);

				foreach (var arg in arguments.GetType().GetProperties())
				{
					var value = arg.GetValue(arguments);
					if (value != null)
					{
						if ((arg.Name.ToLower() == relatedId.ToLower()) && (value != null))
						{
							if (typeof(long?).IsAssignableFrom(arg.PropertyType))
								log.RelatedId = (value as long?).Value;
							else if (typeof(long).IsAssignableFrom(arg.PropertyType))
								log.RelatedId = (long)value;
							else if (typeof(int?).IsAssignableFrom(arg.PropertyType))
								log.RelatedId = (value as int?).Value;
							else if (typeof(int).IsAssignableFrom(arg.PropertyType))
								log.RelatedId = (int)value;
							else if (typeof(string).IsAssignableFrom(arg.PropertyType))
							{
								long val;
								if (long.TryParse(value as string, out val))
									log.RelatedId = val;
							}
						}

						log.Arguments.Add(new LogArgument
						{
							Name = arg.Name,
							Value = value.GetType().IsValueType ?
								value.ToString() :
								value.ToJson()
						});
					}
				}

				log.Results.Add(new LogResult
				{
					Name = "Result",
					Value = result == null ? "" : result.ToJson()
				});

				await context.SaveChangesAsync();

				return log;
			}
		}
		#endregion CreateLog

		#region CheckError
		private void CheckError(string code)
		{
			// Success
			if (code == "0000") return;
			if (code == "0061") return;

			// Error
			if (code == "-1") throw new ApplicationException("User not authorised to use service");
			if (code == "-2") throw new ApplicationException("Operation Failed Connectivity issues");
			if (code == "-3") throw new ApplicationException("Card Format Error");
			if (code == "-4") throw new ApplicationException("Amount parameter non numeric");
			if (code == "-5") throw new ApplicationException("Transaction type format error");
			if (code == "-6") throw new ApplicationException("Currency Code parameter format error");
			if (code == "-7") throw new ApplicationException("Settlement Amount format error");
			if (code == "-9") throw new ApplicationException("Terminal Owner format Error");
			if (code == "-10") throw new ApplicationException("Transaction Description format error");
			if (code == "-11") throw new ApplicationException("Variant format error");
			if (code == "-12") throw new ApplicationException("Store Account Number format error");
			if (code == "-13") throw new ApplicationException("Terminal State format error");
			if (code == "-14") throw new ApplicationException("Terminal ID format error");
			if (code == "-15") throw new ApplicationException("Country format error");
			if (code == "-16") throw new ApplicationException("Transaction Flat Fee format error");
			if (code == "-17") throw new ApplicationException("Fee description format error");
			if (code == "-18") throw new ApplicationException("Direct fee format error");
			if (code == "-19") throw new ApplicationException("Charge band format error");
			if (code == "-20") throw new ApplicationException("Transaction charge type format error");
			if (code == "-21") throw new ApplicationException("Store description format error");
			if (code == "-22") throw new ApplicationException("Card To format error");
			if (code == "-23") throw new ApplicationException("Status Format error");
			if (code == "-24") throw new ApplicationException("Date format error");
			if (code == "-25") throw new ApplicationException("Description format error");
			if (code == "-26") throw new ApplicationException("Fee format error");
			if (code == "-27") throw new ApplicationException("First Name Format Error");
			if (code == "-28") throw new ApplicationException("Middle Initial Format Error");
			if (code == "-29") throw new ApplicationException("Last Name format Error");
			if (code == "-30") throw new ApplicationException("Address 1 format error");
			if (code == "-31") throw new ApplicationException("Address 2 format error");
			if (code == "-32") throw new ApplicationException("Address 3 format error");
			if (code == "-33") throw new ApplicationException("Address 4 format error");
			if (code == "-34") throw new ApplicationException("City format error");
			if (code == "-35") throw new ApplicationException("Country Name format error");
			if (code == "-36") throw new ApplicationException("Country Name 2 format error");
			if (code == "-37") throw new ApplicationException("State format error");
			if (code == "-38") throw new ApplicationException("Zip Code format error");
			if (code == "-39") throw new ApplicationException("County name format error");
			if (code == "-40") throw new ApplicationException("County name 2 format error");
			if (code == "-41") throw new ApplicationException("Client Holder Id format error");
			if (code == "-42") throw new ApplicationException("Country Code format error");
			if (code == "-43") throw new ApplicationException("Phone format error");
			if (code == "-44") throw new ApplicationException("Phone2 format error");
			if (code == "-45") throw new ApplicationException("SecurityField1 format error");
			if (code == "-46") throw new ApplicationException("SecurityField2 format error");
			if (code == "-47") throw new ApplicationException("SecurityField3 format error");
			if (code == "-48") throw new ApplicationException("SecurityField4 format error");
			if (code == "-49") throw new ApplicationException("Email format error");
			if (code == "-50") throw new ApplicationException("Userdefinedfield1 format error");
			if (code == "-51") throw new ApplicationException("Userdefinedfield2 format error");
			if (code == "-52") throw new ApplicationException("SocialSecurityNumber format error");
			if (code == "-54") throw new ApplicationException("DistributorCode format error");
			if (code == "-56") throw new ApplicationException("CompanyName format error");
			if (code == "-57") throw new ApplicationException("CardStyle format error");
			if (code == "-58") throw new ApplicationException("Embossname format error");
			if (code == "-59") throw new ApplicationException("ExpirationDate format error");
			if (code == "-60") throw new ApplicationException("ProducePlastic format error");
			if (code == "-61") throw new ApplicationException("DeliveryType format error");
			if (code == "-62") throw new ApplicationException("OFACOvide format error");
			if (code == "-63") throw new ApplicationException("VerifyDOBOvide format error");
			if (code == "-64") throw new ApplicationException("VerifySSNOvide format error");
			if (code == "-65") throw new ApplicationException("BoeCheckOvide format error");
			if (code == "-66") throw new ApplicationException("Addresslineforsecondaryaddress format error");
			if (code == "-67") throw new ApplicationException("Addressline2forsecondaryaddress format error");
			if (code == "-68") throw new ApplicationException("Addressline3forsecondaryaddress format error");
			if (code == "-69") throw new ApplicationException("Addressline4forsecondaryaddress format error");
			if (code == "-70") throw new ApplicationException("IP format error");
			if (code == "-71") throw new ApplicationException("Illegal characters in Userdefinedfield3");
			if (code == "-72") throw new ApplicationException("Illegal characters in Userdefinedfield4");
			if (code == "-73") throw new ApplicationException("Illegal characters in Secondary Address City");
			if (code == "-74") throw new ApplicationException("Secondary Address Zip");
			if (code == "-75") throw new ApplicationException("Secondary Address state");
			if (code == "-76") throw new ApplicationException("Pin Length error");
			if (code == "-77") throw new ApplicationException("TransType format error 1 or 2 only allowed");
			if (code == "-78") throw new ApplicationException("Operation timed out");
			if (code == "-79") throw new ApplicationException("Operation web error");
			if (code == "-80") throw new ApplicationException("Secondary Country Code Error");
			if (code == "-81") throw new ApplicationException("Bin Format error");
			if (code == "-82") throw new ApplicationException("Pin No Error");
			if (code == "-83") throw new ApplicationException("Mobile number format error");
			if (code == "-84") throw new ApplicationException("IP denied Access to API");
			if (code == "-85") throw new ApplicationException("Serialization Failed");
			if (code == "-86") throw new ApplicationException("Supply a reason for card closure");
			if (code == "-87") throw new ApplicationException("Message ID not found");
			if (code == "-88") throw new ApplicationException("Please supply a Message ID");
			if (code == "-89") throw new ApplicationException("MessageID Not Unique");
			if (code == "-90") throw new ApplicationException("Supply a reason for replacement of card");
			if (code == "-91") throw new ApplicationException("New card created, but failed card link, deposit to new card and close current card");
			if (code == "-92") throw new ApplicationException("New card created, old card closed but failed deposit and card link");
			if (code == "-93") throw new ApplicationException("New card created, deposit done but failed to close card and card link");
			if (code == "-94") throw new ApplicationException("New card created, deposit done, old card closed but failed card link");
			if (code == "-95") throw new ApplicationException("New card created, card link done but failed deposit and close card");
			if (code == "-96") throw new ApplicationException("New card created, card linked, closed card but deposit failed");
			if (code == "-97") throw new ApplicationException("New card created, card linked, deposit done but failed to close card");
			if (code == "-98") throw new ApplicationException("Card Replaced successfully");
			if (code == "-99") throw new ApplicationException("New card created, deposit and close card failed");
			if (code == "-100") throw new ApplicationException("Failed to link mobile to card");
			if (code == "-101") throw new ApplicationException("Mobile Not Found");
			if (code == "-102") throw new ApplicationException("New card created, closed card but deposit failed");
			if (code == "-103") throw new ApplicationException("New card created, deposit done but close card failed");
			if (code == "-104") throw new ApplicationException("Card Replaced successfully");
			if (code == "-105") throw new ApplicationException("Card is already closed");
			if (code == "-106") throw new ApplicationException("Search for this bin number is not authorized");
			if (code == "-107") throw new ApplicationException("Failed to retrieve replacement card details");
			if (code == "-108") throw new ApplicationException("KYC for new card failed.");
			if (code == "-109") throw new ApplicationException("XSD validation failed.");
			if (code == "-110") throw new ApplicationException("Currency FX failed.");
			if (code == "-111") throw new ApplicationException("Guid is incorrect.");
			if (code == "-112") throw new ApplicationException("Insufficient Balance.");
			if (code == "-113") throw new ApplicationException("Full name is not in the correct format.");
			if (code == "-114") throw new ApplicationException("Passowrd is forced to be changed.");
			if (code == "-115") throw new ApplicationException("Password is not strong enough.");
			if (code == "-116") throw new ApplicationException("Password reset failed.");
			if (code == "-117") throw new ApplicationException("Amount exceeded load limit.");
			if (code == "-118") throw new ApplicationException("DC is not allowed to load.");
			if (code == "-119") throw new ApplicationException("Secondary card is not allowed to load.");
			if (code == "-120") throw new ApplicationException("Bin credentials mismatch.");
			if (code == "-121") throw new ApplicationException("Record not found.");
			if (code == "-122") throw new ApplicationException("Card is not SDD.");
			if (code == "-123") throw new ApplicationException("Restricted country");
			if (code == "5000") throw new ApplicationException("Invalid message type");
			if (code == "5001") throw new ApplicationException("Missing card number and control number");
			if (code == "5002") throw new ApplicationException("Negative balance not allowed");
			if (code == "5003") throw new ApplicationException("Invalid card prefix");
			if (code == "5004") throw new ApplicationException("Error processing request message");
			if (code == "5005") throw new ApplicationException("Card not yet activated");
			if (code == "5006") throw new ApplicationException("Card status cannot be changed");
			if (code == "5007") throw new ApplicationException("Message not in recognizable format");
			if (code == "5008") throw new ApplicationException("Missing cardholder id");
			if (code == "5009") throw new ApplicationException("bin #s don't match");
			if (code == "5010") throw new ApplicationException("fiids do not match");
			if (code == "5011") throw new ApplicationException("Cannot link card, limit exceeded");
			if (code == "5012") throw new ApplicationException("Fiids do not match");
			if (code == "5013") throw new ApplicationException("Amount less than minimum limit");
			if (code == "5014") throw new ApplicationException("Amount exceeds maximum limit");
			if (code == "5015") throw new ApplicationException("fiids do not match");
			if (code == "5016") throw new ApplicationException("Error constructing direct deposit number");
			if (code == "5017") throw new ApplicationException("Fiids do not match");
			if (code == "5018") throw new ApplicationException("Exceeded daily maximum deposit credits");
			if (code == "5019") throw new ApplicationException("Exceeded daily maximum deposit amount");
			if (code == "5100") throw new ApplicationException("Card record not found");
			if (code == "5101") throw new ApplicationException("Card record locked");
			if (code == "5102") throw new ApplicationException("Error reading from card file");
			if (code == "5103") throw new ApplicationException("Error reading from card file");
			if (code == "5104") throw new ApplicationException("Error reading from card file");
			if (code == "5110") throw new ApplicationException("Balance record not found");
			if (code == "5111") throw new ApplicationException("Balance record locked");
			if (code == "5112") throw new ApplicationException("Error reading from balance file");
			if (code == "5113") throw new ApplicationException("Error reading from balance file");
			if (code == "5114") throw new ApplicationException("Error reading from balance file");
			if (code == "5120") throw new ApplicationException("Error reading from idf file");
			if (code == "5121") throw new ApplicationException("Error reading from idf file");
			if (code == "5122") throw new ApplicationException("Error reading from idf file");
			if (code == "5130") throw new ApplicationException("Error reading from tran log control file");
			if (code == "5131") throw new ApplicationException("Error reading from tran log control file");
			if (code == "5140") throw new ApplicationException("Error reading control number file");
			if (code == "5141") throw new ApplicationException("Error reading control number file");
			if (code == "5150") throw new ApplicationException("Error reading from fee file");
			if (code == "5160") throw new ApplicationException("Error reading from card prefix file");
			if (code == "5170") throw new ApplicationException("Error reading from key auth file");
			if (code == "5171") throw new ApplicationException("Error reading from key auth file");
			if (code == "5175") throw new ApplicationException("Error reading from customer xref file");
			if (code == "5176") throw new ApplicationException("Error reading from customer xref file");
			if (code == "5177") throw new ApplicationException("Error updating customer xref file");
			if (code == "5180") throw new ApplicationException("Error reading from card save info file");
			if (code == "5181") throw new ApplicationException("Error reading from card save info file");
			if (code == "5182") throw new ApplicationException("Error reading from cardholder demo file");
			if (code == "5183") throw new ApplicationException("Error reading from cardholder demo file");
			if (code == "5184") throw new ApplicationException("Error updating cardholder demo file");
			if (code == "5185") throw new ApplicationException("Error updating cardholder demo file");
			if (code == "5186") throw new ApplicationException("Error updating cardholder demo file");
			if (code == "5190") throw new ApplicationException("Error reading from hist log control file");
			if (code == "5191") throw new ApplicationException("Error reading from hist log control file");
			if (code == "5195") throw new ApplicationException("Error reading from tran log history file");
			if (code == "5196") throw new ApplicationException("Error reading from tran log history file");
			if (code == "5199") throw new ApplicationException("Error reading from parameter file");
			if (code == "5200") throw new ApplicationException("Error updating card file");
			if (code == "5201") throw new ApplicationException("Error updating card file");
			if (code == "5210") throw new ApplicationException("Error updating balance file");
			if (code == "5211") throw new ApplicationException("Error updating balance file");
			if (code == "5240") throw new ApplicationException("Error updating control number file");
			if (code == "5255") throw new ApplicationException("Error updating direct deposit control file");
			if (code == "5280") throw new ApplicationException("Error updating card save info file");
			if (code == "5281") throw new ApplicationException("Error updating card save info file");
			if (code == "5284") throw new ApplicationException(" Error assigning a card, card already assigned");
			if (code == "5295") throw new ApplicationException("Error updating tran log file");
			if (code == "5299") throw new ApplicationException("Error updating audit file");
			if (code == "5300") throw new ApplicationException("Missing PIN");
			if (code == "5301") throw new ApplicationException("Error validating PIN");
			if (code == "5302") throw new ApplicationException("Error changing PIN");
			if (code == "5305") throw new ApplicationException("Error changing PIN");
			if (code == "5306") throw new ApplicationException("PIN tries exceeded");
			if (code == "5307") throw new ApplicationException("Error generating PIN");
			if (code == "5303") throw new ApplicationException("Invalid PIN initialize request");
			if (code == "5304") throw new ApplicationException("Invalid PIN");
			if (code == "6000") throw new ApplicationException("Negative balance on card");
			if (code == "7000") throw new ApplicationException("Amount cannot be negative");
			if (code == "7001") throw new ApplicationException("Fee cannot be negative");
			if (code == "7002") throw new ApplicationException("Fee cannot be negative");
			if (code == "7003") throw new ApplicationException("Invalid trans type indicator");
			if (code == "7004") throw new ApplicationException("The card has a status of deposit only");
			if (code == "7005") throw new ApplicationException("Invalid date specified if either the starting or ending dates are invalid");
			if (code == "000") throw new ApplicationException("Success");
			if (code == "001") throw new ApplicationException("Unspecified Error");
			if (code == "002") throw new ApplicationException("Login Error");
			if (code == "003") throw new ApplicationException("Message ID record not found for < Message ID Number supplied by user >");
			if (code == "004") throw new ApplicationException("Message ID already filed");
			if (code == "005") throw new ApplicationException("Invalid Message ID Format");
			if (code == "006") throw new ApplicationException("Function number is not supported");
			if (code == "007") throw new ApplicationException("Invalid Client Number");
			if (code == "008") throw new ApplicationException("Missing required parameters(< Missing parameter listing >)");
			if (code == "009") throw new ApplicationException("(Reserved for future use)");
			if (code == "010") throw new ApplicationException("(Reserved for future use)");
			if (code == "011") throw new ApplicationException("Invalid Card Number");
			if (code == "012") throw new ApplicationException("Invalid Account Number / SSN");
			if (code == "013") throw new ApplicationException("Invalid Amount");
			if (code == "014") throw new ApplicationException("Invalid Transaction Type");
			if (code == "015") throw new ApplicationException("Invalid Old Pin");
			if (code == "016") throw new ApplicationException("Invalid New Pin");
			if (code == "017") throw new ApplicationException("Invalid Start Date");
			if (code == "018") throw new ApplicationException("Invalid End Date");
			if (code == "019") throw new ApplicationException("The start date is earlier then the end date");
			if (code == "020") throw new ApplicationException("Time date out of range");
			if (code == "021") throw new ApplicationException("Invalid currency code for current BIN number");
			if (code == "022") throw new ApplicationException("Host Down");
			if (code == "050") throw new ApplicationException("OFAC Match Found");
			if (code == "051") throw new ApplicationException("Credit Card Pre - Auth failure or declined");
			if (code == "052") throw new ApplicationException("API Address Validation Failed");
			if (code == "053") throw new ApplicationException("Credit Card is already associated with another gkard");
			if (code == "054") throw new ApplicationException("No credit card provided and no default card is associated!");
			if (code == "056") throw new ApplicationException("Load amount is less than transfer amount");
			if (code == "057") throw new ApplicationException("No Credit Card Provided For Instant Issue!");
			if (code == "058") throw new ApplicationException("Limit reached for Credit Cards association for InstantIssue!");
			if (code == "059") throw new ApplicationException("No credit card provided and card association is turned OFF!");
			if (code == "062") throw new ApplicationException("Load amount is less than transfer amount");
			if (code == "063") throw new ApplicationException("Could not find a response for Message ID");
			if (code == "071") throw new ApplicationException("Exceeded aggregate credit card load count limit.Actual load count would be greater than load ceiling count for any card.");
			if (code == "072") throw new ApplicationException("Exceeded aggregate credit card load amount limit.Actual load amount would be greater than load ceiling amount for any card.");
			if (code == "073") throw new ApplicationException("Exceeded aggregate credit card load amount limit.Actual load amount would be greater than load ceiling amount for any card.");
			if (code == "074") throw new ApplicationException("Exceeded credit card load amount limit.Actual load amount would be greater than load ceiling amount.");
			if (code == "075") throw new ApplicationException("SSN Validation failed");
			if (code == "076") throw new ApplicationException("DOB Validation failed");
			if (code == "077") throw new ApplicationException("Invalid credential combination.WalletID found, need a matching i24Card");
			if (code == "078") throw new ApplicationException("reserved");
			if (code == "079") throw new ApplicationException("reserved");
			if (code == "080") throw new ApplicationException("i24Wallet provisioning failed");
			if (code == "081") throw new ApplicationException("Missing minimum required credentials");
			if (code == "082") throw new ApplicationException("reserved");
			if (code == "083") throw new ApplicationException("Invalid credential combination");
			if (code == "084") throw new ApplicationException("WalletID invalid");
			if (code == "085") throw new ApplicationException("Wallet password invalid");
			if (code == "086") throw new ApplicationException("Email address associated to another WalletID");
			if (code == "087") throw new ApplicationException("Wallet provisioning failed.Possible duplicate user");
			if (code == "088") throw new ApplicationException("Wallet provisioning failed.Invalid reference");
			if (code == "089") throw new ApplicationException("Email address associated to another WalletID");
			if (code == "090") throw new ApplicationException("Inadequate permissions to execute this function");
			if (code == "091") throw new ApplicationException("Unsupported function specified");
			if (code == "092") throw new ApplicationException("IssueCredit: Invalid Transid");
			if (code == "093") throw new ApplicationException("IssueCredit: Failure unloading card");
			if (code == "094") throw new ApplicationException("IssueCredit: Connectivity failure to i24Gateway");
			if (code == "095") throw new ApplicationException("Unknown code");
			if (code == "096") throw new ApplicationException("AutoLoad disabled by cardholder");
			if (code == "0004") throw new ApplicationException("Cardholder ID not found");
			if (code == "-400") throw new ApplicationException("More Than One Active Card With this MobileNumber");
			if (code == "-401") throw new ApplicationException("Mobile Number Not Found");
			if (code == "-402") throw new ApplicationException("Email Not Found");
			if (code == "-403") throw new ApplicationException("Email is in the wrong Format");
			if (code == "-405") throw new ApplicationException("More than one Issue Pack found while searching");
			if (code == "-406") throw new ApplicationException("Issue Pack not found");
			if (code == "-407") throw new ApplicationException("Cardholder not found");
			if (code == "-408") throw new ApplicationException("Dealer Number Invalid Format");
			if (code == "-409") throw new ApplicationException("Sale was Unsuccessful");
			if (code == "-410") throw new ApplicationException("Issue pack already was sold");
			if (code == "-411") throw new ApplicationException("Name or surname provided are invalid");
			if (code == "-412") throw new ApplicationException("Zipcode Provided was invalid");
			if (code == "-413") throw new ApplicationException("Loadamount provided is invalid");
			if (code == "-414") throw new ApplicationException("There are fields that are required that where not provided");
			if (code == "-415") throw new ApplicationException("Address provided is invalid");
			if (code == "-416") throw new ApplicationException("Invalid Country or invalid county");
			if (code == "-417") throw new ApplicationException("Invalid Mobile number");
			if (code == "-418") throw new ApplicationException("Invalid user defined field");
			if (code == "-419") throw new ApplicationException("Invalid voucher code");
			if (code == "-420") throw new ApplicationException("While Selling, more than one issue pack was found with the same details");
			if (code == "-421") throw new ApplicationException("Invalid Terminal information");
			if (code == "-422") throw new ApplicationException("Merchant Sale that needs to be voided was not found");
			if (code == "-423") throw new ApplicationException("Void merchant sale Successful");
			if (code == "-424") throw new ApplicationException("There is no need for a void");
			if (code == "-425") throw new ApplicationException("Message Id provided was not found");
			if (code == "-426") throw new ApplicationException("There is no application assigned to the username you are using");
			if (code == "-427") throw new ApplicationException("While performing a merchant sale something went wrong and it had to be voided");
			if (code == "-428") throw new ApplicationException("The Terminal Id provided is incorrect");
			if (code == "-429") throw new ApplicationException("Checkpoint API failed.");
			if (code == "-430") throw new ApplicationException("This is a Three Mobile but not found assigned to any card");
			if (code == "-431") throw new ApplicationException("This is a Three BroadBand but not found assigned to any card");
			if (code == "-432") throw new ApplicationException("This is a O2 Mobile but not found assigned to any card");
			if (code == "-433") throw new ApplicationException("This is a O2 BroadBand but not found assigned to any card");
			if (code == "-434") throw new ApplicationException("This is a Three customer which is not MBB or mobile. No card assigned to it.");
			if (code == "-435") throw new ApplicationException("This is a O2 customer which it is not MBB or mobile. No card assigned to it.");
			if (code == "-436") throw new ApplicationException("Unknown organization");
			if (code == "-437") throw new ApplicationException("Unkwown organization");
			if (code == "-438") throw new ApplicationException("This is not a mobile number.");
			if (code == "-439") throw new ApplicationException("The supplied number are 3 MBB abd Mobile.Only 3 Mobile is required in this case");
			if (code == "-440") throw new ApplicationException("The supplied Mobile number is a 3Customer but not a mobile");
			if (code == "-441") throw new ApplicationException("You are not Authorized to perform actions on this cardholder.");
			if (code == "-442") throw new ApplicationException("TerminalID Verification not required");
			if (code == "-501") throw new ApplicationException("Cancelling of a bank payment has failed");
			if (code == "-504") throw new ApplicationException("Recurring bank payment not found");
			if (code == "-600") throw new ApplicationException("Product Type is invalid");
			if (code == "-503") throw new ApplicationException("No payee found");
			if (code == "-602") throw new ApplicationException("Reference Number Format error");
			if (code == "-603") throw new ApplicationException("First Name Format error");
			if (code == "-604") throw new ApplicationException("Middle Initial Format error");
			if (code == "-605") throw new ApplicationException("Last Name Format Error");
			if (code == "-606") throw new ApplicationException("Address Format Error");
			if (code == "-607") throw new ApplicationException("City Format Error");
			if (code == "-608") throw new ApplicationException("County Format Error");
			if (code == "-609") throw new ApplicationException("Zip Code Format Error");
			if (code == "-610") throw new ApplicationException("Country Code Format Error. Must Only contail 2 letters");
			if (code == "-611") throw new ApplicationException("Country Name Format Error");
			if (code == "-612") throw new ApplicationException("Phone Format Error Must contain 7 - 16 characters including a +");
			if (code == "-613") throw new ApplicationException("Language Format Error");
			if (code == "-614") throw new ApplicationException("Default Lang Format Error");
			if (code == "-615") throw new ApplicationException("Email Format Error");
			if (code == "-616") throw new ApplicationException("DOB Format Error must by dd/ mm / yyyy format and cannot be older than 100 years");
			if (code == "-617") throw new ApplicationException("Gender Format Error.must be M / F");
			if (code == "-618") throw new ApplicationException("Barcode Prefix Format Error. Must contain only 3 characters");
			if (code == "-619") throw new ApplicationException("Refernce number already in use");
			if (code == "-620") throw new ApplicationException("Refernce number already in use in consumer application");
			if (code == "-700") throw new ApplicationException("Voucher has already been used");
			if (code == "-300") throw new ApplicationException("Success");
			if (code == "-301") throw new ApplicationException("Failed");
			if (code == "-302") throw new ApplicationException("Authorisation Failed");
			if (code == "-303") throw new ApplicationException("General failure, unexpected exception");
			if (code == "-304") throw new ApplicationException("Some or all the fields in the request were empty");
			if (code == "-305") throw new ApplicationException("XML malformed");
			if (code == "-306") throw new ApplicationException("Some or all the elements in the XML were empty");
			if (code == "-307") throw new ApplicationException("Some or all supplied data was not in the correct format");
			if (code == "-308") throw new ApplicationException("Wallet creation failed");
			if (code == "-309") throw new ApplicationException("Wallet already exits");
			if (code == "-310") throw new ApplicationException("Wallet does not exist");
			if (code == "-311") throw new ApplicationException("Card Holder ID does not exist");
			if (code == "-312") throw new ApplicationException("Client could not be created, Card Holder ID already exists");
			if (code == "-313") throw new ApplicationException("Client creation failed");
			if (code == "-314") throw new ApplicationException("Client created but wallets were not");
			if (code == "-315") throw new ApplicationException("A transaction cannot be processed between two E-Card wallets");
			if (code == "-316") throw new ApplicationException("This transaction cannot be procesed, wallets have the same currency");
			if (code == "-317") throw new ApplicationException("This transaction cannot be procesed, wallets do not have the same currency");
			if (code == "-318") throw new ApplicationException("Transactional process was not completed, funds refunded back to original state");
			if (code == "-319") throw new ApplicationException("Main wallet does not exists");
			if (code == "-320") throw new ApplicationException("The distributor code does not exist");
			if (code == "-321") throw new ApplicationException("Client could not be created, client already exists");
			if (code == "-322") throw new ApplicationException("Client does not exist");
			if (code == "-323") throw new ApplicationException("Trade Corp wallets do not exists, E-card wallets could not be created");
			if (code == "-324") throw new ApplicationException("Corporate client already exists");
			if (code == "-325") throw new ApplicationException("Corporate client does not exist");
			if (code == "-326") throw new ApplicationException("Corporate parent programme does not exist");
			if (code == "-327") throw new ApplicationException("Failed to create wallet at processor");
			if (code == "-328") throw new ApplicationException("Programme does not exist");
			if (code == "-329") throw new ApplicationException("Transaction type not authorised");
			if (code == "-330") throw new ApplicationException("Corporate name already exists");
			if (code == "-331") throw new ApplicationException("Sales Force ID already exists");
			if (code == "-332") throw new ApplicationException("Request cannot be processed, not authorised");
			if (code == "-333") throw new ApplicationException("Transaction not logged in report");
			if (code == "-334") throw new ApplicationException("Corporate DC already exists");
			if (code == "-335") throw new ApplicationException("Transaction has been put in a pending state");
			if (code == "-336") throw new ApplicationException("Failed to credit E-Card after successful debit in E - Card to E-Card transfer");
			if (code == "-350") throw new ApplicationException("Card is not a multicurrency card");
			if (code == "-351") throw new ApplicationException("No currencies found");
			if (code == "-352") throw new ApplicationException("Deposit type trade flag parameter not supported");
			if (code == "-353") throw new ApplicationException("Trade failed");
			if (code == "-354") throw new ApplicationException("Invalid Rate");
			if (code == "-355") throw new ApplicationException("Invalid Amount");
			if (code == "-356") throw new ApplicationException("Action not recognized");
			if (code == "-109") throw new ApplicationException("XML Validation failed");
			if (code == "-110") throw new ApplicationException("Currency conversion failed");
			if (code == "-111") throw new ApplicationException("GUID already used");
			if (code == "-112") throw new ApplicationException("Insufficient Balance for card transfer");
			if (code == "-113") throw new ApplicationException("Full name length is longer than 26 characters");
			if (code == "-114") throw new ApplicationException("Force Password change incorrect format");
			if (code == "-115") throw new ApplicationException("Password is incorrect format");
			if (code == "-116") throw new ApplicationException("Password reset failed");
			if (code == "-117") throw new ApplicationException("Amount would exceed load limit");
			if (code == "-118") throw new ApplicationException("Load for card distributor code not allowed");
			if (code == "-119") throw new ApplicationException("Secondary card load for card distributor code not allowed");
			if (code == "-120") throw new ApplicationException("Invalid bin with credentials in use");
			if (code == "-121") throw new ApplicationException("Card Record Not Found");
			if (code == "-122") throw new ApplicationException("This has no SDD limits set");
			if (code == "-123") throw new ApplicationException("Country Name is not a valid country to issue Prepaid Cards");
			if (code == "-124") throw new ApplicationException("The transaction currency does not match the card bin");
			if (code == "-125") throw new ApplicationException("Card status does not permit to load funds");
			if (code == "-5018") throw new ApplicationException("Exceeded daily maximum deposit credits");
			if (code == "-5019") throw new ApplicationException("Exceeded number of load tries per day");
			if (code == "-5013") throw new ApplicationException("Amount less than minimum limit");
			if (code == "-5014") throw new ApplicationException("Amount exceeds maximum limit");
			if (code == "-5021") throw new ApplicationException("Amount exceeded maximum card deposit amount");
			if (code == "-5020") throw new ApplicationException("Cannot reverse a load which was not successful");
			if (code == "-5022") throw new ApplicationException("Transaction already voided");
			if (code == "-5023") throw new ApplicationException("Load amount is less than minimum card to card load amount");
			if (code == "-5000") throw new ApplicationException("No Funds In Wallet");
			if (code == "-5024") throw new ApplicationException("Maximum monthly load limits exceeded");
			if (code == "-5001") throw new ApplicationException("Wallet not available");
			if (code == "-5025") throw new ApplicationException("Maximum yearly load limits exceeded");
			if (code == "-127") throw new ApplicationException("Product Type not found for bin");
			if (code == "-126") throw new ApplicationException("Loading is not allowed on card request");
			if (code == "-128") throw new ApplicationException("Product Type does not exist");
			if (code == "-129") throw new ApplicationException("Product Type requires full KYC");
			if (code == "-130") throw new ApplicationException("Product Type requires company KYC");
			if (code == "-131") throw new ApplicationException("Product Type cannot be updated to new product type");
			if (code == "-132") throw new ApplicationException("Debit transaction is not allowed");
			if (code == "-133") throw new ApplicationException("Credit transaction is not allowed");
			if (code == "-134") throw new ApplicationException("Fee is configured for both E-wallet and for Card");
			if (code == "-135") throw new ApplicationException("Transaction not allowed");
			if (code == "-136") throw new ApplicationException("Card to Card transfer is not allowed for SDD cards");
			if (code == "-137") throw new ApplicationException("Transaction not allowed on card distributor code");
			if (code == "-138") throw new ApplicationException("Card is in negative balance");
			if (code == "-139") throw new ApplicationException("Required one alternate field");
			if (code == "-140") throw new ApplicationException("Card to Card transfer is not allowed for SDD cards with different mobile");
			if (code == "-141") throw new ApplicationException("Exceeded total number of load tries allowed");
			if (code == "-142") throw new ApplicationException("No product type limits on card");
			if (code == "-143") throw new ApplicationException("Currency Rates not available");
			if (code == "-144") throw new ApplicationException("Multi currency card fee cannot be charged on currency not set as default");
			if (code == "-145") throw new ApplicationException("Failed conversion on purse balance to default currency");
			if (code == "-146") throw new ApplicationException("Insufficient Funds");
			if (code == "-147") throw new ApplicationException("Card inquiry failed to get balance");
			if (code == "-148") throw new ApplicationException("Card inquiry failed to check status");
			if (code == "-149") throw new ApplicationException("Error while depositing into wallet");
			if (code == "-150") throw new ApplicationException("Date to is before Date From");
			if (code == "-151") throw new ApplicationException("Application transfer funds type is not valid");
			if (code == "-152") throw new ApplicationException("One of the cards was not found");
			if (code == "-153") throw new ApplicationException("Transaction not allowed, cards are not same application group");
			if (code == "-154") throw new ApplicationException("Transaction not allowed, same application group but card currencies do not match");
			if (code == "-155") throw new ApplicationException("Failed to assign cardnumber and cardholderid to wallet");
			if (code == "-156") throw new ApplicationException("Applications do not match for the two cards");
			if (code == "-157") throw new ApplicationException("Both Cards are primary or secondary cards");
			if (code == "-158") throw new ApplicationException("Primary Card is not SOLO");
			if (code == "-159") throw new ApplicationException("Amount of secondary cards is already exceeded");
			if (code == "-160") throw new ApplicationException("Failed in linking the cards");
			if (code == "-161") throw new ApplicationException("Error while updating card");
			if (code == "-162") throw new ApplicationException("Secondary Card is already linked to another card");
			if (code == "-163") throw new ApplicationException("Primary Card passed is a secondary card");
			if (code == "-164") throw new ApplicationException("Primary Card and secondary card cannot be the same");
			if (code == "-165") throw new ApplicationException("ValidationOnly format error");
			if (code == "-166") throw new ApplicationException("Error while creating new CardHolderID");
			if (code == "-167") throw new ApplicationException("Card Reissued successfully");
			if (code == "-168") throw new ApplicationException("Primary Card is not KYC");
			if (code == "-169") throw new ApplicationException("Card is already a physical card");
			if (code == "-170") throw new ApplicationException("The old and new card have different bin");
			if (code == "-171") throw new ApplicationException("Secondary Card Status invalid/ Not active");
			if (code == "-172") throw new ApplicationException("Amount cannot be 0");
			if (code == "-173") throw new ApplicationException("This pack is already sold. Cannot Re-sell");
			if (code == "-174") throw new ApplicationException("This pack is not sold. Cannot activate");
			if (code == "-175") throw new ApplicationException("This card has already been reissued before and is waiting to be processed");
			if (code == "-176") throw new ApplicationException("The card must be supplied with a PIN code to be issued");
			if (code == "-177") throw new ApplicationException("The card must NOT be supplied with a PIN code to be issued");
			if (code == " 178") throw new ApplicationException("The Document Expiry Date cannot be less than three months in future from today's date");
			if (code == "-179") throw new ApplicationException("The Document Expiry Date is mandatory");
			if (code == "-180") throw new ApplicationException("The Nationality is mandatory");
			if (code == "-181") throw new ApplicationException("The Country of Issuance is mandatory");
			if (code == "-182") throw new ApplicationException("Nationality should contain only letters");
			if (code == "-183") throw new ApplicationException("The Document Number is mandatory");
			if (code == "-184") throw new ApplicationException("The Document Number is set more than the limit");
			if (code == "-185") throw new ApplicationException("Card Transfer not allowed due card status PNV");
			if (code == "-186") throw new ApplicationException("User details already used");
			if (code == "-187") throw new ApplicationException("Reissue did not complete due card status");
			if (code == "-188") throw new ApplicationException("Card Monthly Limit Reached");
			if (code == "-189") throw new ApplicationException("Card Yearly Limit Reached");
			if (code == "-190") throw new ApplicationException("Mobile Monthly Limit Reached");
			if (code == "-191") throw new ApplicationException("Mobile Yearly Limit Reached");
			if (code == "-192") throw new ApplicationException("Email Monthly Limit Reached");
			if (code == "-193") throw new ApplicationException("Email Yearly Limit Reached");
			if (code == "-194") throw new ApplicationException("Name & DOB Monthly Limit Reached");
			if (code == "-195") throw new ApplicationException(" Name & DOB Yearly Limit Reached");
			if (code == "-196") throw new ApplicationException("Name Monthly Limit Reached");
			if (code == "-197") throw new ApplicationException("Name Yearly Limit Reached");
			if (code == "-198") throw new ApplicationException("Load declined due to AML Validation");
			if (code == "-199") throw new ApplicationException("Country is not allowed");
			if (code == "-200") throw new ApplicationException("CardToCard is not related");
			if (code == "-201") throw new ApplicationException("CardToCard is not allowed for Anonymous Cards");
			if (code == "-202") throw new ApplicationException("Gender Format Error");
			if (code == "-203") throw new ApplicationException("Validate pin tried exceeded");
			if (code == "-204") throw new ApplicationException("Invalid Reason");
			if (code == "-205") throw new ApplicationException("Card Status Does not have permission to execute this transaction");
			if (code == "-206") throw new ApplicationException("Name to be printed on Card too long");
			if (code == "-207") throw new ApplicationException("Card number not found");
			if (code == "-208") throw new ApplicationException("UdfData was not inserted in correct format");
			if (code == "-209") throw new ApplicationException("Card not latest card.");
			if (code == "-210") throw new ApplicationException("Card Style Not Allowed ReIssue");
			if (code == "-211") throw new ApplicationException("Not Enough Funds For ReIssue Fee");
			if (code == "-212") throw new ApplicationException("Failed to transfer details due to the card status");
			if (code == "-213") throw new ApplicationException("Fee Narrative not found in WhiteList");
			if (code == "-214") throw new ApplicationException("Bin is not allowed to use this function");
			if (code == "-215") throw new ApplicationException("Contactless UDF not found");
			if (code == "-219") throw new ApplicationException("Load limit on card was not found");
			if (code == "-220") throw new ApplicationException("Failed to Chane Card Status");
			// New 29/09/2019
			if (code == "-221") throw new ApplicationException("Multi currency cards are not allowed to use function");
			if (code == "-223") throw new ApplicationException("Too many concurrent transactions where requested");
			if (code == "-224") throw new ApplicationException("Bin and distributor code combination is not valid");
			if (code == "-225") throw new ApplicationException("Failed to find any other cards associated");
			if (code == "-226") throw new ApplicationException("Application ID not found for card");
			if (code == "-227") throw new ApplicationException("Configuration to use this feature is missing.");
			if (code == "-0228") throw new ApplicationException("Cardholder Opted Out.");
			if (code == "-0229") throw new ApplicationException("This application is not configured for autorenewability.");
			if (code == "-0230") throw new ApplicationException("Inputted data was in incorrect format.");
			if (code == "-0231") throw new ApplicationException("Could not find cardholder in reissue list.");
			if (code == "-0234") throw new ApplicationException("Cardholder has no autorenewable entries.");
			if (code == "-235") throw new ApplicationException("User already exists in EndUser");
			if (code == "-236") throw new ApplicationException("CardIssue not found");
			if (code == "-237") throw new ApplicationException("Username already in use");
			if (code == "-243") throw new ApplicationException("Narrative is not allowed");
			// Fin New 29/09/2019
			if (code == "-800") throw new ApplicationException("Client wallet not found");
			if (code == "-801") throw new ApplicationException("Failed to retrieve wallet details");
			if (code == "-802") throw new ApplicationException("Failed to retrieve account details");
			if (code == "-0956") throw new ApplicationException("Mobile number is already assigned to the maximum number of cards allowed by the application.");
			if (code == "-0957") throw new ApplicationException("IDNumber / Document Number is already assigned to the maximum number of cards allowed by the application");
			if (code == "0001") throw new ApplicationException("Username or Password wrong");
			if (code == "0002") throw new ApplicationException("Signiture / Data not in correct format");
			if (code == "0003") throw new ApplicationException("Cardnumber not found");
			if (code == "0004") throw new ApplicationException("Card Holder ID not found");
			if (code == "0005") throw new ApplicationException("Connectivity Issue");
			if (code == "0006") throw new ApplicationException("Card Holder ID not registered");
			if (code == "0007") throw new ApplicationException("Insufficent Balance");
			if (code == "0008") throw new ApplicationException("Gift Voucher Phone Number Not Mached");
			if (code == "0009") throw new ApplicationException("Load Permission not granted");
			if (code == "0010") throw new ApplicationException("Generic SMS Error");
			if (code == "0011") throw new ApplicationException("No Load Card Registered");
			if (code == "0012") throw new ApplicationException("Card Status Not Allowed");
			if (code == "0013") throw new ApplicationException("Invalid DOB");
			if (code == "0014") throw new ApplicationException("SMS Configuration Missing.Failed To Send SMS");
			if (code == "0015") throw new ApplicationException("Failed To Send SMS");
			if (code == "0016") throw new ApplicationException("Failed To Receive SMS Successful Status");
			if (code == "0017") throw new ApplicationException("Invalid Mobile Information Entered");
			if (code == "0018") throw new ApplicationException("Ukash transaction not successful");
			if (code == "0019") throw new ApplicationException("Invalid Date");
			if (code == "0020") throw new ApplicationException("Records Not Found");
			if (code == "0021") throw new ApplicationException("Transaction not allowed on specified country");
			if (code == "0022") throw new ApplicationException("Restricted IP");
			if (code == "0023") throw new ApplicationException("MessageID is already used");
			if (code == "0024") throw new ApplicationException("Invalid Security Question");
			if (code == "0025") throw new ApplicationException("Invalid Security Answer");
			if (code == "0030") throw new ApplicationException("Ukash Authentication Failed");
			if (code == "0031") throw new ApplicationException("Cardholder Authentication Failed");
			if (code == "0033") throw new ApplicationException("Connectivity Issue");
			if (code == "0032") throw new ApplicationException("Operation Not Allowed on this Card Status");
			if (code == "0034") throw new ApplicationException("Service(ALS) already enabled");
			if (code == "0035") throw new ApplicationException("Checking ownership has failed");
			if (code == "0036") throw new ApplicationException("Upgrade card did not upgrade successfully");
			if (code == "0037") throw new ApplicationException("Card Link PXSX has failed");
			if (code == "0038") throw new ApplicationException("Incorrect Login Type");
			if (code == "0039") throw new ApplicationException("Login was unsuccessful");
			if (code == "0040") throw new ApplicationException("Secondary Card is not linked to Primary Card");
			if (code == "0041") throw new ApplicationException("An error occured while trying to set up your application");
			if (code == "0042") throw new ApplicationException("An error occured while verifying if you have a card allocated");
			if (code == "0043") throw new ApplicationException("Either MCC or MCCGroup must be provided");
			if (code == "0044") throw new ApplicationException("Invalid Currency");
			if (code == "0045") throw new ApplicationException("No secondary cards found");

			throw new ApplicationException($"Error {code} not documented in PFS API");
		}
		#endregion CheckError

		#region CheckResultAsync
		private async Task<XElement> CheckResultAsync(string resultString, string resultName)
		{
			var element = XElement.Parse(resultString);
			if (element.Name != "AccountAPIv2")
				throw new Exception("PFS service xml response elemnet is not well formatted");

			var errorCode = element.Elements()
				.Where(x => x.Name == "ErrorCode")
				.Select(x => x.Value)
				.FirstOrDefault();
			CheckError(errorCode);

			if (
				(resultName != "DepositToCard") &&
				(resultName != "CardToCard") &&
				(resultName != "UpgradeCard")
			)
				element = element.Elements()
					.Where(x => x.Name == resultName)
					.FirstOrDefault();
			return element;
		}
		#endregion CheckResultAsync

		#region ProcessAsync
		private async Task<XElement> ProcessAsync(string apiSigniture, Guid messageId, string data)
		{
			ServicePointManager.Expect100Continue = true;
			ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;

			var arguments = new
			{
				Username = PfsUser,
				Password = PfsPassword,
				APISigniture = apiSigniture,
				MessageID = messageId.ToString(),
				Data = data
			};
			var contentText = arguments.ToUrlEncoding();

			try
			{
				using (var client = new HttpClient())
				{
					var content = new StringContent(contentText, Encoding.UTF8);
					content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");

					var response = await client.PostAsync(new Uri(PfsUrl), content);

					var resultString = (await response.Content.ReadAsStringAsync());
					var element = XElement.Parse(resultString);
					var result = element.Value;

					var resultXml = await CheckResultAsync(result, apiSigniture);

					await CreateLogAsync(apiSigniture, "", TimeSpan.Zero, null, new { Body = contentText }, resultXml);

					return resultXml;
				}
			}
			catch (Exception ex)
			{
				await CreateLogAsync(apiSigniture, "", TimeSpan.Zero, null, new { Body = contentText }, null, ex);
				throw;
			}
		}
		#endregion ProcessAsync

		#region CheckResultAsync
		private async Task<XElement> CheckTpvResultAsync(string resultString, string resultName)
		{
			var element = XElement.Parse(resultString);
			if (element.Name != "R")
				throw new Exception("PFS tunnel xml response element is not well formatted");

			var errorCode = element.Elements()
				.Where(x => x.Name == "R1")
				.Select(x => x.Value)
				.FirstOrDefault();
			CheckError(errorCode);

			return element;
		}
		#endregion CheckResultAsync
		
		#region TpvProcessAsync
		private async Task<XElement> TpvProcessAsync(string apiSignature, Guid messageId, string cardHolderId, string data)
		{
			ServicePointManager.Expect100Continue = true;
			ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;

			var arguments = new
			{
				Username = PfsUser,
				Password = PfsPassword,
				MessageId = messageId.ToString(),
				APISignature = apiSignature,
				cardHolderId,
				// corporateDc
				isCorporateLoad = false,
				Data = data
			};
			var contentText = arguments.ToUrlEncoding();

			try
			{
				using (var client = new HttpClient())
				{
					var content = new StringContent(contentText, Encoding.UTF8);
					content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");

					var response = await client.PostAsync(new Uri(TpvUrl), content);

					var result = (await response.Content.ReadAsStringAsync());
					var resultXml = await CheckTpvResultAsync(result, apiSignature);
					
					await CreateLogAsync(apiSignature, "", TimeSpan.Zero, null, new { Body = contentText }, resultXml);

					return resultXml;
				}
			}
			catch (Exception ex)
			{
				await CreateLogAsync(apiSignature, "", TimeSpan.Zero, null, new { Body = contentText }, null, ex);
				throw;
			}
		}
		#endregion TpvProcessAsync

		#region GetCardIdAsync
		/// <summary>
		/// Devolver el CardHolder a partir del CardNumber
		/// </summary>
		/// <returns></returns>
		public async Task<PfsServiceGetCardIdResult> GetCardIdAsync(string cardNumber)
		{
			var messageId = Guid.NewGuid();
			var data = new PfsServiceGetCardIdArguments
			{
				CardNumber = cardNumber
			};

			var xml = data.ToXmlString();
			var element = await ProcessAsync("GetCardID", messageId, xml);
			return new PfsServiceGetCardIdResult(element);
		}
		#endregion GetCardIdAsync

		#region CardToCardAsync
		/// <summary>
		/// Data to be supplied should be
		/// </summary>
		/// <returns></returns>
		public async Task<PfsServiceCardToCardResult> CardToCardAsync(
			string cardHolderIdSource, string cardHolderIdTarget, decimal amount,
			JustMoneyCurrencyCode currencyCode,
			string terminalOwner, string terminalLocation, string terminalCity, string terminalState, string terminalId, JustMoneyCountryEnum? country,
			JustMoneyCurrencyCode settlementCurrencyCode
		)
		{
			var messageId = Guid.NewGuid();

			var data = new PfsServiceCardToCardArguments
			{
				CardHolderId = cardHolderIdSource,
				CardHolderIdTo = cardHolderIdTarget,
				Amount = amount,
				CurrencyCode = currencyCode,
				TerminalOwner = terminalOwner,
				TerminalLocation = terminalLocation.Replace(",", ""),
				TerminalCity = terminalCity,
				TerminalState = terminalState,
				TerminalId = 0,
				Country = country,
				Description = "Card to Card Transfer",
				SettlementCurrencyCode = settlementCurrencyCode,
				DirectFee = "**WTR"
			};

			var xml = data.ToXmlString();
			var element = await ProcessAsync("CardToCard", messageId, xml);
			return new PfsServiceCardToCardResult(element);
		}
		#endregion UpdateCardAsync

		#region CardIssueAsync
		/// <summary>
		/// Crear un CardHolder con la información de usuario
		/// </summary>
		/// <returns></returns>
		public async Task<PfsServiceCardIssueResult> CardIssueAsync(
			CardStyle cardStyle, string pin,
			string firstName, string lastName, DateTime birthDay,
			string address, string address2, string zipCode, string city, string state, JustMoneyCountryEnum country,
			string phone, string mobile, string email
		)
		{
			var messageId = Guid.NewGuid();

			var data = new PfsServiceCardIssueArguments
			{
				Subbin = PfsSubbin,
				UserId = PfsUser,
				CardStyle = cardStyle,
				CardType = CardType.Standard,
				Pin = pin,
				IsVirtualCard = false,
				FirstName = firstName,
				LastName = lastName,
				BirthDay = birthDay,
				Address = address,
				Address2 = address2,
				ZipCode = zipCode,
				City = city,
				State = state,
				Country = country,
				Phone = phone,
				Mobile = mobile,
				Email = email
			};

			var xml = data.ToXmlString();
			var element = await ProcessAsync("CardIssue", messageId, xml);
			return new PfsServiceCardIssueResult(element);
		}
		#endregion CardIssueAsync

		#region UpgradeCardAsync
		/// <summary>
		/// Elevar el tiipo de targeta a upgrade
		/// </summary>
		/// <returns></returns>
		public async Task<PfsServiceUpgradeCardResult> UpgradeCardAsync(
			string cardHolderId, CardType cardType
		)
		{
			var messageId = Guid.NewGuid();

			var data = new PfsServiceUpgradeCardArguments
			{
				CardHolderId = cardHolderId,
				CardType = cardType
			};

			var xml = data.ToXmlString();
			var element = await ProcessAsync("UpgradeCard", messageId, xml);
			return new PfsServiceUpgradeCardResult(element);
		}
		#endregion UpgradeCardAsync

		#region UpdateCardAsync
		/// <summary>
		/// Modificar la información del usuario asociada a un CardHolder
		/// </summary>
		/// <returns></returns>
		public async Task<PfsServiceUpdateCardResult> UpdateCardAsync(
			string cardHolderId, CardStyle cardStyle,
			string firstName, string lastName, DateTime birthDay,
			string address, string address2, string zipCode, string city, string state, JustMoneyCountryEnum country,
			string phone, string mobile, string email
		)
		{
			var messageId = Guid.NewGuid();

			var data = new PfsServiceUpdateCardArguments
			{
				CardHolderId = cardHolderId,
				//Subbin = PfsSubbin,
				UserId = PfsUser,
				CardStyle = cardStyle,
				CardType = CardType.Standard,
				IsVirtualCard = false,
				FirstName = firstName,
				LastName = lastName,
				BirthDay = birthDay,
				Address1 = address,
				Address2 = address2,
				ZipCode = zipCode,
				City = city,
				State = state,
				Country = country,
				Phone = phone,
				Mobile = mobile,
				Email = email
			};

			var xml = data.ToXmlString();
			var element = await ProcessAsync("UpdateCard", messageId, xml);
			return new PfsServiceUpdateCardResult(element);
		}
		#endregion UpdateCardAsync

		#region CardInquiryAsync
		/// <summary>
		/// Devolver la información de un CardHolder
		/// </summary>
		/// <returns></returns>
		public async Task<PfsServiceCardInquiryResult> CardInquiryAsync(string cardHolderId)
		{
			var messageId = Guid.NewGuid();

			var data = new PfsServiceCardInquiryArguments
			{
				CardHolderid = cardHolderId
			};

			var xml = data.ToXmlString();
			var element = await ProcessAsync("CardInquiry", messageId, xml);
			return new PfsServiceCardInquiryResult(element);
		}
		#endregion CardInquiryAsync

		#region GetCardBalanceAsync
		/// <summary>
		/// Obtener saldo de un CardHolder
		/// </summary>
		/// <returns></returns>
		public async Task<PfsServiceGetCardBalanceResult> GetCardBalanceAsync(string cardHolder)
		{
			var messageId = Guid.NewGuid();

			var data = new PfsServiceGetCardBalanceArguments
			{
				CardHolderId = cardHolder
			};

			var xml = data.ToXmlString();
			var element = await ProcessAsync("GetCardBalance", messageId, xml);
			return new PfsServiceGetCardBalanceResult(element);
		}
		#endregion GetCardBalanceAsync

		#region ChangeCardStatusAsync
		/// <summary>
		/// Cambiar estado de un CardHolder
		/// </summary>
		/// <returns></returns>
		public async Task<PfsServiceChangeCardStatusResult> ChangeCardStatusAsync(string cardHolderId, CardStatus oldStatus, CardStatus newStatus)
		{
			var messageId = Guid.NewGuid();

			var data = new PfsServiceChangeCardStatusArguments
			{
				CardHolderId = cardHolderId,
				OldStatus = oldStatus,
				NewStatus = newStatus
			};

			var element = await ProcessAsync("ChangeCardStatus", messageId, data.ToXmlString());
			return new PfsServiceChangeCardStatusResult(element);
		}
		#endregion ChangeCardStatusAsync

		#region LockUnlockAsync
		/// <summary>
		/// Method to lock / unlock cards
		/// </summary>
		/// <returns></returns>
		public async Task<PfsServiceLockUnlockResult> LockUnlockAsync(string cardHolderId, CardStatus oldStatus, CardStatus newStatus)
		{
			var messageId = Guid.NewGuid();

			var data = new PfsServiceLockUnlockArguments
			{
				CardHolderId = cardHolderId,
				OldStatus = oldStatus,
				NewStatus = newStatus
			};

			var xml = data.ToXmlString();
			var element = await ProcessAsync("LockUnlock", messageId, xml);
			return new PfsServiceLockUnlockResult(element);
		}
		#endregion LockUnlockAsync

		#region RegisterTokenAsync
		/// <summary>
		/// To be supplied should be Encoded and sent in the following order and should be like shown below
		/// </summary>
		/// <returns></returns>
		public async Task<PfsServiceRegisterTokenResult> RegisterTokenAsync(string reference,
			string firstName, string lastName,
			string address1, string address2, string zipCode, string city, string state, JustMoneyCountryEnum country,
			string successURL, string nonSuccessURL,
			string payload
		)
		{
			var messageId = Guid.NewGuid();

			var data = new PfsServiceRegisterTokenArguments
			{
				MerchantTokenCode = reference,
				FirstName = firstName,
				LastName = lastName,
				Address = address1,
				Address2 = address2,
				ZipCode = zipCode,
				City = city,
				State = state,
				Country = country,
				SuccessUrl = successURL,
				NonSuccessUrl = nonSuccessURL,
				Payload = payload
			};

			var xml = data.ToXmlString();
			var element = await ProcessAsync("registertoken", messageId, xml);
			return new PfsServiceRegisterTokenResult(element);
		}
		#endregion RegisterTokenAsync

		#region ViewStatementAsync
		/// <summary>
		/// Used to view transactions history
		/// </summary>
		/// <returns></returns>
		public async Task<PfsServiceViewStatementResult> ViewStatementAsync(string cardHolderId,
			int startDateYear, int startDateMonth, int startDateDay,
			int endDateYear, int endDateMonth, int endDateDay
		)
		{
			var messageId = Guid.NewGuid();

			var data = new PfsServiceViewStatementArguments
			{
				CardHolderid = cardHolderId,
				StartDateYear = startDateYear,
				StartDateMonth = startDateMonth,
				StartDateDay = startDateDay,
				EndDateYear = endDateYear,
				EndDateMonth = endDateMonth,
				EndDateDay = endDateDay
			};

			var xml = data.ToXmlString();
			var element = await ProcessAsync("ViewStatement", messageId, xml);
			return new PfsServiceViewStatementResult(element);
		}
		#endregion ViewStatementAsync

		#region RegisterToken
		/// <summary>
		/// To be supplied should be Encoded and sent in the following order and should be like shown below
		/// </summary>
		/// <returns></returns>
		public async Task<PfsServiceRegisterTokenResult> RegisterToken(string cardHolderId,
			string merchantTokenCode,
			string firstName, string middleName, string lastName,
			string address, string address2, string zipCode, string city, string state, JustMoneyCountryEnum country,
			string successUrl, string nonSuccessUrl,
			string currency, string payload)
		{
			var messageId = Guid.NewGuid();

			var data = new PfsServiceRegisterTokenArguments
			{
				MerchantTokenCode = merchantTokenCode,
				FirstName = firstName,
				MiddleName = middleName,
				LastName = lastName,
				Address = address,
				Address2 = address2,
				ZipCode = zipCode,
				City = city,
				State = state,
				Country = country,
				SuccessUrl = successUrl,
				NonSuccessUrl = nonSuccessUrl,
				Currency = currency,
				Payload = payload
			};

			var xml = data.ToXmlString();
			var element = await TpvProcessAsync("registertoken", messageId, cardHolderId, xml);
			return new PfsServiceRegisterTokenResult(element);
		}
		#endregion RegisterPayByToken

		#region RegisterPayByToken
		/// <summary>
		/// Data to be supplied should be encoded and sent in the following order
		/// </summary>
		/// <returns></returns>
		public async Task<PfsServiceRegisterPayByTokenResult> RegisterPayByToken(string cardHolderId,
			string firstName, string middleInitial, string lastName,
			string address, string address2, string zipCode, string city, string state, JustMoneyCountryEnum country,
			decimal amount,
			string successUrl, string nonSuccessUrl,
			string currency, string payload)
		{
			var messageId = Guid.NewGuid();

			var data = new PfsServiceRegisterPayByTokenArguments
			{
				MerchantTokenCode = messageId.ToString(),
				FirstName = firstName,
				MiddleInitial = middleInitial,
				LastName = lastName,
				Address = address,
				Address2 = address2,
				ZipCode = zipCode,
				City = city,
				State = state,
				Country = country,
				VendorId = PfsVendorId,
				Amount = amount,
				SuccessUrl = ConfimUrl,
				NonSuccessUrl = ErrorUrl,
				MerchantTransactionCode = messageId.ToString(),
				Currency = currency,
				Payload = payload
			};

			var xml = data.ToXmlString();
			var element = await TpvProcessAsync("registerpaybytoken", messageId, cardHolderId, xml);
			var result = new PfsServiceRegisterPayByTokenResult(element, messageId.ToString());

			return result;
		}
		#endregion RegisterPayByToken

		#region PayByToken
		/// <summary>
		/// Data to be supplied should be Encoded and sent in the following order and should be like shown below
		/// </summary>
		/// <returns></returns>
		public async Task<PfsServicePayByTokenResult> PayByToken(string cardHolderId,
			string merchantTokenCode,
			string vendorId,
			decimal amount,
			string merchantTransactionCode,
			string currency, string payload)
		{
			var messageId = Guid.NewGuid();

			var data = new PfsServicePayByTokenArguments
			{
				MerchantTokenCode = merchantTokenCode,
				VendorId = vendorId,
				Amount = amount,
				MerchantTransactionCode = merchantTransactionCode,
				Currency = currency,
				Payload = payload
			};

			var xml = data.ToXmlString();
			var element = await TpvProcessAsync("paybytoken", messageId, cardHolderId, xml);
			return new PfsServicePayByTokenResult(element);
		}
		#endregion PayByToken

		#region DepositToCard
		/// <summary>
		/// Data to be supplied should be Encoded and sent in the following order and should be like shown below
		/// </summary>
		/// <returns></returns>
		public async Task<PfsServiceDepositToCardResult> DepositToCard(string cardHolderId,
			decimal amount,
			string transactionDescription,
			string terminalLocation,
			string directFee,
			string currencyCode, string settlementCurrencyCode
		)
		{
			var messageId = Guid.NewGuid();

			var data = new PfsServiceDepositToCardArguments
			{
				CardHolderId = cardHolderId,
				//TransactionType = JustMoneyTransactionType.Debit,
				Amount = amount,
				TransactionDescription = transactionDescription,
				TerminalLocation = terminalLocation,
				DirectFee = directFee,
				CurrencyCode = currencyCode,
				SettlementCurrencyCode = settlementCurrencyCode
			};

			var xml = data.ToXmlString();
			var element = await ProcessAsync("DepositToCard", messageId, xml);
			var result = new PfsServiceDepositToCardResult(element);

			return result;
		}
		#endregion DepositToCard
	}
}
