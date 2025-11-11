using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudentActivities.src.Dtos.TrainingScores;
using StudentActivities.src.Services.Interfaces;

namespace StudentActivities.src.Controllers
{
    [ApiController]
    [Route("api/admin/training-scores")]
    [Authorize(Roles = "Admin")]
    public class TrainingScoreController : ApiControllerBase
    {
        private readonly ITrainingScoreService _trainingScoreService;

        public TrainingScoreController(
            ITrainingScoreService trainingScoreService,
            ILogger<TrainingScoreController> logger) : base(logger)
        {
            _trainingScoreService = trainingScoreService;
        }

        /// <summary>
        /// Xem danh sách điểm rèn luyện với filter (Admin only)
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAllTrainingScores([FromQuery] TrainingScoreFilterDto filter)
        {
            try
            {
                var result = await _trainingScoreService.GetAllTrainingScoresAsync(filter);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return ReturnException(ex);
            }
        }

        /// <summary>
        /// Xem chi tiết điểm rèn luyện (Admin only)
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTrainingScoreById(int id)
        {
            try
            {
                var result = await _trainingScoreService.GetTrainingScoreByIdAsync(id);

                if (result == null)
                {
                    return NotFound($"Không tìm thấy điểm rèn luyện với Id = {id}");
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                return ReturnException(ex);
            }
        }

        /// <summary>
        /// Cập nhật điểm rèn luyện (Admin only)
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTrainingScore(int id, [FromBody] UpdateTrainingScoreDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var result = await _trainingScoreService.UpdateTrainingScoreAsync(id, dto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return ReturnException(ex);
            }
        }

        /// <summary>
        /// Cập nhật điểm rèn luyện hàng loạt cho tất cả sinh viên đã tham dự sự kiện (Admin only)
        /// </summary>
        [HttpPut("events/{eventId}/bulk-update")]
        public async Task<IActionResult> UpdateEventTrainingPoints(int eventId, [FromBody] UpdateEventTrainingPointsDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var result = await _trainingScoreService.UpdateEventTrainingPointsAsync(eventId, dto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return ReturnException(ex);
            }
        }

        /// <summary>
        /// Cập nhật điểm rèn luyện hàng loạt cho tất cả thành viên câu lạc bộ (Admin only)
        /// </summary>
        [HttpPut("clubs/{clubId}/bulk-update")]
        public async Task<IActionResult> UpdateClubTrainingPoints(int clubId, [FromBody] UpdateClubTrainingPointsDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var result = await _trainingScoreService.UpdateClubTrainingPointsAsync(clubId, dto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return ReturnException(ex);
            }
        }
    }
}
