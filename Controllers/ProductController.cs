using Microsoft.AspNetCore.Mvc;
using QRMenuManagementSystem.Models;
using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp.Response;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace QRMenuManagementSystem.Controllers
{
    public class ProductController : Controller
    {

        FirebaseConfigConnection connection = new FirebaseConfigConnection();


        public IActionResult Index()
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

            return View(list);
        }

        [HttpGet]
        public IActionResult Delete(string id)
        {
            connection.client = new FireSharp.FirebaseClient(connection.config);
            FirebaseResponse response = connection.client.Delete("Products/" + id);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> DetailsAsync(string id)
        {
            connection.client = new FireSharp.FirebaseClient(connection.config);
            FirebaseResponse response = await connection.client.GetAsync("Products/" + id);
            Product data = JsonConvert.DeserializeObject<Product>(response.Body);
            return View(data);
        }

        [HttpGet]
        public async Task<IActionResult> EditAsync(string id)
        {
            connection.client = new FireSharp.FirebaseClient(connection.config);
            FirebaseResponse response = await connection.client.GetAsync("Products/" + id);
            Product data = JsonConvert.DeserializeObject<Product>(response.Body);
            ViewBag.categories = CategoriesList();
            return View(data);
        }

        [HttpPost]
        public IActionResult Edit(Product _product)
        {
            connection.client = new FireSharp.FirebaseClient(connection.config);
            Category _category = CategoriesList().Find(x => x.CategoryId == _product.CategoryId);
            _product.category = _category;
            SetResponse response = connection.client.Set("Products/" + _product.ProductId, _product);
            return RedirectToAction("Index");
        }


        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.categories = CategoriesList();
            return View();
        }

        [HttpPost]
        public IActionResult Create(Product _product)
        {
            try
            {
                Category _category = CategoriesList().Find(x => x.CategoryId == _product.CategoryId);
                _product.category = _category;
                var data = _product;
                PushResponse response = connection.client.Push("Products/", data);
                data.ProductId = response.Result.name;
                SetResponse setResponse = connection.client.Set("Products/" + data.ProductId, data);

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
            ViewBag.categories = CategoriesList();
            return View();

        }

        public List<Category> CategoriesList()
        {
            connection.client = new FireSharp.FirebaseClient(connection.config);
            FirebaseResponse response = connection.client.Get("Categories");
            dynamic data = JsonConvert.DeserializeObject<dynamic>(response.Body);
            var list = new List<Category>();
            if (data != null)
            {
                foreach (var item in data)
                {
                    list.Add(JsonConvert.DeserializeObject<Category>(((JProperty)item).Value.ToString()));
                }
            }
            return list;
        }

    }
}
