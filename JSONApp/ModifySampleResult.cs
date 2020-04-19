using System.Collections.Generic;
using Newtonsoft.Json;

namespace JSONApp
{
    internal class ModifySampleResult
    {
        /// <summary>
        /// Gets or sets the sum identifier result.
        /// </summary>
        /// <value>
        /// The sum identifier result.
        /// </value>
        [JsonProperty("summ")]
        public int SumIdResult { get; set; }

        /// <summary>
        /// Gets or sets the strings values.
        /// </summary>
        /// <value>
        /// The strings values.
        /// </value>
        [JsonProperty("strings")]
        public  IReadOnlyCollection<string> StringsValues { get; set; }

    }
}