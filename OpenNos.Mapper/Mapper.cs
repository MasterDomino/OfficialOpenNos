using OpenNos.Mapper.Mappers;
namespace OpenNos.Mapper
{
    public class Mapper
    {
        private AccountMapper _accountMapper;

        private CharacterMapper _characterMapper;

        private PenaltyLogMapper _penaltyLogMapper;

        private static Mapper _instance;

        public Mapper()
        {
            _accountMapper = new AccountMapper();
            _characterMapper = new CharacterMapper();
            _penaltyLogMapper = new PenaltyLogMapper();
        }

        public static Mapper Instance => _instance ?? (_instance = new Mapper());

        public AccountMapper AccountMapper { get { return _accountMapper; } }

        public CharacterMapper CharacterMapper { get { return _characterMapper; } }
    }
}
