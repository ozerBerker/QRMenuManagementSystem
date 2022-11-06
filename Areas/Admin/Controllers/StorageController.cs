using FirebaseAdmin;
using FirebaseAdmin.Auth;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.Firestore;
using Microsoft.AspNetCore.Mvc;
using QRMenuManagementSystem.Areas.Admin.Data;

namespace QRMenuManagementSystem.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class StorageController : Controller
    {
        //FirestoreDb firestoreDb;

        public async Task<IActionResult> Index()
        {
            var list = new List<UserRecord>();

            // Iterate through all users. This will still retrieve users in batches,
            // buffering no more than 1000 users in memory at a time.
            var enumerator = FirebaseAuth.DefaultInstance.ListUsersAsync(null).GetAsyncEnumerator();
            while (await enumerator.MoveNextAsync())
            {
                ExportedUserRecord user = enumerator.Current;
                list.Add(user);
            }

            return View(list);

        }
        /*
         * CLOUD FIRESTORAGE CONNECTION EXAMPLE
         * 
        public async Task<IActionResult> Create()
        {
            FirestoreDb firestoreDb = FirestoreDb.Create("qrmenumanagementsystem");
            DocumentReference docRef = firestoreDb.Collection("cities").Document("Arizona");
            Dictionary<string, object> city = new Dictionary<string, object>
            {
                { "name", "Hakkari" },
                { "state", "123" },
                { "country", "Arizona" }
            };
            await docRef.SetAsync(city);
            return RedirectToAction(nameof(Index));
        }
        */
        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            UserRecord userRecord = await FirebaseAuth.DefaultInstance.GetUserAsync(id);
            UserRecordArgs args = new UserRecordArgs()
            {
                Uid = userRecord.Uid,
                Email = userRecord.Email,
                PhoneNumber = userRecord.PhoneNumber,
                EmailVerified = userRecord.EmailVerified,
                DisplayName = userRecord.DisplayName,
                PhotoUrl = userRecord.PhotoUrl,
                Disabled = userRecord.Disabled,
            };
            return View(args);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(UserRecordArgs user)
        {
            try
            {
                UserRecord userRecord = await FirebaseAuth.DefaultInstance.UpdateUserAsync(user);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
            }
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                await FirebaseAuth.DefaultInstance.DeleteUserAsync(id);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(UserRecordArgs user)
        {
            try
            {

                UserRecord userRecord = await FirebaseAuth.DefaultInstance.CreateUserAsync(user);

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {

                ModelState.AddModelError(string.Empty, ex.Message);

            }
            return View();
        }
    }
}
