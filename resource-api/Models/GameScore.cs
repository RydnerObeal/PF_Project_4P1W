using System;

namespace resource_api.Models
{
    public class GameScore
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid PuzzleId { get; set; }
        public Guid PackId { get; set; } // Track which pack the puzzle belongs to
        public int Score { get; set; }
        public bool IsSolved { get; set; }
        public DateTime SolvedAt { get; set; }

        // Navigation properties
        public Puzzle? Puzzle { get; set; }
        public Pack? Pack { get; set; }
    }
}
