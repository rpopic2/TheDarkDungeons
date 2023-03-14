public class Depth {
// private:

    const int _ROW = 3;
    const int _COLUMN = 3;
    List<List<Passage>> _passageMatrix;
    (int, int)[] _directions = new (int, int)[] {
        (0, 1), (1, 0), (0, -1), (-1, 0)
    };
    Random _random = new Random();

// public:

    public Depth() {
        // Initialize the passage matrix
        _passageMatrix = new(_ROW);
        for (int i = 0; i < _ROW; i++) {
            _passageMatrix.Add(new(_COLUMN));
            for (int j = 0; j < _COLUMN; j++) {
                _passageMatrix[i].Add(new Passage());
            }
        }

        var visited = new bool[_ROW, _COLUMN];

        var is_valid = (int row, int column) => {
            return row >= 0 && row < _ROW && column >= 0 && column < _COLUMN;
        };

        // preform dfs, but in random directions
        Queue<(int, int)> queue = new();
        queue.Enqueue((0, 0));

        Console.WriteLine("Start depth creation procedure");

        // connect passages
        while (queue.Count > 0) {
            (int row, int column) = queue.Dequeue();
            Passage passage = _passageMatrix[row][column];
            visited[row, column] = true;

            // get all viable directions
            List<(int, int)> viable_directions = new();
            foreach (var (rowoff, coloff) in _directions) {
                var nrow = row + rowoff;
                var ncol = column + coloff;
                if (is_valid(nrow, ncol) && !visited[nrow, ncol]) {
                    viable_directions.Add((nrow, ncol));
                }
            }

            if (viable_directions.Count == 0) {
                Console.WriteLine("No viable directions");
                continue;
            }
            int new_row, new_column;
            var next = _random.Next(viable_directions.Count);
            (new_row, new_column) = viable_directions[next];

            Console.WriteLine($"Passages connected: ({row}, {column}) and ({new_row}, {new_column})");
            Passage new_passage = _passageMatrix[new_row][new_column];
            passage.ConnectedPassages.Add(new_passage);
            new_passage.ConnectedPassages.Add(passage);
            queue.Enqueue((new_row, new_column));
        } // while

        Console.WriteLine("Connect unconnected passages");
        // connect unconnected passages
        for (int i = 0; i < _ROW; i++) {
            for (int j = 0; j < _COLUMN; j++) {
                if (visited[i, j]) continue;
                Passage passage = _passageMatrix[i][j];
                List<(int, int)> viable_directions = new();
                foreach (var (rowoff, coloff) in _directions) {
                    var nrow = i + rowoff;
                    var ncol = j + coloff;
                    if (is_valid(nrow, ncol) && _passageMatrix[nrow][ncol].ConnectedPassages.Count > 0) {
                        viable_directions.Add((nrow, ncol));
                    }
                }

                if (viable_directions.Count == 0) {
                    Console.WriteLine("No viable directions. This room is a dead room");
                    continue;
                }
                int new_row, new_column;
                var next = _random.Next(viable_directions.Count);
                (new_row, new_column) = viable_directions[next];

                Console.WriteLine($"Passages connected: ({i}, {j}) and ({new_row}, {new_column})");
                Passage new_passage = _passageMatrix[new_row][new_column];
                passage.ConnectedPassages.Add(new_passage);
                new_passage.ConnectedPassages.Add(passage);
            }
        } //for
    }
}

public class Passage {
    public List<Passage> ConnectedPassages { get; set; } = new();
}
