namespace Entities;
public class Lunatic : Monster, ISpawnable
{
    private static StatInfo stat = new(stat: new(1, 1, 3), energy: 4, killExp: 3, Sight: 1);
    private static MonsterData data = new(name: "광신도", '>', '<', stat, new Item[] { Fightable.holySword, Fightable.tearOfLun });

    public Lunatic(Position spawnPoint) : base(data, spawnPoint)
    {
    }

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

    public Monster Instantiate(Position spawnPoint)
    {
        return new Lunatic(spawnPoint);
    }
}