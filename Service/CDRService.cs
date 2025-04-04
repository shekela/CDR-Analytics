using CDR_Analytics.DataContext;
using CDR_Analytics.DTO_s;
using CDR_Analytics.Entities;
using CDR_Analytics.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Runtime.CompilerServices;

namespace CDR_Analytics.Service
{
    public class CDRService : ICDRService
    {
        private readonly ICDRRepository _cdrRepository;

        public CDRService(ICDRRepository cdrRepository)
        {
            _cdrRepository = cdrRepository;
        }

        public async Task UploadCDRFileAsync(Stream fileStream)
        {
            var cdrRecords = new List<CDR>(); 

            var requiredFieldsIndexes = new[] { 0, 1, 2, 3, 4, 5, 6, 7 };

            using (var reader = new StreamReader(fileStream))
            {
                while (!reader.EndOfStream)
                {
                    var line = await reader.ReadLineAsync();
                    var values = line.Split(',');

                    // Check if any required field is missing
                    if (requiredFieldsIndexes.Any(i => string.IsNullOrEmpty(values[i])))
                    {
                        Console.WriteLine($"Skipping row due to missing data: {line}");
                        continue;  // Skip this record if any required field is missing
                    }

                    // Parse the date field with the correct format
                    DateOnly callDate;
                    bool isDateValid = DateOnly.TryParseExact(values[2], "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out callDate);

                    if (!isDateValid)
                    {
                        Console.WriteLine($"Invalid date: {values[2]}");
                        continue;  // Skip this record if the date is invalid
                    }

                    // Parse the decimal field (cost)
                    decimal cost;
                    try
                    {
                        cost = decimal.Parse(values[5], CultureInfo.InvariantCulture);  // Use InvariantCulture for dot-based decimal parsing
                    }
                    catch (FormatException ex)
                    {
                        Console.WriteLine($"Error parsing decimal value: {values[5]}. Error: {ex.Message}");
                        continue;  // Skip this record if the cost is invalid
                    }

                    // Create the CDR object and add it to the list
                    var cdr = new CDR
                    {
                        CallerID = values[0],
                        Recipient = values[1],
                        CallDate = callDate,  // Using the valid parsed date
                        EndTime = TimeOnly.ParseExact(values[3], "HH:mm:ss", CultureInfo.InvariantCulture),
                        Duration = int.Parse(values[4]),
                        Cost = cost,
                        Reference = values[6],
                        Currency = Enum.Parse<CurrencyType>(values[7], true)
                    };

                    cdrRecords.Add(cdr);
                }
            }

            // Add CDR's in bulk instead of one by one
            await _cdrRepository.SaveCDRAsync(cdrRecords);
        }



        public async Task<decimal> GetAverageCallCostAsync()
        {
            return await _cdrRepository.GetAverageCallCostAsync();
        }

        public async Task<CDRDto> GetLongestCallAsync()
        {
            var longestCall = await _cdrRepository.GetLongestCallAsync();
            if (longestCall == null) return null;

            var callDateOnly = longestCall.CallDate;

            return new CDRDto
            {
                CallerID = longestCall.CallerID,
                Recipient = longestCall.Recipient,
                CallDate = callDateOnly,
                Duration = longestCall.Duration,
                Cost = longestCall.Cost,
                EndTime = longestCall.EndTime,   
                Reference = longestCall.Reference,  
                Currency = longestCall.Currency.ToString() //Map Currency
            };
        }


        public async Task<int> GetTotalCallsInPeriodAsync(DateOnly startDate, DateOnly endDate)
        {
            return await _cdrRepository.GetTotalCallsInPeriodAsync(startDate, endDate);
        }

        public async Task<decimal> GetTotalCallCostByCallerAsync(string callerId)
        {
            return await _cdrRepository.GetTotalCallCostByCallerAsync(callerId);
        }

        public async Task<IEnumerable<CDRDto>> GetCallRecordsByPhoneNumberAsync(string phoneNumber)
        {
            var cdrRecords = await _cdrRepository.GetCallRecordsByPhoneNumberAsync(phoneNumber);

            // Convert CDR entities to DTOs
            return cdrRecords.Select(c => new CDRDto
            {
                CallerID = c.CallerID,
                Recipient = c.Recipient,
                CallDate = c.CallDate,
                EndTime = c.EndTime,
                Duration = c.Duration,
                Cost = c.Cost,
                Reference = c.Reference,
                Currency = c.Currency.ToString()
            }).ToList();
        }

        public async Task<string> GetMostFrequentCallerAsync()
        {
            return await _cdrRepository.GetMostFrequentCallerAsync();
        }
    }
}
