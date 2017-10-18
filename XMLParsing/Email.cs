using System.Net.Mail;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System;

namespace XMLParsing
{
    class Email
    {
        private string distributionLists;
        private string indiVidualUsers;
        private string fromEmail = "dummyEmail@sharp.com";
        private string emailSubject;
        private string emailBody;
        private readonly string SMTP = "exmail.sharp.com";

        // Getters and setters
        public string DistributionLists { get => distributionLists; set => distributionLists = value; }
        public string IndiVidualUsers { get => indiVidualUsers; set => indiVidualUsers = value; }
        public string FromEmail { get => fromEmail; set => fromEmail = value; }
        public string EmailSubject { get => emailSubject; set => emailSubject = value; }
        public string EmailBody { get => emailBody; set => emailBody = value; }

        /// <summary>
        /// Four parameter constructor overlaods the two parameter constructor
        /// </summary>
        /// <param name="fromEmail">Email address to send the e-mail from</param>
        /// <param name="indiVidualUsers">An array of individual Email addresses to receive the Email notificattion</param>
        /// <param name="emailSubject">This is where the e-mail subject goes</param>
        /// <param name="emailBody">This is where the e-mal body goes</param>
        public Email(string fromEmail, string indiVidualUsers, string emailSubject, string emailBody)
            : this(fromEmail, indiVidualUsers)
        {
            EmailSubject = emailSubject;
            EmailBody = EmailBody;
        }

        /// <summary>
        /// Tree Parameter constructor overloads the two parameter constructor
        /// </summary>
        /// <param name="fromEmail">Email address to send the e-mail from</param>
        /// <param name="indiVidualUsers">An array of individual Email addresses to receive the Email notificattion</param>
        /// <param name="distributionLists">An array of DLs to  to receive the Email notification</param>
        public Email(string fromEmail, string indiVidualUsers, string distributionLists)
            : this(fromEmail, indiVidualUsers) => DistributionLists = distributionLists;

        /// <summary>
        /// Two parameter constructor
        /// </summary>
        /// <param name="fromEmail">Email address to send the e-mail from</param>
        /// <param name="indiVidualUsers">An array of individual Email addresses to receive the Email notificattion</param>
        public Email (string fromEmail, string indiVidualUsers)
        {
            FromEmail = fromEmail;
            IndiVidualUsers = indiVidualUsers;
        }

        // Default constructor no params
        public Email() { }

        /// <summary>
        /// Method that sends the email
        /// All properties are collected from the constructor when
        /// the object is instantiated
        /// </summary>
        public void SendMail()
        {
            SmtpClient client = new SmtpClient(SMTP);

            string[] fromName = FromEmail.Split('@');
            MailAddress from = new MailAddress(FromEmail, fromName[0]);
            MailMessage message = new MailMessage();

            message.Subject = EmailSubject;
            message.Body = EmailBody;
            message.From = from;

            string[] users = ProcessEmails(indiVidualUsers);
            string[] dls = ProcessEmails(distributionLists);

            // Get all the individual and DL email addresses
            for (int i = 0; i < dls.Length; i++)
            {
                try
                {
                    message.To.Add(dls[i]);
                }
                catch (FormatException)
                {
                    Console.WriteLine("Verify all emails are typed in correctly. One or more addresses are not in a recognized format.");
                }
            }

            for (int i = 0; i < users.Length; i++)
            {
                try
                {
                    message.To.Add(users[i]);
                }
                catch (FormatException)
                {
                    Console.WriteLine("Verify all emails are typed in correctly. One or more addresses are not in a recognized format.");
                }
            }

            // Send the e-mail
            try
            {
                client.Send(message);
            }
            catch (SmtpFailedRecipientsException)
            {
                Console.WriteLine("Please verify correct e-mail address is entered. The message could not be delivered to one or more of the recipients.");
            }
            catch (SmtpException)
            {
                Console.WriteLine("The connection to the SMTP server failed.");
            }
        }

        /// <summary>
        /// Converts string of comma separated emails to string array of emails
        /// </summary>
        /// <param name="emails">Comma separated emails</param>
        /// <returns>String array of emails</returns>
        public string[] ProcessEmails(string emails)
        {
            string[] temp = emails.Split(',');

            for (int i = 0; i < temp.Length; i++)
            {
                if (temp[i].StartsWith("\""))
                {
                    temp[i] = temp[i].Substring(1, temp[i].Length - 2).Trim();
                }
            }

            return temp;
        }

        /// <summary>
        /// Formats the email body to be sent out
        /// </summary>
        /// <param name="differences">List of strings with file names that are different</param>
        /// <param name="applicatioName">Current application name</param>
        /// <param name="regValue">Which registry value is being searched. Default: ProcessWhiteList</param>
        /// <returns>Email body formatted as a string</returns>
        public string FormatEmailBody(List<string> differences, string applicatioName, string regValue = "ProcessWhiteList")
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(string.Format("Application: {0}\n", applicatioName));
            sb.Append(string.Format("Following files found to not exist in the registry value: {0}\n\n", regValue));

            differences.ForEach(s => sb.Append(string.Format("{0}, ", s)));

            
            return sb.ToString().TrimEnd(' ').TrimEnd(',');
        }
    }
}
