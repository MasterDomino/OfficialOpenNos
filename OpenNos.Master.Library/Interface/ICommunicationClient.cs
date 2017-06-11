using OpenNos.Master.Library.Data;

namespace OpenNos.Master.Library.Interface
{
    public interface ICommunicationClient
    {
        #region Methods

        void CharacterConnected(long characterId);

        void CharacterDisconnected(long characterId);

        void KickSession(long? accountId, int? sessionId);

        void SendMessageToCharacter(SCSCharacterMessage message);

        void Shutdown();

        void UpdateBazaar(long bazaarItemId);

        void UpdateFamily(long familyId);

        void UpdatePenaltyLog(int penaltyLogId);

        void UpdateRelation(long relationId);

        #endregion
    }
}