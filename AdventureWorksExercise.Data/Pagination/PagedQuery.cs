namespace AdventureWorksExercise.Data.Pagination
{
    public class PagedQuery
    {
        #region Constructors

        public PagedQuery()
        {
            SortTerms = new List<SortTerm>();
        }

        #endregion

        #region Members

        public int PageSize { get; set; }

        public int Page { get; set; }

        public List<SortTerm> SortTerms { get; set; }

        #endregion
    }
}
