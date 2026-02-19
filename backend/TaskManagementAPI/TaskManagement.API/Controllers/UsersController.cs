using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using TaskManagement.Application.DTOs.Users;
using TaskManagement.Application.Services;
using Microsoft.AspNetCore.Authorization;

namespace TaskManagement.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/users")]
    public class UsersController : ControllerBase
    {
        private readonly UserService _service;
        private readonly IValidator<CreateUserRequestDto> _validator;

        public UsersController(UserService service, IValidator<CreateUserRequestDto> validator)
        {
            _service = service;
            _validator = validator;
        }

        /// <summary>
        /// Permite crear un usuario en el sistema.
        /// </summary>
        /// <remarks>
        /// Este endpoint registra un usuario con nombre completo y correo electrónico.
        /// El correo debe tener un formato válido.
        /// </remarks>
        /// <param name="request">Datos del usuario a registrar.</param>
        /// <returns>Id del usuario creado.</returns>
        /// <response code="200">Usuario creado correctamente.</response>
        /// <response code="400">Error de validación o datos incorrectos.</response>
        /// <response code="401">No autorizado. Falta token JWT.</response>
        [HttpPost]
        [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Create([FromBody] CreateUserRequestDto request)
        {
            var validation = await _validator.ValidateAsync(request);

            if (!validation.IsValid)
            {
                var errors = validation.Errors.Select(e => e.ErrorMessage).ToList();
                return BadRequest(new { mensaje = "Validación fallida.", errores = errors });
            }

            var id = await _service.CreateAsync(request);

            return Ok(new
            {
                mensaje = "Usuario creado correctamente.",
                id = id
            });
        }

        /// <summary>
        /// Retorna el listado de usuarios registrados.
        /// </summary>
        /// <remarks>
        /// Se utiliza para mostrar usuarios disponibles al momento de asignar tareas.
        /// </remarks>
        /// <returns>Lista de usuarios.</returns>
        /// <response code="200">Listado retornado correctamente.</response>
        /// <response code="401">No autorizado. Falta token JWT.</response>
        [HttpGet]
        [ProducesResponseType(typeof(List<UserResponseDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetAll()
        {
            var users = await _service.GetAllAsync();
            return Ok(users);
        }
    }
}
