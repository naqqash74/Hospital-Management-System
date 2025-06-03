using AutoMapper;
using Hospital_Management_System.Data;
using Hospital_Management_System.Data.Repository;
using Hospital_Management_System.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Hospital_Management_System.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IHospitalRepository<Person> _personRepository;
        private APIResponse _apiResponse;

        public PersonController(IMapper mapper, IHospitalRepository<Person> personRepository)
        {
            _mapper = mapper;
            _personRepository = personRepository;
            _apiResponse = new();
        }
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Route("All", Name = "GetAllPersons")]
        public async Task<ActionResult<IEnumerable<PersonDTO>>> GetPersons()
        {
            try
            {
                var persons = await _personRepository.GetAllAsync();
                var personDTOData = _mapper.Map<List<PersonDTO>>(persons);
                return Ok(personDTOData);
            }
            catch (Exception ex)
            {
                _apiResponse.Status = false;
                _apiResponse.StatusCode = HttpStatusCode.InternalServerError;
                _apiResponse.Errors.Add(ex.Message);
                return Ok(_apiResponse);
            }

        }
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Route("{id:int}", Name = "GetPersonById")]
        public async Task<ActionResult<PersonDTO>> GetPersonByIdAsync(int id)
        {
            try
            {
                if (id <= 0)
                    return BadRequest();
                var person = await _personRepository.GetAsync(person => person.PersonId == id);
                if (person == null)
                    return NotFound($"The student with id: {id} not found");

                var personDTO = _mapper.Map<PersonDTO>(person);

                return Ok(personDTO);
            }
            catch (Exception ex)
            {
                _apiResponse.Status = false;
                _apiResponse.StatusCode = HttpStatusCode.InternalServerError;
                _apiResponse.Errors.Add(ex.Message);
                return Ok(_apiResponse);
            }

        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Route("Create")]
        public async Task<ActionResult<PersonDTO>> CreatePersonAsync([FromBody]PersonDTO model)
        {
            try
            {
                if (model == null)
                    return BadRequest();

                Person person = _mapper.Map<Person>(model);
                var personAfterCreation = await _personRepository.CreateAsync(person);
                model.PersonId = personAfterCreation.PersonId;

                return CreatedAtRoute("GetPersonById", new { id = model.PersonId }, model);
            }
            catch (Exception ex)
            {
                _apiResponse.Status = false;
                _apiResponse.StatusCode = HttpStatusCode.InternalServerError;
                _apiResponse.Errors.Add(ex.Message);
                return Ok(_apiResponse);
            }
        }

        [HttpPatch]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Route("Update")]
        public async Task<ActionResult> UpdatePersonAsync(int id ,[FromBody] JsonPatchDocument<PersonDTO> patchDocument)
        {
            try
            {
                if (patchDocument == null || id <= 0)
                    return BadRequest();
                var existingPerson = await _personRepository.GetAsync(Person => Person.PersonId == id, true);

                if (existingPerson == null)
                    return NotFound();

                var personDTO = _mapper.Map<PersonDTO>(existingPerson);

                patchDocument.ApplyTo(personDTO, ModelState);
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                existingPerson = _mapper.Map<Person>(personDTO);

                await _personRepository.UpdateAsync(existingPerson);

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
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Route("Delete/{id}", Name = "DeletePersonById")]
        public async Task<ActionResult<bool>> DeletePersonByIdAsync(int id)
        {
            try
            {
                if (id <= 0)
                    return BadRequest();
                var person = await _personRepository.GetAsync(person => person.PersonId == id);
                if (person == null)
                    return NotFound($"The student with id: {id} not found");

                await _personRepository.DeleteAsync(person);
                return Ok(true);
            }
            catch (Exception ex)
            {
                _apiResponse.Status = false;
                _apiResponse.StatusCode = HttpStatusCode.InternalServerError;
                _apiResponse.Errors.Add(ex.Message);
                return Ok(_apiResponse);
            }

        }

    }
}
