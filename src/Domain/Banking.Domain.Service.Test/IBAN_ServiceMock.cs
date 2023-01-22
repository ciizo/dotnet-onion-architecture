using Banking.Domain.Service.AccountLogic;

namespace Banking.Domain.Service.Test
{
    public class IBAN_ServiceMock : IIBAN_Service
    {
        public async Task<string> GenerateIBAN()
        {
            return "NL05INGB7925653426";
        }
    }
}