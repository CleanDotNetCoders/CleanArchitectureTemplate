namespace Domain.Entities.Common;

public class Entity
{
    public string Id { get; set; }

    public Entity()
    {
    }

    public Entity(string id) : this()
    {
        Id = id;
    }
}