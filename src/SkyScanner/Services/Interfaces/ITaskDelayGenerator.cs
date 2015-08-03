namespace SkyScanner.Services.Interfaces
{
    /// <summary>
    /// The ITaskDelayGenerator interfaces describes a type that generates 
    /// intervals with which tasks can be delayed
    /// </summary>
    internal interface ITaskDelayGenerator
    {
        /// <summary>
        /// Gets the next interval
        /// </summary>
        int NextInterval { get; }
    }
}