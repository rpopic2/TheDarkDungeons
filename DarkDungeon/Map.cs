/// <summary>
/// Renderes map into System.String.
/// Also holds position of creatures.
/// </summary>
public class Map {
// private:

    const int BOSS_DEPTH = 5;

    readonly string _pushDown;
    readonly char[] _empty;
    readonly char[] _tiles;
    readonly ISteppable?[] _steppables;
    readonly Creature?[] _creaturesByPos;
    readonly List<Creature> _tempDeadCreatures;
    readonly char[] _rendered;

    static Random s_rnd = Random.Shared;
    static int s_spawnrate = 11;

    Corpse? _corpseToPass;
    Action _onTurnPre = () => { };
    Action _onTurn = () => { };
    Action _onTurnEnd = () => { };
    DepthGraph _depthGraph;


    static ISpawnable[] s_monsterData = {
        new Bat(default), new Shaman(default), new Snake(default), new Rat(default), new Marksman(default), new Lunatic(default)
    };

    static ISpawnable[] s_bossData = {
        new QuietKnight(default)
    };

    static Player s_player {
        get => Player.instance;
    }

    static void NewDepth() {
        TurnInCurrentDepth = 0;
        Depth += 1;
        DepthGraph depth = new();
    }

    Map(int length, bool spawnMobs = true, IPortal? portal = null, Corpse? corpseFromPrev = null) {
        Current = this;
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
        if (Depth == BOSS_DEPTH) {
            this.DoSpawnMobs = false;
            Spawn(s_bossData[0]);
            IO.pr("조용한 기사가 등장했다.");
        }
        OnNewMap.Invoke();
    }

    void SetupSteppables(IPortal? portal, Corpse? corpseFromPrev) {
        if (corpseFromPrev is Corpse corpse)
            _steppables[s_player.Pos.x] = corpse;
        if (portal is null)
            return;
        if (portal is RandomPortal) {
            int pitChanceMax = PitChanceMax;
            if (pitChanceMax == -1) {
                portal = new Door();
            } else {
                bool spawnPit = s_rnd.Next(0, pitChanceMax) == 0;
                portal = spawnPit ? new Pit() : new Door();
            }
        }
        int portalIndex = Depth != 1 ? s_rnd.Next(0, Length - 1) : s_rnd.Next(2, Length - 1);
        _steppables![portalIndex] = portal;
    }

    public void OnTurnElapse() {
        _onTurnPre.Invoke();
        _onTurn.Invoke();
        _onTurnEnd.Invoke();//update target and reset stance, onturnend
        _onTurn = delegate { };

        ReplaceToCorpse();
        if (DoSpawnMobs && Turn % s_spawnrate == 0)
            SpawnRandom();
        Turn++;
        TurnInCurrentDepth++;
        if (Current.DoLoadNewMap)
            NewMap();
        IO.Redraw();
    }

    Monster? Spawn(ISpawnable prefab) {
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

    void ReplaceToCorpse() {
        if (_tempDeadCreatures.Count <= 0)
            return;
        foreach (Creature creature in _tempDeadCreatures) {
            CreateCorpse(creature);
        }
        _tempDeadCreatures.Clear();

        void CreateCorpse(Creature creature) {
            int pos = creature.Pos.x;
            ISteppable? old = _steppables[pos];
            Corpse temp = new Corpse(creature.ToChar(), creature.Name + "의 시체", creature.Inven);
            if (Monster.DropOutOf(creature.Stat.rnd, 5))
                temp.droplist.Add(Creature.boneOfTheDeceased);
            if (old is Corpse cor)
                _steppables[pos] = cor + temp;
            else if (old is IPortal)
                _corpseToPass = temp;
            else
                _steppables[pos] = temp;
        }
    }

    void Render() {
        _empty.CopyTo(_rendered, 0);
        renderVisible(Tiles);
        renderVisible(_steppables);
        renderVisible(_creaturesByPos);
        //RenderAllMobs();//debug
        _rendered[s_player.Pos.x] = s_player.ToChar();

        void renderVisible<T>(T[] target) {
            for (int i = 0; i < s_player.Sight; i++) {
                int targetTile = s_player.Pos.Front(i + 1);
                bool success = target.TryGet(targetTile, out T? obj);
                if (!success)
                    continue;
                if (obj is Creature mov)
                    _rendered[targetTile] = mov.ToChar();
                else if (obj is char chr)
                    _rendered[targetTile] = chr;
                else if (obj is ISteppable cor)
                    _rendered[targetTile] = cor.ToChar();
                else if (obj is not null)
                    throw new Exception("등록되지 않은 맵 오브젝트입니다.");

                if (IO.IIO is GameSocket gs && obj is Corpse corpse) {
                    gs.pr_corpse(targetTile, corpse);
                }
            }
        }

#pragma warning disable 8321
        //function for debugging
        void RenderAllMobs() {
            for (int i = 0; i < Length; i++) {
                if (_creaturesByPos[i] is Monster m)
                    _rendered[i] = m.ToChar();
            }
        }
#pragma warning restore 8321
    }


// public:

    public const int START_DEPTH = 1;

    public readonly int Length;
    public readonly bool DoSpawnMobs;

    public static Action OnNewMap = () => { };
    public static Map Current = default!;

    public bool DoLoadNewMap = false;

    public static int Turn {
        get;
        private set;
    }

    /// <summary>
    /// Is 1 when initialised
    /// </summary>
    public static int Depth {
        get;
        private set;
    } = START_DEPTH;

    public static int TurnInCurrentDepth {
        get;
        private set;
    }

    public ref readonly char[] Tiles
        => ref _tiles;

    public ref readonly ISteppable?[] Steppables
        => ref _steppables;

    public int PitChanceMax {
        get {
            if (TurnInCurrentDepth == 0) return -1;
            return (int)MathF.Max(0, 50 - TurnInCurrentDepth);
        }
    }

    public string Rendered {
        get {
            Render();
            return new string(_rendered);
        }
    }

    public static void ResetMapTurn() {
        Turn = 0;
        Depth = START_DEPTH;
        TurnInCurrentDepth = 0;
    }

    public static void NewMap() {
        if (Player._instance is not null && s_player.UnderFoot is Pit) 
            NewDepth();

        int addMapWidth = Depth.FloorMult(Rules.MapWidthByLevel);
        int newLength = s_rnd.Next(Rules.MapLengthMin, Rules.MapLengthMin + addMapWidth);
        newLength = Math.Max(newLength, Rules.MapLengthMin);

        if (Current is not null && newLength < Current.Length) {
            newLength = Current.Length;
        }
        Current = new Map(newLength, true, new RandomPortal(), Current?._corpseToPass);
        if (Player._instance is not null && Map.TurnInCurrentDepth == 0 && Depth > 1)
            IO.rk($"{s_player.Name}은 깊이 {Depth}에 도달했다.");
    }


    public ISteppable? GetSteppable(int index)
        => _steppables[index];

    public void OnTurn(Action action, bool isPrepend = false) {
        if (isPrepend)
            _onTurn = action + _onTurn;
        else
            _onTurn += action;
    }

    public void AddToOnTurnPre(Action action)
        => _onTurnPre += action;

    public void AddToOnTurnEnd(Action action)
        => _onTurnEnd += action;

#pragma warning disable CS8601
    public void RemoveFromOnTurnPre(Action action)
        => _onTurnPre -= action;

    public void RemoveFromOnTurnEnd(Action action)
        => _onTurnEnd -= action;
#pragma warning restore CS8601

    public Monster? SpawnRandom() {
        if (Depth == BOSS_DEPTH)
            return null;
        int max = Math.Min(Depth + 1, s_monsterData.Length);
        int min = Math.Max(0, max - 2);
        int randomIndex = s_rnd.Next(min, max);
        ISpawnable data = s_monsterData[randomIndex];
        return Spawn(data);
    }

    private List<int> GetSpawnableIndices() {
        List<int> spawnables = new List<int>(Length);
        for (int i = 0; i < Length; i++) {
            if (_creaturesByPos[i] is null)
                spawnables.Add(i);
        }
        if (Player._instance is not null) {
            int playerX = s_player.Pos.x;
            spawnables.Remove(playerX);
            spawnables.Remove(playerX - 1);
            spawnables.Remove(playerX + 1);
        }
        return spawnables;
    }
    public void UpdateFightable(Creature mov) {
        Position pos = mov.Pos;
        if (mov is Creature fight && !fight.IsAlive) {
            if (_tempDeadCreatures.Contains(fight))
                return;
            _tempDeadCreatures.Add(fight);
            _creaturesByPos[pos.x] = null;
            return;
        }
        int oldIndex = Array.IndexOf(_creaturesByPos, mov);
        if (oldIndex != -1)
            _creaturesByPos[oldIndex] = null;
        _creaturesByPos[pos.x] = mov;
    }

    public Creature? GetCreatureAt(int index)
        => _creaturesByPos.ElementAtOrDefault(index);

    public void SpawnPortal(Position pos) {
        IPortal portal = new Pit();
        _steppables[pos.x] = portal;
    }

    public Creature? RayCast(Position origin, int range) {
        Creature? f;
        for (int i = 0; i < range; i++) {
            f = GetCreatureAt(origin.Front(i + 1));
            if (f is Creature)
                return f;
        }
        return null;
    }

    public bool IsEnd(int index) {
        if (index <= 0 || index >= Length - 1)
            return true;
        return false;
    }

    public void OnCorpsePickUp(Corpse corpse) {
        if (corpse.droplist.Count() <= 0) {
            int index = Array.IndexOf(_steppables, corpse);
            _steppables[index] = null;
        }
    }

    public override string ToString()
        => _pushDown + string.Join(" ", Rendered);
}
