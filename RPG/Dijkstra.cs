using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace RPG
{
    public static class DijkstraPathFinder
    {
        public static IEnumerable<PathWithCost> GetPathsByDijkstra(Map map,
            IEnumerable<Point> targets, Point start)
        {
            Point? toOpen = start;
            var tracks = new Dictionary<Point, PathWithCost>();
            tracks[start] = new PathWithCost(0, new Point[] { start });
            var opened = new HashSet<Point>() { start };
            var wantToVisit = new HashSet<Point?>();
            var targetSet = new HashSet<Point>(targets);

            while (true)
            {
                if (toOpen == null) break;
                if (targetSet.Contains(toOpen.Value))
                    yield return tracks[toOpen.Value];
                OpenPoints(toOpen, map, opened, wantToVisit, tracks, targetSet);
                if (wantToVisit.Count == 0) break;
                toOpen = wantToVisit
                    .OrderBy(x => tracks[x.Value].Cost)
                    .First();
                opened.Add(toOpen.Value);

                OpenPoints(toOpen, map, opened, wantToVisit, tracks, targetSet);
            }
        }

        public static void OpenPoints(Point? toOpen, Map map, HashSet<Point> opened,
            HashSet<Point?> wantToVisit, Dictionary<Point, PathWithCost> tracks, HashSet<Point> targetSet)
        {
            wantToVisit.Remove(toOpen.Value);
            foreach (var dir in PossibleDirections)
            {
                var nextPoint = toOpen + dir;
                if (map.InBounds(nextPoint) && !opened.Contains(nextPoint.Value) &&
                    (map[nextPoint].Content.Entity == null || targetSet.Contains((Point)nextPoint)))
                {
                    wantToVisit.Add(nextPoint);
                    var currentCost = map[nextPoint].Content.Cost
                        + tracks[toOpen.Value].Cost;
                    if (!tracks.ContainsKey(nextPoint.Value) || currentCost < tracks[nextPoint.Value].Cost)
                        tracks[nextPoint.Value] = new PathWithCost(currentCost, tracks[toOpen.Value].Path
                                .Append(nextPoint.Value)
                                .ToArray());
                }
            }
        }

        public static readonly IReadOnlyList<Size> PossibleDirections = new List<Size> {
            new Size(0, -1), new Size(0, 1), new Size(-1, 0), new Size(1, 0)
        };
    }

    public class PathWithCost
    {
        public List<Point> Path { get; }
        public int Cost { get; }

        public PathWithCost(int cost, params Point[] path)
        {
            Cost = cost;
            Path = path.ToList();
        }

        public Point Start => Path.First();
        public Point End => Path.Last();
        public IEnumerable<Point> Endpoints => new[] { Start, End };

        public override string ToString()
        {
            var result = $"Cost: {Cost}, Path: {string.Join(" ", Path.Select(p => p.ToString()))}";
            return result;
        }
    }

    public static class GreedyPathFinder
    {
        public static List<Point> FindPathToCompleteGoal(Map map, List<Point> goals, Entity entity)
        {
            var notChecked = new HashSet<Point>(goals);
            var position = entity.Position;
            var energy = entity.MaxEnergy;
            var result = new List<Point>();
            for (var i = 0; i < goals.Count; i++)
            {
                var path = DijkstraPathFinder.GetPathsByDijkstra(map, goals, position)
                    .FirstOrDefault();
                if (path == null || path.Cost > energy)
                {
                    result.AddRange(path.Path);
                    return result;
                }
                energy -= path.Cost;
                position = path.End;
                result.AddRange(path.Path.Skip(1));
                notChecked.Remove(position);
            }
            return result;
        }
    }
}
