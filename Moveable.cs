public class Moveable : Entity
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
        bool success = current.Tiles.TryGet(newPos.x, out obj);
        bool fail = current.Moveables.TryGet(newPos.x, out Moveable? mov);
        bool result = success && !fail;
        if (result)
        {
            Pos = newPos;
            current.UpdateMoveable(this);
        }
        else
        {
            if (newPos.facing != Pos.facing)
            {
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

    protected override void OnDeath()
    {
        base.OnDeath();
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