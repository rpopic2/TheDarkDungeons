namespace Entities;
public class Lunatic : Monster, ISpawnable
{
    private static StatInfo stat = new(stat: new(1, 1, 3), energy: 4, killExp: 3);
    private static MonsterData data = new(name: "광신도", '>', '<', stat, new Item[] { Fightable.holySword, Fightable.tearOfLun });

    public Lunatic(Position spawnPoint) : base(data, spawnPoint) { }

    public Monster Instantiate(Position spawnPoint)
    {
        return new Lunatic(spawnPoint);
    }
    protected override void OnEnergyDeplete()
    {
        SelectBasicBehaviour(1, 0, -1); //pickup offence
    }
    protected override void OnTarget()
    {
        if (Inven.GetMeta(Fightable.holySword).magicCharge > 0) _SelectSkill(0, 0);
        else _SelectSkill(0, 1);
    }
}