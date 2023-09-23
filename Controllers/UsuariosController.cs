using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using test.Controllers;
using test.Models;

namespace Inmobiliaria2.Controllers
{
    public class UsuariosController : Controller
    {

        RepositorioUsuario repositorioUsuario = new();
        private readonly IConfiguration configuration;
        private readonly IWebHostEnvironment environment;
        private readonly ILogger<UsuariosController> _logger;


        public UsuariosController(ILogger<UsuariosController> logger, IConfiguration configuration, IWebHostEnvironment environment)
        {
            _logger = logger;
            this.configuration = configuration;
            this.environment = environment;
        }
        // GET: Usuarios
        [Authorize(Policy = "Admin")]
        public ActionResult Index()
        {
            var usuarios = repositorioUsuario.ObtenerUsuarios();
            return View(usuarios);
        }

        // GET: Usuarios/Details/5
        [Authorize(Policy = "Admin")]
        public ActionResult Details(int id)
        {
            var user = repositorioUsuario.ObtenerUsuario(id);
            return View(user);
        }

        // GET: Usuarios/Create
        [Authorize(Policy = "Admin")]
        public ActionResult Create()
        {
            ViewBag.roles = Usuario.ObtenerRoles();
            return View();
        }

        // POST: Usuarios/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "Admin")]
        public ActionResult Create(Usuario u)
        {
            // if (!ModelState.IsValid)
            //     return View();
            try
            {
                string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                                password: u.Password,
                                salt: System.Text.Encoding.ASCII.GetBytes(configuration["Salt"]),
                                prf: KeyDerivationPrf.HMACSHA1,
                                iterationCount: 1000,
                                numBytesRequested: 256 / 8));
                u.Password = hashed;
                //u.Rol = User.IsInRole("Administrador") ? u.Rol : (int)Roles.Empleado;
                var nbreRnd = Guid.NewGuid();
                int res = repositorioUsuario.Alta(u);
                if (u.AvatarFile != null && u.IdUsuario > 0)
                {
                    // Obtén el directorio raíz de la aplicación
                    var appRoot = AppDomain.CurrentDomain.BaseDirectory;

                    // Luego, construye las rutas relativas desde appRoot
                    string wwwPath = Path.Combine(appRoot, "wwwroot");
                    string path = Path.Combine(wwwPath, u.Avatar.TrimStart('/'));

                    // string wwwPath = environment.WebRootPath;
                    // string path = Path.Combine(wwwPath, "Uploads");
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    //Path.GetFileName(u.AvatarFile.FileName);//este nombre se puede repetir
                    string fileName = "avatar_" + u.IdUsuario + Path.GetExtension(u.AvatarFile.FileName);
                    string pathCompleto = Path.Combine(path, fileName);
                    u.Avatar = Path.Combine("/Uploads", fileName);
                    // Esta operación guarda la foto en memoria en la ruta que necesitamos
                    using (FileStream stream = new FileStream(pathCompleto, FileMode.Create))
                    {
                        u.AvatarFile.CopyTo(stream);
                    }
                    repositorioUsuario.Editar(u);
                }
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.Roles = Usuario.ObtenerRoles();
                return View(ex.Message);
            }
        }

        // GET: Usuarios/Edit/5

        public ActionResult Edit(int id)
        {
            var user = repositorioUsuario.ObtenerUsuario(id);
            if (TempData.ContainsKey("Mensaje"))
                ViewBag.Mensaje = TempData["Mensaje"];
            if (TempData.ContainsKey("Error"))
                ViewBag.Error = TempData["Error"];
            return View(user);
        }

        // POST: Usuarios/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(Usuario user)
        {
            try
            {
                var originalUser = repositorioUsuario.ObtenerUsuario(user.IdUsuario);
                user.Avatar = originalUser.Avatar;
                if (user.Rol == 0 || user.Rol == null)
                {
                    // Establece el valor predeterminado (2 para "Empleado")
                    user.Rol = 2;
                }

                repositorioUsuario.Editar(user);

                TempData["Mensaje"] = "Datos guardados correctamente";
                if (originalUser.Email != user.Email && originalUser.Email == User.Identity.Name)
                {
                    await HttpContext.SignOutAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme);
                    return RedirectToAction("Login", "Usuarios");
                }
                else if (User.Identity.Name == originalUser.Email)
                {
                    // Es el propio perfil del usuario, redirige al perfil
                    return RedirectToAction("Perfil");
                }
                else if (User.IsInRole("Admin"))
                {
                    // El usuario es un administrador, redirige a la lista de usuarios
                    return RedirectToAction("Index");
                }
                else
                {
                    // Otro caso, por ejemplo, usuario común editando otro perfil
                    // Redirige a una página de acceso denegado o realiza otra acción adecuada
                    return RedirectToAction("AccesoDenegado"); // Personaliza esta acción según tus necesidades
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                ViewBag.StackTrace = ex.StackTrace;
                return View(user);
            }
        }
        [Authorize]
        public ActionResult CambiarContraseña(int id)
        {
            CambioContraseña cambioContraseña = new CambioContraseña();
            return View(cambioContraseña);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult CambiarContraseña(CambioContraseña modelo)
        {
            if (ModelState.IsValid)
            {
                // Obtén el usuario actual
                var usuarioActual = repositorioUsuario.ObtenerPorEmail(User.Identity.Name);

                // Valida la contraseña actual ingresada por el usuario
                string hashedClaveVieja = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                    password: modelo.ClaveVieja,
                    salt: System.Text.Encoding.ASCII.GetBytes(configuration["Salt"]),
                    prf: KeyDerivationPrf.HMACSHA1,
                    iterationCount: 1000,
                    numBytesRequested: 256 / 8));

                if (usuarioActual.Password == hashedClaveVieja)
                {
                    // La contraseña actual es válida, ahora puedes cambiar la contraseña

                    // Hashea la nueva contraseña
                    string hashedNuevaClave = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                        password: modelo.ClaveNueva,
                        salt: System.Text.Encoding.ASCII.GetBytes(configuration["Salt"]),
                        prf: KeyDerivationPrf.HMACSHA1,
                        iterationCount: 1000,
                        numBytesRequested: 256 / 8));

                    // Actualiza la contraseña en la base de datos
                    usuarioActual.Password = hashedNuevaClave;
                    repositorioUsuario.CambiarContraseña(usuarioActual);

                    TempData["Mensaje"] = "Contraseña cambiada exitosamente.";
                    return RedirectToAction("Perfil"); // Puedes redirigir a donde desees después de cambiar la contraseña.
                }
                else
                {
                    // La contraseña actual ingresada por el usuario es incorrecta
                    ModelState.AddModelError("ClaveVieja", "La contraseña actual es incorrecta.");
                }
            }

            // Si hay errores de validación, muestra la vista nuevamente con los errores.
            return View(modelo);
        }

        public IActionResult CambiarAvatar()
        {
            // Aquí puedes agregar la lógica para verificar si el usuario actual tiene permiso para cambiar su avatar.
            // Si no tiene permiso, puedes redirigirlo a una página de acceso denegado.

            return View();
        }

        [HttpPost]
        public IActionResult CambiarAvatar(IFormFile avatar)
        {
            // Procesa la carga de la nueva imagen de avatar y actualiza la base de datos.
            // Asegúrate de manejar la lógica de validación y manejo de errores aquí.

            var usuarioActual = repositorioUsuario.ObtenerPorEmail(User.Identity.Name);

            if (usuarioActual == null)
            {
                // El usuario autenticado no existe en la base de datos, puedes manejar esto como desees.
                return RedirectToAction("AccesoDenegado"); // Por ejemplo, redirigirlo a una página de acceso denegado.
            }

            if (avatar != null && avatar.Length > 0)
            {
                try
                {
                    // Verifica si el usuario ya tenía un avatar y elimina el archivo anterior si existe
                    if (!string.IsNullOrEmpty(usuarioActual.Avatar))
                    {
                        string wwwPath = environment.WebRootPath;
                        string pathCompleto = Path.Combine(wwwPath, usuarioActual.Avatar.TrimStart('/'));

                        if (System.IO.File.Exists(pathCompleto))
                        {
                            System.IO.File.Delete(pathCompleto);
                        }
                    }

                    // Guarda la nueva imagen de avatar en el servidor y obtén la ruta
                    string wwwRoot = environment.WebRootPath;
                    string fileName = "avatar_" + usuarioActual.IdUsuario + Path.GetExtension(avatar.FileName);
                    string pathToSave = Path.Combine(wwwRoot, "Uploads", fileName);

                    using (var stream = new FileStream(pathToSave, FileMode.Create))
                    {
                        avatar.CopyTo(stream);
                    }

                    // Actualiza el campo "Avatar" en la base de datos
                    repositorioUsuario.CambiarAvatar(usuarioActual.IdUsuario, "/Uploads/" + fileName);

                    TempData["Mensaje"] = "Avatar cambiado exitosamente.";
                    return RedirectToAction("Perfil"); // Puedes redirigir a donde desees después de cambiar el avatar.
                }
                catch (Exception ex)
                {
                    // Maneja cualquier error que pueda ocurrir durante el proceso
                    ViewBag.Error = ex.Message;
                    ViewBag.StackTrace = ex.StackTrace;
                    return View("CambiarAvatar"); // Redirige de nuevo a la vista de cambio de avatar
                }
            }

            // Si no se proporciona un archivo de avatar, muestra un mensaje de error
            ModelState.AddModelError("avatar", "Por favor, selecciona una imagen de avatar.");
            return View("CambiarAvatar"); // Redirige de nuevo a la vista de cambio de avatar
        }



        // GET: Usuarios/Delete/5
        [Authorize(Policy = "Admin")]
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Usuarios/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "Admin")]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
        [AllowAnonymous]
        // GET: Usuarios/Login/
        public ActionResult Login(string returnUrl)
        {
            TempData["returnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginView login)
        {
            try
            {
                var returnUrl = String.IsNullOrEmpty(TempData["returnUrl"] as string) ? "/Home" : TempData["returnUrl"].ToString();
                if (ModelState.IsValid)
                {
                    string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                        password: login.Clave,
                        salt: System.Text.Encoding.ASCII.GetBytes(configuration["Salt"]),
                        prf: KeyDerivationPrf.HMACSHA1,
                        iterationCount: 1000,
                        numBytesRequested: 256 / 8));

                    var e = repositorioUsuario.ObtenerPorEmail(login.Usuario);
                    if (e == null || e.Password != hashed)
                    {
                        ModelState.AddModelError("", "El email o la clave no son correctos");
                        TempData["returnUrl"] = returnUrl;
                        return View();
                    }

                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, e.Email),
                        new Claim("FullName", e.Nombre + " " + e.Apellido),
                        new Claim(ClaimTypes.Role, e.RolNombre),
                    };

                    var claimsIdentity = new ClaimsIdentity(
                            claims, CookieAuthenticationDefaults.AuthenticationScheme);

                    await HttpContext.SignInAsync(
                            CookieAuthenticationDefaults.AuthenticationScheme,
                            new ClaimsPrincipal(claimsIdentity));
                    TempData.Remove("returnUrl");
                    //return Redirect(returnUrl);
                    return RedirectToAction("Index", "Home");
                }
                TempData["returnUrl"] = returnUrl;
                return View();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View();
            }
        }

        public async Task<ActionResult> Logout()
        {
            await HttpContext.SignOutAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Usuarios");
        }

        [Authorize]
        public ActionResult Perfil()
        {
            ViewData["Title"] = "Mi perfil";
            // var u = repositorioUsuario.ObtenerUsuario(id);
            var u = repositorioUsuario.ObtenerPorEmail(User.Identity.Name);
            ViewBag.Roles = Usuario.ObtenerRoles();
            return View("Edit", u);
        }
    }
}