public class Bat : Monster, ISpawnable
{
    private static StatInfo stat = new(stat: new(1, 3, 2), energy: 3, killExp: 4);
    private static MonsterData data = new(name: "박쥐", 'b', 'd', stat, new Item[] { Creature.batItem });
    public Monster Instantiate(Position spawnPoint) => new Bat(spawnPoint);
    private bool isAngry;

    public Bat(Position spawnPoint) : base(data, spawnPoint)
    {
    }
    protected override void OnEnergyDeplete()
    {
        SelectBasicBehaviour(1, 0, -1); //pickup offence
    }
    protected override void OnTarget()
    {
        if (DistanceToTarget > 1)
        {
            FollowTarget();
            return;
        }
        if (isAngry)
        {
            _SelectSkill(0, 0);
            isAngry = false;
        }  //들이박기
        else if (Energy.Cur <= 1) _SelectSkill(0, 0); //들이박기
        else _SelectSkill(0, 1); //구르기
        if (_target!.CurAction.CurrentBehav?.Stance == StanceName.Charge) isAngry = true;
    }
}