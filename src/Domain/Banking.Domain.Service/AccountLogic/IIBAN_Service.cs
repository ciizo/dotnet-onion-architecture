namespace Banking.Domain.Service.AccountLogic
{
    public interface IIBAN_Service
    {
        Task<string> GenerateIBAN();
    }
}