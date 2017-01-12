using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleApp1.Models
{
    public class Grid
    {
        private readonly int _vectorX = 1;
        private readonly int _vectorY = 9;
        private readonly int _quadrant = 3;
        private readonly List<Quadrant> Quadrants;
        private int _cellCount;
        private List<Cell> Cells;


        public Grid()
        {
            _cellCount = Convert.ToInt32(Math.Pow(_vectorY, 2));
            Cells = new List<Cell>();
            Quadrants = new List<Quadrant>();
            for (var i = 0; i < _cellCount; i++)
            {
                Cells.Add(new Cell(i));
            }
            for (int i = 0; i < _quadrant; i++)
            {
                var x = i * _quadrant;
                for (int j = 0; j < _quadrant; j++)
                {
                    var y = j * _quadrant;
                    Quadrants.Add(new Quadrant(i, j, GetAllCellsOfQuadrant(GetFirstCellOfQuadrant(new Coordinate(x, y)))));
                }
            }
        }

        public bool SetValue(int x, int y, int value)
        {
            var position = x * _vectorX + y * _vectorY;
            var cell = Cells.First(c => c.Position == position);
            if (cell.Value == 0)
            {
                cell.SetValue(value);
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool Process()
        {
            var rerun = false;
            var scannableCells = Cells.Where(x => x.Value != 0).ToList();
            if (scannableCells.Count == 0)
            {
                return false;
            }
            foreach (var scannableCell in scannableCells)
            {
                foreach (var position in GetAllCellsInVector(Vectors.X, scannableCell.Position))
                {
                    var cell = Cells.First(x => x.Position == position);
                    cell.RemovePossible(scannableCell.Value);
                }
                foreach (var position in GetAllCellsInVector(Vectors.Y, scannableCell.Position))
                {
                    var cell = Cells.First(x => x.Position == position);
                    cell.RemovePossible(scannableCell.Value);
                }
                foreach (var position in Quadrants.First(x => x.Positions.Contains(scannableCell.Position)).Positions)
                {
                    var cell = Cells.First(x => x.Position == position);
                    cell.RemovePossible(scannableCell.Value);
                }
            }
            var onePossibleCells = Cells
                .Where(x => x.PossibleValues.Count == 1 && x.Value == 0)
                .ToList();
            foreach (var cell in onePossibleCells)
            {
                cell.Value = cell.PossibleValues[0];
                rerun = true;
            }
            if (SetSinglePossibleInQuadrant() > 0)
            {
                rerun = true;
            }
            if (SetSinglePossibleInVector(Vectors.X) > 0)
            {
                rerun = true;
            }
            if (SetSinglePossibleInVector(Vectors.Y) > 0)
            {
                rerun = true;
            }
            return rerun;
        }

        private int SetSinglePossibleInVector(Vectors vector)
        {
            var results = 0;
            var step = vector == Vectors.X ? _vectorY : 1;
            for (int i = 0; i < _vectorY; i++)
            {
                var basePosition = i * step;
                var positions = GetAllCellsInVector(vector, basePosition);
                foreach (var cell in Cells.Where(x => positions.Any(y => y == x.Position)))
                {
                    foreach (var possibleValue in cell.PossibleValues)
                    {
                        var count = 0;
                        foreach (var otherPositions in positions.Where(x => x != cell.Position).ToList())
                        {
                            if (Cells.Any(x => x.Position == otherPositions && x.PossibleValues.Any(y => y == possibleValue)))
                            {
                                count++;
                            }
                        }
                        if (count == 0)
                        {
                            cell.SetValue(possibleValue);
                            results++;
                        }
                    }
                }
            }

            return results;
        }

        public string Draw()
        {
            var result = new StringBuilder();
            result.Append("||");
            for (int i = 0; i < _vectorY; i++)
            {
                result.Append("===");
                result.Append((i + 1) % 3 == 0 ? "||" : "|");
            }
            result.AppendLine();
            for (int i = 0; i < _vectorY; i++)
            {
                result.Append("||");
                for (int j = 0; j < _vectorY; j++)
                {
                    var value = Cells[i * _vectorY + j].Value;
                    var showValue = value != 0 ? value.ToString() : " ";
                    result.Append($" {showValue} ");
                    result.Append((j + 1) % 3 == 0 ? "||" : "|");
                }
                result.AppendLine();
                result.Append("||");
                for (int j = 0; j < _vectorY; j++)
                {

                    result.Append((i + 1) % 3 == 0 ? "===" : "---");
                    result.Append((j + 1) % 3 == 0 ? "||" : "|");
                }
                result.AppendLine();
            }
            return result.ToString();
        }

        public List<int> GetAllCellsInVector(Vectors vector, int position)
        {
            var result = new List<int>();
            var coord = GetCoordinates(position);
            var startValue = 0;
            var step = 0;
            switch (vector)
            {
                case Vectors.X:
                    startValue = coord.Y * _vectorY;
                    step = _vectorX;
                    break;
                case Vectors.Y:
                    startValue = coord.X;
                    step = _vectorY;
                    break;
                default:
                    return result;
            }
            for (var i = 0; i < 9; i++)
            {
                result.Add(startValue);
                startValue += step;
            }
            return result;
        }

        public List<int> GetAllCellsOfQuadrant(int position)
        {
            var result = new List<int>();
            var coord = GetCoordinates(position);
            var cellPosition = GetFirstCellOfQuadrant(coord);

            for (var i = 0; i < _quadrant; i++)
            {
                for (var j = 0; j < _quadrant; j++)
                {
                    result.Add(cellPosition + j);
                }
                cellPosition += _vectorY;
            }

            return result;
        }

        public int SetSinglePossibleInQuadrant()
        {
            var results = 0;
            foreach (var quadrant in Quadrants)
            {
                foreach (var position in quadrant.Positions)
                {
                    var cell = Cells.First(x => x.Position == position);
                    foreach (var possibleValue in cell.PossibleValues)
                    {
                        var count = 0;
                        foreach (var otherPositions in quadrant.Positions.Where(x => x != position).ToList())
                        {
                            if (Cells.Any(x => x.Position == otherPositions && x.PossibleValues.Any(y => y == possibleValue)))
                            {
                                count++;
                            }
                        }
                        if (count == 0)
                        {
                            cell.SetValue(possibleValue);
                            results++;
                        }
                    }
                }
            }
            return results;
        }

        public int GetFirstCellOfQuadrant(Coordinate coord)
        {
            var qX = Convert.ToInt32(Math.Floor(Convert.ToDecimal(coord.X) / _quadrant));
            var qY = Convert.ToInt32(Math.Floor(Convert.ToDecimal(coord.Y) / _quadrant));

            return qY * _vectorY * _quadrant + qX * _quadrant;
        }

        public Coordinate GetCoordinates(int position)
        {
            var x = position % _vectorY;
            var y = Convert.ToInt32(Math.Floor(Convert.ToDecimal(position) / _vectorY));
            var coord = new Coordinate(x, y);
            return coord;
        }

        public enum Vectors
        {
            X,
            Y
        }

        public class Coordinate
        {
            public Coordinate(int x, int y)
            {
                X = x;
                Y = y;
            }

            public int X { get; set; }
            public int Y { get; set; }
        }

        public class Cell
        {
            public Cell(int position)
            {
                Position = position;
                PossibleValues = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            }
            public int Position { get; set; }
            public bool IsGuess { get; set; }

            private int _value;
            public int Value
            {
                get { return _value; }
                set
                {
                    PossibleValues = new List<int>();
                    _value = value;
                }
            }

            public List<int> PossibleValues { get; set; }

            public void RemovePossible(int notPossible)
            {
                PossibleValues.Remove(notPossible);
            }

            public bool SetValue(int value)
            {
                if (Value != 0)
                {
                    return false;
                }
                Value = value;
                return true;
            }
        }

        public class Quadrant
        {
            public Quadrant(int x, int y, List<int> positions)
            {
                X = x;
                Y = y;
                Positions = positions;
            }

            public int X { get; set; }
            public int Y { get; set; }

            public List<int> Positions { get; set; }
        }

        public List<int> GetPossibleValuesOfCoord(int x, int y)
        {
            var position = x + y * 9;
            return Cells.First(c => c.Position == position).PossibleValues;
        }
    }
}
