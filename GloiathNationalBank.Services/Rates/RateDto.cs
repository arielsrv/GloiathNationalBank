namespace GloiathNationalBank.Services.Rates
{
    public class RateDto
    {
        /// <summary>
        ///     Gets or sets from.
        /// </summary>
        /// <value>
        ///     From.
        /// </value>
        public string From { get; set; }

        /// <summary>
        ///     Gets or sets to.
        /// </summary>
        /// <value>
        ///     To.
        /// </value>
        public string To { get; set; }

        /// <summary>
        ///     Gets or sets the rate.
        /// </summary>
        /// <value>
        ///     The rate.
        /// </value>
        public double Rate { get; set; }
    }
}