using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace FineSMSURLUAT
{
    internal static class CommonUtility
    {
        internal static DateTime RetrieveLocalTimeFromUTCTime(IOrganizationService service, DateTime utcTime)
        {
            return RetrieveLocalTimeFromUTCTime(utcTime, RetrieveCurrentUsersSettings(service), service);
        }

        internal static DateTime RetrieveUTCTimeFromLocalTime(IOrganizationService service, DateTime localTime)
        {
            return RetrieveUTCTimeFromLocalTime(localTime, RetrieveCurrentUsersSettings(service), service);
        }

        internal static int? RetrieveCurrentUsersSettings(IOrganizationService service)
        {
            var currentUserSettings = service.RetrieveMultiple(
                new QueryExpression("usersettings")
                {
                    ColumnSet = new ColumnSet("timezonecode"),
                    Criteria = new FilterExpression
                    {
                        Conditions =
                        {
                    new ConditionExpression("systemuserid", ConditionOperator.EqualUserId)
                        }
                    }
                }).Entities[0].ToEntity<Entity>();
            return (int?)currentUserSettings.Attributes["timezonecode"];
        }

        internal static DateTime RetrieveLocalTimeFromUTCTime(DateTime utcTime, int? timeZoneCode, IOrganizationService service)
        {
            if (!timeZoneCode.HasValue)
                return DateTime.Now;
            var request = new LocalTimeFromUtcTimeRequest
            {
                TimeZoneCode = timeZoneCode.Value,
                UtcTime = utcTime.ToUniversalTime()
            };
            var response = (LocalTimeFromUtcTimeResponse)service.Execute(request);
            return response.LocalTime;
        }

        internal static DateTime RetrieveUTCTimeFromLocalTime(DateTime localTime, int? timeZoneCode, IOrganizationService service)
        {
            if (!timeZoneCode.HasValue)
                return DateTime.Now;
            var request = new UtcTimeFromLocalTimeRequest
            {
                TimeZoneCode = timeZoneCode.Value,
                LocalTime = localTime
            };
            var response = (UtcTimeFromLocalTimeResponse)service.Execute(request);
            return response.UtcTime;
        }
    }
}