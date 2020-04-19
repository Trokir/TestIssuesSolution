using Newtonsoft.Json;

namespace JSONApp
{
    internal class JsonSample
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        [JsonProperty("i")]
        public int Id { get; set; }
        /// <summary>
        /// Gets or sets the string text.
        /// </summary>
        /// <value>
        /// The string text.
        /// </value>
        [JsonProperty("s")]
        public string StringText { get; set; }
    }
}