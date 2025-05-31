using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using concesionaria_menichetti.Models;
using Microsoft.AspNetCore.Authorization;
using concesionaria_menichetti.Repositories;

namespace concesionaria_menichetti.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly UsuarioRepository _usuarioRepo;
    private readonly VehiculoRepository _vehiculoRepository;
    private readonly ConcesionariaRepository _concesionariaRepository;
    private readonly UsuarioRepository _usuarioRepository;
    private readonly HomeRepository _homeRepository;
    private readonly FavoritoRepository _favoritoRepository;


    public HomeController(
    ILogger<HomeController> logger,
    UsuarioRepository usuarioRepo,
    HomeRepository homeRepository,
    VehiculoRepository vehiculoRepository,
    ConcesionariaRepository concesionariaRepository,
    FavoritoRepository favoritoRepository,
    UsuarioRepository usuarioRepository)
    {
        _logger = logger;
        _usuarioRepo = usuarioRepo;
        _homeRepository = homeRepository;
        _vehiculoRepository = vehiculoRepository;
        _concesionariaRepository = concesionariaRepository;
        _usuarioRepository = usuarioRepository;
        _favoritoRepository = favoritoRepository;
    }

    public async Task<IActionResult> Index(
    int? idMarca,
    int? idModelo,
    int? anoDesde,
    int? anoHasta,
    decimal? precioDesde,
    decimal? precioHasta,
    // string? propietario,
    int? estado = 1,
    int page = 1)

    {
        try
        {
            int pageSize = 10;

            var result = await _homeRepository.ObtenerVehiculosFiltradosAsync(
            idMarca, idModelo, anoDesde, anoHasta, precioDesde, precioHasta, estado, page, pageSize);
            //  precioDesde, precioHasta, propietario,

            // Obtenemos las marcas y modelos únicos
            var marcas = await _vehiculoRepository.GetMarcasAsync(); // Método para obtener las marcas
            var modelos = await _vehiculoRepository.GetModelosAsync(); // Método para obtener los modelos


            //pasamos la data a la vista
            ViewBag.Marcas = marcas.Select(m => new
            {
                id = m.Id,
                descripcion = m.Descripcion
            }).ToList();

            ViewBag.Modelos = modelos.Select(m => new
            {
                id = m.Id,
                descripcion = m.Descripcion,
                idMarca = m.IdMarca
            }).ToList();

            //agregamos los favoritos del usuario logueado
            var idClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
            if (idClaim != null && int.TryParse(idClaim.Value, out int usuarioId))
            {
                var favoritos = await _favoritoRepository.ObtenerPorUsuarioIdAsync(usuarioId);
                ViewBag.FavoritosIds = favoritos
                    .Where(f => f.VehiculoId.HasValue)
                    .Select(f => f.VehiculoId.Value)
                    .ToList();

            }



            ViewBag.PaginaActual = page;
            ViewBag.TotalPaginas = result.TotalPaginas;



            return View(result.Vehiculos);
        }
        catch (Exception ex)
        {
            ViewBag.ErrorMessage = $"Error al cargar los vehiculos: {ex.Message}";
            return View();
        }
    }

    public async Task<IActionResult> Details(int id)
    {
        // if (id == null) return NotFound();

        try
        {
            int idUser = 0;

            if (User.Identity != null && User.Identity.IsAuthenticated)
            {
                var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
                if (userIdClaim != null && int.TryParse(userIdClaim.Value, out int parsedId))
                {
                    idUser = parsedId;
                }
            }

            var (vehiculo, tieneAcceso) = await _homeRepository.GetVehiculoByIdAsyncConFoto(id, idUser);
            if (vehiculo == null) return NotFound();

            ViewBag.TieneAcceso = tieneAcceso;

            return View(vehiculo);
        }
        catch (Exception ex)
        {
            ViewBag.ErrorMessage = $"Error al obtener los detalles del vehiculo: {ex.Message}";
            return View();
        }
    }


    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
    ///

    [HttpPost]
    public async Task<IActionResult> MarcarFavorito(int vehiculoId)
    {
        int usuarioId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)!.Value);
        await _favoritoRepository.AgregarFavoritoAsync(usuarioId, vehiculoId);
        return Ok(new { success = true });
    }

    [HttpPost]
    public async Task<IActionResult> QuitarFavorito(int vehiculoId)
    {
        int usuarioId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)!.Value);
        await _favoritoRepository.QuitarFavoritoAsync(usuarioId, vehiculoId);
        return Ok(new { success = true });
    }





}
