using CDR_Analytics.Entities;
using System.Collections.Generic;

namespace CDR_Analytics.Interfaces
{
    public interface ICDRRepository
    {
        Task SaveCDRAsync(IEnumerable<CDR> cdrRecords); // Bulk insert CDRs
        Task<IEnumerable<CDR>> GetAllCDRsAsync(); // Fetch all CDRs
        Task<decimal> GetAverageCallCostAsync(); // Calculate avg call cost
        Task<CDR> GetLongestCallAsync(); // Fetch longest call
        Task<int> GetTotalCallsInPeriodAsync(DateOnly startDate, DateOnly endDate); // Count calls in a period
        Task<decimal> GetTotalCallCostByCallerAsync(string callerId); // Sum call cost per caller
        Task<IEnumerable<CDR>> GetCallRecordsByPhoneNumberAsync(string phoneNumber); // List calls by number
        Task<string> GetMostFrequentCallerAsync(); // Identify most frequent caller
    }
}


// IEnumerable<CDR> instead of List<CDR> because it is more flexible for query optimizations.
// SaveCDRAsync(IEnumerable < CDR > cdrRecords) because it allows batch inserts, critical for large CSV