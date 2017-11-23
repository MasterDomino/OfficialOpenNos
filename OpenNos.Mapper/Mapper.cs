using OpenNos.Data;
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

        private NpcMonsterMapper _npcMonsterMapper;

        private NpcMonsterSkillMapper _npcMonsterSkillMapper;

        private PenaltyLogMapper _penaltyLogMapper;

        private PortalMapper _portalMapper;

        private QuestMapper _questMapper;

        private QuestProgressMapper _questProgressMapper;

        private QuicklistEntryMapper _quicklistEntryMapper;

        private RecipeItemMapper _recipeItemMapper;

        private RecipeListMapper _recipeListMapper;

        private RecipeMapper _recipeMapper;

        private RespawnMapper _respawnMapper;

        private RespawnMapTypeMapper _respawnMapTypeMapper;

        private RollGeneratedItemMapper _rollGeneratedItemMapper;

        private ScriptedInstanceMapper _scriptedInstanceMapper;

        private ShellEffectMapper _shellEffectMapper;

        private ShopItemMapper _shopItemMapper;

        private ShopMapper _shopMapper;

        private ShopSkillMapper _shopSkillMapper;

        private SkillMapper _skillMapper;

        private SpecialistInstanceMapper _specialistInstanceMapper;

        private StaticBonusMapper _staticBonusMapper;

        private StaticBuffMapper _staticBuffMapper;

        private TeleporterMapper _teleporterMapper;

        private UsableInstanceMapper _usableInstanceMapper;

        private WearableInstanceMapper _wearableInstanceMapper;

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
            _npcMonsterMapper = new NpcMonsterMapper();
            _npcMonsterSkillMapper = new NpcMonsterSkillMapper();
            _penaltyLogMapper = new PenaltyLogMapper();
            _portalMapper = new PortalMapper();
            _questMapper = new QuestMapper();
            _questProgressMapper = new QuestProgressMapper();
            _quicklistEntryMapper = new QuicklistEntryMapper();
            _recipeItemMapper = new RecipeItemMapper();
            _recipeListMapper = new RecipeListMapper();
            _recipeMapper = new RecipeMapper();
            _respawnMapper = new RespawnMapper();
            _respawnMapTypeMapper = new RespawnMapTypeMapper();
            _rollGeneratedItemMapper = new RollGeneratedItemMapper();
            _scriptedInstanceMapper = new ScriptedInstanceMapper();
            _shellEffectMapper = new ShellEffectMapper();
            _shopItemMapper = new ShopItemMapper();
            _shopMapper = new ShopMapper();
            _shopSkillMapper = new ShopSkillMapper();
            _skillMapper = new SkillMapper();
            _specialistInstanceMapper = new SpecialistInstanceMapper();
            _staticBonusMapper = new StaticBonusMapper();
            _staticBuffMapper = new StaticBuffMapper();
            _teleporterMapper = new TeleporterMapper();
            _usableInstanceMapper = new UsableInstanceMapper();
            _wearableInstanceMapper = new WearableInstanceMapper();
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

        public NpcMonsterMapper NpcMonsterMapper { get { return _npcMonsterMapper; } }

        public NpcMonsterSkillMapper NpcMonsterSkillMapper { get { return _npcMonsterSkillMapper; } }

        public PenaltyLogMapper PenaltyLogMapper { get { return _penaltyLogMapper; } }

        public PortalMapper PortalMapper { get { return _portalMapper; } }

        public QuestMapper QuestMapper { get { return _questMapper; } }

        public QuestProgressMapper QuestProgressMapper { get { return _questProgressMapper; } }

        public QuicklistEntryMapper QuicklistEntryMapper { get { return _quicklistEntryMapper; } }

        public RecipeItemMapper RecipeItemMapper { get { return _recipeItemMapper; } }

        public RecipeListMapper RecipeListMapper { get { return _recipeListMapper; } }

        public RecipeMapper RecipeMapper { get { return _recipeMapper; } }

        public RespawnMapper RespawnMapper { get { return _respawnMapper; } }

        public RespawnMapTypeMapper RespawnMapTypeMapper { get { return _respawnMapTypeMapper; } }

        public RollGeneratedItemMapper RollGeneratedItemMapper { get { return _rollGeneratedItemMapper; } }

        public ScriptedInstanceMapper ScriptedInstanceMapper { get { return _scriptedInstanceMapper; } }

        public ShellEffectMapper ShellEffectMapper { get { return _shellEffectMapper; } }

        public ShopItemMapper ShopItemMapper { get { return _shopItemMapper; } }

        public ShopMapper ShopMapper { get { return _shopMapper; } }

        public ShopSkillMapper ShopSkillMapper { get { return _shopSkillMapper; } }

        public SkillMapper SkillMapper { get { return _skillMapper; } }

        public SpecialistInstanceMapper SpecialistInstanceMapper { get { return _specialistInstanceMapper; } }

        public StaticBonusMapper StaticBonusMapper { get { return _staticBonusMapper; } }

        public StaticBuffMapper StaticBuffMapper { get { return _staticBuffMapper; } }

        public TeleporterMapper TeleporterMapper { get { return _teleporterMapper; } }

        public UsableInstanceMapper UsableInstanceMapper { get { return _usableInstanceMapper; } }

        public WearableInstanceMapper WearableInstanceMapper { get { return _wearableInstanceMapper; } }
    }
}
