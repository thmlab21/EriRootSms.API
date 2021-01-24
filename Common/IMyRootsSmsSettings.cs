using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.IdentityModel.Protocols;
using Twilio.AspNet;
using Twilio.Rest.Api.V2010.Account.Usage;

namespace EriRootSms.API.common
{
    public interface IMyRootsSmsSettings
    {
         string AccountSid { get; set; }
         string AuthToken { get; set; }
         string TwilioPhoneFrom { get; set; }
    }
}