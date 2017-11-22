using OpenNos.DAL.EF;
using OpenNos.Data;

namespace OpenNos.Mapper.Mappers
{
    public class AccountMapper
    {
        public AccountMapper()
        {
        }

        public void ToAccountDTO(Account input, AccountDTO output)
        {
            output.AccountId = input.AccountId;
            output.Authority = input.Authority;
            output.Email = input.Email;
            output.Name = input.Name;
            output.Password = input.Password;
            output.ReferrerId = input.ReferrerId;
            output.RegistrationIP = input.RegistrationIP;
            output.VerificationToken = input.VerificationToken;
        }

        public void ToAccount(AccountDTO input, Account output)
        {
            output.AccountId = input.AccountId;
            output.Authority = input.Authority;
            output.Email = input.Email;
            output.Name = input.Name;
            output.Password = input.Password;
            output.ReferrerId = input.ReferrerId;
            output.RegistrationIP = input.RegistrationIP;
            output.VerificationToken = input.VerificationToken;
        }
    }
}
