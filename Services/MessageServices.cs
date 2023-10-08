namespace PCR.Services
{
    using Microsoft.Extensions.Options;
    using System.Threading.Tasks;
    using Twilio;
    using Twilio.Rest.Api.V2010.Account;
    using Twilio.Types;

    /// <summary>
    /// Defines the <see cref="AuthMessageSender" />.
    /// </summary>
    public class AuthMessageSender : ISmsSender
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AuthMessageSender"/> class.
        /// </summary>
        /// <param name="optionsAccessor">The optionsAccessor<see cref="IOptions{AuthMessageSMSSenderOptions}"/>.</param>
        public AuthMessageSender(IOptions<AuthMessageSMSSenderOptions> optionsAccessor)
        {
            Options = optionsAccessor.Value;
        }

        /// <summary>
        /// Gets the Options.
        /// </summary>
        public AuthMessageSMSSenderOptions Options { get; }

        /// <summary>
        /// The SendEmailAsync.
        /// </summary>
        /// <param name="email">The email<see cref="string"/>.</param>
        /// <param name="subject">The subject<see cref="string"/>.</param>
        /// <param name="message">The message<see cref="string"/>.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        public Task SendEmailAsync(string email, string subject, string message)
        {
            // Plug in your email service here to send an email.
            return Task.FromResult(0);
        }

        /// <summary>
        /// The SendSmsAsync.
        /// </summary>
        /// <param name="number">The number<see cref="string"/>.</param>
        /// <param name="message">The message<see cref="string"/>.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        public Task SendSmsAsync(string number, string message)
        {
            string SMSAccountIdentification = "AC5fbdb1a5ad154e9bd550d6000cf07282";
            string SMSAccountPassword = "f796393a48642d5821a27e037300a0d0";
            string SMSAccountFrom = "+19706488047";
            // Plug in your SMS service here to send a text message.
            // Your Account SID from twilio.com/console
            var accountSid = SMSAccountIdentification;
            // Your Auth Token from twilio.com/console
            var authToken = SMSAccountPassword;

            TwilioClient.Init(accountSid, authToken);

            return MessageResource.CreateAsync(
                to: new PhoneNumber(number),
                from: new PhoneNumber(SMSAccountFrom),
                body: message);
        }
    }
}
