using Firebase.Auth;
using FireSharp.Interfaces;
using FireSharp.Response;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using QRMenuManagementSystem.Areas.Admin.Data;
using QRMenuManagementSystem.Areas.Admin.Models;

namespace QRMenuManagementSystem.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class HomeController : Controller
    {
        public IFirebaseConfig config = new FireSharp.Config.FirebaseConfig
        {
            AuthSecret = new ApiKeys().API_SECRET,
            BasePath = new ApiKeys().API_PATH
        };
        public IFirebaseClient client;

        // GET: HomeController
        public ActionResult Index()
        {
            client = new FireSharp.FirebaseClient(config);
            FirebaseResponse response = client.Get("Customers");
            dynamic data = JsonConvert.DeserializeObject<dynamic>(response.Body);
            var list = new List<Customer>();
            if (data != null)
            {
                foreach (var item in data)
                {
                    list.Add(JsonConvert.DeserializeObject<Customer>(((JProperty)item).Value.ToString()));
                }
            }

            return View(list);
        }

        //// GET: HomeController/Details/5
        //public async Task<ActionResult> DetailsAsync(string id)
        //{
        //    client = new FireSharp.FirebaseClient(config);
        //    FirebaseResponse response = await client.GetAsync("Customers/" + id);
        //    Customer data = JsonConvert.DeserializeObject<Customer>(response.Body);
        //    return View(data);
        //}

        // GET: HomeController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: HomeController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Customer customer)
        {
            try
            {
                client = new FireSharp.FirebaseClient(config);

                var LocalId = Convert.ToString(Registation());
                var data = customer;
                data.CustomerId = LocalId;

                PushResponse response = await client.PushAsync("Customers/", data);
                //data.CustomerId = response.Result.name;
                SetResponse setResponse = client.Set("Customers/" + data.CustomerId, data);

                if (setResponse.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    ModelState.AddModelError(string.Empty, "Added Succesfully");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Something went wrong!!");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View();
            }

            return RedirectToAction("Registration", "Account", new LoginModel("test123@test.com", "12345TEST"));
        }

        public async Task<string> Registation()
        {
            FirebaseAuthProvider firebaseAuthProvider = new FirebaseAuthProvider(new FirebaseConfig(new ApiKeys().API_KEY));
            try
            {
                //create the user
                var firebaseAuthLink = await firebaseAuthProvider.CreateUserWithEmailAndPasswordAsync("test123@test.com", "12345TEST");
                var LocalId = firebaseAuthLink.User.LocalId;
                //saving the token in a session variable
                if (LocalId != null)
                {
                    return LocalId;
                }
            }
            catch (FirebaseAuthException ex)
            {
                var firebaseEx = JsonConvert.DeserializeObject<FirebaseError>(ex.ResponseData);
                ModelState.AddModelError(String.Empty, firebaseEx.error.message);
                return null;
            }

            return null;

        }

        // GET: HomeController/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            client = new FireSharp.FirebaseClient(config);

            FirebaseResponse response = await client.GetAsync("Customers/" + id);
            Customer data = JsonConvert.DeserializeObject<Customer>(response.Body);
            return View(data);
        }

        // POST: HomeController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Customer customer)
        {
            client = new FireSharp.FirebaseClient(config);
            try
            {
                SetResponse response = client.Set("Customers/" + customer.CustomerId, customer);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View();
            }
        }

        // POST: HomeController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditCustomerActive(string id)
        {
            client = new FireSharp.FirebaseClient(config);
            try
            {

                FirebaseResponse response = await client.GetAsync("Customers/" + id);
                Customer data = JsonConvert.DeserializeObject<Customer>(response.Body);
                data.CustomerActive = !(data.CustomerActive);
                SetResponse setresponse = client.Set("Customers/" + data.CustomerId, data);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View();
            }
        }

        // POST: HomeController/Delete/5
        [HttpGet]
        public ActionResult Delete(string id)
        {
            client = new FireSharp.FirebaseClient(config);
            try
            {
                FirebaseResponse response = client.Delete("Customers/" + id);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
