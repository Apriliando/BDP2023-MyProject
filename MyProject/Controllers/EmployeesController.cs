using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyProject.Models;
using MyProject.Repository;
using MyProject.View_Models;
using System.Collections;
using System.Net;

namespace MyProject.Controllers
{
    [Route("api/[controller]"), Authorize]
    [ApiController]

    public class EmployeesController : ControllerBase
    {
        private readonly EmployeeRepository employeeRepository;
        public EmployeesController(EmployeeRepository employeeRepository)
        {
            this.employeeRepository = employeeRepository;
        }
        [HttpPost]
        public ActionResult Insert(Employee employee)
        {
            var postResponse = employeeRepository.Insert(employee);
            ProblemDetails problemDetails = new ProblemDetails();
            problemDetails.Status = StatusCodes.Status400BadRequest;
            problemDetails.Extensions.Add(new KeyValuePair<string, object>("Data", employee.NIK));
            if (postResponse > 0)
            {
                return this.StatusCode(200, new {status= HttpStatusCode.OK, message = "Berhasil memasukkan data!", Data = employee});
            }
            else
            {
                switch (postResponse)
                {
                    case -1:
                        problemDetails.Detail = $"Sudah ada yang menggunakan NIK dengan nilai {employee.NIK}!";
                        return this.StatusCode((int)StatusCodes.Status400BadRequest, problemDetails);
                    case -2:
                        problemDetails.Detail = $"Sudah ada yang menggunakan Nomor Telepon dengan nilai {employee.Phone}!";
                        return this.StatusCode((int)StatusCodes.Status400BadRequest, problemDetails);
                    case -3:
                        problemDetails.Detail = $"Kolom Nomor Telepon ({employee.Phone}) mengandung karakter non-digit!";
                        return this.StatusCode((int)StatusCodes.Status400BadRequest, problemDetails);
                    case -4:
                        problemDetails.Detail = $"Sudah ada yang menggunakan Email dengan nilai {employee.Email}!";
                        return this.StatusCode((int)StatusCodes.Status400BadRequest, problemDetails);
                    case -5:
                        problemDetails.Detail = $"Format e-mail {employee.Email} salah / tidak valid!";
                        return this.StatusCode((int)StatusCodes.Status400BadRequest, problemDetails);
                    default:
                        return BadRequest();
                }
            }
        }
        [HttpGet]
        public ActionResult Get()
        {
            var get = employeeRepository.Get();
            if (get == null)
            {
                return this.StatusCode(404, new {status = StatusCodes.Status404NotFound, message = "Gagal mengambil data pegawai - kosong!"});
            }
            return this.StatusCode(200, new {status = StatusCodes.Status200OK, message = "Berhasil mengambil data pegawai!", data = get});
        }
        [HttpGet("getAll")]
        public ActionResult GetAll()
        {
            var get = employeeRepository.GetAll();
            if (get == null)
            {
                return this.StatusCode(404, new { status = StatusCodes.Status404NotFound, message = "Gagal mengambil data pegawai - kosong!" });
            }
            ArrayList newGet = new ArrayList();
            foreach(Employee g in get)
            {
                newGet.Add(new { NIK = g.NIK, FullName = g.FirstName + ' ' + g.LastName, DepartmentName = g.Department.Name });
            }
            return this.StatusCode(200, new { statusCode = StatusCodes.Status200OK, message = "Berhasil mengambil data (dengan format)", data = newGet });
        }
        [HttpGet("{NIK}")]
        public ActionResult Get(string NIK)
        {
            var get = employeeRepository.Get(NIK);

            if(get == null)
            {
                //return NotFound();
                ProblemDetails problemDetails = new ProblemDetails();
                problemDetails.Status = StatusCodes.Status404NotFound;
                problemDetails.Detail = $"Pegawai dengan NIK {NIK} tidak ditemukan!";
                problemDetails.Extensions.Add(new KeyValuePair<string, object>("Data", NIK));

                return this.StatusCode((int)StatusCodes.Status404NotFound, problemDetails);
            }
            return this.StatusCode(200, new { status = StatusCodes.Status200OK, message = "Berhasil mengambil data karyawan berdasarkan NIK!", data = get });
        }
        [HttpGet("TestCORS")]
        public ActionResult TestCORS()
        {
            return Ok("Test CORS berhasil");
        }
        [HttpPut]
        public ActionResult Edit(Employee employee)
        {
            if(employeeRepository.Update(employee) > 0)
            {
                return this.StatusCode(200, new { status = StatusCodes.Status200OK, message = "Berhasil memperbaharui data karyawan!", data = employee });
            }
            else
            {
                ProblemDetails problemDetails = new ProblemDetails();
                problemDetails.Status = StatusCodes.Status400BadRequest;
                problemDetails.Detail = $"Gagal memperbaharui data pegawai - NIK dengan nilai {employee.NIK} tidak ditemukan!";
                problemDetails.Extensions.Add(new KeyValuePair<string, object>("Data", employee.NIK));
                return this.StatusCode((int)StatusCodes.Status400BadRequest, problemDetails);
            }
        }
        [HttpDelete]
        public ActionResult Delete(string NIK)
        {
            if (employeeRepository.Delete(NIK) > 0)
            {
                return this.StatusCode(200, new { status = StatusCodes.Status200OK, message = "Berhasil menghapus data karyawan berdasarkan NIK!", NIK = NIK });
            }
            else
            {
                ProblemDetails problemDetails = new ProblemDetails();
                problemDetails.Status = StatusCodes.Status404NotFound;
                problemDetails.Detail = $"Gagal menghapus data pegawai -  NIK dengan nilai {NIK} tidak ditemukan!";
                problemDetails.Extensions.Add(new KeyValuePair<string, object>("Data", NIK));
                return this.StatusCode((int)StatusCodes.Status404NotFound, problemDetails);
            }
        }
        [HttpPost("register")]
        public async Task <ActionResult> Register(RegisterVM registerVM)
        {
            int register = await employeeRepository.Register(registerVM);
            ProblemDetails problemDetails = new ProblemDetails();
            problemDetails.Status = StatusCodes.Status400BadRequest;
            problemDetails.Extensions.Add(new KeyValuePair<string, object>("Data", registerVM));
            if (register > 0)
                return this.StatusCode(200, new { status = StatusCodes.Status200OK, message = "Berhasil mendaftarkan pegawai", data =  registerVM });
            else
            {
                switch (register)
                {
                    case -1:
                        problemDetails.Detail = $"Panjang kata sandi harus 8 - 64 karakter!";
                        return this.StatusCode((int)StatusCodes.Status400BadRequest, problemDetails);
                    case -2:
                        problemDetails.Detail = $"Sudah ada yang menggunakan Nomor Telepon dengan nilai {registerVM.Phone}!";
                        return this.StatusCode((int)StatusCodes.Status400BadRequest, problemDetails);
                    case -3:
                        problemDetails.Detail = $"Kolom Nomor Telepon ({registerVM.Phone}) mengandung karakter non-digit!";
                        return this.StatusCode((int)StatusCodes.Status400BadRequest, problemDetails);
                    case -4:
                        problemDetails.Detail = $"Sudah ada yang menggunakan Email dengan nilai {registerVM.Email}!";
                        return this.StatusCode((int)StatusCodes.Status400BadRequest, problemDetails);
                    case -5:
                        problemDetails.Detail = $"Format e-mail {registerVM.Email} salah / tidak valid!";
                        return this.StatusCode((int)StatusCodes.Status400BadRequest, problemDetails);
                    default:
                        return BadRequest();
                }
            }
        }
    }
}
