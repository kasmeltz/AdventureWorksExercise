using AdventureWorksExercise.Data.Pagination;

namespace AdventureWorksExercise.WebAPI.ViewModels.Filtering
{
    /// <summary>
    /// Represents the input to an API request that describes how to filter the results.
    /// </summary>
    public abstract class PaginatedFilter
    {
        #region Members

        public int? Offset { get; set; }

        public int? Limit { get; set; }

        public string? Fields { get; set; }

        public string? SortBy { get; set; }

        public string? Search { get; set; }

        protected abstract Dictionary<string, string> ModelFieldTranslations { get; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Translates this PaginatedFilter into a PaginatedQuery that can be used with the DataServices layer.
        /// </summary>
        /// <returns>A PaginatedQuery with the information contained in the PaginatedFilter</returns>
        /// <exception cref="ArgumentException">Thrown if any of the filter parameters are invalid.</exception>
        public PaginatedQuery ToPaginatedQuery(int defaultLimit, int maxLimit)
        {
            if (Offset < 0)
            {
                throw new ArgumentException("Offset can not be less than 0");
            }

            if (Limit < 0)
            {
                throw new ArgumentException("Limit can not be less than 0");
            }

            if (Limit > maxLimit)
            {
                throw new ArgumentException($"Limit can not be greater than {maxLimit}");
            }

            int offset = 0;
            int limit = 0;

            if (Limit.HasValue)
            {
                limit = Limit.Value;
            }
            else
            {
                limit = defaultLimit;
            }

            if (Offset.HasValue)
            {
                offset = Offset.Value;
            }

            var paginatedQuery = new PaginatedQuery
            {
                Offset = offset,
                Limit = limit
            };

            string? fields = Fields?.ToLower();
            if (!string.IsNullOrEmpty(fields))
            {
                string[] fieldTokens = fields.Split(',');
                foreach (var fieldToken in fieldTokens)
                {
                    paginatedQuery
                        .SelectedFields
                        .Add(TranslateFieldToModel(fieldToken));
                }
            }

            string? sortBy = SortBy?.ToLower();
            if (!string.IsNullOrEmpty(sortBy))
            {
                string[] sortTerms = sortBy
                    .Split(',');

                if (sortTerms
                    .Any())
                {
                    foreach (var sortTerm in sortTerms)
                    {
                        var plusOrMins = sortTerm.Substring(0, 1);

                        if (plusOrMins != "-" &&
                            plusOrMins != "+")
                        {
                            throw new ArgumentException("Sort terms must start with + (ascending) or - (descending)");
                        }

                        var sortDirection = SortDirection.Ascending;

                        if (sortTerm
                            .StartsWith('-'))
                        {
                            sortDirection = SortDirection.Descending;
                        }

                        var cleanedSortTerm = sortTerm
                            .Substring(1, sortTerm.Length - 1);

                        cleanedSortTerm = TranslateFieldToModel(cleanedSortTerm);

                        paginatedQuery
                            .AddSortTerm(cleanedSortTerm, sortDirection);
                    }
                }
            }

            return paginatedQuery;
        }

        #endregion

        #region Protected Methods
        
        protected string TranslateFieldToModel(string fieldName)
        {
            if (string.IsNullOrEmpty(fieldName))
            {
                return string.Empty;
            }

            fieldName = fieldName
                .ToLower();

            if (!ModelFieldTranslations
                .ContainsKey(fieldName))
            {
                return fieldName;
            }

            return ModelFieldTranslations[fieldName]
                .ToLower();
        }

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