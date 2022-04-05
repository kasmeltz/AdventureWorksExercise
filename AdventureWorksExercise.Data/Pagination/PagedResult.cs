namespace AdventureWorksExercise.Data.Pagination
{
    public class PagedResult<T>
    {
        public PagedQuery Query { get; set; }

        public IEnumerable<T> Records { get; set; }

        public int TotalRecordCount { get; set; }
    }
}
