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
    public class DoctorController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IHospitalRepository<Doctor> _doctorRepository;
        private APIResponse _apiResponse;

        public DoctorController(IMapper mapper, IHospitalRepository<Doctor> doctorRepository)
        {
            _mapper = mapper;
            _doctorRepository = doctorRepository;
            _apiResponse = new();
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [Route("Create")]
        public async Task<ActionResult<APIResponse>> CreateDoctorAsync(DoctorDTO dto)
        {
            try
            {
                if (dto == null)
                    return BadRequest();
                Doctor doctor = _mapper.Map<Doctor>(dto);

                var result = await _doctorRepository.CreateAsync(doctor);
                dto.DoctorId = result.DoctorId;
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
        [Route("All", Name = "GetAllDoctors")]
        public async Task<ActionResult<APIResponse>> GetDoctorsAsync()
        {
            try
            {
                var doctors = await _doctorRepository.GetAllAsync();

                _apiResponse.data = _mapper.Map<List<DoctorDTO>>(doctors);
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
        [Route("{id:int}", Name = "GetDoctorById")]
        public async Task<ActionResult<APIResponse>> GetDoctorByIdAsync(int id)
        {
            try
            {

                if (id <= 0)
                    return BadRequest();
                var doctor = await _doctorRepository.GetAsync(doctor => doctor.DoctorId == id);

                if (doctor == null)
                    return NotFound($"The doctor not found with id: {id}");

                _apiResponse.data = _mapper.Map<DoctorDTO>(doctor);
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
        public async Task<ActionResult> UpdateDoctorAsync(int id, [FromBody] JsonPatchDocument<DoctorDTO> patchDocument)
        {
            try
            {
                if (patchDocument == null || id <= 0)
                    return BadRequest();
                var existingDoctor = await _doctorRepository.GetAsync(doctor => doctor.DoctorId == id, true);

                if (existingDoctor == null)
                    return NotFound();

                var doctorDTO = _mapper.Map<DoctorDTO>(existingDoctor);

                patchDocument.ApplyTo(doctorDTO, ModelState);
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                existingDoctor = _mapper.Map<Doctor>(doctorDTO);

                await _doctorRepository.UpdateAsync(existingDoctor);

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
        [Route("Delete/{id}", Name = "DeleteDoctorById")]
        public async Task<ActionResult<APIResponse>> DeleteDoctorAsync(int id)
        {
            try
            {
                if (id <= 0)
                    return BadRequest();

                var doctor = await _doctorRepository.GetAsync(doctor => doctor.DoctorId == id);

                if (doctor == null)
                    return BadRequest($"the doctor not found with the id: {id} to delete");

                await _doctorRepository.DeleteAsync(doctor);
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
