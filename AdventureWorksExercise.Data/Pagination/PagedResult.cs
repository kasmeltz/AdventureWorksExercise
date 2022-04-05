namespace AdventureWorksExercise.Data.Pagination
{
    public class PagedResult<T> : PagedQuery
    {
        public IEnumerable<T> Records { get; set; }
    }
}
