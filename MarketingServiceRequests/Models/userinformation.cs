using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MarketingServiceRequests.Models
{
    public class UserInformation
    {
        
        [Required]
        public string Name { get; set; }
        [Required]
        public string Position { get; set; }
        [Required]
        public string IDNumber { get; set; }
        [Required]

        public string PhoneNumber { get; set; }
        [Required]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }

    }

    public class ServiceDetails
    {        
        public string Title { get; set; }

        public string BusinessObjective { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DesiredLaunchDate { get; set; }


        public string TargetAudience { get; set; }

        public string CostCenter { get; set; }

        public HttpPostedFileBase Sponsors { get; set; }
        public HttpPostedFileBase ApprovedBudgetFromManagement { get; set; }
    }

    public class ServiceDetailsSelection
    {
        public bool Public { get; set; }
        public bool IMCPatients { get; set; }
        public bool IMCEmployees { get; set; }
        public bool Other { get; set; }
        public bool Sponsor { get; set; }
        public bool ApprovedBudget { get; set; }

    }

    public class ServiceCopyWriting
    {
        public HttpPostedFileBase ContentVerification { get; set; }
        public HttpPostedFileBase Translation { get; set; }
        public HttpPostedFileBase TextDeveloping { get; set; }

    }

    public class ServiceDesigns
    {
        public bool Booklet { get; set; }
        public bool Flyer { get; set; }
        public bool Brochure { get; set; }
        public bool InvitationCard { get; set; }
        public bool PopupBanner { get; set; }
        public bool Poster { get; set; }

        public HttpPostedFileBase BookletFile { get; set; }
        public HttpPostedFileBase FlyerFile { get; set; }
        public HttpPostedFileBase BrochureFile { get; set; }
        public HttpPostedFileBase InvitationCardFile { get; set; }
        public HttpPostedFileBase PopupBannerFile { get; set; }
        public HttpPostedFileBase PosterFile { get; set; }
    }

    public class ServiceEvents
    {
        public bool IMCMainlobby { get; set; }
        public bool IMCAuditorium { get; set; }

        [Required]
        public int duration { get; set; }
        [Required]
        public int Expectedparticipant { get; set; }


        public bool AudiovisualSystem { get; set; }
        public bool Stage { get; set; }
        public bool PopupBanners { get; set; }
        public bool Flowers { get; set; }
        public bool Water { get; set; }
        public bool Printed { get; set; }
        public bool Lighting { get; set; }
        public bool Giveaways { get; set; }
        public bool VIPgifts { get; set; }
        public bool VIPLunch { get; set; }
        public bool PhotoBooth { get; set; }
        public bool DecorativeItems { get; set; }
        public bool SMSInvitation { get; set; }
        public bool EmployeesInvitationEmails { get; set; }
        public bool Fulleventcoverage { get; set; }
        public bool ActivityItems { get; set; }
        public bool SocialMedia { get; set; }
        public bool SocialMediaconverge { get; set; }
        public bool IMCWebsiteBanner { get; set; }
        public bool IMCWebsiteAnnouncement { get; set; }
    }
    public class ServiceSocialMedia
    {
        public HttpPostedFileBase SocialMediaContentdevelopment { get; set; }
        public HttpPostedFileBase MediaCoverage { get; set; }
    }

    public class ServiceIMCWebsite
    {
        public HttpPostedFileBase PhysicianProﬁle { get; set; }
        public HttpPostedFileBase PhysicianPhoto { get; set; }
        public HttpPostedFileBase DepartmentInformation { get; set; }
        public HttpPostedFileBase LandingPage { get; set; }
        public HttpPostedFileBase IMCAcademyCourses { get; set; }        

    }

    public class ServiceProduction
    {
        public HttpPostedFileBase VideoProduction  { get; set; }
        public HttpPostedFileBase PhotographyCoverage  { get; set; }

        public HttpPostedFileBase VideographyCoveragee   { get; set; }
        public HttpPostedFileBase PhotographySessions  { get; set; }
        public HttpPostedFileBase RecordingSession  { get; set; }

        public HttpPostedFileBase PhotographyPermission { get; set; }
    }

    public class Login
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
    }

    //public class GridParameter
    //{
    //    [Required]
    //    public int Id { get; set; }
    //    [Required]
    //    public string column2 { get; set; }

    //    public string column3 { get; set; }
    //    public string column4 { get; set; }
    //    public string column5 { get; set; }

    //}

    public class Contact
    {
        public string Fullname { get; set; }
        public string ContactId { get; set; }
    }

    public class MarketingServiceRequest
    {
        public string ProjectTitle { get; set; }
        public string DesiredDate { get; set; }
        public string BusinessObjective { get; set; }
        public string CostCenter { get; set; }
        public string Status { get; set; }

    }

    public class SMS
    {
        
        public string SMSTextContent { get; set; }
        public bool EnglishSMS { get; set; }
        public bool ArabicSMS { get; set; }
        public string TargetAudienceDescription { get; set; }  
    }

    public class Email
    {
        public string EmailSubject { get; set; }
        public string EmailTextContent { get; set; }
        public bool InternalEmail { get; set; }
        public bool ExternalEmail { get; set; }
        public string TargetAudienceDescription { get; set; }

    }



}