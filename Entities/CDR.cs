namespace CDR_Analytics.Entities
{
    public class CDR
    {
       public int Id { get; set; }
       public string? CallerID { get; set; }
       public string? Recipient {  get; set; } 
       public DateOnly CallDate { get; set; } // What date the call happened
       public TimeOnly? EndTime { get; set; } // What time the call ended
       public int Duration { get; set; } // How long the call lasted in seconds
       public decimal Cost { get; set; } // How much the call cost
       public string? Reference { get; set; } // Unique identifier of the call
       public CurrencyType Currency { get; set; } // What currency the call was in

    }

    public enum CurrencyType
    {
        GBP,
        USD,
        EUR
    }

}

