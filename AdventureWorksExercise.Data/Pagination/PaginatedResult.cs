namespace AdventureWorksExercise.Data.Pagination
{
    public class PaginatedResult<T>
    {
        #region Constructors

        public PaginatedResult(PaginatedQuery query)
        {
            Query = query;
        }

        #endregion
        public PaginatedQuery Query { get; set; }

        public IEnumerable<T>? Records { get; set; }

        public int TotalRecordCount { get; set; }
    }
}
