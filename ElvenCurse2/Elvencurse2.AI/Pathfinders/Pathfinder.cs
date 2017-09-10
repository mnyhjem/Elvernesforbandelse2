using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Elvencurse2.AI.Pathfinders
{
    public static class Pathfinder
    {
        /// <summary>
        /// Meget inspriration fra Peter Blain
        /// http://xfleury.github.io/graphsearch.html
        /// </summary>
        /// <param name="map"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="start"></param>
        /// <param name="destination"></param>
        /// <returns></returns>
        public static List<Point> Run(int[,] map, int width, int height, Point start, Point destination)
        {
            var openList = new PriorityQueue<AStar<Point, Cost>.Node>();
            var closedList = new Dictionary<Point, Cost>();

            var solution = new List<Point>();

            var mapSolver = new MapSolver();

            if (start.X == destination.X && start.Y == destination.Y)
            {
                solution.Add(destination);
                return solution;
            }

            mapSolver.Graph(map, width, height, start, destination, openList, closedList);
            if (!mapSolver.Solution.HasValue)
            {
                return null;
                //throw new Exception("No solution");
            }

            var cost = mapSolver.Solution.Value.Cost;
            do
            {
                var position = mapSolver.ToPosition(cost.ParentIndex);
                cost = closedList[position];
                //bitmap.SetPixel(position.X, position.Y, Color.Green);
                solution.Add(new Point(position.X, position.Y));
            }
            while (cost.ParentIndex >= 0);

            return solution;
        }
    }

    public class MapSolver : AStar<Point, Cost>
    {
        private const int BaseOrthogonalCost = 5;
        private const int BaseDiagonalCost = 7;

        private int[,] _map;
        private Point _destination;
        private Dictionary<Point, Cost> _closedList;
        private int _height;
        private int _width;

        public Node? Solution { get; set; }

        public void Graph(
            int[,] map,
            int width,
            int height,
            Point start,
            Point destination,
            PriorityQueue<Node> openList,
            Dictionary<Point, Cost> closedList)
        {
            _map = map;
            _width = width;
            _height = height;
            _closedList = closedList;
            _destination = destination;
            Graph(new Node(start, new Cost(-1, 0, GetDistance(Point.Zero, _destination))), openList, closedList);
        }

        private int ToIndex(Point position)
        {
            return position.Y * _width + position.X;
        }

        public Point ToPosition(int index)
        {
            return new Point(index % _width, index / _width);
        }

        protected override void AddNeighbours(Node node, PriorityQueue<Node> openList)
        {
            var parentIndex = ToIndex(node.Position);
            for (var x = -1; x <= 1; x++)
            {
                for (var y = -1; y <= 1; y++)
                {
                    if (!(x == 0 && y == 0))
                    {
                        var newPos = new Point(node.Position.X + x, node.Position.Y + y);
                        if (newPos.X >= 0 && newPos.X < _width && newPos.Y >= 0 && newPos.Y < _height)
                        {
                            if (_map[newPos.X, newPos.Y] != 0)
                            {
                                var distanceCost = node.Cost.DistanceTravelled + ((x == 0 || y == 0) ? BaseOrthogonalCost : BaseDiagonalCost);
                                openList.Insert(new Node(newPos, new Cost(parentIndex, distanceCost, distanceCost + GetDistance(newPos, _destination))));
                            }
                        }
                    }
                }
            }

        }

        private static int GetDistance(Point source, Point destination)
        {
            var dx = Math.Abs(destination.X - source.X);
            var dy = Math.Abs(destination.Y - source.Y);
            var diagonal = Math.Min(dx, dy);
            var orthogonal = dx + dy - 2 * diagonal;
            return diagonal * BaseDiagonalCost + orthogonal * BaseOrthogonalCost;
        }

        protected override bool IsDestination(Point position)
        {
            var isSolved = position == _destination;
            if (isSolved)
            {
                Solution = new Node(position, _closedList[position]);
            }
            return isSolved;
        }
    }

    public struct Cost : IComparable<Cost>
    {
        public readonly int ParentIndex;
        public readonly int DistanceTravelled; /*g(x)*/
        public readonly int TotalCost; /*f(x)*/

        public Cost(int parentIndex, int distanceTravelled, int totalCost)
        {
            ParentIndex = parentIndex;
            DistanceTravelled = distanceTravelled;
            TotalCost = totalCost;
        }
        public int CompareTo(Cost other) { return TotalCost.CompareTo(other.TotalCost); }
    }

    public abstract class AStar<TKey, TValue> where TValue : IComparable<TValue>
    {
        protected void Graph(Node start, PriorityQueue<Node> openList, Dictionary<TKey, TValue> closedList)
        {
            openList.Insert(start);
            while (openList.Count > 0)
            {
                var node = openList.RemoveRoot();
                if (closedList.ContainsKey(node.Position))
                {
                    continue;
                }

                closedList.Add(node.Position, node.Cost);

                if (IsDestination(node.Position))
                {
                    return;
                }

                AddNeighbours(node, openList);
            }
        }

        protected abstract void AddNeighbours(Node node, PriorityQueue<Node> openList);
        protected abstract bool IsDestination(TKey position);

        public struct Node : IComparable<Node>
        {
            public readonly TKey Position;
            public readonly TValue Cost;
            public Node(TKey position, TValue cost)
            {
                Position = position;
                Cost = cost;
            }

            public int CompareTo(Node other)
            {
                return Cost.CompareTo(other.Cost);
            }
        }
    }

    public class PriorityQueue<T> where T : IComparable<T>
    {
        private readonly List<T> _items = new List<T>();

        public int Count
        {
            get
            {
                return _items.Count;
            }
        }

        public void Clear()
        {
            _items.Clear();
        }

        public void Insert(T item)
        {
            int i = _items.Count;
            _items.Add(item);
            while (i > 0 && _items[(i - 1) / 2].CompareTo(item) > 0)
            {
                _items[i] = _items[(i - 1) / 2];
                i = (i - 1) / 2;
            }
            _items[i] = item;
        }

        public T Peek()
        {
            return _items[0];
        }

        public T RemoveRoot()
        {
            T firstItem = _items[0];
            T tempItem = _items[_items.Count - 1];
            _items.RemoveAt(_items.Count - 1);
            if (_items.Count > 0)
            {
                int i = 0;
                while (i < _items.Count / 2)
                {
                    int j = (2 * i) + 1;
                    if ((j < _items.Count - 1) && (_items[j].CompareTo(_items[j + 1]) > 0))
                    {
                        ++j;
                    }

                    if (_items[j].CompareTo(tempItem) >= 0)
                    {
                        break;
                    }
                    _items[i] = _items[j];
                    i = j;
                }
                _items[i] = tempItem;
            }
            return firstItem;
        }
    }
}
