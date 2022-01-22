public class Map
{
    private static Player player = Player.instance;
    public static Map Current = default!;
    public static Random rnd = new Random();
    public static int level;
    private char[] tiles;
    public ref readonly char[] Tiles
        => ref tiles;
    private Moveable?[] moveables;
    public ref readonly Moveable?[] Moveables
    => ref moveables;
    public readonly int length;
    public Monster monster { get; private set; } = default!;
    public Map(int length)
    {
        Current = this;
        this.length = length;
        tiles = NewEmptyMap(length, MapSymb.road);
        moveables = new Moveable[length];

        tiles[length - 1] = MapSymb.portal;
        moveables[0] = Player.instance;
        SpawnMob();
    }
    public void SpawnMob()
    {
        int hp = level.RoundMult(Rules.mhpByLevel);
        int expOnKill = 3 + level.RoundMult(Rules.mexpByLevel);
        int sol = 2 + Program.turn.FloorMult(Rules.msolByTurn);
        int lun = 1 + Program.turn.FloorMult(Rules.mlunByTurn);
        int cap = 1 + level.FloorMult(Rules.mcapByLevel);
        Position spawnPoint = new Position(rnd.Next(2, Map.Current.length - 2 - level.FloorMult(0.4f)), 0, Facing.Back);
        monster = new Monster("Bat", ClassName.Warrior, cap, hp, level, sol, lun, 2, expOnKill, spawnPoint);
        if (IsVisible(monster)) monster.Move(2);
        UpdateMoveable(monster);
    }

    public void UpdateMoveable(Moveable mov)
    {
        Position pos = mov.Pos;
        if (!mov.IsAlive)
        {
            moveables[pos.x] = null;
            return;
        }
        if (moveables[pos.oldX] == mov) moveables[pos.oldX] = null;
        moveables[pos.x] = mov;
    }
    public override string ToString()
    {
        Moveable player = Player.instance;
        int front = player.Pos.Front;
        char[] result = NewEmptyMap(length, MapSymb.invisible);
        bool success2 = tiles.TryGet(front, out char obj2);
        if (success2) result[front] = obj2!;

        bool success = moveables.TryGet(front, out Moveable? obj);
        if (success) result[front] = obj!.ToChar();

        if (Rules.MapDebug) result[monster.Pos.x] = monster.ToChar();
        result[player.Pos.x] = Player.instance.ToChar();
        return string.Join(" ", result);
    }
    public bool IsVisible(Moveable mov)
    {
        Position playerPos = Player.instance.Pos;
        int distance = mov.Pos.x - playerPos.x;
        if (distance > 0 && playerPos.facing == Facing.Front) return true;
        else if (distance < 0 && playerPos.facing == Facing.Back) return true;
        else if (distance == 0) return true;
        return false;
    }
    internal bool IsAtEnd(int index)
    {
        if (index <= 0 || index >= length - 1) return true;
        return false;
    }
    public static void NewMap()
    {
        level++;
        int addMapWidth = level.FloorMult(Rules.MapWidthByLevel);
        Current = new Map(rnd.Next(Rules.MapLengthMin + addMapWidth, Rules.MapLengthMax + addMapWidth));
        Player.instance.UpdateTarget();
        Current.monster.UpdateTarget();
    }

    private static char[] NewEmptyMap(int length, char fill)
    {
        char[] result = new char[length];
        for (int i = 0; i < length; i++)
        {
            result[i] = fill;
        }
        return result;
    }
}