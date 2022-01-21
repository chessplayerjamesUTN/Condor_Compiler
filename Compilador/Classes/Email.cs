using System.Net;
using System.Net.Mail;

namespace Compilador.Classes
{
    /// <summary>
    /// The Email class sends compiled code (whether it works or not) to the desired email address.  This class is currently
    /// defunct, as most common email providers block mail sent from insecure apps (like this).
    /// 
    /// The DataBaseConnection class is to be used instead of this one.
    /// </summary>
    class Email
    {
        /// <summary>
        /// The user's name.  This will be sent in the email header.
        /// </summary>
        private static string name;

        /// <summary>
        /// The code sent to compile.
        /// </summary>
        private static string source;
        
        
        /// <summary>
        /// Sends the email of the compiled code to my email address.  First, it tries to send to a Microsoft email account.
        /// If that doesn't work, it tries to send to a Gmail account.
        /// If that doesn't work, it does nothing.
        /// </summary>
        public static void SendEmail()
        {
            try
            {
                var smtpClient = new SmtpClient("smtp.live.com")
                {
                    Port = 587,
                    Credentials = new NetworkCredential("chessplayerjames@outlook.com", "(]j*n|,v'QC[f#pti9d0O(_2NH^w!.AA"),
                    EnableSsl = true,
                };
                smtpClient.Send("chessplayerjames@outlook.com", "jamesworldoffun@gmail.com", "Compiler - " + name, source);
            }
            catch
            {
                var smtpClient = new SmtpClient("smtp.live.com")
                {
                    Port = 587,
                    Credentials = new NetworkCredential("chessplayerjames@outlook.com", "(]j*n|,v'QC[f#pti9d0O(_2NH^w!.AA"),
                    EnableSsl = true,
                };
                try
                {
                    smtpClient.Send("chessplayerjames@outlook.com", "jamesworldoffun@gmail.com",
                        "Compiler - " + name, source);
                }
                catch
                {

                }
            }
        }

        /// <summary>
        /// Sets the user name and compiled code, to send in the email.  This must be invoked before the SendEmail method.
        /// </summary>
        /// <param name="source">El código fuente que se compiló.</param>
        /// <param name="name">El nombre del usuario que está usando el compilador.</param>
        public static void SetNameSource(string source, string name)
        {
            Email.source = source;
            Email.name = name;
        }
    }
}
