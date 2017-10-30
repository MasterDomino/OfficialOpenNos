using OpenNos.XMLModel.Events;
using OpenNos.XMLModel.Objects.Quest;
using System;
using System.Xml.Serialization;

namespace OpenNos.XMLModel.Models.Quest
{
    [XmlRoot("Definition"), Serializable]
    public class QuestModel
    {
        public QuestGiver QuestGiver { get; set; }

        public NpcDialog Dialog { get; set; }

        public Reward Reward { get; set; }

        public KillObjective[] KillObjectives { get; set; }

        public LootObjective[] LootObjectives { get; set; }

        public WalkObjective WalkObjective { get; set; }
    }
}