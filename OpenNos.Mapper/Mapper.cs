using OpenNos.Mapper.Mappers;
namespace OpenNos.Mapper
{
    public class Mapper
    {
        private AccountMapper _accountMapper;

        private BazaarItemMapper _bazaarItemMapper;

        private BCardMapper _bCardMapper;

        private BoxItemMapper _boxItemMapper;

        private CardMapper _cardMapper;

        private CellonOptionMapper _cellonOptionMapper;

        private CharacterMapper _characterMapper;

        private CharacterRelationMapper _characterRelationMapper;

        private CharacterSkillMapper _characterSkillMapper;

        private ComboMapper _comboMapper;

        private DropMapper _dropMapper;

        private FamilyCharacterMapper _familyCharacterMapper;

        private FamilyLogMapper _familyLogMapper;

        private FamilyMapper _familyMapper;

        private GeneralLogMapper _generalLogMapper;

        private ItemInstanceMapper _itemInstanceMapper;

        private ItemMapper _itemMapper;

        private MailMapper _mailMapper;

        private MaintenanceLogMapper _maintenanceLogMapper;

        private MapMapper _mapMapper;

        private MapMonsterMapper _mapMonsterMapper;

        private MapNPCMapper _mapNPCMapper;

        private MapTypeMapMapper _mapTypeMapMapper;

        private MapTypeMapper _mapTypeMapper;

        private MateMapper _mateMapper;

        private MinilandObjectMapper _minilandObjectMapper;

        private PenaltyLogMapper _penaltyLogMapper;

        private static Mapper _instance;

        public Mapper()
        {
            _accountMapper = new AccountMapper();
            _bazaarItemMapper = new BazaarItemMapper();
            _bCardMapper = new BCardMapper();
            _boxItemMapper = new BoxItemMapper();
            _cardMapper = new CardMapper();
            _cellonOptionMapper = new CellonOptionMapper();
            _characterMapper = new CharacterMapper();
            _characterRelationMapper = new CharacterRelationMapper();
            _characterSkillMapper = new CharacterSkillMapper();
            _comboMapper = new ComboMapper();
            _dropMapper = new DropMapper();
            _familyCharacterMapper = new FamilyCharacterMapper();
            _familyLogMapper = new FamilyLogMapper();
            _familyMapper = new FamilyMapper();
            _generalLogMapper = new GeneralLogMapper();
            _itemInstanceMapper = new ItemInstanceMapper();
            _itemMapper = new ItemMapper();
            _mailMapper = new MailMapper();
            _maintenanceLogMapper = new MaintenanceLogMapper();
            _mapMapper = new MapMapper();
            _mapMonsterMapper = new MapMonsterMapper();
            _mapNPCMapper = new MapNPCMapper();
            _mapTypeMapMapper = new MapTypeMapMapper();
            _mapTypeMapper = new MapTypeMapper();
            _mateMapper = new MateMapper();
            _minilandObjectMapper = new MinilandObjectMapper();
            _penaltyLogMapper = new PenaltyLogMapper();
        }

        public static Mapper Instance => _instance ?? (_instance = new Mapper());

        public AccountMapper AccountMapper { get { return _accountMapper; } }

        public BazaarItemMapper BazaarItemMapper { get { return _bazaarItemMapper; } }

        public BCardMapper BCardMapper { get { return _bCardMapper; } }

        public BoxItemMapper BoxItemMapper { get { return _boxItemMapper; } }

        public CardMapper CardMapper { get { return _cardMapper; } }

        public CellonOptionMapper CellonOptionMapper { get { return _cellonOptionMapper; } }

        public CharacterMapper CharacterMapper { get { return _characterMapper; } }

        public CharacterRelationMapper CharacterRelationMapper { get { return _characterRelationMapper; } }

        public CharacterSkillMapper CharacterSkillMapper { get { return _characterSkillMapper; } }

        public ComboMapper ComboMapper { get { return _comboMapper; } }

        public DropMapper DropMapper { get { return _dropMapper; } }

        public FamilyCharacterMapper FamilyCharacterMapper { get { return _familyCharacterMapper; } }

        public FamilyLogMapper FamilyLogMapper { get { return _familyLogMapper; } }

        public FamilyMapper FamilyMapper { get { return _familyMapper; } }

        public GeneralLogMapper GeneralLogMapper { get { return _generalLogMapper; } }

        public ItemInstanceMapper ItemInstanceMapper { get { return _itemInstanceMapper; } }

        public ItemMapper ItemMapper { get { return _itemMapper; } }

        public MailMapper MailMapper { get { return _mailMapper; } }

        public MaintenanceLogMapper MaintenanceLogMapper { get { return _maintenanceLogMapper; } }

        public MapMapper MapMapper { get { return _mapMapper; } }

        public MapMonsterMapper MapMonsterMapper { get { return _mapMonsterMapper; } }

        public MapNPCMapper MapNPCMapper { get { return _mapNPCMapper; } }

        public MapTypeMapMapper MapTypeMapMapper { get { return _mapTypeMapMapper; } }

        public MapTypeMapper MapTypeMapper { get { return _mapTypeMapper; } }

        public MateMapper MateMapper { get { return _mateMapper; } }

        public MinilandObjectMapper MinilandObjectMapper { get { return _minilandObjectMapper; } }

        public PenaltyLogMapper PenaltyLogMapper { get { return _penaltyLogMapper; } }
    }
}
