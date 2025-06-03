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
    public class PrescriptionController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IHospitalRepository<Prescriptions> _prescriptionRepository;
        private APIResponse _apiResponse;

        public PrescriptionController(IMapper mapper, IHospitalRepository<Prescriptions> prescriptionRepository)
        {
            _mapper = mapper;
            _prescriptionRepository = prescriptionRepository;
            _apiResponse = new();
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [Route("Create")]
        public async Task<ActionResult<APIResponse>> CreatePrescriptionAsync(PrescriptionDTO dto)
        {
            try
            {
                if (dto == null)
                    return BadRequest();
                Prescriptions prescriptiont = _mapper.Map<Prescriptions>(dto);

                var result = await _prescriptionRepository.CreateAsync(prescriptiont);
                dto.PrescriptionID = result.PrescriptionID;
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
        [Route("All", Name = "GetAllPrescriptions")]
        public async Task<ActionResult<APIResponse>> GetPrescriptionsAsync()
        {
            try
            {
                var Prescriptions = await _prescriptionRepository.GetAllAsync();

                _apiResponse.data = _mapper.Map<List<PrescriptionDTO>>(Prescriptions);
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
        [Route("{id:int}", Name = "GetPrescriptionById")]
        public async Task<ActionResult<APIResponse>> GetPrescriptionByIdAsync(int id)
        {
            try
            {

                if (id <= 0)
                    return BadRequest();
                var prescription = await _prescriptionRepository.GetAsync(prescription => prescription.PrescriptionID == id);

                if (prescription == null)
                    return NotFound($"The prescription not found with id: {id}");

                _apiResponse.data = _mapper.Map<PrescriptionDTO>(prescription);
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
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [HttpPatch]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Route("Update")]
        public async Task<ActionResult> UpdatePrescriptionAsync(int id, [FromBody] JsonPatchDocument<PrescriptionDTO> patchDocument)
        {
            try
            {
                if (patchDocument == null || id <= 0)
                    return BadRequest();
                var existingPerscription = await _prescriptionRepository.GetAsync(Perscription => Perscription.PrescriptionID == id, true);

                if (existingPerscription == null)
                    return NotFound();

                var perscriptionDTO = _mapper.Map<PrescriptionDTO>(existingPerscription);

                patchDocument.ApplyTo(perscriptionDTO, ModelState);
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                existingPerscription = _mapper.Map<Prescriptions>(perscriptionDTO);

                await _prescriptionRepository.UpdateAsync(existingPerscription);

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
        [Route("Delete/{id}", Name = "DeletePrescriptionById")]
        public async Task<ActionResult<APIResponse>> DeletePrescriptionAsync(int id)
        {
            try
            {
                if (id <= 0)
                    return BadRequest();

                var prescription = await _prescriptionRepository.GetAsync(prescription => prescription.PrescriptionID == id);

                if (prescription == null)
                    return BadRequest($"the prescription not found with the id: {id} to delete");

                await _prescriptionRepository.DeleteAsync(prescription);
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
