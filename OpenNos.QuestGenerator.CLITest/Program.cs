using OpenNos.XMLModel.Events.Quest;
using OpenNos.XMLModel.Models.Quest;
using OpenNos.XMLModel.Objects;
using OpenNos.XMLModel.Objects.Quest;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace OpenNos.QuestGenerator.CLITest
{
    class Program
    {
        static void Main(string[] args)
        {
            QuestModel questModel = new QuestModel();

            questModel.QuestGiver = new QuestGiver()
            {
                Type = Domain.QuestGiverType.InitialQuest,
                QuestGiverId = -1,
                MinimumLevel = 1,
                MaximumLevel = 255
            };

            questModel.WalkObjective = new WalkObjective()
            {
                MapId = 1,
                MapX = 57,
                MapY = 149
            };

            questModel.Reward = new Reward()
            {
                QuestId = 2,
            };

            using (StreamWriter sw = new StreamWriter("output.xml"))
            {
                new XmlSerializer(typeof(QuestModel)).Serialize(sw, questModel);
                sw.Flush();
            }


            questModel = new QuestModel();

            questModel.KillObjectives = new KillObjective[] { new KillObjective()
            {
                MonsterVNum=24,
                GoalAmount=5
            }};

            questModel.Reward = new Reward()
            {
                QuestId = -1,
                ForceLevelUp = 15,
                ForceJobUp = 15,
                Script = new Script()
                {
                    Type = 1,
                    Value = 52
                },
                GiftItems = new Item[]
                {
                    new Item()
                    {
                        VNum=2339,
                        Amount=5
                    }
                }

            };

            using (StreamWriter sw = new StreamWriter("output2.xml"))
            {
                new XmlSerializer(typeof(QuestModel)).Serialize(sw, questModel);
                sw.Flush();
            }



        }
    }
}
