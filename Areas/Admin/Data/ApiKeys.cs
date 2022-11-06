using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;

namespace QRMenuManagementSystem.Areas.Admin.Data
{
    public class ApiKeys
    {

        public string API_KEY;
        public string API_SECRET;
        public string API_PATH;
        public string GOOGLE_APPLICATION_CREDENTIALS;
        public ApiKeys()
        {
            API_KEY = "AIzaSyDRtWigsZkyCtnXWsJJZvRDPGNrtnq1Tgo";
            API_SECRET = "AE7MP9QOt8z9ZJqWJamEPXY7xTNMwUyzBvDzb9Da";
            API_PATH = "https://qrmenumanagementsystem-default-rtdb.firebaseio.com/";
        }
    }
}