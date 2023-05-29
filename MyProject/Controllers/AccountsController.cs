using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MyProject.Models;
using MyProject.Repository;
using MyProject.View_Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MyProject.Controllers
{
    [Route("api/[controller]"), Authorize]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly AccountRepository accountRepository;
        public AccountsController(AccountRepository accountRepository, IConfiguration configuration)
        {
            this.accountRepository = accountRepository;
            _configuration = configuration;
        }
        [HttpPost]
        public ActionResult Insert(Account account)
        {
            var post = accountRepository.Insert(account);
            ProblemDetails problemDetails = new ProblemDetails();
            problemDetails.Status = StatusCodes.Status400BadRequest;
            problemDetails.Extensions.Add(new KeyValuePair<string, object>("Data", account));
            if (post > 0)
                return StatusCode(200, new {status = StatusCodes.Status200OK, message = "Berhasil memasukkan akun", data = account});
            else
            switch (post)
                {
                    case -1:
                        problemDetails.Detail = $"Gagal menambahkan akun, kolom NIK tidak boleh kosong!";
                        return this.StatusCode((int)StatusCodes.Status400BadRequest, problemDetails);
                    case -2:
                        problemDetails.Detail = $"Gagal menambahkan akun, NIK {account.NIK} sudah dipakai oleh akun lain!";
                        return this.StatusCode((int)StatusCodes.Status400BadRequest, problemDetails);
                    case -3:
                        problemDetails.Detail = $"Gagal menambahkan akun, NIK dengan nilai {account.NIK} tidak ditemukan!";
                        return this.StatusCode((int)StatusCodes.Status400BadRequest, problemDetails);
                    default:
                        return BadRequest();
                }
        }
        [HttpGet]
        public ActionResult Get()
        {
            var get = accountRepository.Get();
            if (get == null)
                return StatusCode(404, new { status = StatusCodes.Status404NotFound, message = "Gagal mengambil akun", data = get });
            else
                return StatusCode(200, new { status = StatusCodes.Status200OK, message = "Berhasil mengambil akun", data = get });
        }
        [HttpGet("{NIK}")]
        public ActionResult Get(string NIK)
        {
            var get = accountRepository.Get(NIK);
            if (get == null)
                return StatusCode(404, new { status = StatusCodes.Status404NotFound, message = "Gagal mengambil akun", data = get });
            else
                return StatusCode(200, new { status = StatusCodes.Status200OK, message = "Berhasil mengambil akun", data = get });
        }
        [HttpPut]
        public ActionResult Edit(Account account)
        {
            var put = accountRepository.Update(account);
            if (put > 0)
                return StatusCode(200, new { status = StatusCodes.Status200OK, message = "Berhasil memperbaharui akun", data = account });
            else
                return StatusCode(400, new { status = StatusCodes.Status400BadRequest, message = "Gagal memperbaharui akun", data = account });
        }
        [HttpDelete]
        public ActionResult Delete(string NIK)
        {
            var delete = accountRepository.Delete(NIK);
            if (delete > 0)
                return StatusCode(200, new { status = StatusCodes.Status200OK, message = "Berhasil menghapus akun", NIK = NIK});
            else
                return StatusCode(400, new { status = StatusCodes.Status400BadRequest, message = "Gagal menghapus akun", NIK = NIK });
        }
        [HttpPost("login"), AllowAnonymous]
        public async Task<ActionResult> Login(LoginVM loginVM)
        {
            //if(HttpContext.Session.GetString("email") == null)
            //{
            var issuer = _configuration.GetValue<string>("Jwt:Issuer");
            var audience = _configuration.GetValue<string>("Jwt:Audience");
            var key = Encoding.UTF8.GetBytes(_configuration.GetValue<string>("Jwt:Key"));
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim("Id", Guid.NewGuid().ToString()),
                    //new Claim(JwtRegisteredClaimNames.Sub, loginVM.Email),
                    new Claim(JwtRegisteredClaimNames.Email, loginVM.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                 }),
                Expires = DateTime.UtcNow.AddHours(1),
                Issuer = issuer,
                Audience = audience,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha512Signature)
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var jwtToken = tokenHandler.WriteToken(token);
            var stringToken = tokenHandler.WriteToken(token);

            var state = await accountRepository.Login(loginVM);
                if (state > 0)
                {
                    //HttpContext.Session.SetString("email", loginVM.Email);
                    return StatusCode(200, new { status = StatusCodes.Status200OK, message = "Berhasil melakukan login - Email dan Password cocok!", Data = accountRepository.GetEmployeeByEmail(loginVM.Email), JWT = stringToken });
                }
                else
                {
                    switch (state)
                    {
                        case -1:
                            return StatusCode(400, new { status = StatusCodes.Status404NotFound, message = $"Gagal melakukan login - Pegawai dengan email {loginVM.Email} tidak ditemukan!" });
                        case -2:
                            return StatusCode(400, new { status = StatusCodes.Status404NotFound, message = $"Gagal melakukan login - Akun dengan email {loginVM.Email} belum dibuat!" });
                        case -3:
                            return StatusCode(400, new { status = StatusCodes.Status404NotFound, message = $"Gagal melakukan login - Password salah!" });
                        default:
                            return NotFound();
                    }
                }
            //}
            //else
            //{
            //    return Redirect("/Departments");
            //}
        }
    }
}
