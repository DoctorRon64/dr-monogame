using System.Collections.Generic;
using Microsoft.Xna.Framework;
using ProDevs.Framework.ECS.Components;

namespace ProDevs {
    public class SpatialGrid {
        private Dictionary<Point, List<Collider>> grid = new();
        private int cellSize;

        public SpatialGrid(int cellSize) {
            this.cellSize = cellSize;
        }

        private Point GetCell(Vector2 position) {
            return new Point((int)(position.X / cellSize), (int)(position.Y / cellSize));
        }

        public void AddCollider(Collider collider) {
            Point cell = GetCell(collider.Bounds.Location.ToVector2());
            if (!grid.ContainsKey(cell))
                grid[cell] = new List<Collider>();
            grid[cell].Add(collider);
        }

        public List<Collider> GetNearbyColliders(Collider collider) {
            Point cell = GetCell(collider.Bounds.Location.ToVector2());
            List<Collider> nearby = new();
        
            for (int x = -1; x <= 1; x++) {
                for (int y = -1; y <= 1; y++) {
                    Point neighborCell = new Point(cell.X + x, cell.Y + y);
                    if (grid.ContainsKey(neighborCell))
                        nearby.AddRange(grid[neighborCell]);
                }
            }

            return nearby;
        }

        public void Clear() {
            grid.Clear();
        }
    }
}
