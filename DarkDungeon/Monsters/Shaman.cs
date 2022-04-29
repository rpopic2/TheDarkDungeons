namespace Entities;
public partial class Monster
{
    private static StatInfo shamanStat = new(sol: 0, lun: 1, con: 1, energy: 4, killExp: 4, Sight: 3);
    public static MonsterData shaman = new(name: "정령술사", '}', '{', shamanStat, (m) => m.ShamanBehav(), new Item[] { spiritStaff, torch });

    private void ShamanBehav()
    {
        if (Energy.Cur <= 0)
        {
            SelectBasicBehaviour(1, 0, -1); //pickup offence
            return;
        }
        else if (_followTarget is null)
        {
            BasicMovement();
            return;
        }
        else if (Inven.GetMeta(Fightable.spiritStaff).magicCharge > 0) _SelectSkill(0, 0);
        else _SelectSkill(0, 1);
    }
}
