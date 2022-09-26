using FireSharp.Config;
using FireSharp.Interfaces;

namespace QRMenuManagementSystem.Models
{
    public class FirebaseConfigConnection
    {
        public IFirebaseConfig config = new FirebaseConfig
        {
            AuthSecret = "AE7MP9QOt8z9ZJqWJamEPXY7xTNMwUyzBvDzb9Da",
            BasePath = "https://qrmenumanagementsystem-default-rtdb.firebaseio.com/"
        };
        public IFirebaseClient client;
    }
}
