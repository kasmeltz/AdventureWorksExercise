using AdventureWorksExercise.WebAPI.ViewModels.Filtering;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Reflection;

namespace AdventureWorksExercise.WebAPI.JSON
{
    public class SelectedFieldContractResolver<T> : DefaultContractResolver where T : class
    {
        #region Constructors

        public SelectedFieldContractResolver(PaginatedFilter paginatedFilter)
        {
            FieldsToSerialize = new HashSet<string>();

            if (string.IsNullOrEmpty(paginatedFilter.Fields))
            {
                return;
            }

            var fieldTokens = paginatedFilter
                .Fields
                .Split(',');

            foreach (var fieldToken in fieldTokens)
            {
                FieldsToSerialize
                    .Add(fieldToken
                        .ToLower()
                        .Trim());
            }
        }

        #endregion

        #region Members

        protected HashSet<string> FieldsToSerialize { get; set; }

        #endregion

        #region DefaultContractResolver

        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            JsonProperty property = base
                .CreateProperty(member, memberSerialization);

            property.ShouldSerialize = instance =>
            {
                if (property.DeclaringType != typeof(T))
                {
                    return true;
                }

                if (!FieldsToSerialize
                    .Any())
                {
                    return true;
                }

                var propertyName = property
                    .PropertyName!
                    .ToLower()
                    .Trim();

                return FieldsToSerialize
                    .Contains(propertyName);
            };

            return property;
        }

        #endregion
    }
}