using AutoMapper;
using Hospital_Management_System.Data.Repository;
using Hospital_Management_System.Data;
using Hospital_Management_System.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Microsoft.AspNetCore.JsonPatch;

namespace Hospital_Management_System.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IHospitalRepository<Appointment> _appointmentRepository;
        private APIResponse _apiResponse;

        public AppointmentController(IMapper mapper, IHospitalRepository<Appointment> appointmentRepository)
        {
            _mapper = mapper;
            _appointmentRepository = appointmentRepository;
            _apiResponse = new();
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [Route("Create")]
        public async Task<ActionResult<APIResponse>> CreateAppointmentAsync(AppointmentDTO dto)
        {
            try
            {
                if (dto == null)
                    return BadRequest();
                Appointment appointment = _mapper.Map<Appointment>(dto);

                var result = await _appointmentRepository.CreateAsync(appointment);
                dto.AppointmentID = result.AppointmentID;
                _apiResponse.data = dto;
                _apiResponse.Status = true;
                _apiResponse.StatusCode = HttpStatusCode.OK;

                return Ok(_apiResponse);
            }
            catch (Exception ex)
            {
                _apiResponse.Status = false;
                _apiResponse.StatusCode = HttpStatusCode.InternalServerError;
                _apiResponse.Errors.Add(ex.Message);
                return _apiResponse;
            }

        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [Route("All", Name = "GetAllAppointments")]
        public async Task<ActionResult<APIResponse>> GetAppointmentsAsync()
        {
            try
            {
                var appointments = await _appointmentRepository.GetAllAsync();

                _apiResponse.data = _mapper.Map<List<AppointmentDTO>>(appointments);
                _apiResponse.Status = true;
                _apiResponse.StatusCode = HttpStatusCode.OK;

                return Ok(_apiResponse);

            }
            catch (Exception ex)
            {
                _apiResponse.Status = false;
                _apiResponse.StatusCode = HttpStatusCode.InternalServerError;
                _apiResponse.Errors.Add(ex.Message);
                return _apiResponse;
            }
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Route("{id:int}", Name = "GetAppointmentById")]
        public async Task<ActionResult<APIResponse>> GetAppointmentByIdAsync(int id)
        {
            try
            {

                if (id <= 0)
                    return BadRequest();
                var appointment = await _appointmentRepository.GetAsync(appointment => appointment.AppointmentID == id);

                if (appointment == null)
                    return NotFound($"The appointment not found with id: {id}");

                _apiResponse.data = _mapper.Map<AppointmentDTO>(appointment);
                _apiResponse.Status = true;
                _apiResponse.StatusCode = HttpStatusCode.OK;

                return Ok(_apiResponse);

            }
            catch (Exception ex)
            {
                _apiResponse.Status = false;
                _apiResponse.StatusCode = HttpStatusCode.InternalServerError;
                _apiResponse.Errors.Add(ex.Message);
                return _apiResponse;
            }
        }

        [HttpPatch]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Route("Update")]
        public async Task<ActionResult> UpdateAppointmentAsync(int id, [FromBody] JsonPatchDocument<AppointmentDTO> patchDocument)
        {
            try
            {
                if (patchDocument == null || id <= 0)
                    return BadRequest();
                var existingAppointment = await _appointmentRepository.GetAsync(Appointment => Appointment.AppointmentID == id, true);

                if (existingAppointment == null)
                    return NotFound();

                var appointmentDTO = _mapper.Map<AppointmentDTO>(existingAppointment);

                patchDocument.ApplyTo(appointmentDTO, ModelState);
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                existingAppointment = _mapper.Map<Appointment>(appointmentDTO);

                await _appointmentRepository.UpdateAsync(existingAppointment);

                return NoContent();
            }
            catch (Exception ex)
            {
                _apiResponse.Status = false;
                _apiResponse.StatusCode = HttpStatusCode.InternalServerError;
                _apiResponse.Errors.Add(ex.Message);
                return Ok(_apiResponse);
            }

        }

        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [Route("Delete/{id}", Name = "DeleteAppointmentById")]
        public async Task<ActionResult<APIResponse>> DeleteAppointmentAsync(int id)
        {
            try
            {
                if (id <= 0)
                    return BadRequest();

                var appointment = await _appointmentRepository.GetAsync(appointment => appointment.AppointmentID == id);

                if (appointment == null)
                    return BadRequest($"the appointment not found with the id: {id} to delete");

                await _appointmentRepository.DeleteAsync(appointment);
                _apiResponse.Status = true;
                _apiResponse.StatusCode = HttpStatusCode.OK;
                _apiResponse.data = true;
                return Ok(_apiResponse);


            }
            catch (Exception ex)
            {
                _apiResponse.Status = false;
                _apiResponse.StatusCode = HttpStatusCode.InternalServerError;
                _apiResponse.Errors.Add(ex.Message);
                return _apiResponse;
            }
        }
    }
}
