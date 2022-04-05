namespace AdventureWorksExercise.Data.Pagination
{
    public class PagedQuery
    {
        public int PageSize { get; set; }

        public int Page { get; set; }

        public SortDirection SortDirection { get; set; }

        public int TotalRecords { get; set; }
    }
}
