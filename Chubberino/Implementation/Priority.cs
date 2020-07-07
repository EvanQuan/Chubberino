namespace Chubberino.Implementation
{
    public enum Priority
    {
        /// <summary>
        /// If messages is on-cooldown, will skip.
        /// </summary>
        Low = 0,

        /// <summary>
        /// Added to the back of the queue.
        /// </summary>
        High = 1
    }
}
