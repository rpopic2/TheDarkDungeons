public class Shaman : Monster, ISpawnable
{
    private static StatInfo stat = new(stat: new(sol:1, lun:1, con:3), energy: 4, killExp: 4);
    public readonly static MonsterData data = new(name: "정령술사", '}', '{', stat, new ItemOld[] { spiritStaff, torch, tearOfLun });

    public Shaman(Position spawnPoint) : base(data, spawnPoint) { }
    
    protected override void OnTarget()
    {
        if (Inven.GetMeta(Creature.spiritStaff)!.magicCharge > 0) _SelectSkill(0, 0);
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
