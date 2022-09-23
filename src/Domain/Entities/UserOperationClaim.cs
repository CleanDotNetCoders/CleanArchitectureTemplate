using Domain.Entities.Common;

namespace Domain.Entities;

public class UserOperationClaim : Entity
{
    public string UserId { get; set; }
    public string OperationClaimId { get; set; }

    public virtual User User { get; set; }
    public virtual OperationClaim OperationClaim { get; set; }

    public UserOperationClaim()
    {
    }

    public UserOperationClaim(string id, string userId, string operationClaimId) : base(id)
    {
        UserId = userId;
        OperationClaimId = operationClaimId;
    }
}