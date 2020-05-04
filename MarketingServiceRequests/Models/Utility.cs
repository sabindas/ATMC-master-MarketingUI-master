using MarketingServiceRequests.Models;
using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.ServiceModel.Description;
using System.Web;
using FineSMSURLUAT;

namespace MarketingServiceRequests.Controllers
{
    public class Utility
    {
        static IOrganizationService service;

        public Guid ValidateUser()
        {
            string username = ConfigurationManager.AppSettings["username"];
            string password = ConfigurationManager.AppSettings["password"];
            string domain = ConfigurationManager.AppSettings["domain"];
            string soap_url = ConfigurationManager.AppSettings["url"];
            ConnectToMSCRM(username, password, domain, soap_url);
            Guid userid = ((WhoAmIResponse)service.Execute(new WhoAmIRequest())).UserId;
            return userid;
        }
        public static void ConnectToMSCRM(string UserName, string Password, string Domain, string SoapOrgServiceUri)
        {
            try
            {
                ClientCredentials credentials = new ClientCredentials();
                credentials.Windows.ClientCredential = new NetworkCredential(UserName, Password, Domain);
                //credentials.UserName.UserName = UserName;
                //credentials.UserName.Password = Password;
                Uri serviceUri = new Uri(SoapOrgServiceUri);
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                OrganizationServiceProxy proxy = new OrganizationServiceProxy(serviceUri, null, credentials, null);
                proxy.EnableProxyTypes();
                service = (IOrganizationService)proxy;
            }
            catch (Exception ex)
            {
                //KTLogger.LogDebugMessage(String.Format("Error while connecting to CRM: {0}  ", ex.Message));

            }
        }

        public Guid CreateRecord(UserInformation objEntity, string ContactId)
        {
            Guid UserId = ValidateUser();
            Guid Id = Guid.Empty;
            if (UserId != null)
            {
                Entity enMSR = new Entity("campaign");
                enMSR["imc_name"] = objEntity.Name;
                enMSR["imc_position"] = objEntity.Position;
                enMSR["imc_idnumber"] = objEntity.IDNumber;
                enMSR["imc_phonenumber"] = objEntity.PhoneNumber;
                enMSR["emailaddress"] = objEntity.Email;
                enMSR["imc_contact"] = new EntityReference("contact", new Guid(ContactId));
                Id = service.Create(enMSR);
            }
            return Id;
        }

        //Requirement Changed. UpdateServiceDetail converted to CreateServiceDetail
        public void UpdateServiceDetail(ServiceDetails paramservicedetails, ServiceDetailsSelection paramcheckboxes, string Id, string ContactId)
        {
            Guid UserId = ValidateUser();
            if (UserId != null)
            {
                Entity enMSR = new Entity("campaign");
                enMSR.Id = new Guid(Id);

                enMSR["imc_projecttitle"] = paramservicedetails.Title;
                enMSR["imc_businessobjective"] = paramservicedetails.BusinessObjective;
                enMSR["imc_desiredlaunchdate"] = paramservicedetails.DesiredLaunchDate;
                enMSR["imc_requestedby"] = new EntityReference("contact", new Guid(ContactId));

                OptionSetValueCollection targetaudiences = new OptionSetValueCollection();

                if (paramcheckboxes.Public)
                    targetaudiences.Add(new OptionSetValue(100000000));
                if (paramcheckboxes.IMCPatients)
                    targetaudiences.Add(new OptionSetValue(100000001));
                if (paramcheckboxes.IMCEmployees)
                    targetaudiences.Add(new OptionSetValue(100000002));
                if (paramcheckboxes.Other)
                    targetaudiences.Add(new OptionSetValue(100000002));

                enMSR["imc_targetaudiences"] = targetaudiences;






                if (paramcheckboxes.Sponsor)
                {
                    enMSR["imc_costcenterbudgetapproval"] = new OptionSetValue(100000000);
                }
                if (paramcheckboxes.ApprovedBudget)
                {
                    enMSR["imc_costcenterbudgetapproval"] = new OptionSetValue(100000001);
                }
                byte[] bytes;

                if (paramcheckboxes.Sponsor && paramservicedetails.Sponsors != null)
                {
                    using (BinaryReader br = new BinaryReader(paramservicedetails.Sponsors.InputStream))
                    {
                        bytes = br.ReadBytes(paramservicedetails.Sponsors.ContentLength);
                    }
                    CreateNotes("Sponsors", paramservicedetails.Sponsors.FileName, bytes, Id.ToString());
                }

                if (paramcheckboxes.ApprovedBudget && paramservicedetails.ApprovedBudgetFromManagement != null)
                {
                    using (BinaryReader br = new BinaryReader(paramservicedetails.ApprovedBudgetFromManagement.InputStream))
                    {
                        bytes = br.ReadBytes(paramservicedetails.ApprovedBudgetFromManagement.ContentLength);
                    }
                    CreateNotes("Approved Budget From Management", paramservicedetails.ApprovedBudgetFromManagement.FileName, bytes, Id.ToString());
                }

                service.Update(enMSR);
            }
        }

        public Guid CreateServiceDetail(ServiceDetails paramservicedetails, ServiceDetailsSelection paramcheckboxes, string ContactId)
        {
            Guid UserId = ValidateUser();
            Guid Id = Guid.Empty;
            if (UserId != null)
            {
                Entity enMSR = new Entity("campaign");
                enMSR["name"] = paramservicedetails.Title;
                enMSR["imc_projecttitle"] = paramservicedetails.Title;
                enMSR["imc_businessobjective"] = paramservicedetails.BusinessObjective;
                enMSR["imc_desiredlaunchdate"] = paramservicedetails.DesiredLaunchDate;
                enMSR["imc_requestedby"] = new EntityReference("contact", new Guid(ContactId));
                enMSR["imc_marketingrequesttype"] = new OptionSetValue(100000004);

                OptionSetValueCollection targetaudiences = new OptionSetValueCollection();

                if (paramcheckboxes.Public)
                    targetaudiences.Add(new OptionSetValue(100000000));
                if (paramcheckboxes.IMCPatients)
                    targetaudiences.Add(new OptionSetValue(100000001));
                if (paramcheckboxes.IMCEmployees)
                    targetaudiences.Add(new OptionSetValue(100000002));
                if (paramcheckboxes.Other)
                    targetaudiences.Add(new OptionSetValue(100000002));

                enMSR["imc_targetaudiences"] = targetaudiences;


                byte[] bytes;
                if (paramcheckboxes.Sponsor)
                {
                    enMSR["imc_costcenterbudgetapproval"] = new OptionSetValue(100000000);
                }
                if (paramcheckboxes.ApprovedBudget)
                {
                    enMSR["imc_costcenterbudgetapproval"] = new OptionSetValue(100000001);
                }
                Id = service.Create(enMSR);
                if (Id != null && Id != Guid.Empty)
                {
                    if (paramcheckboxes.Sponsor && paramservicedetails.Sponsors != null)
                    {
                        using (BinaryReader br = new BinaryReader(paramservicedetails.Sponsors.InputStream))
                        {
                            bytes = br.ReadBytes(paramservicedetails.Sponsors.ContentLength);
                        }
                        CreateNotes("Sponsors", paramservicedetails.Sponsors.FileName, bytes, Id.ToString());
                    }
                }
                if (paramcheckboxes.ApprovedBudget && paramservicedetails.ApprovedBudgetFromManagement != null)
                {
                    using (BinaryReader br = new BinaryReader(paramservicedetails.ApprovedBudgetFromManagement.InputStream))
                    {
                        bytes = br.ReadBytes(paramservicedetails.ApprovedBudgetFromManagement.ContentLength);
                    }
                    CreateNotes("Approved Budget From Management", paramservicedetails.ApprovedBudgetFromManagement.FileName, bytes, Id.ToString());
                }
            }
            return Id;
        }

        public void CopyWriting(ServiceCopyWriting paramservicecopywriting, string Id)
        {
            Guid UserId = ValidateUser();
            if (UserId != null)
            {
                byte[] bytes;
                if (paramservicecopywriting.ContentVerification != null)
                {
                    using (BinaryReader br = new BinaryReader(paramservicecopywriting.ContentVerification.InputStream))
                    {
                        bytes = br.ReadBytes(paramservicecopywriting.ContentVerification.ContentLength);
                    }
                    CreateNotes("Content Verification", paramservicecopywriting.ContentVerification.FileName, bytes, Id);
                }
                if (paramservicecopywriting.TextDeveloping != null)
                {
                    using (BinaryReader br = new BinaryReader(paramservicecopywriting.TextDeveloping.InputStream))
                    {
                        bytes = br.ReadBytes(paramservicecopywriting.TextDeveloping.ContentLength);
                    }
                    CreateNotes("Text Developing", paramservicecopywriting.TextDeveloping.FileName, bytes, Id);
                }
                if (paramservicecopywriting.Translation != null)
                {
                    using (BinaryReader br = new BinaryReader(paramservicecopywriting.Translation.InputStream))
                    {
                        bytes = br.ReadBytes(paramservicecopywriting.Translation.ContentLength);
                    }
                    CreateNotes("Translation", paramservicecopywriting.Translation.FileName, bytes, Id);
                }
            }
        }

        public void Designs(ServiceDesigns paramServiceDesigns, string Id, HttpPostedFileBase Design_inputfile)
        {
            Guid UserId = ValidateUser();
            if (UserId != null)
            {
                byte[] bytes;
                string strDesigntype = "";
                Entity enMSR = new Entity("campaign");
                enMSR.Id = new Guid(Id);

                OptionSetValueCollection typeofdesigns = new OptionSetValueCollection();

                //if (paramcheckboxes.Public)
                //    targetaudiences.Add(new OptionSetValue(100000000));


                if (paramServiceDesigns.Booklet)
                    typeofdesigns.Add(new OptionSetValue(100000000));
                if (paramServiceDesigns.Flyer)
                    typeofdesigns.Add(new OptionSetValue(100000001));
                if (paramServiceDesigns.Brochure)
                    typeofdesigns.Add(new OptionSetValue(100000002));

                if (paramServiceDesigns.InvitationCard)
                    typeofdesigns.Add(new OptionSetValue(100000003));

                if (paramServiceDesigns.PopupBanner)
                    typeofdesigns.Add(new OptionSetValue(100000004));
                if (paramServiceDesigns.Poster)
                    typeofdesigns.Add(new OptionSetValue(100000005));


                enMSR["imc_typeofdesigns"] = typeofdesigns;



                service.Update(enMSR);






                if (paramServiceDesigns.BookletFile != null)
                {
                    using (BinaryReader br = new BinaryReader(paramServiceDesigns.BookletFile.InputStream))
                    {
                        bytes = br.ReadBytes(paramServiceDesigns.BookletFile.ContentLength);
                    }
                    CreateNotes("Booklet", paramServiceDesigns.BookletFile.FileName, bytes, Id);
                }


                if (paramServiceDesigns.FlyerFile != null)
                {
                    using (BinaryReader br = new BinaryReader(paramServiceDesigns.FlyerFile.InputStream))
                    {
                        bytes = br.ReadBytes(paramServiceDesigns.FlyerFile.ContentLength);
                    }
                    CreateNotes("Flyer", paramServiceDesigns.FlyerFile.FileName, bytes, Id);

                }

                if (paramServiceDesigns.BrochureFile != null)
                {
                    using (BinaryReader br = new BinaryReader(paramServiceDesigns.BrochureFile.InputStream))
                    {
                        bytes = br.ReadBytes(paramServiceDesigns.BrochureFile.ContentLength);
                    }
                    CreateNotes("Brochure", paramServiceDesigns.BrochureFile.FileName, bytes, Id);
                }

                if (paramServiceDesigns.InvitationCardFile != null)
                {
                    using (BinaryReader br = new BinaryReader(paramServiceDesigns.InvitationCardFile.InputStream))
                    {
                        bytes = br.ReadBytes(paramServiceDesigns.InvitationCardFile.ContentLength);
                    }
                    CreateNotes("InvitationCard", paramServiceDesigns.InvitationCardFile.FileName, bytes, Id);
                }
                if (paramServiceDesigns.PopupBannerFile != null)
                {
                    using (BinaryReader br = new BinaryReader(paramServiceDesigns.PopupBannerFile.InputStream))
                    {
                        bytes = br.ReadBytes(paramServiceDesigns.PopupBannerFile.ContentLength);
                    }
                    CreateNotes("PopupBanner", paramServiceDesigns.PopupBannerFile.FileName, bytes, Id);
                }

                if (paramServiceDesigns.PosterFile != null)
                {
                    using (BinaryReader br = new BinaryReader(paramServiceDesigns.PosterFile.InputStream))
                    {
                        bytes = br.ReadBytes(paramServiceDesigns.PosterFile.ContentLength);
                    }
                    CreateNotes("Poster", paramServiceDesigns.PosterFile.FileName, bytes, Id);
                }


            }
        }

        public void Events(ServiceEvents paramServiceEvents, string Id, HttpPostedFileBase Events_inputfile)
        {
            Guid UserId = ValidateUser();
            if (UserId != null)
            {
                byte[] bytes;
                string strLocation = "";
                Entity enMSR = new Entity("campaign");
                enMSR.Id = new Guid(Id);

                OptionSetValueCollection locations = new OptionSetValueCollection();
                if (paramServiceEvents.IMCAuditorium)
                {
                    locations.Add(new OptionSetValue(100000000));
                }
                if (paramServiceEvents.IMCMainlobby)
                {
                    locations.Add(new OptionSetValue(100000001));
                }

                enMSR["imc_locations"] = locations;
                enMSR["imc_duration"] = Convert.ToInt32(paramServiceEvents.duration);
                enMSR["imc_expectednumberofparticipants"] = Convert.ToInt32(paramServiceEvents.Expectedparticipant);

                OptionSetValueCollection eventselements = new OptionSetValueCollection();


                if (paramServiceEvents.AudiovisualSystem)
                    eventselements.Add(new OptionSetValue(1));

                if (paramServiceEvents.Stage)
                    eventselements.Add(new OptionSetValue(2));
                if (paramServiceEvents.PopupBanners)
                    eventselements.Add(new OptionSetValue(3));
                if (paramServiceEvents.Flowers)
                    eventselements.Add(new OptionSetValue(4));
                if (paramServiceEvents.Water)
                    eventselements.Add(new OptionSetValue(5));
                if (paramServiceEvents.Printed)
                    eventselements.Add(new OptionSetValue(6));
                if (paramServiceEvents.Lighting)
                    eventselements.Add(new OptionSetValue(7));
                if (paramServiceEvents.Giveaways)
                    eventselements.Add(new OptionSetValue(8));
                if (paramServiceEvents.VIPgifts)
                    eventselements.Add(new OptionSetValue(9));
                if (paramServiceEvents.VIPgifts)
                    eventselements.Add(new OptionSetValue(10));
                if (paramServiceEvents.VIPLunch)
                    eventselements.Add(new OptionSetValue(11));
                if (paramServiceEvents.PhotoBooth)
                    eventselements.Add(new OptionSetValue(12));
                if (paramServiceEvents.SMSInvitation)
                    eventselements.Add(new OptionSetValue(13));
                if (paramServiceEvents.EmployeesInvitationEmails)
                    eventselements.Add(new OptionSetValue(14));
                if (paramServiceEvents.Fulleventcoverage)
                    eventselements.Add(new OptionSetValue(15));
                if (paramServiceEvents.ActivityItems)
                    eventselements.Add(new OptionSetValue(16));
                if (paramServiceEvents.SocialMedia)
                    eventselements.Add(new OptionSetValue(17));
                if (paramServiceEvents.SocialMediaconverge)
                    eventselements.Add(new OptionSetValue(18));
                if (paramServiceEvents.IMCWebsiteBanner)
                    eventselements.Add(new OptionSetValue(19));
                if (paramServiceEvents.IMCWebsiteAnnouncement)
                    eventselements.Add(new OptionSetValue(20));


                enMSR["imc_eventelements"] = eventselements;


                service.Update(enMSR);

                if (Events_inputfile != null)
                {
                    using (BinaryReader br = new BinaryReader(Events_inputfile.InputStream))
                    {
                        bytes = br.ReadBytes(Events_inputfile.ContentLength);
                    }
                    CreateNotes(strLocation, Events_inputfile.FileName, bytes, Id);
                }
            }
        }
        public void SocialMedia(ServiceSocialMedia paramServiceSocialMedia, string Id)
        {
            Guid UserId = ValidateUser();
            if (UserId != null)
            {
                byte[] bytes;
                if (paramServiceSocialMedia.SocialMediaContentdevelopment != null)
                {
                    using (BinaryReader br = new BinaryReader(paramServiceSocialMedia.SocialMediaContentdevelopment.InputStream))
                    {
                        bytes = br.ReadBytes(paramServiceSocialMedia.SocialMediaContentdevelopment.ContentLength);
                    }
                    CreateNotes("Social Media Content Development and Design", paramServiceSocialMedia.SocialMediaContentdevelopment.FileName, bytes, Id);
                }
                if (paramServiceSocialMedia.MediaCoverage != null)
                {
                    using (BinaryReader br = new BinaryReader(paramServiceSocialMedia.MediaCoverage.InputStream))
                    {
                        bytes = br.ReadBytes(paramServiceSocialMedia.MediaCoverage.ContentLength);
                    }
                    CreateNotes("Social Media Coverage", paramServiceSocialMedia.MediaCoverage.FileName, bytes, Id);
                }
            }
        }

        public void IMCWebsite(ServiceIMCWebsite paramServiceIMCWebsite, string Id)
        {
            Guid UserId = ValidateUser();
            if (UserId != null)
            {
                byte[] bytes;
                if (paramServiceIMCWebsite.PhysicianPhoto != null)
                {
                    using (BinaryReader br = new BinaryReader(paramServiceIMCWebsite.PhysicianPhoto.InputStream))
                    {
                        bytes = br.ReadBytes(paramServiceIMCWebsite.PhysicianPhoto.ContentLength);
                    }
                    CreateNotes("Physician Photo", paramServiceIMCWebsite.PhysicianPhoto.FileName, bytes, Id);
                }
                if (paramServiceIMCWebsite.PhysicianProﬁle != null)
                {
                    using (BinaryReader br = new BinaryReader(paramServiceIMCWebsite.PhysicianProﬁle.InputStream))
                    {
                        bytes = br.ReadBytes(paramServiceIMCWebsite.PhysicianProﬁle.ContentLength);
                    }
                    CreateNotes("Physician Proﬁle", paramServiceIMCWebsite.PhysicianProﬁle.FileName, bytes, Id);
                }
                if (paramServiceIMCWebsite.DepartmentInformation != null)
                {
                    using (BinaryReader br = new BinaryReader(paramServiceIMCWebsite.DepartmentInformation.InputStream))
                    {
                        bytes = br.ReadBytes(paramServiceIMCWebsite.DepartmentInformation.ContentLength);
                    }
                    CreateNotes("Department Information", paramServiceIMCWebsite.DepartmentInformation.FileName, bytes, Id);
                }
                if (paramServiceIMCWebsite.LandingPage != null)
                {
                    using (BinaryReader br = new BinaryReader(paramServiceIMCWebsite.LandingPage.InputStream))
                    {
                        bytes = br.ReadBytes(paramServiceIMCWebsite.LandingPage.ContentLength);
                    }
                    CreateNotes("Landing Page", paramServiceIMCWebsite.LandingPage.FileName, bytes, Id);
                }
                if (paramServiceIMCWebsite.IMCAcademyCourses != null)
                {
                    using (BinaryReader br = new BinaryReader(paramServiceIMCWebsite.IMCAcademyCourses.InputStream))
                    {
                        bytes = br.ReadBytes(paramServiceIMCWebsite.IMCAcademyCourses.ContentLength);
                    }
                    CreateNotes("IMC Academy Courses", paramServiceIMCWebsite.IMCAcademyCourses.FileName, bytes, Id);
                }
            }
        }

        public void Production(ServiceProduction paramServiceProduction, string Id)
        {
            Guid UserId = ValidateUser();
            if (UserId != null)
            {
                byte[] bytes;
                if (paramServiceProduction.VideoProduction != null)
                {
                    using (BinaryReader br = new BinaryReader(paramServiceProduction.VideoProduction.InputStream))
                    {
                        bytes = br.ReadBytes(paramServiceProduction.VideoProduction.ContentLength);
                    }
                    CreateNotes("Video Production", paramServiceProduction.VideoProduction.FileName, bytes, Id);
                }
                if (paramServiceProduction.VideographyCoveragee != null)
                {
                    using (BinaryReader br = new BinaryReader(paramServiceProduction.VideographyCoveragee.InputStream))
                    {
                        bytes = br.ReadBytes(paramServiceProduction.VideographyCoveragee.ContentLength);
                    }
                    CreateNotes("Videography Coveragee", paramServiceProduction.VideographyCoveragee.FileName, bytes, Id);
                }
                if (paramServiceProduction.PhotographyCoverage != null)
                {
                    using (BinaryReader br = new BinaryReader(paramServiceProduction.PhotographyCoverage.InputStream))
                    {
                        bytes = br.ReadBytes(paramServiceProduction.PhotographyCoverage.ContentLength);
                    }
                    CreateNotes("Photography Coverage", paramServiceProduction.PhotographyCoverage.FileName, bytes, Id);
                }
                if (paramServiceProduction.PhotographySessions != null)
                {
                    using (BinaryReader br = new BinaryReader(paramServiceProduction.PhotographySessions.InputStream))
                    {
                        bytes = br.ReadBytes(paramServiceProduction.PhotographySessions.ContentLength);
                    }
                    CreateNotes("Photography Sessions", paramServiceProduction.PhotographySessions.FileName, bytes, Id);
                }
                if (paramServiceProduction.RecordingSession != null)
                {
                    using (BinaryReader br = new BinaryReader(paramServiceProduction.RecordingSession.InputStream))
                    {
                        bytes = br.ReadBytes(paramServiceProduction.RecordingSession.ContentLength);
                    }
                    CreateNotes("Recording Session", paramServiceProduction.RecordingSession.FileName, bytes, Id);
                }
                if (paramServiceProduction.PhotographyPermission != null)
                {
                    using (BinaryReader br = new BinaryReader(paramServiceProduction.PhotographyPermission.InputStream))
                    {
                        bytes = br.ReadBytes(paramServiceProduction.PhotographyPermission.ContentLength);
                    }
                    CreateNotes("Videography / Photography Permission", paramServiceProduction.PhotographyPermission.FileName, bytes, Id);
                }
            }
        }

        public void CreateNotes(string subject, string filename, byte[] data, string entityId)
        {
            Entity note = new Entity("annotation");
            note["subject"] = subject;
            note["filename"] = filename;
            note["documentbody"] = Convert.ToBase64String(data);
            note["objectid"] = new EntityReference("campaign", new Guid(entityId));
            service.Create(note);
        }

        public Contact IsValidUser(Login objLogin)
        {
            Guid UserId = ValidateUser();
            Contact objContact = new Contact();
            if (UserId != null)
            {
                QueryExpression qryContact = new QueryExpression("contact");
                qryContact.Criteria.AddCondition("emailaddress1", ConditionOperator.Equal, objLogin.UserName);
                qryContact.Criteria.AddCondition("emailaddress1", ConditionOperator.Equal, objLogin.Password);
                qryContact.ColumnSet = new ColumnSet(new string[] { "fullname", "contactid" });
                EntityCollection ecContact = service.RetrieveMultiple(qryContact);
                if (ecContact.Entities.Count > 0)
                {
                    objContact.Fullname = ecContact.Entities[0].Attributes["fullname"].ToString();
                    objContact.ContactId = ecContact.Entities[0].Attributes["contactid"].ToString();
                }
            }
            return objContact;
        }

        public List<MarketingServiceRequest> GetMarketingServiceRequest(string ContactId)
        {

            QueryExpression qryMSR = new QueryExpression("campaign");
            qryMSR.Criteria.AddCondition("imc_requestedby", ConditionOperator.Equal, new Guid(ContactId));
            qryMSR.Criteria.AddCondition("statecode", ConditionOperator.Equal, 0);
            qryMSR.ColumnSet = new ColumnSet(true);
            EntityCollection ecMSR = service.RetrieveMultiple(qryMSR);
            List<MarketingServiceRequest> lstMSR = new List<MarketingServiceRequest>();
            Guid UserId = ValidateUser();
            if (UserId != null)
            {
                if (ecMSR.Entities.Count > 0)
                {
                    string strCostCenter = "";
                    string strStatus = "";
                    string DesireDate = "";
                    string strProjectTitle = "";
                    string strBusinessObjective = "";
                    string[] arrDesireDate = null;
                    string strDesireDate = "";
                    for (int i = 0; i < ecMSR.Entities.Count; i++)
                    {
                        if (ecMSR.Entities[i].Contains("imc_costcenterbudgetapproval"))
                        {
                            if (((OptionSetValue)ecMSR.Entities[i].Attributes["imc_costcenterbudgetapproval"]).Value == 100000000)
                                strCostCenter = "Sponsers";
                            else if (((OptionSetValue)ecMSR.Entities[i].Attributes["imc_costcenterbudgetapproval"]).Value == 100000001)
                                strCostCenter = "Approved budget From Management";
                        }
                        if (ecMSR.Entities[i].Contains("imc_servicerequeststatus"))
                        {
                            if (((OptionSetValue)ecMSR.Entities[i].Attributes["imc_servicerequeststatus"]).Value == 100000000)
                                strStatus = "Pending";
                            else if (((OptionSetValue)ecMSR.Entities[i].Attributes["imc_servicerequeststatus"]).Value == 100000001)
                                strStatus = "Rejected";
                            else if (((OptionSetValue)ecMSR.Entities[i].Attributes["imc_servicerequeststatus"]).Value == 100000002)
                                strStatus = "Approved";
                            else if (((OptionSetValue)ecMSR.Entities[i].Attributes["imc_servicerequeststatus"]).Value == 100000003)
                                strStatus = "In Progress";
                            else if (((OptionSetValue)ecMSR.Entities[i].Attributes["imc_servicerequeststatus"]).Value == 100000004)
                                strStatus = "Completed";
                        }

                        if (ecMSR.Entities[i].Contains("imc_desiredlaunchdate"))
                        {
                            DateTime ParameterDesiredDate = (DateTime)ecMSR.Entities[i].Attributes["imc_desiredlaunchdate"];
                            DesireDate = CommonUtility.RetrieveLocalTimeFromUTCTime(service, ParameterDesiredDate).ToString();
                            arrDesireDate = DesireDate.Split(' ');
                            strDesireDate = arrDesireDate[0];
                        }
                        if (ecMSR.Entities[i].Contains("imc_projecttitle"))
                        {
                            strProjectTitle = ecMSR.Entities[i].Attributes["imc_projecttitle"].ToString();
                        }
                        if (ecMSR.Entities[i].Contains("imc_businessobjective"))
                        {
                            strBusinessObjective = ecMSR.Entities[i].Attributes["imc_businessobjective"].ToString();
                        }
                        //ecMSR.Entities[i].Attributes["imc_desireddatelaunchdate"]

                        lstMSR.Add(new MarketingServiceRequest
                        {
                            ProjectTitle = strProjectTitle,
                            DesiredDate = strDesireDate,
                            BusinessObjective = strBusinessObjective,
                            CostCenter = strCostCenter,
                            Status = strStatus
                        });
                    }
                }
            }

            return lstMSR;
        }
    }
}