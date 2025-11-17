using System.ComponentModel.DataAnnotations;

namespace StudentActivities.src.Dtos.TrainingScores
{
    public class UpdateTrainingScoreDto
    {
        [Required(ErrorMessage = "Điểm rèn luyện là bắt buộc")]
        [Range(0, 100, ErrorMessage = "Điểm rèn luyện phải từ 0 đến 100")]
        public int Score { get; set; }
    }
}
