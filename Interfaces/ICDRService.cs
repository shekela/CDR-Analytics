using CDR_Analytics.DTO_s;

namespace CDR_Analytics.Interfaces
{
    public interface ICDRService
    {
        Task UploadCDRFileAsync(Stream fileStream); // Handles large CSV uploads
        Task<decimal> GetAverageCallCostAsync(); // Average cost per call
        Task<CDRDto> GetLongestCallAsync(); // Call with longest duration
        Task<int> GetTotalCallsInPeriodAsync(DateOnly startDate, DateOnly endDate); // Count calls in a time range
        Task<decimal> GetTotalCallCostByCallerAsync(string callerId); // Total cost per caller
        Task<IEnumerable<CDRDto>> GetCallRecordsByPhoneNumberAsync(string phoneNumber); // Calls made by a number
        Task<string> GetMostFrequentCallerAsync(); // Most frequent caller
    }
}

// Stream fileStream for efficient CSV handling (instead of byte[] which loads everything in memory).
// Uses asynchronous methods (Task<>) for better scalability.
// Returns DTOs instead of entities to keep the API response clean.