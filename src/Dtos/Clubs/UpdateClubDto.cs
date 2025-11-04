namespace StudentActivities.src.Dtos.Clubs
{
    public class UpdateClubDto
    {
        public string? Name { get; set; }
        public string? Thumbnail { get; set; }
        public string? Description { get; set; }
        public int? MaxCapacity { get; set; }
    }
}