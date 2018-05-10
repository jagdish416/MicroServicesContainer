namespace DAL.Interfaces
{
    public interface IDSecurity
    {
        bool AuthenticateClient(string pstrAPIKey, string pstrReferrer, string pstrController, string pstrAction);
        //string LogRequest(string pstrLog);
    }
}