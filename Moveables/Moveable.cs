public class Moveable : Entity
{
    protected (Stance stance, int amount) stance = (default, default);
    public Moveable(int level, int sol, int lun, int con, string name) : base(level, sol, lun, con, name)
    {
        Pos = new Position();
    }
    public Position Pos { get; set; }

    public (Stance stance, int amount) CurStance => stance;
    public virtual void Move(int x)
    {
        _Move(x, out char obj);
    }

    public virtual char ToChar()
    {
        return Name.ToLower()[0];
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
}