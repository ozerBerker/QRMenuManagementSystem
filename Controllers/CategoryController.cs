using Microsoft.AspNetCore.Mvc;
using QRMenuManagementSystem.Models;
using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp.Response;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace QRMenuManagementSystem.Controllers
{
    public class CategoryController : Controller
    {
        //IFirebaseConfig config = new FirebaseConfig
        //{
        //    AuthSecret = "AE7MP9QOt8z9ZJqWJamEPXY7xTNMwUyzBvDzb9Da",
        //    BasePath = "https://qrmenumanagementsystem-default-rtdb.firebaseio.com/"
        //};
        //IFirebaseClient client;
        FirebaseConfigConnection connection = new FirebaseConfigConnection();
        

        public IActionResult Index()
        {
            connection.client = new FireSharp.FirebaseClient(connection.config);
            FirebaseResponse response = connection.client.Get("Categories");
            dynamic data = JsonConvert.DeserializeObject<dynamic>(response.Body);
            var list = new List<Category>();
            if(data != null)
            {
                foreach (var item in data)
                {
                    list.Add(JsonConvert.DeserializeObject<Category>(((JProperty)item).Value.ToString()));
                }
            }

            return View(list);
        }

        public IActionResult Products(string id)
        {
            connection.client = new FireSharp.FirebaseClient(connection.config);
            FirebaseResponse response = connection.client.Get("Products");
            dynamic data = JsonConvert.DeserializeObject<dynamic>(response.Body);
            var list = new List<Product>();
            if (data != null)
            {
                foreach (var item in data)
                {
                    list.Add(JsonConvert.DeserializeObject<Product>(((JProperty)item).Value.ToString()));
                }
            }
            list = list.Where(x=>x.CategoryId == id).ToList();
            return View(list);
        }

        [HttpGet]
        public IActionResult Delete(string id)
        {
            connection.client = new FireSharp.FirebaseClient(connection.config);
            FirebaseResponse response = connection.client.Delete("Categories/" + id);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> DetailsAsync(string id)
        {
            connection.client = new FireSharp.FirebaseClient(connection.config);
            FirebaseResponse response = await connection.client.GetAsync("Categories/" + id);
            Category data = JsonConvert.DeserializeObject<Category>(response.Body);
            return View(data);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            connection.client = new FireSharp.FirebaseClient(connection.config);
            FirebaseResponse response = await connection.client.GetAsync("Categories/" + id);
            Category data = JsonConvert.DeserializeObject<Category>(response.Body);
            return View(data);
        }

        [HttpPost]
        public IActionResult Edit(Category _category)
        {
            connection.client = new FireSharp.FirebaseClient(connection.config);
            SetResponse response = connection.client.Set("Categories/" + _category.CategoryId, _category);
            return RedirectToAction("Index");
        }


        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Category _category)
        {
            try
            {
                connection.client = new FireSharp.FirebaseClient(connection.config);
                var data = _category;
                PushResponse response = connection.client.Push("Categories/", data);
                data.CategoryId = response.Result.name;
                SetResponse setResponse = connection.client.Set("Categories/" + data.CategoryId, data);

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
            }

            return View();

        }

    }
}
