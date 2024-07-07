namespace MyAudios.Scripts
{
    /// <summary> Extension methods for the Message Class </summary>
    public static class MessageExtensions
    {
        /// <summary> Quick method to send a Message </summary>
        /// <param name="self"> Message </param>
        /// <typeparam name="T"> Type of Message </typeparam>
        public static void Send<T>(this T self) where T : Message =>
            Message.Send(self);
    }
}