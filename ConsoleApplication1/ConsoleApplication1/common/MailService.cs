using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace ConsoleApplication1
{
   public  class MailService
    {

        //public  static void SendEmail(string subject)
        //{
        //    string toAddress = ConfigurationManager.AppSettings["toAddress"];
        //    string senderAddress = ConfigurationManager.AppSettings["senderAddress"];
        //    string senderPassword = ConfigurationManager.AppSettings["senderPassword"];

        //    if (toAddress != null || toAddress != "")
        //    {
        //      ;
        //        string body = "parsing data";
                
        //        try
        //        {
        //            SmtpClient smtp = new SmtpClient
        //            {
        //                DeliveryMethod = SmtpDeliveryMethod.Network,
        //                Host = "smtp.office365.com",
        //                Port = 587,
        //                Credentials = new System.Net.NetworkCredential(senderAddress, senderPassword),
        //                EnableSsl = true
        //            };
        //            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;

        //            MailMessage message = new MailMessage(senderAddress, toAddress, subject, body);
        //            smtp.Send(message);
        //            Console.WriteLine("Email has been sent");



        //        }
        //        catch (Exception ex)
        //        {
        //            Console.WriteLine(ex.ToString());
        //        }
        //    }
        //}\


       public static void SendEmail(string subject)
       {
           var bodymsg = "Parsing Data";
           //1.The ACCOUNT
           string senderAddress = ConfigurationManager.AppSettings["senderAddress"];
           string senderPassword = ConfigurationManager.AppSettings["senderPassword"];
           MailAddress fromAddress = new MailAddress(senderAddress);
           String fromPassword = senderPassword;

           //2.The Destination email Addresses
           MailAddressCollection TO_addressList = new MailAddressCollection();

           //3.Prepare the Destination email Addresses list
           string mailto = ConfigurationManager.AppSettings["toAddress"];
           foreach (var curr_address in mailto.Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries))
           {
               MailAddress mytoAddress = new MailAddress(curr_address, "Custom display name");
               TO_addressList.Add(mytoAddress);
           }

           //4.The Email Body Message
           String body = bodymsg;

           //5.Prepare GMAIL SMTP: with SSL on port 587
           var smtp = new SmtpClient
           {
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    Host = "smtp.office365.com",
                    Port = 587,
                    Credentials = new System.Net.NetworkCredential(senderAddress, senderPassword),
                    EnableSsl = true
           };


           //6.Complete the message and SEND the email:
           using (var message = new MailMessage()
           {
               From = fromAddress,
               Subject = subject,
               Body = body,
           })
           {
               message.To.Add(TO_addressList.ToString());
               smtp.Send(message);
           }



       }


    }
}
