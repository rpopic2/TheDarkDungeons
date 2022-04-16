public class Map
{
    private static Player player { get => Player.instance; }
    public static Map Current = default!;
    public static Random rnd = new Random();
    public static int level;
    private char[] tiles;
    public ref readonly char[] Tiles
        => ref tiles;
    private Fightable?[] fightablePositions;
    public ref readonly Fightable?[] FightablePositions => ref fightablePositions;
    private List<Fightable> fightables = new();
    public ref readonly List<Fightable> Fightables => ref fightables;
    private List<Fightable> temp_deadFightables = new();
    public ISteppable?[] steppables;
    private Corpse? corpseToNext;
    private char[] rendered;
    private readonly char[] empty;
    public readonly int length;
    private const bool debug = false;
    public bool SpawnMobs {get; private set;}
    public Map(int length, Corpse? corpseFromPrev, bool spawnMobs = true)
    {
        Current = this;
        this.length = length;
        tiles = NewEmptyArray(length, MapSymb.road);
        fightablePositions = new Fightable[length];
        empty = NewEmptyArray(length, MapSymb.Empty);
        steppables = new ISteppable?[length];
        rendered = new char[length];

        steppables[length - 1] = new Portal();
        if (corpseFromPrev is Corpse corpse) steppables[0] = corpse;
        FightablePositions[0] = Player.instance;
        fightables.Add(player);
        this.SpawnMobs = spawnMobs;
        if(spawnMobs) Spawn();
    }
    public Fightable? GetFightableAt(int index)
    {
        if (index < 0 || index >= FightablePositions.Length || FightablePositions[index] is null) return null;
        return FightablePositions[index];
    }
    public Fightable? RayCast(Position origin, int range)
    {
        Fightable? f;
        for (int i = 0; i < range; i++)
        {
            f = GetFightableAt(origin.Front(i + 1));
            if (f is Fightable) return f;
        }
        return null;
    }
    public void Spawn()
    {
        List<int> spawnableIndices = GetSpawnableIndices();
        if (spawnableIndices.Count <= 0) return;

        int randomInt = rnd.Next(0, Monster.Count);
        MonsterData data = Monster.data[1];

        int index = rnd.Next(0, spawnableIndices.Count);
        int newPos = spawnableIndices[index];
        Position spawnPoint = new Position(newPos, Facing.Left);
        _Spawn(data, spawnPoint);
    }
    public void _Spawn(MonsterData data, Position spawnPoint)
    {
        Fightable mov;
        mov = new Monster(data, spawnPoint);
        fightables.Add((Fightable)mov);
        UpdateFightable(mov);
    }
    private List<int> GetSpawnableIndices()
    {
        List<int> fullMap = new List<int>(length);
        for (int i = 0; i < length; i++)
        {
            if (FightablePositions[i] is null) fullMap.Add(i);
        }
        int playerX = player.Pos.x;
        fullMap.Remove(0);
        fullMap.Remove(1);
        fullMap.Remove(playerX);
        fullMap.Remove(playerX - 1);
        fullMap.Remove(playerX + 1);
        return fullMap;
    }

    public void UpdateFightable(Fightable mov)
    {
        Position pos = mov.Pos;
        if (mov is Fightable fight && !fight.IsAlive)
        {
            temp_deadFightables.Add(fight);
            FightablePositions[pos.x] = null;
            return;
        }
        int oldIndex = Array.IndexOf(FightablePositions, mov);
        if (oldIndex != -1) FightablePositions[oldIndex] = null;
        FightablePositions[pos.x] = mov;
    }
    public void RemoveAndCreateCorpse()
    {
        foreach (var item in temp_deadFightables)
        {
            fightables.Remove(item);
            CreateCorpse(item);
        }
        temp_deadFightables.Clear();
    }
    public void CreateCorpse(Fightable fight)
    {
        int pos = fight.Pos.x;
        ISteppable? old = steppables[pos];

        Corpse temp = new Corpse(fight.Name + "의 시체", fight.Inven.Content);
        if (old is Corpse cor) steppables[pos] = cor + temp;
        else if (old is Portal) corpseToNext = temp;
        else steppables[pos] = temp;
    }
    private void Render()
    {
        empty.CopyTo(rendered, 0);
        RenderVisible(Tiles);
        RenderVisible(steppables);
        RenderVisible(FightablePositions);
        //if(debug) RenderAllMobs();
        rendered[player.Pos.x] = MapSymb.player;
    }
    private void RenderAllMobs()
    {
        for (int i = 0; i < length; i++)
        {
            if (FightablePositions[i] is Monster m) rendered[i] = m.ToChar();
        }
    }
    private void RenderVisible<T>(T[] target)
    {
        for (int i = 0; i < player.Sight; i++)
        {
            int targetTile = player.Pos.Front(i + 1);
            bool success = target.TryGet(targetTile, out T? obj);
            if (!success) continue;
            if (obj is Fightable mov) rendered[targetTile] = mov.ToChar();
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
        int length = rnd.Next(Rules.MapLengthMin + addMapWidth, Rules.MapLengthMax + addMapWidth);
        Current = new Map(length, Current?.corpseToNext);
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