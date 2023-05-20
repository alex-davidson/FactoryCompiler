using System.Text.Json.Serialization;

namespace FactoryCompiler.Model
{
    public static partial class Dto
    {
        public class FactoryDescription
        {
            public Region[]? Regions { get; set; }

            /// <summary>
            /// A named region containing groups of factories and optionally connected to
            /// transport networks.
            /// </summary>
            /// <remarks>
            /// Factories within a region are assumed to be connected optimally.
            /// </remarks>
            public class Region
            {
                public string? RegionName { get; set; }
                public Group[]? Groups { get; set; }
                /// <summary>
                /// Resources may be sourced from these networks.
                /// </summary>
                [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
                public Transport[]? Inbound { get; set; }
                /// <summary>
                /// Excess resources may be sent to these networks.
                /// </summary>
                [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
                public Transport[]? Outbound { get; set; }
            }

            /// <summary>
            /// A group of factories processing the same recipe.
            /// </summary>
            public class Production
            {
                /// <summary>
                /// Factory type name. Only required if there's any ambiguity about the recipe.
                /// </summary>
                [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
                public string? FactoryName { get; set; }
                /// <summary>
                /// Recipe name.
                /// </summary>
                [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
                public string? Recipe { get; set; }
                /// <summary>
                /// Effective number of factories processing this recipe.
                /// </summary>
                [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
                public string? Count { get; set; }
            }

            /// <summary>
            /// Either a single named Production, or a named list of Groups.
            /// </summary>
            public class Group : Production
            {
                [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
                public string? Repeat { get; set; }
                [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
                public string? GroupName { get; set; }
                [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
                public Group[]? Groups { get; set; }
            }

            /// <summary>
            /// A connection to a transport network. May be inbound or outbound.
            /// </summary>
            public class Transport
            {
                public string? Item { get; set; }
                public string? Network { get; set; }
            }
        }
    }
}
