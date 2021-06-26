using PayIn.Common;

namespace PayIn.BusinessLogic.Common
{
    public interface ISessionData
    {
		string		ApplicationName	{ get; }
		string		Login           { get; }
		string		Name            { get; }
		string		Email           { get; }
		string[]	Roles           { get; }
		string  	ClientId        { get; }
		string		TaxNumber       { get; }
		string		TaxName         { get; }
		string		TaxAddress      { get; }
		string      Token           { get; }
        LanguageEnum?    Language        { get; }
    }
}
