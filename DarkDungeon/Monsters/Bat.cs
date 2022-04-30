namespace Entities;
public partial class Monster
{

    public static StatInfo batStat = new(stat: new(1, 3, 2), energy: 3, killExp: 4, Sight: 1);
    public static MonsterData bat = new(name: "박쥐", 'b', 'd', batStat, (m) => m.BatBehav(), new Item[] { Fightable.batItem });
    public void BatBehav()
    {
        if (Energy.Cur > 0)
        {
            if (_followTarget is null) BasicMovement();
            else
            {
                if (metaData["isAngry"] == 1)
                {
                    _SelectSkill(0, 0);
                    metaData["isAngry"] = 0;
                }  //들이박기
                else if (Energy.Cur <= 1) _SelectSkill(0, 0); //들이박기
                else _SelectSkill(0, 1); //구르기
            }
        }
        else
        {
            SelectBasicBehaviour(1, 0, -1); //pickup offence
        }
        if (_followTarget?.Status.CurrentBehav?.Stance == StanceName.Charge) metaData["isAngry"] = 1;
        else metaData["isAngry"] = 0;
    }

}