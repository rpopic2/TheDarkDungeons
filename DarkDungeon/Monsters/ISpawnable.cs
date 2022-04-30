namespace Entities
{
    public interface ISpawnable
    {
        Monster Instantiate(Position spawnPoint);
    }
}