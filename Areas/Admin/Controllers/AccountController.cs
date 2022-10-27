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
                return RedirectToAction("Index", "Home");
            }
            else
            {
                return RedirectToAction("SignIn");
            }
        }

        [HttpPost]
        public async Task<string> Registration(LoginModel loginModel)
        {
            FirebaseAuthProvider firebaseAuthProvider = new FirebaseAuthProvider(new FirebaseConfig(new ApiKeys().API_KEY));
            try
            {
                //create the user
                var firebaseAuthLink = await firebaseAuthProvider.CreateUserWithEmailAndPasswordAsync(loginModel.Email, loginModel.Password);
                string token = firebaseAuthLink.User.LocalId;


                //saving the token in a session variable
                if (token != null)
                {
                    return token;
                }
            }
            catch (FirebaseAuthException ex)
            {
                var firebaseEx = JsonConvert.DeserializeObject<FirebaseError>(ex.ResponseData);
                ModelState.AddModelError(String.Empty, firebaseEx.error.message);
                return null;
            }

            return null;

            //FirebaseAuthProvider firebaseAuthProvider = new FirebaseAuthProvider(new FirebaseConfig(new ApiKeys().API_KEY));
            //try
            //{
            //    //create the user
            //    await firebaseAuthProvider.CreateUserWithEmailAndPasswordAsync(loginModel.Email, loginModel.Password);
            //    //log in the new user
            //    var fbAuthLink = await firebaseAuthProvider
            //                    .SignInWithEmailAndPasswordAsync(loginModel.Email, loginModel.Password);
            //    string token = fbAuthLink.FirebaseToken;
            //    //saving the token in a session variable
            //    if (token != null)
            //    {
            //        HttpContext.Session.SetString("_UserToken", token);

            //        return RedirectToAction("Index", "Home");
            //    }
            //}
            //catch (FirebaseAuthException ex)
            //{
            //    var firebaseEx = JsonConvert.DeserializeObject<FirebaseError>(ex.ResponseData);
            //    ModelState.AddModelError(String.Empty, firebaseEx.error.message);
            //    return View(loginModel);
            //}

            //return View();
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
