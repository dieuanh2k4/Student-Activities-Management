using System.Threading.Tasks;
using StudentActivities.src.Dtos.TrainingScores;

namespace StudentActivities.src.Services.Interfaces
{
    public interface ITrainingScoreService
    {
        // Chức năng 1: Xem và chỉnh sửa điểm rèn luyện từng record
        Task<PagedTrainingScoreDto> GetAllTrainingScoresAsync(TrainingScoreFilterDto filter);
        Task<TrainingScoreDto?> GetTrainingScoreByIdAsync(int id);
        Task<TrainingScoreDto> UpdateTrainingScoreAsync(int id, UpdateTrainingScoreDto dto);

        // Chức năng 2: Cập nhật điểm rèn luyện hàng loạt cho sự kiện/CLB
        Task<BulkUpdateResultDto> UpdateEventTrainingPointsAsync(int eventId, UpdateEventTrainingPointsDto dto);
        Task<BulkUpdateResultDto> UpdateClubTrainingPointsAsync(int clubId, UpdateClubTrainingPointsDto dto);
    }
}
