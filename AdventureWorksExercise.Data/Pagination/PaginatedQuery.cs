using System.Text;

namespace AdventureWorksExercise.Data.Pagination
{
    public class PaginatedQuery
    {
        #region Constructors

        public PaginatedQuery()
        {
            SortTerms = new List<SortTerm>();
            SearchValues = new List<object>();
            SortStringBuilder = new StringBuilder();
            SearchStringBuilder = new StringBuilder();
        }

        #endregion

        #region Members

        public int Offset { get; set; }

        public int Limit { get; set; }

        protected List<SortTerm> SortTerms { get; set; }

        public List<object> SearchValues { get; set; }

        protected StringBuilder SearchStringBuilder { get; set; }

        protected StringBuilder SortStringBuilder { get; set; }

        public string SortString { get { return SortStringBuilder.ToString(); } }

        public string SearchString {  get { return SearchStringBuilder.ToString(); } }

        #endregion

        #region Public Methods

        public void ResetAll()
        {
            ResetSearch();
            ResetSort();
        }

        public void ResetSearch()
        {
            SearchStringBuilder
                .Clear();

            SearchValues
                .Clear();
        }

        public void ResetSort()
        {
            SortStringBuilder
                .Clear();

            SortTerms
              .Clear();
        }

        public void AddSortTerm(string key, SortDirection direction)
        {
            if (SortStringBuilder.Length > 0)
            {
                SortStringBuilder.Append(", ");
            }

            SortStringBuilder
                .Append(key);

            if (direction == SortDirection.Descending)
            {
                SortStringBuilder
                    .Append(" desc");
            }

            SortTerms
                .Add(new SortTerm { Key = key, Direction = direction });
        }

        public void Equals(string property, object value)
        {
            AddQuery(property, value, (o) =>
            {
                o.Append(property);

                o.Append("=@");

                o.Append(SearchValues.Count);
            });
        }

        public void NotEquals(string property, object value)
        {
            AddQuery(property, value, (o) =>
            {
                o.Append(property);

                o.Append("!=@");

                o.Append(SearchValues.Count);
            });
        }

        public void IsNull(string property)
        {
            AddQuery(property, null, (o) =>
            {
                o.Append("it.");

                o.Append(property);

                o.Append(" == null");
            }, true);
        }

        public void IsNotNull(string property)
        {
            AddQuery(property, null, (o) =>
            {
                o.Append("it.");

                o.Append(property);

                o.Append(" != null");
            }, true);
        }

        public void LessThan(string property, object value)
        {
            AddQuery(property, value, (o) =>
            {
                o.Append(property);

                o.Append("<@");

                o.Append(SearchValues.Count);
            });
        }

        public void LessThanOrEquals(string property, object value)
        {
            AddQuery(property, value, (o) =>
            {
                o.Append(property);

                o.Append("<=@");

                o.Append(SearchValues.Count);
            });
        }

        public void GreaterThan(string property, object value)
        {
            AddQuery(property, value, (o) =>
            {
                o.Append(property);

                o.Append(">@");

                o.Append(SearchValues.Count);
            });
        }

        public void GreaterThanOrEquals(string property, object value)
        {
            AddQuery(property, value, (o) =>
            {
                o.Append(property);

                o.Append(">=@");

                o.Append(SearchValues.Count);
            });
        }

        public void Contains(string property, object value)
        {
            AddQuery(property, value, (o) =>
            {
                o.Append("it.");

                o.Append(property);

                o.Append(".Contains(@");

                o.Append(SearchValues.Count);

                o.Append(")");
            });
        }

        public void StartsWith(string property, object value)
        {
            AddQuery(property, value, (o) =>
            {
                o.Append("it.");

                o.Append(property);

                o.Append(".StartsWith(@");

                o.Append(SearchValues.Count);

                o.Append(")");
            });
        }

        public void FromString(string query)
        {
            AddQuery(query, null, (o) =>
            {
                o.Append(query);
            }, true);
        }

        #endregion

        #region Protected Methods

        protected void AddQuery(string property, object? value, Action<StringBuilder> queryFunction, bool useNull = false)
        {
            if (string.IsNullOrEmpty(property))
            {
                return;
            }

            if (value == null && !useNull)
            {
                return;
            }

            if (SearchStringBuilder.Length > 0)
            {
                SearchStringBuilder.Append(" and ");
            }

            queryFunction(SearchStringBuilder);

            if (value != null)
            {
                SearchValues
                    .Add(value);
            }
        }

        #endregion
    }
}
