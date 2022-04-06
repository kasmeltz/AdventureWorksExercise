namespace AdventureWorksExercise.WebAPI.ViewModels.Filtering
{
    public class PaginatedFilter
    {
        public int? Offset { get; set; }

        public int? Limit { get; set; }

        public string? SortBy { get; set; }

        public string? Search { get; set; }
    }
}
