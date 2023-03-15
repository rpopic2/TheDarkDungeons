public record struct Coord(int X, int Y); 

public class DepthGraph {
// private:

    const int _ROW = 3;
    const int _COLUMN = 3;

    List<List<Passage>> _passageMatrix;

    Coord[] _directions = new Coord[] {
        new(0, 1), new(1, 0), new(0, -1), new(-1, 0)
    };

    bool[,] visited = new bool[_ROW, _COLUMN];

    List<Coord> _viable_coord_buf = new(4);

    /// <summary>
    /// Bound checking of given coordinates
    /// </summary>
    bool IsValid(int row, int column) {
        return row >= 0 && row < _ROW && column >= 0 && column < _COLUMN;
    }

    /// <summary>
    /// Get a list of viable coordinates from given coordinate
    /// </summary>
    List<Coord> GetViableDirections(int row, int column, bool checkVisited, bool checkConnected) {
        _viable_coord_buf.Clear();
        foreach (var (rowoff, coloff) in _directions) {
            var nrow = row + rowoff;
            var ncol = column + coloff;
            if (!IsValid(nrow, ncol))
                continue;
            if (checkVisited && visited[nrow, ncol])
                continue;
            if (checkConnected && _passageMatrix[nrow][ncol].ConnectedPassages.Count <= 0) 
                continue;
            _viable_coord_buf.Add(new(nrow, ncol));
        }
        return _viable_coord_buf;
    }

    /// <summary>
    /// Start procedural map generation
    /// </summary>
    void Generate() {
        GenerateMainPath();
        JoinUnconnectedPassages();
    }

    /// <summary>
    /// Generate the main path of the map.
    /// </summary>
    void GenerateMainPath() {
        // dfs but in random directions
        Queue<Coord> queue = new();
        int start_row = Random.Shared.Next(_ROW);
        int start_column = Random.Shared.Next(_COLUMN);
        queue.Enqueue(new(start_row, start_column));

        while (queue.Count > 0) {
            (int row, int column) = queue.Dequeue();
            Passage passage = _passageMatrix[row][column];
            visited[row, column] = true;

            List<Coord> viable_directions = GetViableDirections(row, column, true, false);

            if (viable_directions.Count == 0) 
                continue;

            int new_row, new_column;
            var next = Random.Shared.Next(viable_directions.Count);
            (new_row, new_column) = viable_directions[next];

            Passage new_passage = _passageMatrix[new_row][new_column];
            passage.ConnectPassage(new_passage);
            queue.Enqueue(new(new_row, new_column));
        } // while
    }

    /// <summary>
    /// Connect leftover unconnected passages
    /// These might not always get connected.
    /// </summary>
    void JoinUnconnectedPassages() {
        // connect unconnected passages
        for (int i = 0; i < _ROW; i++) {
            for (int j = 0; j < _COLUMN; j++) {
                if (visited[i, j]) continue;
                Passage passage = _passageMatrix[i][j];
                List<Coord> viable_directions = GetViableDirections(i, j, false, true);

                if (viable_directions.Count == 0) {
                    continue;
                }
                var next = Random.Shared.Next(viable_directions.Count);
                var (new_row, new_column) = viable_directions[next];

                Console.WriteLine($"Passages connected: ({i}, {j}) and ({new_row}, {new_column})");
                Passage new_passage = _passageMatrix[new_row][new_column];
                passage.ConnectPassage(new_passage);
            }
        } //for
    }

// public:

    public DepthGraph() {
        // Initialize the passage matrix
        _passageMatrix = new(_ROW);
        for (int i = 0; i < _ROW; i++) {
            _passageMatrix.Add(new(_COLUMN));
            for (int j = 0; j < _COLUMN; j++) {
                _passageMatrix[i].Add(new Passage());
            }
        }
        Generate();
    }
}

/// <summary>
/// Stores the information of a passage.
/// </summary>
public class Passage {
//private:

    static int GetRandomPassageLength() {
        var addMapWidth = Map.Depth.FloorMult(Rules.MapWidthByLevel);
        return Random.Shared.Next(Rules.MapLengthMin, Rules.MapLengthMin + addMapWidth);
    }

    List<int> _doorIndices = new();

    void PlaceDoorAtRandom() {
        if (ConnectedPassages.Count >= Length)
            return;
        var randomPos = 0;
        do {
            randomPos = Random.Shared.Next(0, Length);
        } while (_doorIndices.Contains(randomPos));
        _doorIndices.Add(randomPos);
    }



//public:

    public List<Passage> ConnectedPassages {
        get;
        private init;
    } = new();

    public int Length {
        get;
        private init;
    } = 0;

    public Passage() {
        Length = GetRandomPassageLength();
    }

    /// <summary>
    /// Connects this passage to another passage.
    /// It also places door at random position for both passages.
    /// </summary>
    public void ConnectPassage(Passage value) {
        this.ConnectedPassages.Add(value);
        value.ConnectedPassages.Add(this);
        this.PlaceDoorAtRandom();
        value.PlaceDoorAtRandom();
    }
}
