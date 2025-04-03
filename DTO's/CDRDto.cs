using System.IO;

namespace CDR_Analytics.DTO_s
{
    public class CDRDto
    {
        public int Id { get; set; } // Unique ID of the record

        public string CallerID { get; set; } // Phone number of the caller
        public string Recipient { get; set; } // Phone number of the recipient

        public DateOnly CallDate { get; set; } // Date of the call
        public TimeOnly EndTime { get; set; } // Time the call ended

        public int Duration { get; set; } // Call duration in seconds
        public decimal Cost { get; set; } // Call cost

        public string Reference { get; set; } // Unique call reference
        public string Currency { get; set; } // ISO alpha-3 currency code
    }
}



//CurrencyType is converted to string to avoid enum serialization issues in API responses. It also prevents exposing internal properties directly.