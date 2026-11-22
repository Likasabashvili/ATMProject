using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Text;
using ATMProject.Application.Services.Interfaces;


namespace University.Application.Services.Implemetations
{
	public class EmailService : IEmailService
	{
		
		public void SendEmail(string to, string subject, string body) 
		{
			SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587);
			smtpClient.UseDefaultCredentials = false;
			smtpClient.Credentials = new NetworkCredential("sendv39@gmail.com", "pdio zosc gfvy ptrk");
			smtpClient.EnableSsl = true;


			MailMessage message = new MailMessage();
			message.From = new MailAddress("sendv39@gmail.com");
			message.To.Add(to);
			message.Subject = subject;
			message.Body = body;
			message.IsBodyHtml = true;


			smtpClient.Send(message);
		}



public void SendVerificationCode(string email, string verificationCode)
{
    string htmlBody = $@"
    <!DOCTYPE html>
    <html lang='ka'>
    <head>
        <meta charset='UTF-8'>
        <meta name='viewport' content='width=device-width, initial-scale=1.0'>
        <title>ATM Verification</title>
    </head>

    <body style='margin:0; padding:0; background-color:#F0FDFA;
                 font-family:Arial, Helvetica, sans-serif;'>

        <table width='100%' cellpadding='0' cellspacing='0'
               style='padding:50px 15px; background-color:#F0FDFA;'>
            <tr>
                <td align='center'>

                    <table width='100%' cellpadding='0' cellspacing='0'
                           style='max-width:520px; background:#FFFFFF;
                                  border-radius:18px; overflow:hidden;
                                  box-shadow:0 8px 30px rgba(15,118,110,0.10);'>

                        <!-- Header -->
                        <tr>
                            <td style='background:#0F766E; padding:38px 25px;
                                       text-align:center;'>

                                <div style='font-size:13px; color:#CCFBF1;
                                            letter-spacing:4px;
                                            text-transform:uppercase;
                                            margin-bottom:12px;'>
                                    ATM PROJECT
                                </div>

                                <h1 style='color:#ffffff; margin:0;
                                           font-size:27px; font-weight:600;'>
                                    Email Verification
                                </h1>

                                <p style='color:#99F6E4; font-size:14px;
                                          margin:12px 0 0;'>
                                    Secure your account
                                </p>

                            </td>
                        </tr>

                        <!-- Main Content -->
                        <tr>
                            <td style='padding:40px 32px 35px;
                                       text-align:center;'>

                                <h2 style='color:#134E4A; font-size:23px;
                                           margin:0 0 15px;'>
                                    მოგესალმებით!
                                </h2>

                                <p style='color:#64748B; font-size:15px;
                                          line-height:1.8; margin:0;'>
                                    მადლობა, რომ აირჩიეთ ჩვენი საბანკო სისტემა.
                                    თქვენი ანგარიშის გასააქტიურებლად
                                    გთხოვთ, შეიყვანოთ ქვემოთ მოცემული კოდი.
                                </p>

                                <!-- Verification Code -->
                                <div style='background:#F0FDFA;
                                            border:1px solid #99F6E4;
                                            border-radius:12px;
                                            padding:25px 15px;
                                            margin:30px 0;'>

                                    <p style='color:#0F766E; font-size:12px;
                                              letter-spacing:2px;
                                              margin:0 0 14px;
                                              text-transform:uppercase;'>
                                        YOUR VERIFICATION CODE
                                    </p>

                                    <div style='font-size:36px;
                                                font-weight:bold;
                                                letter-spacing:10px;
                                                color:#0F766E;'>
                                        {verificationCode}
                                    </div>

                                </div>

                                <p style='color:#0F766E; font-size:13px;
                                          margin:0 0 25px;'>
                                    კოდი მოქმედებს 5 წუთის განმავლობაში.
                                </p>

                                <div style='height:1px;
                                            background:#CCFBF1;
                                            margin:25px 0;'>
                                </div>

                                <p style='color:#94A3B8; font-size:12px;
                                          line-height:1.7; margin:0;'>
                                    თუ თქვენ არ მოგითხოვიათ ანგარიშის
                                    ვერიფიკაცია, უგულებელყავით ეს შეტყობინება.
                                    თქვენი ანგარიში უსაფრთხოდ დარჩება.
                                </p>

                            </td>
                        </tr>

                        <!-- Footer -->
                        <tr>
                            <td style='background:#F0FDFA; padding:23px;
                                       text-align:center;'>

                                <p style='color:#0F766E; font-size:13px;
                                          font-weight:bold; margin:0 0 8px;'>
                                    ATM PROJECT
                                </p>

                                <p style='color:#94A3B8; font-size:11px;
                                          margin:0;'>
                                    © 2026 ATM Project. All rights reserved.
                                </p>

                                <p style='color:#94A3B8; font-size:11px;
                                          margin:8px 0 0;'>
                                    Automated message — please do not reply.
                                </p>

                            </td>
                        </tr>

                    </table>

                </td>
            </tr>
        </table>

    </body>
    </html>";

    SendEmail(email, "ATM Project | Email Verification", htmlBody);
}

	}
}