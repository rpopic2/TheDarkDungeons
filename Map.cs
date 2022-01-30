public class Map
{
    private static Player player { get => Player.instance; }
    public static Map Current = default!;
    public static Random rnd = new Random();
    public static int level;
    private char[] tiles;
    public ref readonly char[] Tiles
        => ref tiles;
    private Moveable?[] moveables;
    public ref readonly Moveable?[] Moveables
    => ref moveables;
    private char[] rendered;
    private char[] empty;
    public readonly int length;
    public Monster monster { get; private set; } = default!;
    public Map(int length)
    {
        Current = this;
        this.length = length;
        tiles = NewEmptyArray(length, MapSymb.road);
        moveables = new Moveable[length];
        empty = NewEmptyArray(length, MapSymb.Empty);
        rendered = new char[length];

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
        fullMap.Remove(1);
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
    private void Render()
    {
        empty.CopyTo(rendered, 0);
        RenderFrom(Tiles);
        RenderFrom(Moveables);
        rendered[player.Pos.x] = MapSymb.player;
    }
    public void RenderFrom<T>(T[] target)
    {
        int sight = player.sight;
        int front = player.Pos.FrontIndex;
        for (int i = 0; i < sight; i++)
        {
            int targetTile = front + i;
            bool success = target.TryGet(targetTile, out T? obj);
            if (!success) continue;
            if (obj is Moveable mov) rendered[targetTile] = mov.ToChar();
            else if (obj is char chr) rendered[targetTile] = chr;
            else if (obj is not null) throw new Exception();
        }
    }
    public override string ToString()
    {
        Render();
        return string.Join(" ", rendered);
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