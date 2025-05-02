using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using MonoEngine.Framework.components;

namespace MonoEngine.Framework.Manager {
    public class SpatialPartitionManager : BaseSingleton<SpatialPartitionManager> {
        private static readonly Dictionary<Point, List<Entity>> cells = new();
        private const int cellSize = 128;

        public static void Rebuild() {
            cells.Clear();

            foreach (Entity entity in SceneManager.Instance.Entities) {
                Transform? transform = entity.GetComponent<Transform>();
                Collider? collider = entity.GetComponent<Collider>();

                Rectangle worldBounds = new(
                    (int)(transform.Position.X + collider.Bounds.X),
                    (int)(transform.Position.Y + collider.Bounds.Y),
                    (int)collider.Bounds.Width,
                    (int)collider.Bounds.Height
                );

                Point min = new(worldBounds.Left / cellSize, worldBounds.Top / cellSize);
                Point max = new(worldBounds.Right / cellSize, worldBounds.Bottom / cellSize);

                for (int x = min.X; x <= max.X; x++) {
                    for (int y = min.Y; y <= max.Y; y++) {
                        Point cell = new Point(x, y);
                        if (!cells.ContainsKey(cell)) cells[cell] = new();
                        cells[cell].Add(entity);
                    }
                }
            }
        }

        public static IEnumerable<Entity> Query(Rectangle area) {
            HashSet<Entity> result = new();
            Point min = new(area.Left / cellSize, area.Top / cellSize);
            Point max = new(area.Right / cellSize, area.Bottom / cellSize);

            for (int x = min.X; x <= max.X; x++) {
                for (int y = min.Y; y <= max.Y; y++) {
                    Point cell = new Point(x, y);
                    if (!cells.TryGetValue(cell, out var list)) continue;
                    foreach (Entity e in list) result.Add(e);
                }
            }

            return result;
        }
    }
}