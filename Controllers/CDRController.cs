using CDR_Analytics.DTO_s;
using CDR_Analytics.Interfaces;
using CDR_Analytics.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CDR_Analytics.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CDRController : ControllerBase
    {
        private readonly ICDRService _cdrService;

        public CDRController(ICDRService cdrService)
        {
            _cdrService = cdrService;
        }


        [HttpPost("upload")]
        public async Task<IActionResult> UploadCDRFileAsync(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("No file uploaded.");
            }

            if (file.Length > 100 * 1024 * 1024) // MAX 100MB
            {
                return BadRequest("File size exceeds the maximum allowed limit of 100MB.");
            }

            try
            {
                await _cdrService.UploadCDRFileAsync(file.OpenReadStream());

                return Ok("File uploaded and processed successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }


        [HttpGet("average-call-cost")]
        public async Task<IActionResult> GetAverageCallCostAsync()
        {
            try
            {
                var averageCost = await _cdrService.GetAverageCallCostAsync();

                if (averageCost == 0)
                {
                    return NotFound("No call records found to calculate average cost.");
                }

                return Ok(new { AverageCost = averageCost });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }


        [HttpGet("longest-call")]
        public async Task<CDRDto> GetLongestCallAsync()
        {
            var longestCall = await _cdrService.GetLongestCallAsync();
            if (longestCall == null) return null;

            return new CDRDto
            {
                CallerID = longestCall.CallerID,
                Recipient = longestCall.Recipient,
                CallDate = longestCall.CallDate,
                EndTime = longestCall.EndTime ?? new TimeOnly(0, 0), 
                Duration = longestCall.Duration,
                Cost = longestCall.Cost,
                Reference = longestCall.Reference,
                Currency = longestCall.Currency?.ToString()
            };
        }

        [HttpGet("total-calls-in-period")]
        public async Task<IActionResult> GetTotalCallsInPeriodAsync([FromQuery] DateOnly startDate, [FromQuery] DateOnly endDate)
        {
            try
            {
                var totalCalls = await _cdrService.GetTotalCallsInPeriodAsync(startDate, endDate);

                if (totalCalls == 0)
                {
                    return NotFound("No calls found in the specified period.");
                }

                return Ok(totalCalls);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }


        [HttpGet("total-call-cost-by-caller")]
        public async Task<IActionResult> GetTotalCallCostByCallerAsync([FromQuery] string callerID)
        {
            try
            {
                var totalCost = await _cdrService.GetTotalCallCostByCallerAsync(callerID);

                if (totalCost == 0)
                {
                    return NotFound("No calls found for the specified caller.");
                }

                return Ok(totalCost);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }


        [HttpGet("call-records-by-phone-number")]
        public async Task<IActionResult> GetCallRecordsByPhoneNumberAsync([FromQuery] string phoneNumber)
        {
            try
            {
                var cdrRecords = await _cdrService.GetCallRecordsByPhoneNumberAsync(phoneNumber);

                if (cdrRecords == null || !cdrRecords.Any())
                {
                    return NotFound("No call records found for the specified phone number.");
                }

                return Ok(cdrRecords);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }


        [HttpGet("most-frequent-caller")]
        public async Task<IActionResult> GetMostFrequentCallerAsync()
        {
            try
            {
                var mostFrequentCaller = await _cdrService.GetMostFrequentCallerAsync();

                if (string.IsNullOrEmpty(mostFrequentCaller))
                {
                    return NotFound("No call records found.");
                }

                return Ok(new { mostFrequentCaller });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }




    }
}
