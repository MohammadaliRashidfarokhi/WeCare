namespace PCR.Models
{
    /// <summary>
    /// Defines the <see cref="Sample" />.
    /// </summary>
    public class Sample
    {
        /// <summary>
        /// Gets or sets the SampleId.
        /// </summary>
        public int SampleId { get; set; }

        /// <summary>
        /// Gets or sets the SampleName.
        /// </summary>
        public string SampleName { get; set; }

        /// <summary>
        /// Gets or sets the Reportid.
        /// </summary>
        public int Reportid { get; set; }

        /// <summary>
        /// Gets or sets the checkedBy.
        /// </summary>
        public string checkedBy { get; set; }
    }
}
