using concesionaria_menichetti.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace concesionaria_menichetti.Controllers
{
    [Authorize]
    public class FavoritosController : Controller
    {
        private readonly FavoritoRepository _favoritoRepository;
        private readonly DestacadoRepository _destacadosRepository;
        private readonly VehiculoRepository _vehiculoRepository;
        private readonly ContratoSuscripcionRepository _contratoSuscripcionRepository;
        private readonly ContratoPlanesRepository _contratoPlanesRepository;
        private readonly UsuarioRepository _usuarioRepository;
        private readonly ConcesionariaRepository _concesionariaRepository;

        public FavoritosController(FavoritoRepository favoritoRepository, DestacadoRepository destacadosRepository, VehiculoRepository vehiculoRepository, ContratoPlanesRepository contratoPlanesRepository, ContratoSuscripcionRepository contratoSuscripcionRepository, UsuarioRepository usuarioRepository, ConcesionariaRepository concesionariaRepository)

        {
            _vehiculoRepository = vehiculoRepository;
            _favoritoRepository = favoritoRepository;
            _destacadosRepository = destacadosRepository;
            _contratoSuscripcionRepository = contratoSuscripcionRepository;
            _contratoPlanesRepository = contratoPlanesRepository;
            _usuarioRepository = usuarioRepository;
            _concesionariaRepository = concesionariaRepository;
        }



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
}