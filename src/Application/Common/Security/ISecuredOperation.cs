namespace Application.Common.Security;

public interface ISecuredRequest
{
    public string[] Roles { get; }
}