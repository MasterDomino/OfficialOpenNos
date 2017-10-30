using OpenNos.DAL.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenNos.Data;
using OpenNos.Data.Enums;

namespace OpenNos.DAL.Mock
{
    public class QuestProgressDAO : IQuestProgressDAO
    {
        public DeleteResult DeleteById(long id)
        {
            throw new NotImplementedException();
        }

        public void InitializeMapper()
        {
            throw new NotImplementedException();
        }

        public QuestProgressDTO InsertOrUpdate(QuestProgressDTO questProgress)
        {
            throw new NotImplementedException();
        }

        public void InsertOrUpdateFromList(List<QuestProgressDTO> questProgessList)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<QuestProgressDTO> LoadByCharacterId(long characterId)
        {
            throw new NotImplementedException();
        }

        public QuestProgressDTO LoadById(long id)
        {
            throw new NotImplementedException();
        }

        public IMappingBaseDAO RegisterMapping(Type gameObjectType)
        {
            throw new NotImplementedException();
        }
    }
}
