using concesionaria_menichetti.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace concesionaria_menichetti.Api
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class VehiculoController : ControllerBase
    {
        private readonly VehiculoRepository vehiculoRepo;

        public VehiculoController(VehiculoRepository vehiculoRepo)
        {
            this.vehiculoRepo = vehiculoRepo;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var vehiculos = await vehiculoRepo.ObtenerVehiculosActivosAsync();
                return Ok(vehiculos);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
