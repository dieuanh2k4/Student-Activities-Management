using System.Collections.Generic;

namespace StudentActivities.src.Dtos.TrainingScores
{
    public class PagedTrainingScoreDto
    {
        public List<TrainingScoreDto> Items { get; set; } = new List<TrainingScoreDto>();
        public int TotalCount { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
    }
}
