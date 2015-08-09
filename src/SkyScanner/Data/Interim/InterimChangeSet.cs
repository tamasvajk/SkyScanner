using System.Collections.Generic;

namespace SkyScanner.Data.Interim
{
    /// <summary>
    /// The InterimChangeSet type, is a data transfer object that can be used to store
    /// interim data sets when interim results become available. It also allows to explicitly
    /// store the additional and updated values in separate sets
    /// </summary>
    public class InterimChangeSet<T>
    {
        /// <summary>
        /// All items recieved thusfar
        /// </summary>
        public IEnumerable<T> All { get; private set; }

        /// <summary>
        /// The additional items
        /// </summary>
        public IEnumerable<T> Additions { get; private set; }

        /// <summary>
        /// The updated items
        /// </summary>
        public IEnumerable<InterimPair<T>> Updates { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="InterimChangeSet{T}"/> class. 
        /// </summary>
        /// <param name="all">All the items that were recieved thusfar</param>
        /// <param name="additions">New additions - compared to the previous event</param>
        /// <param name="updated">Updated itineraries - compared to the previous event</param>
        public InterimChangeSet(IEnumerable<T> all, IEnumerable<T> additions, IEnumerable<InterimPair<T>> updated)
        {
            All = all ?? new List<T>();
            Additions = additions ?? new List<T>();
            Updates = updated ?? new List<InterimPair<T>>();
        }
    }
}