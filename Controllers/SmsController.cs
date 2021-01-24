using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EriRootSms.API.common;
using EriRootSms.API.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Twilio;
using Twilio.AspNet.Common;
using Twilio.AspNet.Core;
using Twilio.Rest.Api.V2010.Account;
using Twilio.TwiML;
using Twilio.Types;

namespace EriRootSms.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SmsController : TwilioController
    {
        private readonly IMyRootsSmsSettings _myRootsSmsSettings;

        public SmsController(IMyRootsSmsSettings myRootsSmsSettings)
        {
            _myRootsSmsSettings = myRootsSmsSettings;
        }


        [HttpGet("{to}")]
        public IActionResult SendSms(string to)
        {
            var accountSid = _myRootsSmsSettings.AccountSid;

            to = $"{to}";
            TwilioClient.Init(_myRootsSmsSettings.AccountSid, _myRootsSmsSettings.AuthToken);

            var fromPhoneNumber = new PhoneNumber(_myRootsSmsSettings.TwilioPhoneFrom);

            var toPhoneNumbers = to.Split(',', '|');

            var sids = (from phoneNumber in toPhoneNumbers
                        where !string.IsNullOrEmpty(phoneNumber)
                        select new PhoneNumber(phoneNumber)
                into toPhoneNumber
                        select MessageResource.Create(toPhoneNumber, @from: fromPhoneNumber,
                            body: "Hello from EriRoots. Have a great day!")
                into message
                        select message.Sid).ToList();


            return new JsonResult(sids);
        }


        [HttpPost]
        public IActionResult SendMessage([FromBody] List<SmsMessageViewModel> vms)
        {
            if (vms == null || vms.Count == 0) return NoContent();

            var accountSid = _myRootsSmsSettings.AccountSid;

            TwilioClient.Init(_myRootsSmsSettings.AccountSid, _myRootsSmsSettings.AuthToken);

            var fromPhoneNumber = new PhoneNumber(_myRootsSmsSettings.TwilioPhoneFrom);

            var messageResourceTasks = new List<Task<MessageResource>>();

            var mtsks = new Task[vms.Count];

            //var msgIndex = 0;

            var sids = new List<MessageResource>();

            foreach (var vm in vms)
            {
                var toPhoneNumber = vm.PhoneNumber;
                //var msgResourceTask = MessageResource.CreateAsync(toPhoneNumber, _myRootsSmsSettings.AccountSid, fromPhoneNumber,
                //    vm.Message);
                var msgResource = MessageResource.Create(toPhoneNumber, from: fromPhoneNumber,
                    body: vm.Message
                );
                // mtsks[msgIndex++] = msgResourceTask;
                //messageResourceTasks.Add(msgResourceTask);
                sids.Add(msgResource);
            }

            //Task.WaitAll(mtsks);

            //for (var i=0; i < mtsks.Length; i++)
            //{
            //    sids.Add(mtsks[i].r);
            //}

            //foreach (var i in mtsks)
            //{
            //        sids.Add(task.Result);
            //}

            return new JsonResult(sids);
        }


        public TwiMLResult ReceiveSms(SmsRequest incomigMessage)
        {
            var response = new MessagingResponse();
            response.Message($"Incoming Message says: {incomigMessage} but Twilio says all is going to be good!");

            return TwiML(response);
        }
    }
}
