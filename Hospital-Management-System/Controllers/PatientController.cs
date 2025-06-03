using AutoMapper;
using Hospital_Management_System.Data;
using Hospital_Management_System.Data.Repository;
using Hospital_Management_System.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using System.Net;

namespace Hospital_Management_System.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatientController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IHospitalRepository<Patient> _patientRepository;
        private APIResponse _apiResponse;
        public PatientController(IMapper mapper, IHospitalRepository<Patient> patientRepository)
        {
            _mapper = mapper; 
            _patientRepository = patientRepository;
            _apiResponse = new();
        }
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [Route("Create")]
        public async Task<ActionResult<APIResponse>> CreatePatientAsync(PatientDTO dto)
        {
            try
            {
                if (dto == null)
                    return BadRequest();
                Patient patient = _mapper.Map<Patient>(dto);

                var result = await _patientRepository.CreateAsync(patient);
                dto.PatientId = result.PatientId;
                _apiResponse.data = dto;
                _apiResponse.Status = true;
                _apiResponse.StatusCode = HttpStatusCode.OK;

                return Ok();
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
        [Route("All", Name ="GetAllPatients")]
        public async Task<ActionResult<APIResponse>> GetPatientsAsync()
        {
            try
            {
                var patients = await _patientRepository.GetAllAsync();

                _apiResponse.data = _mapper.Map<List<PatientDTO>>(patients);
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
        [Route("{id:int}", Name = "GetPatientById")]
        public async Task<ActionResult<APIResponse>> GetPatientByIdAsync(int id)
        {
            try
            {

                if (id <= 0)
                    return BadRequest();
                var patient = await _patientRepository.GetAsync(patient => patient.PatientId == id);

                if (patient == null)
                    return NotFound($"The patient not found with id: {id}");

                _apiResponse.data = _mapper.Map<PatientDTO>(patient);
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
        public async Task<ActionResult> UpdatePatientAsync(int id, [FromBody] JsonPatchDocument<PatientDTO> patchDocument)
        {
            try
            {
                if (patchDocument == null || id <= 0)
                    return BadRequest();
                var existingPatient = await _patientRepository.GetAsync(Patient => Patient.PatientId == id, true);

                if (existingPatient == null)
                    return NotFound();

                var patientDTO = _mapper.Map<PatientDTO>(existingPatient);

                patchDocument.ApplyTo(patientDTO, ModelState);
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                existingPatient = _mapper.Map<Patient>(patientDTO);

                await _patientRepository.UpdateAsync(existingPatient);

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
        [Route("Delete/{id}", Name = "DeletePatientById")]
        public async Task<ActionResult<APIResponse>> DeletePatientAsync(int id)
        {
            try
            {
                if (id <= 0) 
                    return BadRequest();

                var patient =await _patientRepository.GetAsync(patient => patient.PatientId == id);

                if (patient == null)
                    return BadRequest($"the patient not found with the id: {id} to delete");

              await _patientRepository.DeleteAsync(patient);
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
