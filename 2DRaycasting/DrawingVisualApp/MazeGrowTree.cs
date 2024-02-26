using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace DrawingVisualApp
{
    class GrowTree
    {
        enum CellState { Close, Open };
        class Cell
        {
            public Cell(Point currentPosition)
            {
                Visited = false;
                Position = currentPosition;
            }

            public CellState Left { get; set; }
            public CellState Right { get; set; }
            public CellState Bottom { get; set; }
            public CellState Top { get; set; }
            public Boolean Visited { get; set; }
            public Point Position { get; set; }
        }

        private Int32 _Width, _Height;
        private Cell[,] Cells;

        private int cellSize;

        public GrowTree(DrawingVisualClass g, int size)
        {
            cellSize = size;

            _Width = (int)g.Width / cellSize;
            _Height = (int)g.Height / cellSize;

            Initialized();
        }


        void Initialized()
        {
            Cells = new Cell[_Width, _Height];

            for (int y = 0; y < _Height; y++)
                for (int x = 0; x < _Width; x++)
                    Cells[x, y] = new Cell(new Point(x, y));

            Random rand = new Random();
            Int32 startX = rand.Next(_Width);
            Int32 startY = rand.Next(_Height);

            Stack<Cell> path = new Stack<Cell>();

            Cells[startX, startY].Visited = true;
            path.Push(Cells[startX, startY]);

            while (path.Count > 0)
            {
                Cell _cell = path.Peek(); // возвращает элемент в начале стека

                List<Cell> nextStep = new List<Cell>(); // Создается отдельный список хранения следующих ячеек (шагов)
                if (_cell.Position.X > 0 && !Cells[Convert.ToInt32(_cell.Position.X - 1), Convert.ToInt32(_cell.Position.Y)].Visited)
                    nextStep.Add(Cells[Convert.ToInt32(_cell.Position.X) - 1, Convert.ToInt32(_cell.Position.Y)]);
                if (_cell.Position.X < _Width - 1 && !Cells[Convert.ToInt32(_cell.Position.X) + 1, Convert.ToInt32(_cell.Position.Y)].Visited)
                    nextStep.Add(Cells[Convert.ToInt32(_cell.Position.X) + 1, Convert.ToInt32(_cell.Position.Y)]);
                if (_cell.Position.Y > 0 && !Cells[Convert.ToInt32(_cell.Position.X), Convert.ToInt32(_cell.Position.Y) - 1].Visited)
                    nextStep.Add(Cells[Convert.ToInt32(_cell.Position.X), Convert.ToInt32(_cell.Position.Y) - 1]);
                if (_cell.Position.Y < _Height - 1 && !Cells[Convert.ToInt32(_cell.Position.X), Convert.ToInt32(_cell.Position.Y) + 1].Visited)
                    nextStep.Add(Cells[Convert.ToInt32(_cell.Position.X), Convert.ToInt32(_cell.Position.Y) + 1]);

                /// За один проход может отработать несколько If.
                /// Соотвественно в nextStep сразу может добавиться несколько элементов.

                if (nextStep.Count() > 0)
                {
                    Cell next = nextStep[rand.Next(nextStep.Count())]; // случайно выбирается шаг из списка следующих шагов

                    // если выбран шаг движения по горизонтали
                    if (next.Position.X != _cell.Position.X)
                    {
                        if (_cell.Position.X - next.Position.X > 0)
                        {
                            // Если выбран шаг слева от текущей клетки
                            // Значение Open меняется сразу у обеих ячеек, текущей и шаговой.
                            _cell.Left = CellState.Open;
                            next.Right = CellState.Open;
                        }
                        else
                        {
                            // Если выбран шаг справа от текущей клетки
                            _cell.Right = CellState.Open;
                            next.Left = CellState.Open;
                        }
                    }

                    // если выбран шаг движения по вертикали
                    if (next.Position.Y != _cell.Position.Y)
                    {
                        if (_cell.Position.Y - next.Position.Y > 0)
                        {
                            _cell.Top = CellState.Open;
                            next.Bottom = CellState.Open;
                        }
                        else
                        {
                            _cell.Bottom = CellState.Open;
                            next.Top = CellState.Open;
                        }
                    }

                    next.Visited = true; // шаговая ячейка помечается как просмотреная
                    path.Push(next);
                }
                else
                {
                    path.Pop(); // Удаляется элемент вначале стека
                }
            } // while
        }

        public List<Boundary> GetWalls()
        {
            List<Boundary> walls = new List<Boundary>();
            
            for (int y = 0; y < _Height; y++)
                for (int x = 0; x < _Width; x++)
                {
                    if (Cells[x, y].Top == CellState.Close)
                        walls.Add(new Boundary(cellSize * x, cellSize * y, cellSize * x + cellSize, cellSize * y));

                    if (Cells[x, y].Left == CellState.Close)
                        walls.Add(new Boundary(cellSize * x, cellSize * y, cellSize * x, cellSize * y + cellSize));

                    if (Cells[x, y].Right == CellState.Close)
                        walls.Add(new Boundary(cellSize * x + cellSize, cellSize * y, cellSize * x + cellSize, cellSize * y + cellSize));

                    if (Cells[x, y].Bottom == CellState.Close)
                        walls.Add(new Boundary(cellSize * x, cellSize * y + cellSize, cellSize * x + cellSize, cellSize * y + cellSize));
                }
            return walls;
        }
    }
}
