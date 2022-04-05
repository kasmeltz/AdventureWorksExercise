namespace AdventureWorksExercise.Data.Pagination
{
    public class PaginatedResult<T>
    {
        #region Constructors

        public PaginatedResult(PaginatedQuery query)
        {
            Query = query;
        }

        public int Page { get; set; }

        #endregion
        public PaginatedQuery Query { get; set; }

        public IEnumerable<T>? Records { get; set; }

        public int TotalRecordCount { get; set; }

        public int PageCount
        {
            get
            {
                if (TotalRecordCount == 0)
                {
                    return 1;
                }

                return ((TotalRecordCount - 1) / Query.PageSize) + 1;
            }
        }
    }
}
