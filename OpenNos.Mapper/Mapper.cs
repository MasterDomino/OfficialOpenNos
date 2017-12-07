using OpenNos.Mapper.Mappers;

namespace OpenNos.Mapper
{
    public class Mapper
    {
        #region Members

        private static Mapper _instance;

        #endregion

        #region Instantiation

        public Mapper()
        {
            AccountMapper = new AccountMapper();
            BazaarItemMapper = new BazaarItemMapper();
            BCardMapper = new BCardMapper();
            BoxItemMapper = new ItemInstanceMapper();
            CardMapper = new CardMapper();
            CellonOptionMapper = new CellonOptionMapper();
            CharacterMapper = new CharacterMapper();
            CharacterRelationMapper = new CharacterRelationMapper();
            CharacterSkillMapper = new CharacterSkillMapper();
            ComboMapper = new ComboMapper();
            DropMapper = new DropMapper();
            FamilyCharacterMapper = new FamilyCharacterMapper();
            FamilyLogMapper = new FamilyLogMapper();
            FamilyMapper = new FamilyMapper();
            GeneralLogMapper = new GeneralLogMapper();
            ItemInstanceMapper = new ItemInstanceMapper();
            ItemMapper = new ItemMapper();
            MailMapper = new MailMapper();
            MaintenanceLogMapper = new MaintenanceLogMapper();
            MapMapper = new MapMapper();
            MapMonsterMapper = new MapMonsterMapper();
            MapNPCMapper = new MapNPCMapper();
            MapTypeMapMapper = new MapTypeMapMapper();
            MapTypeMapper = new MapTypeMapper();
            MateMapper = new MateMapper();
            MinigameLogMapper = new MinigameLogMapper();
            MinilandObjectMapper = new MinilandObjectMapper();
            NpcMonsterMapper = new NpcMonsterMapper();
            NpcMonsterSkillMapper = new NpcMonsterSkillMapper();
            PenaltyLogMapper = new PenaltyLogMapper();
            PortalMapper = new PortalMapper();
            QuestMapper = new QuestMapper();
            QuestProgressMapper = new QuestProgressMapper();
            QuicklistEntryMapper = new QuicklistEntryMapper();
            RecipeItemMapper = new RecipeItemMapper();
            RecipeListMapper = new RecipeListMapper();
            RecipeMapper = new RecipeMapper();
            RespawnMapper = new RespawnMapper();
            RespawnMapTypeMapper = new RespawnMapTypeMapper();
            RollGeneratedItemMapper = new RollGeneratedItemMapper();
            ScriptedInstanceMapper = new ScriptedInstanceMapper();
            ShellEffectMapper = new ShellEffectMapper();
            ShopItemMapper = new ShopItemMapper();
            ShopMapper = new ShopMapper();
            ShopSkillMapper = new ShopSkillMapper();
            SkillMapper = new SkillMapper();
            StaticBonusMapper = new StaticBonusMapper();
            StaticBuffMapper = new StaticBuffMapper();
            TeleporterMapper = new TeleporterMapper();
        }

        #endregion

        #region Properties

        public static Mapper Instance => _instance ?? (_instance = new Mapper());

        public AccountMapper AccountMapper { get; }

        public BazaarItemMapper BazaarItemMapper { get; }

        public BCardMapper BCardMapper { get; }

        public ItemInstanceMapper BoxItemMapper { get; }

        public CardMapper CardMapper { get; }

        public CellonOptionMapper CellonOptionMapper { get; }

        public CharacterMapper CharacterMapper { get; }

        public CharacterRelationMapper CharacterRelationMapper { get; }

        public CharacterSkillMapper CharacterSkillMapper { get; }

        public ComboMapper ComboMapper { get; }

        public DropMapper DropMapper { get; }

        public FamilyCharacterMapper FamilyCharacterMapper { get; }

        public FamilyLogMapper FamilyLogMapper { get; }

        public FamilyMapper FamilyMapper { get; }

        public GeneralLogMapper GeneralLogMapper { get; }

        public ItemInstanceMapper ItemInstanceMapper { get; }

        public ItemMapper ItemMapper { get; }

        public MailMapper MailMapper { get; }

        public MaintenanceLogMapper MaintenanceLogMapper { get; }

        public MapMapper MapMapper { get; }

        public MapMonsterMapper MapMonsterMapper { get; }

        public MapNPCMapper MapNPCMapper { get; }

        public MapTypeMapMapper MapTypeMapMapper { get; }

        public MapTypeMapper MapTypeMapper { get; }

        public MateMapper MateMapper { get; }

        public MinigameLogMapper MinigameLogMapper { get; }

        public MinilandObjectMapper MinilandObjectMapper { get; }

        public NpcMonsterMapper NpcMonsterMapper { get; }

        public NpcMonsterSkillMapper NpcMonsterSkillMapper { get; }

        public PenaltyLogMapper PenaltyLogMapper { get; }

        public PortalMapper PortalMapper { get; }

        public QuestMapper QuestMapper { get; }

        public QuestProgressMapper QuestProgressMapper { get; }

        public QuicklistEntryMapper QuicklistEntryMapper { get; }

        public RecipeItemMapper RecipeItemMapper { get; }

        public RecipeListMapper RecipeListMapper { get; }

        public RecipeMapper RecipeMapper { get; }

        public RespawnMapper RespawnMapper { get; }

        public RespawnMapTypeMapper RespawnMapTypeMapper { get; }

        public RollGeneratedItemMapper RollGeneratedItemMapper { get; }

        public ScriptedInstanceMapper ScriptedInstanceMapper { get; }

        public ShellEffectMapper ShellEffectMapper { get; }

        public ShopItemMapper ShopItemMapper { get; }

        public ShopMapper ShopMapper { get; }

        public ShopSkillMapper ShopSkillMapper { get; }

        public SkillMapper SkillMapper { get; }

        public StaticBonusMapper StaticBonusMapper { get; }

        public StaticBuffMapper StaticBuffMapper { get; }

        public TeleporterMapper TeleporterMapper { get; }

        #endregion
    }
}