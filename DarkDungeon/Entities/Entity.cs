namespace Entities;
public class Entity
{
    public int Level { get; protected set; }
    public readonly string Name;
    public Entity(int level, string name)
    {
        this.Level = level;

        Name = name;
    }
}