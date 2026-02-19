using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using TaskManagement.Application.DTOs.Tasks;
using TaskManagement.Application.Services;
using Microsoft.AspNetCore.Authorization;

namespace TaskManagement.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/tasks")]
    public class TasksController : ControllerBase
    {
        private readonly TaskService _service;
        private readonly IValidator<CreateTaskRequestDto> _createValidator;
        private readonly IValidator<UpdateTaskStatusRequestDto> _statusValidator;

        public TasksController(
            TaskService service,
            IValidator<CreateTaskRequestDto> createValidator,
            IValidator<UpdateTaskStatusRequestDto> statusValidator
        )
        {
            _service = service;
            _createValidator = createValidator;
            _statusValidator = statusValidator;
        }

        /// <summary>
        /// Permite crear una tarea y asignarla a un usuario.
        /// </summary>
        /// <remarks>
        /// - El título es obligatorio.
        /// - La tarea debe estar asignada a un usuario existente.
        /// - El estado inicial siempre será "Pending".
        /// - ExtraData puede almacenar JSON adicional como prioridad, etiquetas, etc.
        /// </remarks>
        /// <param name="request">Datos necesarios para registrar una tarea.</param>
        /// <returns>Id de la tarea creada.</returns>
        /// <response code="200">Tarea creada correctamente.</response>
        /// <response code="400">Error de validación o datos incorrectos.</response>
        /// <response code="401">No autorizado. Falta token JWT.</response>
        [HttpPost]
        [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Create([FromBody] CreateTaskRequestDto request)
        {
            var validation = await _createValidator.ValidateAsync(request);

            if (!validation.IsValid)
            {
                var errors = validation.Errors.Select(e => e.ErrorMessage).ToList();
                return BadRequest(new { mensaje = "Validación fallida.", errores = errors });
            }

            var id = await _service.CreateAsync(request);

            return Ok(new
            {
                mensaje = "Tarea creada correctamente.",
                id = id
            });
        }

        /// <summary>
        /// Retorna el listado de tareas.
        /// </summary>
        /// <remarks>
        /// Se puede filtrar opcionalmente por estado:
        /// Pending, InProgress o Done.
        /// 
        /// También se puede filtrar por prioridad dentro del JSON:
        /// Low, Medium o High.
        /// </remarks>
        /// <param name="status">Filtro opcional por estado.</param>
        /// <param name="priority">Filtro opcional por prioridad (JSON).</param>
        /// <returns>Listado de tareas.</returns>
        /// <response code="200">Listado retornado correctamente.</response>
        /// <response code="401">No autorizado. Falta token JWT.</response>
        [HttpGet]
        [ProducesResponseType(typeof(List<TaskResponseDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetAll([FromQuery] string? status, [FromQuery] string? priority)
        {
            var tasks = await _service.GetAllAsync(status, priority);
            return Ok(tasks);
        }

        /// <summary>
        /// Permite cambiar el estado de una tarea.
        /// </summary>
        /// <remarks>
        /// Reglas:
        /// - Estados permitidos: Pending, InProgress, Done
        /// - No se permite cambiar directamente de Pending a Done.
        /// </remarks>
        /// <param name="id">Id de la tarea.</param>
        /// <param name="request">Nuevo estado.</param>
        /// <returns>Confirmación del cambio.</returns>
        /// <response code="200">Estado actualizado correctamente.</response>
        /// <response code="400">Error de validación o regla de negocio incumplida.</response>
        /// <response code="401">No autorizado. Falta token JWT.</response>
        [HttpPut("{id}/status")]
        [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> UpdateStatus(int id, [FromBody] UpdateTaskStatusRequestDto request)
        {
            var validation = await _statusValidator.ValidateAsync(request);

            if (!validation.IsValid)
            {
                var errors = validation.Errors.Select(e => e.ErrorMessage).ToList();
                return BadRequest(new { mensaje = "Validación fallida.", errores = errors });
            }

            var updated = await _service.UpdateStatusAsync(id, request.Status);

            return Ok(new
            {
                mensaje = "Estado actualizado correctamente.",
                actualizado = updated
            });
        }
    }
}
