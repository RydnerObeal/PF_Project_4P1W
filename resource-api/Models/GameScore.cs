using System;

namespace resource_api.Models
{
    public class GameScore
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid PuzzleId { get; set; }
        public int Score { get; set; }
        public bool IsSolved { get; set; }
        public DateTime SolvedAt { get; set; }

        // Navigation property
        public Puzzle? Puzzle { get; set; }
    }
}
