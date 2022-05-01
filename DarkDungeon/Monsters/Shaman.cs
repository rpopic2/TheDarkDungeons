namespace Entities;
public class Shaman : Monster, ISpawnable
{
    private static StatInfo stat = new(stat: new(0, 1, 1), energy: 4, killExp: 4, Sight: 3);
    public static MonsterData data = new(name: "정령술사", '}', '{', stat, new Item[] { spiritStaff, torch });

    public Shaman(Position spawnPoint) : base(data, spawnPoint) { }
    
    protected override void OnFollowTarget()
    {
        if (Inven.GetMeta(Fightable.spiritStaff).magicCharge > 0) _SelectSkill(0, 0);
        else _SelectSkill(0, 1);
    }
    protected override void OnEnergyDeplete()
    {
        SelectBasicBehaviour(1, 0, -1); //pickup offence
    }
    protected override void OnNothing()
    {
        BasicMovement();
    }

    public Monster Instantiate(Position spawnPoint) => new Shaman(spawnPoint);
}
