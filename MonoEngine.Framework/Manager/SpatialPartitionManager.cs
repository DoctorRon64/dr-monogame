using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using MonoEngine.Framework.components;

namespace MonoEngine.Framework.Manager
{
    public class SpatialPartitionManager : BaseSingleton<SpatialPartitionManager>
    {
        public static Dictionary<Point, List<Entity>> Cells = new();
        private static int CellSize = 128;

        public static void Rebuild()
        {
            Cells.Clear();

            foreach (var entity in SceneManager.Instance.Entities)
            {
                Transform? transform = entity.GetComponent<Transform>();
                Collider? collider = entity.GetComponent<Collider>();

                Rectangle worldBounds = new(
                    (int)(transform.Position.X + collider.Bounds.X),
                    (int)(transform.Position.Y + collider.Bounds.Y),
                    (int)collider.Bounds.Width,
                    (int)collider.Bounds.Height
                );

                Point min = new(worldBounds.Left / CellSize, worldBounds.Top / CellSize);
                Point max = new(worldBounds.Right / CellSize, worldBounds.Bottom / CellSize);

                for (int x = min.X; x <= max.X; x++)
                {
                    for (int y = min.Y; y <= max.Y; y++)
                    {
                        var cell = new Point(x, y);
                        if (!Cells.ContainsKey(cell)) Cells[cell] = new();
                        Cells[cell].Add(entity);
                    }
                }
            }
        }

        public static IEnumerable<Entity> Query(Rectangle area)
        {
            HashSet<Entity> result = new();
            Point min = new(area.Left / CellSize, area.Top / CellSize);
            Point max = new(area.Right / CellSize, area.Bottom / CellSize);

            for (int x = min.X; x <= max.X; x++)
            {
                for (int y = min.Y; y <= max.Y; y++)
                {
                    var cell = new Point(x, y);
                    if (Cells.TryGetValue(cell, out var list))
                    {
                        foreach (var e in list) result.Add(e);
                    }
                }
            }

            return result;
        }
    }

}

