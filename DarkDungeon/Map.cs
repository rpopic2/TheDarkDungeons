public class Map
{
    private static Random s_rnd = new Random();
    public static Map Current = default!;
    ///<summary>is 1 by default</summary>
    public static int Depth;
    public readonly int Length;
    public readonly bool SpawnMobs;
    private readonly string _pushDown;
    private readonly char[] _empty;
    private char[] _tiles;
    public ISteppable?[] Steppables;
    private List<Fightable> _fightables;
    private Fightable?[] _fightablePositions;
    private List<Fightable> _tempDeadFightables;
    private char[] _rendered;
    private Corpse? _fallenCorpse;
    public bool LoadNewMap = false;
    // private const bool _debug = false;
    public Map(int length, Corpse? corpseFromPrev, bool spawnMobs = true)
    {
        Current = this;
        this.Length = length;
        this.SpawnMobs = spawnMobs;
        _pushDown = new('\n', Depth - 1);

        _empty = Extensions.NewFilledArray(length, MapSymb.Empty);
        _tiles = Extensions.NewFilledArray(length, MapSymb.road);
        Steppables = new ISteppable?[length];
        _fightables = new();
        _fightablePositions = new Fightable[length];
        _rendered = new char[length];
        _tempDeadFightables = new();

        int portalIndex = Depth != 1 ? s_rnd.Next(0, length - 1) : s_rnd.Next(2, length - 1);
        Steppables[portalIndex] = new Portal();
        if (corpseFromPrev is Corpse corpse) Steppables[s_player.Pos.x] = corpse;
        _fightables.Add(s_player);
        if (spawnMobs) Spawn();
    }
    private static Player s_player { get => Player.instance; }
    public ref readonly Fightable?[] FightablePositions => ref _fightablePositions;
    public ref readonly List<Fightable> Fightables => ref _fightables;
    public ref readonly char[] Tiles
    => ref _tiles;
    public void Spawn()
    {
        List<int> spawnableIndices = GetSpawnableIndices();
        if (spawnableIndices.Count <= 0) return;

        int max = Math.Min(Depth + 1, Monster.Count);
        int randomData = s_rnd.Next(0, max);
        MonsterData data = Monster.data[randomData];

        int randomIndex = s_rnd.Next(0, spawnableIndices.Count);
        int newPos = spawnableIndices[randomIndex];
        Facing randomFace = (Facing)s_rnd.Next(0, 2);
        Position spawnPoint = new Position(newPos, randomFace);

        Fightable mov;
        mov = new Monster(data, spawnPoint);
        _fightables.Add((Fightable)mov);
        UpdateFightable(mov);

        List<int> GetSpawnableIndices()
        {
            List<int> fullMap = new List<int>(Length);
            for (int i = 0; i < Length; i++)
            {
                if (FightablePositions[i] is null) fullMap.Add(i);
            }
            int playerX = s_player.Pos.x;
            fullMap.Remove(0);
            fullMap.Remove(1);
            fullMap.Remove(playerX);
            fullMap.Remove(playerX - 1);
            fullMap.Remove(playerX + 1);
            return fullMap;
        }
    }
    public void UpdateFightable(Fightable mov)
    {
        Position pos = mov.Pos;
        if (mov is Fightable fight && !fight.IsAlive)
        {
            _tempDeadFightables.Add(fight);
            FightablePositions[pos.x] = null;
            return;
        }
        int oldIndex = Array.IndexOf(FightablePositions, mov);
        if (oldIndex != -1) FightablePositions[oldIndex] = null;
        FightablePositions[pos.x] = mov;
    }
    public void RemoveAndCreateCorpse()
    {
        foreach (var item in _tempDeadFightables)
        {
            _fightables.Remove(item);
            CreateCorpse(item);
        }
        _tempDeadFightables.Clear();

        void CreateCorpse(Fightable fight)
        {
            int pos = fight.Pos.x;
            ISteppable? old = Steppables[pos];

            Corpse temp = new Corpse(fight.Name + "의 시체", fight.Inven.Content);
            if (old is Corpse cor) Steppables[pos] = cor + temp;
            else if (old is Portal) _fallenCorpse = temp;
            else Steppables[pos] = temp;
        }
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
    public bool IsAtEnd(int index)
    {
        if (index <= 0 || index >= Length - 1) return true;
        return false;
    }
    public static void NewMap()
    {
        Depth++;
        int addMapWidth = Depth.FloorMult(Rules.MapWidthByLevel);
        int newLength = s_rnd.Next(Rules.MapLengthMin, Rules.MapLengthMin + addMapWidth);
        if (newLength > Rules.MapLengthMax) newLength = Rules.MapLengthMax;
        if (Current is not null && newLength < Current.Length) newLength = Current.Length;
        Current = new Map(newLength, Current?._fallenCorpse);
        if (Depth > 1) IO.rk($"{s_player.Name}은 깊이 {Depth}에 도달했다.");
    }
    private void Render()
    {
        _empty.CopyTo(_rendered, 0);
        RenderVisible(Tiles);
        RenderVisible(Steppables);
        RenderVisible(FightablePositions);
        //if(true) RenderAllMobs();//debug
        _rendered[s_player.Pos.x] = s_player.ToChar();

        void RenderVisible<T>(T[] target)
        {
            for (int i = 0; i < s_player.Sight; i++)
            {
                int targetTile = s_player.Pos.Front(i + 1);
                bool success = target.TryGet(targetTile, out T? obj);
                if (!success) continue;
                if (obj is Fightable mov) _rendered[targetTile] = mov.ToChar();
                else if (obj is char chr) _rendered[targetTile] = chr;
                else if (obj is ISteppable cor) _rendered[targetTile] = cor.ToChar();
                else if (obj is not null) throw new Exception("등록되지 않은 맵 오브젝트입니다.");
            }
        }
        // void RenderAllMobs()
        // {
        //     for (int i = 0; i < Length; i++) if (FightablePositions[i] is Monster m) _rendered[i] = m.ToChar();
        // }
    }

    public override string ToString()
    {
        Render();
        return _pushDown + string.Join(" ", _rendered);
    }
}