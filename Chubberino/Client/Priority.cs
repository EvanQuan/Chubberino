namespace Chubberino.Client
{
    public enum Priority
    {
        /// <summary>
        /// If messages is on-cooldown, will skip.
        /// </summary>
        Low = 0,

        /// <summary>
        /// Will directly send message.
        /// </summary>
        Medium = 1,

        /// <summary>
        /// Added to the back of the delayed message queue, ensuring the
        /// message will be sent, although it may be heavily delayed.
        /// </summary>
        High = 2
    }
}
