using AdventureWorksExercise.Data.Pagination;

namespace AdventureWorksExercise.WebAPI.ViewModels.Filtering
{
    public abstract class PaginatedFilter
    {
        #region Members

        public int? Offset { get; set; }

        public int? Limit { get; set; }

        public string? SortBy { get; set; }

        public string? Search { get; set; }

        protected abstract Dictionary<string, string> ModelFieldTranslations { get; }

        #endregion

        #region Public Methods

        public string TranslateFieldToModel(string fieldName)
        {
            if (string.IsNullOrEmpty(fieldName))
            {
                return string.Empty;
            }

            if (!ModelFieldTranslations.ContainsKey(fieldName))
            {
                return fieldName;
            }

            return ModelFieldTranslations[fieldName]
                .ToLower();
        }

        #endregion

        #region Protected Methods

        

        protected void SetPaginatedQueryTerm(PaginatedQuery paginatedQuery, string termAccessor, string? value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return;
            }

            string[] tokens = value
                .Split(':');

            if (tokens.Length != 1 && tokens.Length != 2)
            {
                throw new ArgumentException("Filter terms must be in the format [COMMAND:VALUE]AND/OR[COMMAND:VALUE]");
            }

            var command = tokens[0]
                .ToLower();

            string objectValue = string.Empty;
            if (tokens.Length == 2)
            {
                objectValue = tokens[1]
                    .ToLower();
            }
            
            switch (command)
            {
                case "cn":
                case "contains":
                    paginatedQuery
                        .Contains(termAccessor, value);
                    break;
                case "sw":
                case "startswith":
                    paginatedQuery
                        .StartsWith(termAccessor, value);
                    break;
                case "eq":
                case "equals":
                    paginatedQuery
                       .Equals(termAccessor, value);
                    break;
                case "neq":
                case "notequals":
                    paginatedQuery
                       .NotEquals(termAccessor, value);
                    break;

                /*

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

                */
                default:
                    throw new ArgumentException($"Filter command '{command}' is not a recognized filter command");
            }
        }

        #endregion
    }
}