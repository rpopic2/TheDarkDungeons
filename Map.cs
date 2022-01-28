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
        tiles = NewEmptyArray(length, MapSymb.road);
        moveables = new Moveable[length];

        tiles[length - 1] = MapSymb.portal;
        moveables[0] = Player.instance;
        SpawnBat();
    }

    public void SpawnBat()
    {
        List<int> fullMap = GetSpawnableIndices();
        int index = rnd.Next(0, fullMap.Count);
        int newPos = fullMap[index];
        Position spawnPoint = new Position(newPos, 0, Facing.Back);

        int hp = 2 + m(0.03f, Program.turn) + m(0.5f, level);
        int sol = 1 + m(0.6f, level);
        int lun = 3;
        int cap = 1 + m(0.16f, level);
        int expOnKill = 3 + (int)MathF.Round(level * 0.3f);

        monster = new("Bat", ClassName.Warrior, level, hp, cap, (sol, lun, 2), expOnKill, spawnPoint);
        UpdateMoveable(monster);
    }

    private int m(float x, int multiplier) => (int)MathF.Round(multiplier * x);

    private List<int> GetSpawnableIndices()
    {
        List<int> fullMap = new List<int>(length);
        for (int i = 0; i < length; i++)
        {
            fullMap.Add(i);
        }
        int playerX = player.Pos.x;
        fullMap.Remove(0);
        fullMap.Remove(playerX);
        fullMap.Remove(playerX - 1);
        fullMap.Remove(playerX + 1);
        return fullMap;
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
        int front = player.Pos.FrontIndex;
        char[] result = NewEmptyArray(length, MapSymb.invisible);
        Foo(result, tiles, front);
        if (Player.instance.torch > 0)
        {
            Foo(result, tiles, front + 1);
            Foo(result, tiles, front + 2);
            Player.instance.torch--;
            if (Player.instance.torch == 0) Player.instance.Inven.Delete(Fightable.ItemData.Torch);
            Foo2(result, moveables, front + 1);
            Foo2(result, moveables, front + 2);
        }

        bool success = moveables.TryGet(front, out Moveable? obj);
        if (success) result[front] = obj!.ToChar();


        if (Rules.MapDebug) result[monster.Pos.x] = monster.ToChar();
        result[player.Pos.x] = Player.instance.ToChar();
        return string.Join(" ", result);
    }
    public void Foo(char[] result, char[] target, int x)
    {
        bool success3 = target.TryGet(x, out char obj3);
        if (success3) result[x] = obj3!;
    }
    public void Foo2(char[] result, Moveable?[] target, int x)
    {
        bool success = target.TryGet(x, out Moveable? obj);
        if (success) result[x] = obj!.ToChar();
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

    private static char[] NewEmptyArray(int length, char fill)
    {
        char[] result = new char[length];
        for (int i = 0; i < length; i++)
        {
            result[i] = fill;
        }
        return result;
    }
}