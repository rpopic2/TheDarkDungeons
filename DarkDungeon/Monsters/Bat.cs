namespace Entities;
public class Bat : Monster
{
    public static StatInfo batStat = new(stat: new(1, 3, 2), energy: 3, killExp: 4, Sight: 1);
    public static MonsterData data = new(name: "박쥐", 'b', 'd', batStat, new Item[] { Fightable.batItem });

    public Bat(Position spawnPoint) : base(data, spawnPoint) { metaData.Add("isAngry", 0); }
    protected override void OnEnergyDeplete()
    {
        SelectBasicBehaviour(1, 0, -1); //pickup offence
    }
    protected override void OnFollowTarget()
    {
        if (metaData["isAngry"] == 1)
        {
            _SelectSkill(0, 0);
            metaData["isAngry"] = 0;
        }  //들이박기
        else if (Energy.Cur <= 1) _SelectSkill(0, 0); //들이박기
        else _SelectSkill(0, 1); //구르기
    }
    protected override void OnNothing()
    {
        BasicMovement();
    }
}