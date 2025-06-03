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
    public class PaymentController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IHospitalRepository<Payment> _paymentRepository;
        private APIResponse _apiResponse;

        public PaymentController(IMapper mapper, IHospitalRepository<Payment> paymentRepository)
        {
            _mapper = mapper;
            _paymentRepository = paymentRepository;
            _apiResponse = new();
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [Route("Create")]
        public async Task<ActionResult<APIResponse>> CreatePaymentAsync(PaymentDTO dto)
        {
            try
            {
                if (dto == null)
                    return BadRequest();
                Payment payment = _mapper.Map<Payment>(dto);

                var result = await _paymentRepository.CreateAsync(payment);
                dto.PaymentID = result.PaymentID;
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
        [Route("All", Name = "GetAllPayments")]
        public async Task<ActionResult<APIResponse>> GetPaymentsAsync()
        {
            try
            {
                var Payments = await _paymentRepository.GetAllAsync();

                _apiResponse.data = _mapper.Map<List<PaymentDTO>>(Payments);
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
        [Route("{id:int}", Name = "GetPaymentById")]
        public async Task<ActionResult<APIResponse>> GetPaymentByIdAsync(int id)
        {
            try
            {

                if (id <= 0)
                    return BadRequest();
                var payment = await _paymentRepository.GetAsync(payment => payment.PaymentID == id);

                if (payment == null)
                    return NotFound($"The payment not found with id: {id}");

                _apiResponse.data = _mapper.Map<PaymentDTO>(payment);
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
        public async Task<ActionResult> UpdatePaymentAsync(int id, [FromBody] JsonPatchDocument<PaymentDTO> patchDocument)
        {
            try
            {
                if (patchDocument == null || id <= 0)
                    return BadRequest();
                var existingPayment = await _paymentRepository.GetAsync(Payment => Payment.PaymentID == id, true);

                if (existingPayment == null)
                    return NotFound();

                var paymentDTO = _mapper.Map<PaymentDTO>(existingPayment);

                patchDocument.ApplyTo(paymentDTO, ModelState);
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                existingPayment = _mapper.Map<Payment>(paymentDTO);

                await _paymentRepository.UpdateAsync(existingPayment);

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
        [Route("Delete/{id}", Name = "DeletePaymentById")]
        public async Task<ActionResult<APIResponse>> DeletePaymentAsync(int id)
        {
            try
            {
                if (id <= 0)
                    return BadRequest();

                var payment = await _paymentRepository.GetAsync(payment => payment.PaymentID == id);

                if (payment == null)
                    return BadRequest($"the payment not found with the id: {id} to delete");

                await _paymentRepository.DeleteAsync(payment);
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
