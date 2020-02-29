using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using DataLayer;
namespace RMATracker.Models
{
    public class SurveyModel : IDisposable
    {
        public string ViewType { get; set; }
        public SurveySiteInspection SurveySiteInspection { get; set; }
        public List<SurveyIntegrityChecks> SurveyIntegrityChecks { get; set; }
        public List<SurveyObstructions> SurveyObstructions { get; set; }
        public List<SurveyReaderArrivalStates> SurveyReaderArrivalStates { get; set; }
        public Survey Survey { get; set; }
        
        public SurveyModel()
        {
            SurveySiteInspection = new SurveySiteInspection();
            this.SurveyIntegrityChecks = DataHelper.GetQueryList<SurveyIntegrityChecks>("select s.*, t.message as _message from SurveyIntegrityChecks s right outer join SurveyTemplateSets t on s.seq_no=t.seq_no and 1=0 where set_type='integrity_check' order by t.seq_no");
            this.SurveyObstructions = DataHelper.GetQueryList<SurveyObstructions>("select s.*, t.message as _message from SurveyObstructions s right outer join SurveyTemplateSets t on s.seq_no=t.seq_no and 1=0 where set_type='overlay_alarm' order by t.seq_no");
            //SurveyIntegrityChecks = new List<SurveyIntegrityChecks>();
            //for (int i = 1; i <= 8; i++)
            //{
            //    SurveyIntegrityChecks.Add(new SurveyIntegrityChecks());
            //}
            //SurveyObstructions = new List<SurveyObstructions>();
            //for (int i = 1; i <= 4; i++)
            //{
            //    SurveyObstructions.Add(new SurveyObstructions());
            //}
            SurveyReaderArrivalStates = new List<SurveyReaderArrivalStates>();
            for (int i = 1; i <= 2; i++)
            {
                SurveyReaderArrivalStates.Add(new SurveyReaderArrivalStates());
            }



        }
        //private void InitImagePaths()
        //{
        //    ImagePathsMap = new List<string>();
        //    for (int i = 1; i <= 100; i++)
        //    {
        //        ImagePathsMap.Add("");
        //    }
        //}
        public SurveyModel(string SurveyID):this()
            {
                Survey = new Survey();
                Survey.Retrieve(new string[] { "id" }, new string[] { SurveyID });
                if ((Survey.id ?? 0) != 0)
                {
                    LoadDetails(Survey.id);
                
                }
            

            }
        public void LoadDetails(int? survey_id)
        {
            var visit_reqs = from c in this.Survey.visit_reqs.ToCharArray() select c == '1';
            this.Survey._visit_reqs = visit_reqs.ToList();
            this.SurveyIntegrityChecks = DataHelper.GetQueryList<SurveyIntegrityChecks>("select s.*, t.message as _message from SurveyIntegrityChecks s right outer join SurveyTemplateSets t on s.seq_no=t.seq_no and set_type='integrity_check' and survey_id = '" + survey_id + "' where set_type='integrity_check' order by t.seq_no");
            this.SurveyObstructions = DataHelper.GetQueryList<SurveyObstructions>("select s.*, t.message as _message from SurveyObstructions s right outer join SurveyTemplateSets t on s.seq_no=t.seq_no and set_type='overlay_alarm' and survey_id = '" + survey_id + "' where set_type='overlay_alarm' and s.id is not null order by t.seq_no");
            this.SurveyReaderArrivalStates = DataHelper.GetQueryList<SurveyReaderArrivalStates>("select s.* from SurveyReaderArrivalStates s where survey_id = '" + survey_id + "'");
            this.SurveySiteInspection = new SurveySiteInspection();
            SurveySiteInspection.Retrieve(new string[] { "survey_id", }, new string[] { this.Survey.id.ToString() });
        }
        public dynamic GetValue(dynamic className, dynamic propertyName, dynamic index = null)
        {
            dynamic WorkObject = Convert.ChangeType(this.GetType().GetProperty(className.ToString()), this.GetType().GetProperty(className.ToString()).PropertyType);
            if (index == null) //!this.GetType().GetProperty(className.ToString()).PropertyType.IsGenericType)
                return this.GetType().GetProperty(className.ToString()).GetType().GetProperty(propertyName.ToString()).GetValue(this, null);
            else
            {
                List<dynamic> list = (List<dynamic>)this.GetType().GetProperty(className.ToString());
                return list[index].GetType().GetProperty(propertyName.ToString()).GetValue(this, null);
            }
        }

        public bool Save(HttpRequestBase Request)
        {
    
            var visit_reqs = from b in this.Survey._visit_reqs select b ? "1":"0";
            this.Survey.visit_reqs = String.Join("", visit_reqs.ToArray());
            List<WebImage> WebImageList = new List<WebImage>();
            for (var i = 1; i <= Request.Files.Count; i++)
            {
                WebImage image = WebImage.GetImageFromRequest("upload-file-" + i);
                if (image != null)
                {
                    WebImageList.Add(image);
                }
            }
                    //this.Survey.visit_reqs = new string(this.Survey._visit_reqs);            

            SaveImageForObject(WebImageList, Survey);
            if (!this.Survey.Save())
                return false;

            int seq = 1;

            foreach (SurveyIntegrityChecks SurveyIntegrityCheck in SurveyIntegrityChecks)
            {
                SurveyIntegrityCheck.survey_id = this.Survey.id;
                SurveyIntegrityCheck.seq_no = seq;
                seq++;
                SaveImageForObject(WebImageList, SurveyIntegrityCheck);
                if (!SurveyIntegrityCheck.Save()) return false;
            }
            seq = 1;
            foreach (SurveyObstructions SurveyObstruction in SurveyObstructions)
            {
                SurveyObstruction.survey_id = this.Survey.id;
                SurveyObstruction.seq_no = seq;
                seq++;
                SaveImageForObject(WebImageList, SurveyObstruction);
                if (!SurveyObstruction.Save()) return false;
            }
            seq = 1;
            foreach (SurveyReaderArrivalStates SurveyReaderArrivalState in SurveyReaderArrivalStates)
            {
                SurveyReaderArrivalState.survey_id = this.Survey.id;
                SurveyReaderArrivalState.seq_no = seq;
                seq++;
                SaveImageForObject(WebImageList, SurveyReaderArrivalState);
                if (!SurveyReaderArrivalState.Save()) return false;
            }

            SurveySiteInspection.survey_id = this.Survey.id;
            SaveImageForObject(WebImageList, SurveySiteInspection);
            if (!SurveySiteInspection.Save()) return false;
            return true;
        }

        //private void SaveImages(HttpRequestBase Request)
        //{

        //    for (var i = 1; i <= Request.Files.Count; i++)
        //    {
        //        WebImage image = WebImage.GetImageFromRequest("upload-file-" + i);
        //        if (image != null)
        //        {
        //            SaveImageForObject(image, Survey);
        //            foreach (SurveyIntegrityChecks SurveyIntegrityCheck in SurveyIntegrityChecks)
        //            {
        //                SaveImageForObject(image, SurveyIntegrityCheck);
        //            }
        //            foreach (SurveyObstructions SurveyObstruction in SurveyObstructions)
        //            {
        //                SaveImageForObject(image, SurveyObstruction);
        //            }
        //            foreach (SurveyReaderArrivalStates SurveyReaderArrivalState in SurveyReaderArrivalStates)
        //            {
        //                SaveImageForObject(image, SurveyReaderArrivalState);
        //            }

        //            SaveImageForObject(image, SurveySiteInspection);
        //        }

        //    }
        //}
        private void SaveImageForObject(List<WebImage> ImageList, object WorkObject)
        {   foreach (WebImage image in ImageList)
                {
                    foreach (var pathProperty in WorkObject?.GetType().GetProperties()
                                    .Where(p => p.Name.EndsWith("_path")))
                    {
                        var imagePath = (pathProperty.GetValue(WorkObject, null) ?? "").ToString();
                        if (imagePath.EndsWith("!" + image.FileName))
                        {
                            if (!Directory.Exists(AppDomain.CurrentDomain.BaseDirectory + "images"))
                                Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + "images");
                            image.Save(@"~\" + imagePath);
                        }
                    }
                }
        }
        public string SendEmail(string url)
        {
            var fromAddress = new MailAddress("parabits1@optonline.net", "Parabit");
            string strAddress = ConfigTools.GetConfigValue("Survey E-mail Address(es)", "survey@Parabit.com");

            string TestMode = ConfigTools.GetConfigValue("TestMode", "false");
            if (TestMode == "true" || String.IsNullOrEmpty(strAddress))
            {
                strAddress = ConfigTools.GetConfigValue("Test E-mail", "igory@Parabit.com");
            }

            var toAddress = new MailAddress(strAddress);
            const string fromPassword = "35Debevoise";
            string subject = "Site Survey - ID Number: " + Survey.id;
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
            message.IsBodyHtml = false;
            message.Body = url;

            //foreach (RegistrationParts Part in RegistrationParts.Where(i => i.image_path != null))
            //{
            //    if (!System.IO.File.Exists(AppDomain.CurrentDomain.BaseDirectory + Part.image_path))
            //        continue;
            //    //message.Attachments.Add(new System.Net.Mail.Attachment(Image.ImagePath.Contains("\\") ? Image.ImagePath : ImagesDirectory + "\\" + Image.ImagePath));
            //    string[] PathArray = Part.image_path.Split('!');
            //    Attachment attachment = new System.Net.Mail.Attachment(AppDomain.CurrentDomain.BaseDirectory + Part.image_path, new System.Net.Mime.ContentType(System.Net.Mime.MediaTypeNames.Image.Jpeg));
            //    attachment.Name = PathArray[PathArray.Length - 1];
            //    message.Attachments.Add(attachment);
            //}
            //  message.BodyEncoding = System.Text.Encoding.UTF8;
            //  message.DeliveryNotificationOptions = DeliveryNotificationOptions.OnSuccess;
            string status = "OK";
            try
            {
                smtp.Send(message);
                Console.WriteLine("E-mail successfully sent.");
                SaveEmail(strAddress, message.Body, "Survey");
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
            string EMailTemplateFile = ConfigTools.GetConfigValue("Survey E-Mail Template Path", AppDomain.CurrentDomain.BaseDirectory + "\\Templates\\Registration_Mail_Template.txt");

            if (!System.IO.File.Exists(EMailTemplateFile))
            {
                return "Template File " + EMailTemplateFile + " Not Found";
            }
            StringBuilder NewContent = new StringBuilder();
            string FileBody = System.IO.File.ReadAllText(EMailTemplateFile);
            NewContent.Append("<table>");
            //foreach (string Line in Regex.Split(FileBody, System.Environment.NewLine))
            //{
            //    if (!Line.Contains(":"))
            //    {
            //        if (Line.Trim().Length != 0)
            //            NewContent.Append("<tr><td colspan='2'>");
            //        else
            //            NewContent.Append("<tr><td colspan='2' style = 'height:20;'>");
            //        NewContent.Append(Line);
            //        NewContent.Append("</td></tr>");
            //    }
            //    else
            //    {
            //        string FieldName = Line.Split(':')[0];
            //        string NewEntry = "";
            //        switch (FieldName)
            //        {
            //            case "Submit Date":
            //                //NewContent.Append("<hr>");
            //                NewEntry = EmailEntry(FieldName, DateTime.Now.ToString("MMMM dd, yyyy"));
            //                break;
            //            case "Registration ID":
            //                //NewContent.Append("<hr>");
            //                NewEntry = EmailEntry(FieldName, Registrations.registration_no);
            //                break;
            //            case "Firm Name":
            //                //NewContent.Append("<hr>");
            //                NewEntry = EmailEntry(FieldName, TechInfo.ClientName);
            //                break;
            //            case "Technician Name":
            //                NewEntry = EmailEntry(FieldName, TechInfo.TechName);
            //                break;
            //            case "Technician Email":
            //                NewEntry = EmailEntry(FieldName, TechInfo.TechEmail);
            //                break;
            //            case "Technician Phone":
            //                NewEntry = EmailEntry(FieldName, TechInfo.TechPhone);
            //                break;
            //            case "Bank Name":
            //                NewEntry = EmailEntry(FieldName, BankInfo.BankName);
            //                break;
            //            case "Bank Street Address":
            //                NewEntry = EmailEntry(FieldName, BankInfo.BankStreetAddress);
            //                break;
            //            case "Bank City":
            //                NewEntry = EmailEntry(FieldName, BankInfo.BankCity);
            //                break;
            //            case "Bank State":
            //                NewEntry = EmailEntry(FieldName, BankInfo.BankState);
            //                break;
            //            case "Bank zip":
            //                NewEntry = EmailEntry(FieldName, BankInfo.BankZipCode);
            //                break;
            //            case "Installation Date":
            //                try
            //                {
            //                    DateTime InstallDate = DateTime.Parse(Registrations.date_installed);
            //                    NewEntry = EmailEntry(FieldName, InstallDate.ToString("MM-dd-yy"));
            //                }
            //                catch
            //                {
            //                    NewEntry = EmailEntry(FieldName, "");
            //                }

            //                break;
            //            case var expression when (FieldName.StartsWith("Part ")):
            //                int PartNo_a;
            //                if (int.TryParse(expression.Contains(" ") ? expression.Split(' ')[1] : "", out PartNo_a) && RegistrationParts.Count >= PartNo_a && !String.IsNullOrEmpty(RegistrationParts[PartNo_a - 1].part_types_id))
            //                {
            //                    NewEntry = EmailEntry(FieldName, RegistrationParts[PartNo_a - 1]._PartDescription);
            //                }
            //                break;
            //            case var expression when (FieldName.StartsWith("Serial Number ")):
            //                int PartNo_b;
            //                if (int.TryParse(expression.Contains(" ") ? expression.Split(' ')[2] : "", out PartNo_b) && RegistrationParts.Count >= PartNo_b && !String.IsNullOrEmpty(RegistrationParts[PartNo_b - 1].part_types_id))
            //                    NewEntry = EmailEntry("Serial Number", RegistrationParts[PartNo_b - 1].serial_no);
            //                break;
            //            default:
            //                NewEntry = String.Format("<td  colspan='2'>{0}</td>", Line);
            //                break;
            //        }
            //        if (NewEntry != "")
            //        {
            //            if (NewEntry.Contains("Submit Date"))
            //            {
            //                NewContent.Append("</table>");
            //                NewContent.Append("<table style='width:70%'> <col style='width:50%;'><col style='width:70%;'>");
            //            }

            //            NewContent.Append("<tr>");
            //            NewContent.Append(NewEntry);
            //            NewContent.Append("</tr>");
            //            if (NewEntry.Contains("Dispatch #"))
            //            {
            //                NewContent.Append("</table>");
            //                NewContent.Append("<table>");
            //            }
            //        }
            //    }
            //}
            NewContent.Append("</table>");
            return NewContent.ToString();

        }
        public Boolean SaveEmail(string Addresses, string Body, string EmailType)
        {
            string action_query = "insert into EMails (addresses, EmailType, msgbody, survey_id) values ";
            action_query += "('" + String.Join("','", new string[] { Addresses, EmailType, Body.Replace("'", "''"), Survey.id.ToString() }) + "')";

            return DataHelper.ActionQuery(action_query);
        }
        void IDisposable.Dispose()
        {

        }
    }
}