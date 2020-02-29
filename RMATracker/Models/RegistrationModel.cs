using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using DataLayer;
namespace RMATracker.Models
{
    public class RegistrationsModel : IDisposable
    {
        public int CurrentPartIndex { get; set; }
        public string ViewType { get; set; }
        public BankInfo BankInfo { get; set; }
        public TechInfo_old TechInfo { get; set; }
        public Registrations Registrations { get; set; }
        public List<RegistrationParts> RegistrationParts { get; set; }

        public RegistrationsModel()
        {
            RegistrationParts = new List<RegistrationParts>();
            for (int i = 1; i <= 7; i++)
            {
                RegistrationParts.Add(new RegistrationParts());
            }

        }
        public RegistrationsModel(string RegNo)
        {
            Registrations = new Registrations();
            Registrations.Retrieve(new string[] { "registration_no", }, new string[] { RegNo });
            if ((Registrations.id ?? 0) != 0)
            {
                LoadParts(Registrations.id);
                
                if (Registrations.bankinfo_id.HasValue)
                    BankInfo = new BankInfo(Registrations.bankinfo_id);
                if (Registrations.tech_info_id.HasValue)
                    TechInfo = new TechInfo_old(Registrations.tech_info_id);
            }

        }
        public void LoadParts(int? registration_id)
        {
            this.RegistrationParts = DataHelper.GetQueryList<RegistrationParts>("select p.*, PartNo as _PartNo, PartDescription as _PartDescription from RegistrationParts p left outer join PartTypes t on p.part_types_id =t.id where registration_id = '" + registration_id + "'");
        }

        public bool Save()
        {

            if (BankInfo != null)
            {
                if (!BankInfo.Save()) return false;
                Registrations.bankinfo_id = BankInfo.id;
            }
            
            if (TechInfo != null)
            {
                if (!TechInfo.Save()) return false;
                Registrations.tech_info_id = TechInfo.id;
            }
            
            if (!this.Registrations.Save()) return false;

            int seq = 1;

            foreach (RegistrationParts Part in RegistrationParts)
            {
                Part.registration_id = this.Registrations.id;
                Part.seq_no = seq;
                seq++;
                if (!Part.Save()) return false;
            }

            return true;
        }

        public string SendEmail()
        {
            var fromAddress = new MailAddress("parabits1@optonline.net", "Parabit");
            string strAddress = ConfigTools.GetConfigValue("Registration E-mail Address(es)", "registration@Parabit.com");
            
            string TestMode = ConfigTools.GetConfigValue("TestMode", "false");
            if (TestMode == "true" || String.IsNullOrEmpty(strAddress))
            {
                strAddress = ConfigTools.GetConfigValue("Test E-mail", "igory@Parabit.com");
            }

            var toAddress = new MailAddress(strAddress);
            const string fromPassword = "35Debevoise";
            string subject = "Product Registration - ID Number: " + Registrations.id;
            
            var smtp = new SmtpClient
            {
                Host = "mail.optonline.net",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
            };
 
            var message = new MailMessage();
            message.From = fromAddress;
            foreach (string address in strAddress.Split(';'))
            {
                message.To.Add(address);
            }
            message.Subject = subject;
            message.IsBodyHtml = true;
            message.Body = BuildMessageBody();
            
            foreach (RegistrationParts Part in RegistrationParts.Where(i => i.image_location != null))
            {
                if (!System.IO.File.Exists(AppDomain.CurrentDomain.BaseDirectory + Part.image_location))
                    continue;
                //message.Attachments.Add(new System.Net.Mail.Attachment(Image.ImagePath.Contains("\\") ? Image.ImagePath : ImagesDirectory + "\\" + Image.ImagePath));
                string[] PathArray = Part.image_location.Split('!');
                Attachment attachment = new System.Net.Mail.Attachment(AppDomain.CurrentDomain.BaseDirectory + Part.image_location, new System.Net.Mime.ContentType(System.Net.Mime.MediaTypeNames.Image.Jpeg));
                attachment.Name = PathArray[PathArray.Length - 1];
                message.Attachments.Add(attachment);
            }
            //  message.BodyEncoding = System.Text.Encoding.UTF8;
            //  message.DeliveryNotificationOptions = DeliveryNotificationOptions.OnSuccess;
            string status = "OK";
            try
            {
                smtp.Send(message);
                Console.WriteLine("E-mail successfully sent.");
                SaveEmail(strAddress, message.Body, "Registration");
            }
            catch (Exception ex)
            {
                status = ex.Message;
            }

            message.Dispose();
            return status;
        }
        private string EmailEntry(string FieldName, string Value, bool NewLine = false, bool Padding = false)
        {
            StringBuilder content = new StringBuilder();
            if (!Padding)
                content.Append("<td align='left'>");
            else
                content.Append("<td align='left' style='padding-left:50px'>");
            content.Append((FieldName + ": ").PadRight(40));
            content.Append("</td>");
            if (NewLine)
            {
                content.Append("<tr><td><br/></td></tr>");
            }
            else
            {
                Value = Value ?? "";
            }
            content.Append("<td align='left'>");
            content.Append(Value);
            content.Append("</td>");

            return content.ToString();
        }
        private string BuildMessageBody()
        {
            string EMailTemplateFile = ConfigTools.GetConfigValue("Registration E-Mail Template Path", AppDomain.CurrentDomain.BaseDirectory + "\\Templates\\Registration_Mail_Template.txt");
           
            if (!System.IO.File.Exists(EMailTemplateFile))
            {
                return "Template File " + EMailTemplateFile + " Not Found";
            }
            StringBuilder NewContent = new StringBuilder();
            string FileBody = System.IO.File.ReadAllText(EMailTemplateFile);
            NewContent.Append("<table>");
            foreach (string Line in Regex.Split(FileBody, System.Environment.NewLine))
            {
                if (!Line.Contains(":"))
                {
                    if (Line.Trim().Length != 0)
                        NewContent.Append("<tr><td colspan='2'>");
                    else
                        NewContent.Append("<tr><td colspan='2' style = 'height:20;'>");
                    NewContent.Append(Line);
                    NewContent.Append("</td></tr>");
                }
                else
                {
                    string FieldName = Line.Split(':')[0];
                    string NewEntry = "";
                    switch (FieldName)
                    {
                        case "Submit Date":
                            //NewContent.Append("<hr>");
                            NewEntry = EmailEntry(FieldName, DateTime.Now.ToString("MMMM dd, yyyy"));
                            break;
                        case "Registration ID":
                            //NewContent.Append("<hr>");
                            NewEntry = EmailEntry(FieldName, Registrations.registration_no);
                            break;
                        case "Firm Name":
                            //NewContent.Append("<hr>");
                            NewEntry = EmailEntry(FieldName, TechInfo.ClientName);
                            break;
                        case "Technician Name":
                            NewEntry = EmailEntry(FieldName, TechInfo.TechName);
                            break;
                        case "Technician Email":
                            NewEntry = EmailEntry(FieldName, TechInfo.TechEmail);
                            break;
                        case "Technician Phone":
                            NewEntry = EmailEntry(FieldName, TechInfo.TechPhone);
                            break;
                        case "Bank Name":
                            NewEntry = EmailEntry(FieldName, BankInfo.BankName);
                            break;
                        case "Bank Street Address":
                            NewEntry = EmailEntry(FieldName, BankInfo.BankStreetAddress);
                            break;
                        case "Bank City":
                            NewEntry = EmailEntry(FieldName, BankInfo.BankCity);
                            break;
                        case "Bank State":
                            NewEntry = EmailEntry(FieldName, BankInfo.BankState);
                            break;
                        case "Bank zip":
                            NewEntry = EmailEntry(FieldName, BankInfo.BankZipCode);
                            break;
                        case "Installation Date":
                            try
                            {
                                DateTime InstallDate = DateTime.Parse(Registrations.date_installed);
                                NewEntry = EmailEntry(FieldName, InstallDate.ToString("MM-dd-yy"));
                            }
                            catch
                            {
                                NewEntry = EmailEntry(FieldName, "");
                            }

                            break;
                        case var expression when (FieldName.StartsWith("Part ")):
                            int PartNo_a;
                            if (int.TryParse(expression.Contains(" ") ? expression.Split(' ')[1] : "", out PartNo_a) && RegistrationParts.Count >= PartNo_a && !String.IsNullOrEmpty(RegistrationParts[PartNo_a - 1].part_types_id))
                            {
                                NewEntry = EmailEntry(FieldName, RegistrationParts[PartNo_a - 1]._PartDescription);
                            }
                            break;
                        case var expression when (FieldName.StartsWith("Serial Number ")):
                            int PartNo_b;
                            if (int.TryParse(expression.Contains(" ") ? expression.Split(' ')[2] : "", out PartNo_b) && RegistrationParts.Count >= PartNo_b && !String.IsNullOrEmpty(RegistrationParts[PartNo_b - 1].part_types_id))
                                NewEntry = EmailEntry("Serial Number", RegistrationParts[PartNo_b - 1].serial_no);
                            break;
                        default:
                            NewEntry = String.Format("<td  colspan='2'>{0}</td>", Line);
                            break;
                    }
                    if (NewEntry != "")
                    {
                        if (NewEntry.Contains("Submit Date"))
                        {
                            NewContent.Append("</table>");
                            NewContent.Append("<table style='width:70%'> <col style='width:50%;'><col style='width:70%;'>");
                        }

                        NewContent.Append("<tr>");
                        NewContent.Append(NewEntry);
                        NewContent.Append("</tr>");
                        if (NewEntry.Contains("Dispatch #"))
                        {
                            NewContent.Append("</table>");
                            NewContent.Append("<table>");
                        }
                    }
                }
            }
            NewContent.Append("</table>");
            return NewContent.ToString();

        }
        public Boolean SaveEmail(string Addresses, string Body, string EmailType)
        {
            string action_query = "insert into EMails (addresses, EmailType, msgbody, registration_id) values ";
            action_query += "('" + String.Join("','", new string[] { Addresses, EmailType, Body.Replace("'", "''"), Registrations.id.ToString() }) + "')";

            return DataHelper.ActionQuery(action_query);
        }
        void IDisposable.Dispose()
        {

        }
    }
}