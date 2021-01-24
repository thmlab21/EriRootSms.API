namespace EriRootSms.API.common
{
    public class MyRootsSmsSettings:IMyRootsSmsSettings
    {
        private string _accountSid;
        private string _authToken;
        private string _twilioPhoneFrom;

        public string AccountSid
        {
            get => _accountSid;
            set => _accountSid = value;
        }

        public string AuthToken
        {
            get => _authToken;
            set => _authToken = value;
        }

        public string TwilioPhoneFrom
        {
            get => _twilioPhoneFrom;
            set => _twilioPhoneFrom = value;
        }

        public MyRootsSmsSettings(string sid, string token, string twilioPhone)
        {
            _accountSid = sid;
            _authToken = token;
            _twilioPhoneFrom = twilioPhone;
        }

        public MyRootsSmsSettings()
        {
            
        }
    }
}