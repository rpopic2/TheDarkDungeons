public class Map
{
    private static Player player { get => Player.instance; }
    public static Map Current = default!;
    public static Random rnd = new Random();
    public static int level;
    private char[] tiles;
    public ref readonly char[] Tiles
        => ref tiles;
    private Moveable?[] moveablePositions;
    public ref readonly Moveable?[] MoveablePositions => ref moveablePositions;
    private List<Fightable> fightables = new();
    public ref readonly List<Fightable> Fightables => ref fightables;
    private List<Fightable> deadFightables = new();
    public ISteppable?[] steppables;
    private char[] rendered;
    private readonly char[] empty;
    public readonly int length;
    private const bool debug = false;
    public Map(int length, bool spawn = true)
    {
        Current = this;
        this.length = length;
        tiles = NewEmptyArray(length, MapSymb.road);
        moveablePositions = new Moveable[length];
        empty = NewEmptyArray(length, MapSymb.Empty);
        steppables = new ISteppable?[length];
        rendered = new char[length];

        steppables[length - 1] = new Portal();
        moveablePositions[0] = Player.instance;
        fightables.Add(player);
        if (spawn) Spawn();
    }

    public void Spawn()
    {
        List<int> spawnableIndices = GetSpawnableIndices();
        if (spawnableIndices.Count <= 0) return;

        int randomInt = rnd.Next(0, Monster.Count);
        //int randomInt = 0;
        MonsterData data = Monster.data[randomInt];

        int index = rnd.Next(0, spawnableIndices.Count);
        int newPos = spawnableIndices[index];
        Position spawnPoint = new Position(newPos, Facing.Back);
        _Spawn(data, spawnPoint);
    }
    public void _Spawn(MonsterData data, Position spawnPoint)
    {
        Moveable mov;
        mov = new Monster(data, spawnPoint);
        fightables.Add((Fightable)mov);
        UpdateMoveable(mov);
    }
    private List<int> GetSpawnableIndices()
    {
        List<int> fullMap = new List<int>(length);
        for (int i = 0; i < length; i++)
        {
            if (moveablePositions[i] is null) fullMap.Add(i);
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
            deadFightables.Add(fight);
            moveablePositions[pos.x] = null;
            return;
        }
        if (moveablePositions[pos.oldX] == mov) moveablePositions[pos.oldX] = null;
        moveablePositions[pos.x] = mov;
    }
    public void RemoveAndCreateCorpse()
    {
        foreach (var item in deadFightables)
        {
            fightables.Remove(item);
            steppables[item.Pos.x] = new Corpse(item.Name + "의 시체", item.Inven.Content);
        }
        deadFightables.Clear();
    }
    private void Render()
    {
        empty.CopyTo(rendered, 0);
        RenderVisible(Tiles);
        RenderVisible(steppables);
        RenderVisible(MoveablePositions);
        //if(debug) RenderAllMobs();
        rendered[player.Pos.x] = MapSymb.player;
    }
    private void RenderAllMobs()
    {
        for (int i = 0; i < length; i++)
        {
            if (moveablePositions[i] is Monster m) rendered[i] = m.ToChar();
        }
    }
    private void RenderVisible<T>(T[] target)
    {
        int sight = player.sight;
        int front = player.Pos.GetFrontIndex(1);
        for (int i = 0; i < sight; i++)
        {
            int targetTile = player.Pos.isFacingFront ? front + i : front - i;
            bool success = target.TryGet(targetTile, out T? obj);
            if (!success) continue;
            if (obj is Moveable mov) rendered[targetTile] = mov.ToChar();
            else if (obj is char chr) rendered[targetTile] = chr;
            else if (obj is ISteppable cor) rendered[targetTile] = cor.ToChar();
            else if (obj is not null) throw new Exception("등록되지 않은 맵 오브젝트입니다.");
        }
    }
    public override string ToString()
    {
        Render();
        return string.Join(" ", rendered);
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
        Player._instance?.UpdateTarget();
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