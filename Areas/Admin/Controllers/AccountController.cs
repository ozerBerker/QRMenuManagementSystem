using Firebase.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using QRMenuManagementSystem.Areas.Admin.Data;
using QRMenuManagementSystem.Areas.Admin.Models;

namespace QRMenuManagementSystem.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AccountController : Controller
    {
        public IActionResult Index()
        {
            var token = HttpContext.Session.GetString("_UserToken");

            if (token != null)
            {
                return RedirectToAction("Index", "Storage");
            }
            else
            {
                return RedirectToAction("SignIn");
            }
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult SignIn()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignIn(LoginModel loginModel)
        {
            FirebaseAuthProvider firebaseAuthProvider = new FirebaseAuthProvider(new FirebaseConfig(new ApiKeys().API_KEY));
            try
            {
                //log in an existing user
                var fbAuthLink = await firebaseAuthProvider
                                .SignInWithEmailAndPasswordAsync(loginModel.Email, loginModel.Password);
                string token = fbAuthLink.FirebaseToken;
                //save the token to a session variable
                if (token != null)
                {
                    HttpContext.Session.SetString("_UserToken", token);

                    return RedirectToAction("Index");
                }

            }
            catch (FirebaseAuthException ex)
            {
                var firebaseEx = JsonConvert.DeserializeObject<FirebaseError>(ex.ResponseData);
                ModelState.AddModelError(String.Empty, firebaseEx.error.message);
                return View(loginModel);
            }

            return View();
        }

        public IActionResult LogOut()
        {
            HttpContext.Session.Remove("_UserToken");
            return RedirectToAction("SignIn");
        }
    }
}
