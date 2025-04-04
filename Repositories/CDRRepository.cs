using CDR_Analytics.DataContext;
using CDR_Analytics.Entities;
using CDR_Analytics.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CDR_Analytics.Repositories
{
    public class CDRRepository : ICDRRepository
    {
        private readonly ApplicationDbContext _context;

        public CDRRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        // Bulk insert CDRs
        public async Task SaveCDRAsync(IEnumerable<CDR> cdrRecords)
        {
            await _context.CDRs.AddRangeAsync(cdrRecords); // Add bulk CDR records at once
            await _context.SaveChangesAsync(); 
        }

        // Fetch all CDRs
        public async Task<IEnumerable<CDR>> GetAllCDRsAsync()
        {
            return await _context.CDRs.ToListAsync();
        }

        // Calculate the average call cost
        public async Task<decimal> GetAverageCallCostAsync()
        {
            return await _context.CDRs
                .AverageAsync(c => c.Cost);
        }

        // Fetch the longest call based on duration
        public async Task<CDR> GetLongestCallAsync()
        {
            return await _context.CDRs
                .OrderByDescending(c => c.Duration) // Order by duration descending
                .FirstOrDefaultAsync(); // Return the first call
        }

        // Count calls in a specified date period
        public async Task<int> GetTotalCallsInPeriodAsync(DateOnly startDate, DateOnly endDate)
        {
            return await _context.CDRs
                .Where(c => c.CallDate >= startDate && c.CallDate <= endDate)
                .CountAsync(); 
        }

        // Calculate the total call cost for a specific caller
        public async Task<decimal> GetTotalCallCostByCallerAsync(string callerId)
        {
            return await _context.CDRs
                .Where(c => c.CallerID == callerId) 
                .SumAsync(c => c.Cost); // Sum the cost of all calls made by this caller
        }

        // List all calls made by a specific phone number
        public async Task<IEnumerable<CDR>> GetCallRecordsByPhoneNumberAsync(string phoneNumber)
        {
            return await _context.CDRs
                .Where(c => c.CallerID == phoneNumber || c.Recipient == phoneNumber) // Find calls by this phone number
                .ToListAsync(); 
        }

        // Identify the most frequent caller
        public async Task<string> GetMostFrequentCallerAsync()
        {
            // Fetch most frequent caller
            var caller = await _context.CDRs
                .Where(c => !string.IsNullOrEmpty(c.CallerID)) // Ensuring no null or empty CallerID
                .GroupBy(c => c.CallerID) // Group by caller ID
                .OrderByDescending(g => g.Count()) // Order by the number of calls
                .Select(g => g.Key) // Select the caller ID with the highest count
                .FirstOrDefaultAsync(); // Return the caller ID

            // If no frequent caller found, return a meaningful message
            if (string.IsNullOrEmpty(caller))
            {
                return "No frequent caller found.";
            }

            return caller; // Return the most frequent caller's CallerID
        }

    }
}
