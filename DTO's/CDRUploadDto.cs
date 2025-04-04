namespace CDR_Analytics.DTO_s
{
    public class CDRUploadDto
    {
        public string? CallerID { get; set; } // Caller phone number
        public string? Recipient { get; set; } // Recipient phone number

        public string? CallDate { get; set; } // Raw date from CSV (string for easy parsing)
        public string? EndTime { get; set; } // Raw time from CSV

        public int Duration { get; set; } // Duration in seconds
        public decimal Cost { get; set; } // Cost of the call

        public string? Reference { get; set; } // Unique call reference
        public string? Currency { get; set; } // Currency in ISO format
    }
}

