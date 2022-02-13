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
    private readonly char[] empty;
    public readonly int length;
    private const bool debug = false;
    private bool monsterSpawned = false;
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
        Spawn();
    }

    public void Spawn()
    {
        List<int> spawnableIndices = GetSpawnableIndices();
        if (spawnableIndices.Count <= 0) return;
        int difficulty = (int)MathF.Floor(level / 2) + 1;
        int max = Math.Min(difficulty, MonsterDb.Count);
        int min = monsterSpawned ? 0 : Math.Min(0, max - 1);
        min = Math.Max(min, 0);
        int randomInt = rnd.Next(min, max);
        MonsterData data = MonsterDb.data[randomInt];
        int index = rnd.Next(0, spawnableIndices.Count);
        int newPos = spawnableIndices[index];
        Position spawnPoint = new Position(newPos, 0, Facing.Back);
        if (data.name == MonsterDb.bat.name)
        {
            Bat monster = new(data, spawnPoint);
            UpdateMoveable(monster);
        }
        else
        {
            Monster monster = new(data, spawnPoint);
            UpdateMoveable(monster);
        }
        monsterSpawned = true;
    }
    private List<int> GetSpawnableIndices()
    {
        List<int> fullMap = new List<int>(length);
        for (int i = 0; i < length; i++)
        {
            if (moveables[i] is null) fullMap.Add(i);
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
        if (mov is Fightable fight && !fight.IsAlive)
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
        RenderVisible(Tiles);
        RenderVisible(Moveables);
        //if(debug) RenderAllMobs();
        rendered[player.Pos.x] = MapSymb.player;
    }
    private void RenderAllMobs()
    {
        for (int i = 0; i < length; i++)
        {
            if (moveables[i] is Monster m) rendered[i] = m.ToChar();
        }
    }
    private void RenderVisible<T>(T[] target)
    {
        int sight = player.sight;
        int front = player.Pos.FrontIndex;
        for (int i = 0; i < sight; i++)
        {
            int targetTile = player.Pos.isFacingFront ? front + i : front - i;
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
        if (mov is Player) return true;
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