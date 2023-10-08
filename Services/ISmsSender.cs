namespace PCR.Services
{
    using System.Threading.Tasks;

    /// <summary>
    /// Defines the <see cref="ISmsSender" />.
    /// </summary>
    public interface ISmsSender
    {
        /// <summary>
        /// The SendSmsAsync.
        /// </summary>
        /// <param name="number">The number<see cref="string"/>.</param>
        /// <param name="message">The message<see cref="string"/>.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        Task SendSmsAsync(string number, string message);
    }

    /// <summary>
    /// Defines the <see cref="Struct1" />.
    /// </summary>
    public struct Struct1
    {
    }
}
