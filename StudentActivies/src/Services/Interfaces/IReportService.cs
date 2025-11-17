using StudentActivities.src.Dtos.Reports;

namespace StudentActivities.src.Services.Interfaces
{
    public interface IReportService
    {
        Task<List<TrainingScoreReportDto>> GetScoreByPeriodAsync(ScoreByPeriodFilterDto filter);
        Task<TrainingScoreReportDto> GetScoreByActivityAsync(ScoreByActivityFilterDto filter);
        Task<byte[]> ExportScoreByPeriodToCsvAsync(ScoreByPeriodFilterDto filter);
        Task<byte[]> ExportScoreByActivityToCsvAsync(ScoreByActivityFilterDto filter);
    }
}
