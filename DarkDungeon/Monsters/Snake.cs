namespace Entities;
public partial class Monster
{
    private static StatInfo snakeStat = new(sol: 1, lun: 3, con: 1, energy: 3, killExp: 5, Sight: 3);
    public static MonsterData snake = new(name: "뱀", 'S', '2', snakeStat, (m) => m.SnakeBehav(), new Item[] { snakeItem, poison });

    public void SnakeBehav()
    {
        if (Energy.Cur > 0)
        {
            if (_followTarget is null) BasicMovement();
            else
            {
                if (!metaData.ContainsKey("hissed"))
                {
                    _SelectSkill(0, 0); //hiss
                    metaData["hissed"] = 1;
                    return;
                }
                else if (_followTarget.Pos.Distance(Pos) <= 1 && Inven.GetMeta(Fightable.snakeItem).poison > 0)
                {
                    _SelectSkill(0, 1); //bite
                }
                else
                {
                    Facing newFacing = Pos.LookAt(_followTarget.Pos.x);
                    SelectBasicBehaviour(0, 1, (int)newFacing); //move
                }
            }
        }
        else SelectBasicBehaviour(1, 0, -1); //pickup offence
    }
}