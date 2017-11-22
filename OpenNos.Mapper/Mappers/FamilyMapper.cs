using OpenNos.DAL.EF;
using OpenNos.Data;

namespace OpenNos.Mapper.Mappers
{
    public class FamilyMapper
    {
        public FamilyMapper()
        {

        }

        public void ToFamilyDTO(Family input, FamilyDTO output)
        {
            output.FamilyExperience = input.FamilyExperience;
            output.FamilyHeadGender = input.FamilyHeadGender;
            output.FamilyId = input.FamilyId;
            output.FamilyLevel = input.FamilyLevel;
            output.FamilyMessage = input.FamilyMessage;
            output.ManagerAuthorityType = input.ManagerAuthorityType;
            output.ManagerCanGetHistory = input.ManagerCanGetHistory;
            output.ManagerCanInvite = input.ManagerCanInvite;
            output.ManagerCanNotice = input.ManagerCanNotice;
            output.ManagerCanShout = input.ManagerCanShout;
            output.MaxSize = input.MaxSize;
            output.MemberAuthorityType = input.MemberAuthorityType;
            output.MemberCanGetHistory = input.MemberCanGetHistory;
            output.Name = input.Name;
            output.WarehouseSize = input.WarehouseSize;
        }

        public void ToFamily(FamilyDTO input, Family output)
        {
            output.FamilyExperience = input.FamilyExperience;
            output.FamilyHeadGender = input.FamilyHeadGender;
            output.FamilyId = input.FamilyId;
            output.FamilyLevel = input.FamilyLevel;
            output.FamilyMessage = input.FamilyMessage;
            output.ManagerAuthorityType = input.ManagerAuthorityType;
            output.ManagerCanGetHistory = input.ManagerCanGetHistory;
            output.ManagerCanInvite = input.ManagerCanInvite;
            output.ManagerCanNotice = input.ManagerCanNotice;
            output.ManagerCanShout = input.ManagerCanShout;
            output.MaxSize = input.MaxSize;
            output.MemberAuthorityType = input.MemberAuthorityType;
            output.MemberCanGetHistory = input.MemberCanGetHistory;
            output.Name = input.Name;
            output.WarehouseSize = input.WarehouseSize;
        }
    }
}
