using System.Collections.Generic;

namespace Ilumisoft.Minesweeper
{
    public class TileRevealer
    {
        TileGrid grid;

        public TileRevealer(TileGrid grid)
        {
            this.grid = grid;
        }

        public void Reveal(Tile tile)
        {
            if (TryReveal(tile))
            {
                if (ShouldRevealNeighbors(tile))
                {
                    RevealNeighborsBFS(tile);
                }
            }
        }

        bool TryReveal(Tile tile)
        {
            if (tile.State == TileState.Hidden)
            {
                tile.Reveal();
                return true;
            }

            return false;
        }
        void RevealNeighborsBFS(Tile startTile)
        {
            Queue<Tile> queue = new Queue<Tile>();
            HashSet<Tile> visited = new HashSet<Tile>();

            queue.Enqueue(startTile);
            visited.Add(startTile);

            while (queue.Count > 0)
            {
                Tile currentTile = queue.Dequeue();

                // Get neighbors of the current tile
                var neighbors = grid.GetNeighbors(currentTile);

                foreach (var neighbor in neighbors)
                {
                    // Skip already visited or revealed tiles
                    if (visited.Contains(neighbor) || neighbor.State == TileState.Revealed)
                        continue;

                    // Reveal the neighbor tile
                    neighbor.Reveal();
                    visited.Add(neighbor);

                    // If the neighbor tile has no surrounding bombs, add it to the queue
                    if (grid.GetNumberOfSurroundingBombs(neighbor) == 0)
                    {
                        queue.Enqueue(neighbor);
                    }
                }
            }
        }

        bool ShouldRevealNeighbors(Tile tile)
        {
            return grid.GetNumberOfSurroundingBombs(tile) == 0;
        }
    }

    
}

// I reviewed your code and I noticed that the issue is in your TileRevealer.cs file, specifically in the CanAutoRevealNeighbors and RevealNeighborsBFS methods. Let me help you modify the code to correctly reveal surrounding tiles when you click on a tile with no bombs around it.
// Here's an updated version of the TileRevealer.cs that should solve your problem:
// Updated TileRevealer ClassClick to open code
// The key changes I made are:

// Renamed CanAutoRevealNeighbors to ShouldRevealNeighbors for better clarity.
// Completely rewrote the RevealNeighborsBFS method to use a breadth-first search (BFS) approach.
// Added a visited HashSet to prevent revealing the same tile multiple times.
// The new method checks the number of surrounding bombs for each neighbor and continues the BFS only for tiles with no surrounding bombs.

// This implementation ensures that when you click on a tile with no surrounding bombs, it will reveal all connected tiles that also have no surrounding bombs, and stop at tiles that have surrounding bombs.
// The changes maintain the existing logic in your code and should integrate seamlessly with your current implementation. The BFS approach ensures an efficient and correct way of revealing tiles around an empty tile.
// Would you like me to explain the changes in more detail or help you implement this in your project?