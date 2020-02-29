using System;
using System.Collections.Generic;
using System.Data;
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
    public class RMATrackerViewModel : IDisposable
    {
        public int CurrentPartIndex { get; set; }
        public bool IsQuery { get; set; }
        public string ViewType { get; set; }
        public Assessment Assessment { get; set; }
        public BankInfo BankInfo { get; set; }
        public RmaDates RmaDates { get; set; }
        public PuttyTest PuttyTest { get; set; }
        public RmaSecuritasOptions RmaSecuritasOptions { get; set; }
        public TechInfo_old TechInfo { get; set; }
        public RmaTests RmaTests { get; set; }
        public RmaBase RmaBase { get; set; }
        public RmaReturnAddress RmaReturnAddress { get; set; }
        public RmaForms Form { get; set; }
        public RmaProblemTypes ProblemType { get; set; }
        //public List<Images> Images { get; set; }
        public List<RmaParts> RmaParts { get; set; }

        //public Dictionary<string, string> PartNumbers = new Dictionary<string, string>();
        public RMATrackerViewModel() {
            RmaParts = new List<RmaParts>();
            for (int i = 1; i <= 7; i++)
            {
                RmaParts.Add(new RmaParts());
            }
            Form = new RmaForms();
            ProblemType = new RmaProblemTypes();
        }
        public RMATrackerViewModel(string CaseNo)
        {
            RmaBase = new RmaBase();
            RmaBase.Retrieve(new string[] { "case_no", }, new string[] { CaseNo });
            if ((RmaBase.id??0) !=0 )
            {
                LoadParts(RmaBase.id);
                if (RmaBase.securitas_options_id.HasValue)
                    RmaSecuritasOptions = new RmaSecuritasOptions(RmaBase.securitas_options_id);
                if (RmaBase.bankinfo_id.HasValue)
                    BankInfo = new BankInfo(RmaBase.bankinfo_id);
                if (RmaBase.tech_info_id.HasValue)
                    TechInfo = new TechInfo_old(RmaBase.tech_info_id);
                if (RmaBase.return_address_id.HasValue)
                    RmaReturnAddress = new RmaReturnAddress(RmaBase.return_address_id);
                if (RmaBase.form_id.HasValue)
                    Form = new RmaForms(RmaBase.form_id);
                if (RmaBase.problem_id.HasValue)
                    ProblemType = new RmaProblemTypes(RmaBase.problem_id);
                if (RmaBase.dates_id.HasValue)
                    RmaDates = new RmaDates(RmaBase.dates_id);
            }

        }
        void IDisposable.Dispose()
        {

        }
        //public List<RmaBase> _lstRMAs { get; set; }
      

       
       
        public void LoadParts(int? rma_id)
        {
            this.RmaParts = DataHelper.GetQueryList<RmaParts>("select p.*, PartNo as _PartNo, PartDescription as _PartDescription, Status as _Status from RmaParts p left outer join PartTypes t on p.part_types_id =t.id left outer join RmaStatuses s on p.status_id=s.id where rma_id = '" + rma_id + "'");
        }
        public bool Save()
        {
     
            if (RmaSecuritasOptions != null)
            {
                if (!RmaSecuritasOptions.Save()) return false;
                RmaBase.securitas_options_id = RmaSecuritasOptions.id;
            }
            if (BankInfo != null)
            {
                if (!BankInfo.Save()) return false;
                RmaBase.bankinfo_id = BankInfo.id;
            }
            if (Assessment != null)
            {
                if (!Assessment.Save()) return false;
                RmaBase.assessment_id = Assessment.id;
            }
            if (RmaDates != null)
            {
                if (!RmaDates.Save()) return false;
                RmaBase.dates_id = RmaDates.id;
            }
            if (PuttyTest != null)
            {
                if (!PuttyTest.Save()) return false;
                RmaBase.putty_test_id = PuttyTest.id;
            }
            if (TechInfo != null)
            {
                if (!TechInfo.Save()) return false;
                RmaBase.tech_info_id = TechInfo.id;
            }
            if (RmaTests != null)
            {
                if (!RmaTests.Save()) return false;
                RmaBase.test_results_id = RmaTests.id;
            }
            if (RmaReturnAddress != null)
            {
                if (!RmaReturnAddress.Save()) return false;
                RmaBase.return_address_id = RmaReturnAddress.id;
            }
            if (!this.RmaBase.Save()) return false;

            int seq = 1;
            foreach (RmaParts Part in RmaParts)
            {
                Part.rma_id = this.RmaBase.id;
                Part.seq_no = seq;
                seq++;
                if (!Part.Save()) return false;
            }
            //}
            return true;
        }
       
        public static List<PartTypes> getPartTypes()
        {
            //List<PartTypes> PartTypes = DataHelper.GetQueryList<PartTypes>("PartTypes");
            //PartNumbers.Clear();
            //foreach (PartTypes PartType in PartTypes)
            //{
            //    PartNumbers.Add(PartType.id.ToString(), PartType.PartNo);
            //}
            return DataHelper.GetQueryList<PartTypes>("PartTypes", "PartNo");   

        }
        public static List<RmaProblemTypes> getProblemTypes()
        {
            return DataHelper.GetQueryList<RmaProblemTypes>("RmaProblemTypes");

        }
        public static List<RmaStatuses> getRmaStatuses()
        {
            return DataHelper.GetQueryList<RmaStatuses>("RmaStatuses");

        }
        public static List<RmaForms> getFormTypes()
        {
            return DataHelper.GetQueryList<RmaForms>("RmaForms");

        }
        
        public static List<SelectListItem> getCaseList
        {
            get
            {
                List<SelectListItem> lst = new List<SelectListItem>();
                List<object> Summaries = DataHelper.GetCaseSummaries();
                foreach (var summary in Summaries)
                {
                    string case_no = summary.GetType().GetProperty("case_no").GetValue(summary, null).ToString();
                    string form_name = summary.GetType().GetProperty("form_name").GetValue(summary, null).ToString();
                    //string status = summary.GetType().GetProperty("status").GetValue(summary, null).ToString();

                    if (!String.IsNullOrEmpty(form_name))
                        form_name = " - " + form_name;
                    lst.Add(new SelectListItem { Text = case_no + form_name, Value = case_no });
                }

                return lst;
            }

        }
        private string EmailEntry(string FieldName, string Value, bool NewLine = false, bool Padding=false)
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

        private string EmailEntry_prev(string FieldName, string Value, bool NewLine = false)
        {
            StringBuilder content = new StringBuilder();
            content.Append((FieldName + ": ").PadRight(40));
            if (NewLine)
                content.Append(System.Environment.NewLine);
            else
            {
                Value = (Value ?? "").PadRight(40);
            }
            content.Append(Value);
            return content.ToString();
        }
        private string BuildMessageBody(bool ShowCaseNo = true)
        {
            string EMailTemplateFile = ConfigTools.GetConfigValue("RMA E-Mail Template Path", AppDomain.CurrentDomain.BaseDirectory + "\\Templates\\RMA_Mail_Template.txt");
            StringBuilder NewContent = new StringBuilder();
            if (!System.IO.File.Exists(EMailTemplateFile))
            {
                return "Template File " + EMailTemplateFile + " Not Found";
            }

            string FileBody = System.IO.File.ReadAllText(EMailTemplateFile);
            NewContent.Append("<table>");
            foreach (string Line in Regex.Split(FileBody, System.Environment.NewLine))
            {
                if (!Line.Contains(":"))
                {
                    if (Line.Trim().Length !=0)
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
                        case "Vendor Call # (TR #)":
                            //NewContent.Append("<hr>");
                            NewEntry = EmailEntry(FieldName, RmaBase.tr_no);
                            break;
                        case var expression when (FieldName.Contains("(Securitas)")):
                            //if (this.ViewType == "rma-boa" || this.ViewType == "rma-securitas-general")
                            //{
                                FieldName = FieldName.Replace("(Securitas)", "").Trim();
                                if (FieldName.Contains("Type of RMA") && (!String.IsNullOrEmpty(RmaSecuritasOptions._ReturnTypeDescription)))
                                    NewEntry = EmailEntry(FieldName, RmaSecuritasOptions._ReturnTypeDescription);
                                else if (FieldName.ToLower().Contains("reason for") && (!String.IsNullOrEmpty(RmaSecuritasOptions._CreditReasonDescription)))
                                    NewEntry = EmailEntry(FieldName, RmaSecuritasOptions._CreditReasonDescription);
                            //}
                            //else
                            //    continue;
                            break;
                        case "Case Number":
                            //NewContent.Append("<hr>");
                            NewEntry = EmailEntry(FieldName, RmaBase.case_no);
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
                        case "Return Street Address":
                            //NewContent.Append("<hr>");
                            NewEntry = EmailEntry(FieldName, RmaReturnAddress.StreetAddress);
                            break;
                        case "Return City":
                            NewEntry = EmailEntry(FieldName, RmaReturnAddress.City);
                            break;
                        case "Return State":
                            NewEntry = EmailEntry(FieldName, RmaReturnAddress.State);
                            break;
                        case "Return zip":
                            NewEntry = EmailEntry(FieldName, RmaReturnAddress.ZipCode);
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
                        case "Service Call Date":
                            try
                            {
                                DateTime CaseDate = DateTime.Parse(RmaDates.CaseDate);
                                //NewContent.Append("<hr>");
                                NewEntry = EmailEntry(FieldName, CaseDate.ToString("MM-dd-yy"));
                            }
                            catch
                            {
                                //NewContent.Append("<hr>");
                                NewEntry = EmailEntry(FieldName, "");
                            }

                            break;
                        case "Problem":
                            //NewContent.Append("<hr>");
                            NewEntry = EmailEntry(FieldName, TechInfo.ClientComplaint);
                            break;
                        case "Additional Problem Details":
                            NewEntry = EmailEntry(FieldName, TechInfo.FieldObservation);
                            break;
                        case "Steps Taken":
                            //NewContent.Append("<hr>");
                            NewEntry = EmailEntry(FieldName, TechInfo.StepsUndertaken);
                            break;
                        case var expression when (FieldName.StartsWith("Part ")):
                            int PartNo_a;
                            if (int.TryParse(expression.Contains(" ") ? expression.Split(' ')[1] : "", out PartNo_a) && RmaParts.Count >= PartNo_a && !String.IsNullOrEmpty(RmaParts[PartNo_a - 1].part_types_id))
                            {
                                //NewContent.Append("<hr>");
                                if (ShowCaseNo)
                                {
                                    NewEntry = EmailEntry("Case # " + RmaBase.case_no + "-" + PartNo_a.ToString(), "");
                                    //NewContent.Append(NewEntry + System.Environment.NewLine);
                                    NewContent.Append(NewEntry + "<br>");
                                }
                                NewEntry = EmailEntry("Part", RmaParts[PartNo_a - 1]._PartDescription, Padding: ShowCaseNo);
                            }
                            break;
                        case var expression when (FieldName.StartsWith("Serial Number ")):
                            int PartNo_b;
                            if (int.TryParse(expression.Contains(" ") ? expression.Split(' ')[2] : "", out PartNo_b) && RmaParts.Count >= PartNo_b && !String.IsNullOrEmpty(RmaParts[PartNo_b - 1].part_types_id))
                                NewEntry = EmailEntry("Serial Number", RmaParts[PartNo_b - 1].serial_no, Padding: ShowCaseNo);
                            break;
                        case "Was Parabit Called":
                            //NewContent.Append("<hr>");
                            NewEntry = EmailEntry(FieldName, (bool) (RmaBase.was_parabit_called?? false) ? "Yes" : "No");
                            break;
                        case "Date Called":
                            //DateTime.TryParse(Value == null ? "" : Value.ToString(), out dateValue)
                            try
                            {
                                DateTime ParabitCalledDate = DateTime.Parse(RmaDates.ParabitCalledDate);
                                NewEntry = EmailEntry(FieldName, ParabitCalledDate.ToString("MM-dd-yy"));
                            }
                            catch
                            {
                                NewEntry = EmailEntry(FieldName, "");
                            }
                            break;
                        case "Dispatch #":
                            //NewContent.Append("<hr>");
                            NewEntry = EmailEntry(FieldName, RmaBase.dispatch_no);
                            break;
                        case var expression when (FieldName.StartsWith("Image ")):
                            int PartNo_c;
                            if (int.TryParse(expression.Contains(" ") ? expression.Split(' ')[1] : "", out PartNo_c) && RmaParts.Count >= PartNo_c && !String.IsNullOrEmpty(RmaParts[PartNo_c - 1].part_types_id))
                                NewEntry = EmailEntry("Image Description", RmaParts[PartNo_c - 1].image_description, Padding: ShowCaseNo);
                            break;
                        default:
                            NewEntry = String.Format("<td  colspan='2'>{0}</td>", Line);
                            break;
                    }
                    if (NewEntry != "")
                    {
                        //NewContent.Append(NewEntry + System.Environment.NewLine);
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
        private string BuildMessageBody_prev()
        {
            string EMailTemplateFile = ConfigTools.GetConfigValue("RMA E-Mail Template Path", AppDomain.CurrentDomain.BaseDirectory + "\\Templates\\RMA_Mail_Template.txt");
            StringBuilder NewContent = new StringBuilder();
            if (!System.IO.File.Exists(EMailTemplateFile))
            {
                return "Template File " + EMailTemplateFile + " Not Found";
            }

            string FileBody = System.IO.File.ReadAllText(EMailTemplateFile);
            foreach (string Line in Regex.Split(FileBody, System.Environment.NewLine))
            {
                if (!Line.Contains(":"))
                    NewContent.Append(Line + System.Environment.NewLine);
                else
                {
                    string FieldName = Line.Split(':')[0];
                    string NewEntry = "";
                    switch (FieldName)
                    {
                        case "Submit Date":
                            NewEntry = EmailEntry(FieldName, DateTime.Now.ToString("MMMM dd, yyyy"));
                            break;
                        case "Vendor Call # (TR #)":
                            NewEntry = EmailEntry(FieldName, RmaBase.tr_no);
                            break;
                        case "Case Number":
                            NewEntry = EmailEntry(FieldName, RmaBase.case_no);
                            break;
                        case "Firm Name":
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
                        case "Return Street Address":
                            NewEntry = EmailEntry(FieldName, RmaReturnAddress.StreetAddress);
                            break;
                        case "Return City":
                            NewEntry = EmailEntry(FieldName, RmaReturnAddress.City);
                            break;
                        case "Return State":
                            NewEntry = EmailEntry(FieldName, RmaReturnAddress.State);
                            break;
                        case "Return zip":
                            NewEntry = EmailEntry(FieldName, RmaReturnAddress.ZipCode);
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
                        case "Service Call Date":
                            NewEntry = EmailEntry(FieldName, Convert.ToDateTime(RmaDates.CaseDate).ToString("MM-dd-yy"));
                            break;
                        case "Problem":
                            NewEntry = EmailEntry(FieldName, TechInfo.ClientComplaint);
                            break;
                        case "Additional Problem Details":
                            NewEntry = EmailEntry(FieldName, TechInfo.FieldObservation);
                            break;
                        case "Steps Taken":
                            NewEntry = EmailEntry(FieldName, TechInfo.StepsUndertaken);
                            break;
                        case var expression when (FieldName.StartsWith("Part ")):
                            int PartNo_a;
                            if (int.TryParse(expression.Contains(" ") ? expression.Split(' ')[1] : "", out PartNo_a) && RmaParts[PartNo_a - 1].part_types_id != null)
                            {
                                NewEntry = EmailEntry("Case # " + RmaBase.case_no + "-" + PartNo_a.ToString(), "");
                                NewContent.Append(NewEntry + System.Environment.NewLine);
                                NewEntry = EmailEntry("Part", RmaParts[PartNo_a - 1]._PartDescription);
                            }
                            break;
                        case var expression when (FieldName.StartsWith("Serial Number ")):
                            int PartNo_b;
                            if (int.TryParse(expression.Contains(" ") ? expression.Split(' ')[2] : "", out PartNo_b) && RmaParts[PartNo_b - 1].part_types_id != null)
                                NewEntry = EmailEntry("Serial Number", RmaParts[PartNo_b - 1].serial_no);
                            break;
                        case "Was Parabit Called":
                            NewEntry = EmailEntry(FieldName, (bool)RmaBase.was_parabit_called ? "Yes" : "No");
                            break;
                        case "Date Called":
                            NewEntry = EmailEntry(FieldName, Convert.ToDateTime(RmaDates.ParabitCalledDate).ToString("MM-dd-yy"));
                            break;
                        case "Dispatch #":
                            NewEntry = EmailEntry(FieldName, RmaBase.dispatch_no);
                            break;
                        case var expression when (FieldName.StartsWith("Image ")):
                            int ImageNo;
                            if (int.TryParse(expression.Contains(" ") ? expression.Split(' ')[1] : "", out ImageNo) && RmaParts[ImageNo - 1].part_types_id != null)
                                NewEntry =  EmailEntry("Image Description", RmaParts[ImageNo - 1].image_description);
                            break;
                        default:
                            NewEntry = Line;
                            break;
                    }
                    if (NewEntry != "")
                        NewContent.Append(NewEntry + System.Environment.NewLine);
                }
            }
            return NewContent.ToString();
        }

        public string SendRequestEmail()
        {
            var fromAddress = new MailAddress("parabits1@optonline.net", "Parabit");
            string strAddress = RmaBase._email_address ?? "";
            string strName = RmaBase._email_name??"";

            string TestMode = ConfigTools.GetConfigValue("TestMode", "false");
            if (TestMode == "true" || String.IsNullOrEmpty(strAddress))
            {
                strAddress = ConfigTools.GetConfigValue("Test E-mail", "igory@Parabit.com");
                strName = "";
            }

            if (strName == "") strName = strAddress;
            var toAddress = new MailAddress(strAddress, strName);
            const string fromPassword = "35Debevoise";
            string subject = "RMA Request - Case Number: " + RmaBase.case_no;


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
            //string ImagesDirectory = ConfigTools.GetConfigValue("Images Path", "U:\\temp\\Images\\");
            //if (!Directory.Exists(ImagesDirectory))
            //    try
            //    {
            //        Directory.CreateDirectory(ImagesDirectory);
            //    }
            //    catch
            //    catch
            //    {

            //    }
            foreach (RmaParts Part in RmaParts.Where(i => i.image_path != null))
            {
                if (!System.IO.File.Exists(AppDomain.CurrentDomain.BaseDirectory + Part.image_path))
                    continue;
                //message.Attachments.Add(new System.Net.Mail.Attachment(Image.ImagePath.Contains("\\") ? Image.ImagePath : ImagesDirectory + "\\" + Image.ImagePath));
                string[] PathArray = Part.image_path.Split('!');
                Attachment attachment = new System.Net.Mail.Attachment(AppDomain.CurrentDomain.BaseDirectory + Part.image_path, new System.Net.Mime.ContentType(System.Net.Mime.MediaTypeNames.Image.Jpeg));
                attachment.Name= PathArray[PathArray.Length - 1];
                message.Attachments.Add(attachment);
            }
            //  message.BodyEncoding = System.Text.Encoding.UTF8;
            //  message.DeliveryNotificationOptions = DeliveryNotificationOptions.OnSuccess;
            string status = "OK";
            try
            {
                smtp.Send(message);
                Console.WriteLine("E-mail successfully sent.");
                SaveEmail(strAddress, message.Body, "Request");
            }
            catch (Exception ex)
            {
                status = ex.Message;
            }

            message.Dispose();
            return status;
        }

        public Boolean SaveEmail(string Addresses, string Body, string EmailType)
        {
            string action_query = "insert into EMails (addresses, EmailType, msgbody, rma_id) values ";
            action_query += "('" + String.Join("','", new string[] { Addresses, EmailType, Body.Replace("'", "''"), RmaBase.id.ToString() }) + "')";

            return DataHelper.ActionQuery(action_query);
        }

        public string SendStatusChangeEmail(string rma_status, int? part_no=null)
        {
            var fromAddress = new MailAddress("parabits1@optonline.net", "Parabit");
            string strAddress = TechInfo.TechEmail ?? "";
            string strName = TechInfo.TechName ?? "";

            string TestMode = ConfigTools.GetConfigValue("TestMode", "false");
            if (TestMode == "true" || String.IsNullOrEmpty(strAddress))
            {
                strAddress = ConfigTools.GetConfigValue("Test E-mail", "igory@Parabit.com");
                strName = "";
            }

            if (strName == "") strName = strAddress;
            var toAddress = new MailAddress(strAddress, strName);
            const string fromPassword = "35Debevoise";
            string subject;
            if (part_no != null)
                subject = String.Format("RMA Request - Case # {0}-{1} - Part Status Change", RmaBase.case_no, part_no.ToString());
            else
                subject = String.Format("RMA Request - Case # {0} - Status Change", RmaBase.case_no);

            var smtp = new SmtpClient
            {
                Host = "mail.optonline.net",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
            };
            var message = new MailMessage(fromAddress, toAddress);
            message.Subject = subject;
            message.IsBodyHtml = true;
            message.Body = BuildMessageBody(ShowCaseNo:(part_no==null));
            StringBuilder StatusChangeMsg = new StringBuilder();
            if (part_no!=null)
                StatusChangeMsg.Append(String.Format("<b>Status for this part has been changed to \"{0}\".  Details are below.:</b><br><br><br>", rma_status));
            else
                StatusChangeMsg.Append(String.Format("<b>Status for this RMA has been changed to \"{0}\".  Details are below.:</b><br><br><br>", rma_status));
            message.Body = StatusChangeMsg.ToString() + message.Body;

            if (part_no != null)
            {
                RmaParts Part = this.RmaParts[(int)part_no-1];
                if (System.IO.File.Exists(AppDomain.CurrentDomain.BaseDirectory + Part.image_path))
                {
                    //message.Attachments.Add(new System.Net.Mail.Attachment(Image.ImagePath.Contains("\\") ? Image.ImagePath : ImagesDirectory + "\\" + Image.ImagePath));
                    string[] PathArray = Part.image_path.Split('!');
                    Attachment attachment = new System.Net.Mail.Attachment(AppDomain.CurrentDomain.BaseDirectory + Part.image_path, new System.Net.Mime.ContentType(System.Net.Mime.MediaTypeNames.Image.Jpeg));
                    attachment.Name = PathArray[PathArray.Length - 1];
                    message.Attachments.Add(attachment);
                }
            }

            //message.Body = message.Body.Replace(System.Environment.NewLine, "<br>");
            //  message.BodyEncoding = System.Text.Encoding.UTF8;
            //  message.DeliveryNotificationOptions = DeliveryNotificationOptions.OnSuccess;
            string status = "OK";
            try
            {
                smtp.Send(message);
                Console.WriteLine("E-mail successfully sent.");
                SaveEmail(strAddress, message.Body, "Change Status");
            }
            catch (Exception ex)
            {
                status = ex.Message;
            }

            message.Dispose();
            return status;
        }


    }
}