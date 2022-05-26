public class Map
{
    private const int BOSS_DEPTH = 5;
    public static int Turn { get; private set; }
    private static int Spawnrate = 11;
    public static Map Current = default!;
    private static ISpawnable[] s_monsterData = { new Bat(default), new Shaman(default), new Lunatic(default), new Snake(default), new Rat(default) };
    private static Random s_rnd = new Random();
    ///<summary>is 1 by default</summary>
    public static int Depth;
    public readonly int Length;
    public readonly bool DoSpawnMobs;
    private readonly string _pushDown;
    private readonly char[] _empty;
    private readonly char[] _tiles;
    private readonly ISteppable?[] _steppables;
    private readonly Creature?[] _creaturesByPos;
    private readonly List<Creature> _tempDeadCreatures;
    private readonly char[] _rendered;
    private Corpse? _corpseToPass;
    public bool DoLoadNewMap = false;
    public static Action OnNewMap = () => { };
    private Action _onTurnPre = () => { };
    private Action _onTurn = () => { };
    private Action _onTurnEnd = () => { };
    public Map(int length, bool spawnMobs = true, IPortal? portal = null, Corpse? corpseFromPrev = null)
    {
        Current = this;
        Program.OnTurn = () => OnTurnElapse();
        this.Length = length;
        this.DoSpawnMobs = spawnMobs;
        int push = (int)MathF.Max(Depth - 1, 0);
        _pushDown = new('\n', push);
        _empty = new char[length];
        _tiles = new char[length];
        _rendered = new char[length];
        Array.Fill(_empty, MapSymb.Empty);
        Array.Fill(_tiles, MapSymb.road);
        _steppables = new ISteppable?[length];
        _tempDeadCreatures = new();
        _creaturesByPos = new Creature[length];

        SetupSteppables(portal, corpseFromPrev);
        if (Depth == BOSS_DEPTH)
        {
            this.DoSpawnMobs = false;
            Spawn(new QuietKnight(default));
        }
        OnNewMap.Invoke();
    }
    private static Player s_player { get => Player.instance; }
    public ref readonly char[] Tiles => ref _tiles;
    public ref readonly ISteppable?[] Steppables => ref _steppables;
    public ISteppable? GetSteppable(int index) => _steppables[index];
    private void SetupSteppables(IPortal? portal, Corpse? corpseFromPrev)
    {
        if (corpseFromPrev is Corpse corpse) _steppables[s_player.Pos.x] = corpse;
        if (portal is null) return;
        if (portal is RandomPortal)
        {
            bool spawnPit = s_rnd.Next(0, 3) == 0;
            portal = spawnPit ? new Pit() : new Door();
        }
        int portalIndex = Depth != 1 ? s_rnd.Next(0, Length - 1) : s_rnd.Next(2, Length - 1);
        _steppables![portalIndex] = portal;
    }
    public void AddToOnTurn(Action action, bool isPrepend)
    {
        if (isPrepend) _onTurn = action + _onTurn;
        else _onTurn += action;
    }
    public void AddToOnTurnPre(Action action) => _onTurnPre += action;
    public void AddToOnTurnEnd(Action action) => _onTurnEnd += action;
#pragma warning disable CS8601
    public void RemoveFromOnTurnPre(Action action) => _onTurnPre -= action;
    public void RemoveFromOnTurnEnd(Action action) => _onTurnEnd -= action;

    private void OnTurnElapse()
    {
        _onTurnPre.Invoke();
        _onTurn.Invoke();
        _onTurnEnd.Invoke();//update target and reset stance, onturnend
        _onTurn = delegate { };

        ReplaceToCorpse();
        if (DoSpawnMobs && Turn % Spawnrate == 0) SpawnRandom();
        Turn++;
        if (Current.DoLoadNewMap) NewMap();
        IO.Redraw();
    }
    public Monster? SpawnRandom()
    {
        int max = Math.Min(Depth + 1, s_monsterData.Length);
        int min = Math.Max(0, max - 2);
        int randomIndex = s_rnd.Next(min, max);
        ISpawnable data = s_monsterData[randomIndex];
        return Spawn(data);
    }
    private Monster? Spawn(ISpawnable prefab)
    {
        List<int> spawnableIndices = GetSpawnableIndices();
        if (spawnableIndices.Count <= 0) return null;
        int randomPos = s_rnd.Next(0, spawnableIndices.Count);
        int newPos = spawnableIndices[randomPos];
        Facing randomFace = (Facing)s_rnd.Next(0, 2);
        Position spawnPoint = new Position(newPos, randomFace);
        Monster mov = prefab.Instantiate(spawnPoint);
        UpdateFightable(mov);
        return mov;
    }
    private List<int> GetSpawnableIndices()
    {
        List<int> spawnables = new List<int>(Length);
        for (int i = 0; i < Length; i++)
        {
            if (_creaturesByPos[i] is null) spawnables.Add(i);
        }
        int playerX = s_player.Pos.x;
        spawnables.Remove(playerX);
        spawnables.Remove(playerX - 1);
        spawnables.Remove(playerX + 1);
        return spawnables;
    }
    public void UpdateFightable(Creature mov)
    {
        Position pos = mov.Pos;
        if (mov is Creature fight && !fight.IsAlive)
        {
            if (_tempDeadCreatures.Contains(fight)) return;
            _tempDeadCreatures.Add(fight);
            _creaturesByPos[pos.x] = null;
            return;
        }
        int oldIndex = Array.IndexOf(_creaturesByPos, mov);
        if (oldIndex != -1) _creaturesByPos[oldIndex] = null;
        _creaturesByPos[pos.x] = mov;
    }
    private void ReplaceToCorpse()
    {
        if (_tempDeadCreatures.Count <= 0) return;
        foreach (var item in _tempDeadCreatures)
        {
            CreateCorpse(item);
        }
        _tempDeadCreatures.Clear();

        void CreateCorpse(Creature fight)
        {
            int pos = fight.Pos.x;
            ISteppable? old = _steppables[pos];
            Corpse temp = new Corpse(fight.Name + "의 시체", fight.Inven);
            if (Monster.DropOutOf(fight.Stat.rnd, 5)) temp.droplist.Add(Creature.boneOfTheDeceased);
            if (old is Corpse cor) _steppables[pos] = cor + temp;
            else if (old is Pit) _corpseToPass = temp;
            else _steppables[pos] = temp;
        }
    }
    public Creature? GetCreatureAt(int index) => _creaturesByPos.ElementAtOrDefault(index);
    public Creature? RayCast(Position origin, int range)
    {
        Creature? f;
        for (int i = 0; i < range; i++)
        {
            f = GetCreatureAt(origin.Front(i + 1));
            if (f is Creature) return f;
        }
        return null;
    }
    public bool IsEnd(int index)
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
        if (Current is not null && newLength < Current.Length)
        {
            newLength = Current.Length;
            Program.OnTurn -= () => Current.OnTurnElapse();
            Program.OnTurn = null;
        }
        Current = new Map(newLength, true, new RandomPortal(), Current?._corpseToPass);
        if (Depth > 1) IO.rk($"{s_player.Name}은 깊이 {Depth}에 도달했다.");
    }
    private void Render()
    {
        _empty.CopyTo(_rendered, 0);
        renderVisible(Tiles);
        renderVisible(_steppables);
        renderVisible(_creaturesByPos);
        //RenderAllMobs();//debug
        _rendered[s_player.Pos.x] = s_player.ToChar();

        void renderVisible<T>(T[] target)
        {
            for (int i = 0; i < s_player.Sight; i++)
            {
                int targetTile = s_player.Pos.Front(i + 1);
                bool success = target.TryGet(targetTile, out T? obj);
                if (!success) continue;
                if (obj is Creature mov) _rendered[targetTile] = mov.ToChar();
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
    public void OnCorpsePickUp(Corpse corpse)
    {
        if (corpse.droplist.Count() <= 0)
        {
            int index = Array.IndexOf(_steppables, corpse);
            _steppables[index] = null;
        }
    }

    public override string ToString()
    {
        Render();
        return _pushDown + string.Join(" ", _rendered);
    }
}