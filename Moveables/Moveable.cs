public class Moveable : Fightable
{
    public Position Pos { get; protected set; }

    public Moveable(string name, ClassName className, int cap, int maxHp, int lv, int sol, int lun, int con) : base(name, className, cap, maxHp, lv, sol, lun, con)
    {
        Pos = new Position();
    }
    public virtual void Move(int x)
    {
        _Move(x, out char obj);
    }

    protected virtual bool _Move(int x, out char obj)
    {
        Map current = Map.Current;
        Position newPos = Pos + x;
        bool existsTile = current.Tiles.TryGet(newPos.x, out obj);
        bool obstructed = current.Moveables.TryGet(newPos.x, out Moveable? mov);
        bool result = existsTile && !obstructed;
        if (result)
        {
            stance = (Stance.Move, stance.amount + x);
            Pos = newPos;
            current.UpdateMoveable(this);
        }
        else
        {
            if (newPos.facing != Pos.facing)
            {
                stance = (Stance.Move, stance.amount);
                Pos = !Pos;
                return true;
            }
        }
        return result;
    }

    public override void OnTurnEnd()
    {
        base.OnTurnEnd();
        UpdateTarget();
    }

    protected override void OnDeath(object? sender, EventArgs e)
    {
        base.OnDeath(sender, e);
        Map.Current.UpdateMoveable(this);
        Map.Current.UpdateMoveable(this);
    }
    public void UpdateTarget()
    {
        Map.Current.Moveables.TryGet(Pos.Front, out Moveable? mov);
        Target = mov;
    }
    // public override Moveable? Target
    // {
    //     get
    //     {
    //         Map.Current.Moveables.TryGet(Pos.Front, out Moveable? mov);
    //         return mov;
    //     }
    // }

}