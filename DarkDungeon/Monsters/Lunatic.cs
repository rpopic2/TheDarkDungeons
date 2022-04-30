namespace Entities;
public partial class Monster
{
    private static StatInfo lunaticStat = new(stat: new(1, 1, 3), energy: 4, killExp: 3, Sight: 1);
    public static MonsterData lunatic = new(name: "광신도", '>', '<', lunaticStat, new Item[] { holySword, tearOfLun });
    public void LunaticBehav()
    {
        if (GetHp().Cur != GetHp().Max && Inven.Content.Contains(Fightable.tearOfLun)) _SelectSkill(1, 0);
        else if (Energy.Cur > 0)
        {
            if (_followTarget is null) BasicMovement();
            else
            {
                if (Inven.GetMeta(Fightable.holySword).magicCharge > 0) _SelectSkill(0, 0);
                else _SelectSkill(0, 1);
            }
        }
        else SelectBasicBehaviour(1, 0, -1); //pickup offence
    }
}