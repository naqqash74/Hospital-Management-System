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
    public class MedicalRecordController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IHospitalRepository<Medical_Record> _medicalRecordRepository;
        private APIResponse _apiResponse;

        public MedicalRecordController(IMapper mapper, IHospitalRepository<Medical_Record> medicalRecordRepository)
        {
            _mapper = mapper;
            _medicalRecordRepository = medicalRecordRepository;
            _apiResponse = new();
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [Route("Create")]
        public async Task<ActionResult<APIResponse>> CreateMedicalRecordAsync(MedicalRecordDTO dto)
        {
            try
            {
                if (dto == null)
                    return BadRequest();
                Medical_Record medicalRecord = _mapper.Map<Medical_Record>(dto);

                var result = await _medicalRecordRepository.CreateAsync(medicalRecord);
                dto.MedicalRecordID = result.MedicalRecordID;
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
        [Route("All", Name = "GetAllMedicalRecords")]
        public async Task<ActionResult<APIResponse>> GetMedicalRecordsAsync()
        {
            try
            {
                var medicalrecords = await _medicalRecordRepository.GetAllAsync();

                _apiResponse.data = _mapper.Map<List<MedicalRecordDTO>>(medicalrecords);
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
        [Route("{id:int}", Name = "GetMedicalRecordById")]
        public async Task<ActionResult<APIResponse>> GetMedicalRecordByIdAsync(int id)
        {
            try
            {

                if (id <= 0)
                    return BadRequest();
                var medicalrecord = await _medicalRecordRepository.GetAsync(medicalrecord => medicalrecord.MedicalRecordID == id);

                if (medicalrecord == null)
                    return NotFound($"The MedicalRecord not found with id: {id}");

                _apiResponse.data = _mapper.Map<Medical_Record>(medicalrecord);
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
        public async Task<ActionResult> UpdateMedicalRecordAsync(int id, [FromBody] JsonPatchDocument<MedicalRecordDTO> patchDocument)
        {
            try
            {
                if (patchDocument == null || id <= 0)
                    return BadRequest();
                var existingMedicalRecord = await _medicalRecordRepository.GetAsync(mediccalRecord => mediccalRecord.MedicalRecordID == id, true);

                if (existingMedicalRecord == null)
                    return NotFound();

                var MedicalRecordDTO = _mapper.Map<MedicalRecordDTO>(existingMedicalRecord);

                patchDocument.ApplyTo(MedicalRecordDTO, ModelState);
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                existingMedicalRecord = _mapper.Map<Medical_Record>(MedicalRecordDTO);

                await _medicalRecordRepository.UpdateAsync(existingMedicalRecord);

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
        [Route("Delete/{id}", Name = "DeleteMedicalRecordById")]
        public async Task<ActionResult<APIResponse>> DeleteMedicalRecordAsync(int id)
        {
            try
            {
                if (id <= 0)
                    return BadRequest();

                var medicalmecord = await _medicalRecordRepository.GetAsync(medicalmecord => medicalmecord.MedicalRecordID == id);

                if (medicalmecord == null)
                    return BadRequest($"the medicalmecord not found with the id: {id} to delete");

                await _medicalRecordRepository.DeleteAsync(medicalmecord);
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
