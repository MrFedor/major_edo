

namespace major_scan_folder
{
    using NLog;
    using System;
    using System.ComponentModel;
    using System.Net.Mail;
    public class SDEmail
    {
        private static Logger _logger = LogManager.GetCurrentClassLogger();

        public SDEmail()
        { }

        private static void SendCompletedCallback(object sender, AsyncCompletedEventArgs e)
        {
            MailMessage token = (MailMessage)e.UserState;

            if (e.Cancelled)
            {
                _logger.Error("Отмена отправки: {0}", token.To);
            }
            if (e.Error != null)
            {
                _logger.Error("Ошибка: {0} : {1} : {2}", token.To, token.Subject, e.Error.ToString());
            }
            else
            {
                _logger.Info("Сообщение отправлено: {0}", token.To);
            }            
        }
        public void SendEmail(string[] toAddresses)
        {
            
            SmtpClient smtp = new SmtpClient();

            MailMessage message = new MailMessage();                        
            foreach (string address in toAddresses)
                message.To.Add(new MailAddress(address));

            message.Body = "Пришел файл по ЭДО";            
            message.Body += Environment.NewLine;
            message.BodyEncoding = System.Text.Encoding.UTF8;

            message.Subject = "Пришел файл по ЭДО";
            message.SubjectEncoding = System.Text.Encoding.UTF8;

            // Set the method that is called back when the send operation ends.
            //smtp.SendCompleted += new SendCompletedEventHandler(SendCompletedCallback);

            smtp.SendCompleted += (s, e) => {
                SendCompletedCallback(s, e);
                smtp.Dispose();
                message.Dispose();
            };
            // The userState can be any object that allows your callback 
            // method to identify this send operation.
            // For this example, the userToken is a string constant.
            MailMessage userState = message;
            try
            {
                smtp.SendAsync(message, userState);
            }
            catch (ArgumentNullException ex)
            {
                _logger.Error(ex, "{0} : {1} : {2} : {3}", ex.Message, ex.TargetSite, ex.StackTrace, ex.InnerException);
            }
            catch (InvalidOperationException ex)
            {
                _logger.Error(ex, "{0} : {1} : {2} : {3}", ex.Message, ex.TargetSite, ex.StackTrace, ex.InnerException);
            }
            catch (SmtpException ex)
            {
                _logger.Error(ex, "{0} : {1} : {2} : {3}", ex.Message, ex.TargetSite, ex.StackTrace, ex.InnerException);
            }


            //Console.WriteLine("Sending message... press c to cancel mail. Press any other key to exit.");
            //string answer = Console.ReadLine();
            //// If the user canceled the send, and mail hasn't been sent yet,
            //// then cancel the pending operation.
            //if (answer.StartsWith("c") && mailSent == false)
            //{
            //    smtp.SendAsyncCancel();
            //}
            ////Clean up.
            //message.Dispose();
        }
    }
}
