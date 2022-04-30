namespace Entities
{
    internal interface ISpawnable
    {
        Monster Instantiate(Position spawnPoint);
    }
}