CDR Analytics API - Project Documentation
Overview
The CDR (Call Detail Record) Analytics API is a robust project designed to handle large CSV uploads containing call records and provide insightful analytics through various endpoints. This application utilizes Entity Framework Core (EF Core) for data management and follows the SOLID principles to ensure clean architecture and maintainability.

Key Features:
- Handles CSV uploads for call data.
- Provides endpoints for analytics on call records.
- Implements business logic and data access layers following clean architecture.
- Allows querying of call details, including total cost, total calls, most frequent caller, and specific call records by date or number.
Data Access Layer (DAL)
The Data Access Layer (DAL) is responsible for handling interactions with the database. It consists of the following key components:

1. **Entity Framework Core**
- ApplicationDbContext.cs: Manages the EF Core database context.
- CDR Entity: Defines the call record structure in the database, with key fields like `CallerID`, `Recipient`, `CallDate`, `Duration`, `Cost`, etc.

Example of the `CDR` Entity:
```csharp
public class CDR
{
    public int Id { get; set; }
    public string CallerID { get; set; }
    public string Recipient { get; set; }
    public DateOnly CallDate { get; set; }
    public TimeOnly EndTime { get; set; }
    public int Duration { get; set; }
    public decimal Cost { get; set; }
    public string? Reference { get; set; }
    public CurrencyType Currency { get; set; }
}
```

2. **Repositories**: **CDRRepository**
The `CDRRepository` is responsible for accessing the `CDRs` table in the database and executing queries. It contains methods for performing CRUD operations on the data.

```csharp
public class CDRRepository : ICDRRepository
{
    private readonly ApplicationDbContext _context;

    public CDRRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    // Example method for getting the total call cost by a caller
    public async Task<decimal> GetTotalCallCostByCallerAsync(string callerId)
    {
        return await _context.CDRs
            .Where(c => c.CallerID == callerId)
            .SumAsync(c => c.Cost);
    }

    // Other data access methods for retrieving call records
}
```

Service Layer
The Service Layer holds the business logic and provides an abstraction over the repository layer. It defines the core methods for processing and manipulating data, such as calculating total costs, retrieving the longest call, etc.

Example of a service method:

```csharp
public class CDRService : ICDRService
{
    private readonly ICDRRepository _cdrRepository;

    public CDRService(ICDRRepository cdrRepository)
    {
        _cdrRepository = cdrRepository;
    }

    public async Task<decimal> GetTotalCallCostByCallerAsync(string callerId)
    {
        return await _cdrRepository.GetTotalCallCostByCallerAsync(callerId);
    }
}
```

API Endpoints
The API Endpoints are responsible for receiving requests and providing responses based on the business logic in the service layer.

### Available Endpoints:

1. **Get Total Call Cost by Caller**
- **GET** `/total-call-cost/{callerId}`
- Returns the total call cost made by a specific caller.

2. **Get Total Calls in a Period**
- **GET** `/total-calls/{startDate}/{endDate}`
- Returns the total number of calls made in a specific date range.

3. **Get Call Records by Phone Number**
- **GET** `/call-records/{phoneNumber}`
- Returns all call records for a specific phone number.

4. **Get Most Frequent Caller**
- **GET** `/most-frequent-caller`
- Returns the phone number that made the most calls.

5. **Get Longest Call**
- **GET** `/longest-call`
- Returns the longest call record.

6. **Get Average Call Cost**
- **GET** `/average-call-cost`
- Returns the average cost of a call.

7. **Upload CDR File**
- **POST** `/upload-cdr-file`
- Allows uploading of a CSV file containing CDR records, which are then processed and inserted into the database.

CSV File Upload
The CSV file upload functionality is designed to handle bulk data imports into the system. The file should be in a CSV format, with the following fields:

- **CallerID**
- **Recipient**
- **CallDate**
- **EndTime**
- **Duration**
- **Cost**
- **Reference**
- **Currency**

### Upload Logic:
- Each record is validated before being inserted into the database.
- Records with missing or invalid values for **CallerID** or **Recipient** are filtered out.
- A custom **filter** ensures that all required fields are present in the CSV file.

### Example CSV Row:
```csv
441616674195,19/08/2016,15:36:23,5,0.001,C95AA1D54558503288CC988BD6CBCB367,GBP
```

Data Validation & Integrity
During the upload process, the data is validated using the following checks:
- **Empty fields**: Any record missing required fields like `CallerID`, `Recipient`, `CallDate`, etc., will be skipped.
- **Invalid date format**: If the `CallDate` does not match the expected format, the record will be ignored. Correct date format example (2025-12-25) .
- **Invalid decimal values**: If the `Cost` is not a valid decimal, the record is skipped.

This ensures that the uploaded data is clean and reliable.
Data Model Relationships
- **CDR Entity**: Represents a single call record with fields like `CallerID`, `Recipient`, `CallDate`, `Duration`, `Cost`, and more.
- **Currency**: The currency field is an enumeration (`CurrencyType`) that defines the supported currency codes (e.g., `GBP`, `USD`, `EUR`).

Next Steps
1. **Documentation**: Expand documentation to include examples and usage instructions for each endpoint.
Conclusion
The CDR Analytics API is designed to provide powerful analytics on call records, with a clean and maintainable architecture. The application uses Entity Framework Core for database management, follows the SOLID principles, and implements a flexible service and data access layer to ensure scalability.
CDR Analytics API - Project Documentation
Overview
The CDR (Call Detail Record) Analytics API is a robust project designed to handle large CSV uploads containing call records and provide insightful analytics through various endpoints. This application utilizes Entity Framework Core (EF Core) for data management and follows the SOLID principles to ensure clean architecture and maintainability.

Key Features:
- Handles CSV uploads for call data.
- Provides endpoints for analytics on call records.
- Implements business logic and data access layers following clean architecture.
- Allows querying of call details, including total cost, total calls, most frequent caller, and specific call records by date or number.
Data Access Layer (DAL)
The Data Access Layer (DAL) is responsible for handling interactions with the database. It consists of the following key components:

1. **Entity Framework Core**
- ApplicationDbContext.cs: Manages the EF Core database context.
- CDR Entity: Defines the call record structure in the database, with key fields like `CallerID`, `Recipient`, `CallDate`, `Duration`, `Cost`, etc.

Example of the `CDR` Entity:
```csharp
public class CDR
{
    public int Id { get; set; }
    public string CallerID { get; set; }
    public string Recipient { get; set; }
    public DateOnly CallDate { get; set; }
    public TimeOnly EndTime { get; set; }
    public int Duration { get; set; }
    public decimal Cost { get; set; }
    public string? Reference { get; set; }
    public CurrencyType Currency { get; set; }
}
```

2. **Repositories**: **CDRRepository**
The `CDRRepository` is responsible for accessing the `CDRs` table in the database and executing queries. It contains methods for performing CRUD operations on the data.

```csharp
public class CDRRepository : ICDRRepository
{
    private readonly ApplicationDbContext _context;

    public CDRRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    // Example method for getting the total call cost by a caller
    public async Task<decimal> GetTotalCallCostByCallerAsync(string callerId)
    {
        return await _context.CDRs
            .Where(c => c.CallerID == callerId)
            .SumAsync(c => c.Cost);
    }

    // Other data access methods for retrieving call records
}
```

Service Layer
The Service Layer holds the business logic and provides an abstraction over the repository layer. It defines the core methods for processing and manipulating data, such as calculating total costs, retrieving the longest call, etc.

Example of a service method:

```csharp
public class CDRService : ICDRService
{
    private readonly ICDRRepository _cdrRepository;

    public CDRService(ICDRRepository cdrRepository)
    {
        _cdrRepository = cdrRepository;
    }

    public async Task<decimal> GetTotalCallCostByCallerAsync(string callerId)
    {
        return await _cdrRepository.GetTotalCallCostByCallerAsync(callerId);
    }
}
```

API Endpoints
The API Endpoints are responsible for receiving requests and providing responses based on the business logic in the service layer.

### Available Endpoints:

1. **Get Total Call Cost by Caller**
- **GET** `/total-call-cost/{callerId}`
- Returns the total call cost made by a specific caller.

2. **Get Total Calls in a Period**
- **GET** `/total-calls/{startDate}/{endDate}`
- Returns the total number of calls made in a specific date range.

3. **Get Call Records by Phone Number**
- **GET** `/call-records/{phoneNumber}`
- Returns all call records for a specific phone number.

4. **Get Most Frequent Caller**
- **GET** `/most-frequent-caller`
- Returns the phone number that made the most calls.

5. **Get Longest Call**
- **GET** `/longest-call`
- Returns the longest call record.

6. **Get Average Call Cost**
- **GET** `/average-call-cost`
- Returns the average cost of a call.

7. **Upload CDR File**
- **POST** `/upload-cdr-file`
- Allows uploading of a CSV file containing CDR records, which are then processed and inserted into the database.

CSV File Upload
The CSV file upload functionality is designed to handle bulk data imports into the system. The file should be in a CSV format, with the following fields:

- **CallerID**
- **Recipient**
- **CallDate**
- **EndTime**
- **Duration**
- **Cost**
- **Reference**
- **Currency**

### Upload Logic:
- Each record is validated before being inserted into the database.
- Records with missing or invalid values for **CallerID** or **Recipient** are filtered out.
- A custom **filter** ensures that all required fields are present in the CSV file.

### Example CSV Row:
```csv
441616674195,19/08/2016,15:36:23,5,0.001,C95AA1D54558503288CC988BD6CBCB367,GBP
```

Data Validation & Integrity
During the upload process, the data is validated using the following checks:
- **Empty fields**: Any record missing required fields like `CallerID`, `Recipient`, `CallDate`, etc., will be skipped.
- **Invalid date format**: If the `CallDate` does not match the expected format, the record will be ignored. Correct date format example (2025-12-25) .
- **Invalid decimal values**: If the `Cost` is not a valid decimal, the record is skipped.

This ensures that the uploaded data is clean and reliable.
Data Model Relationships
- **CDR Entity**: Represents a single call record with fields like `CallerID`, `Recipient`, `CallDate`, `Duration`, `Cost`, and more.
- **Currency**: The currency field is an enumeration (`CurrencyType`) that defines the supported currency codes (e.g., `GBP`, `USD`, `EUR`).

Next Steps
1. **Documentation**: Expand documentation to include examples and usage instructions for each endpoint.
Conclusion
The CDR Analytics API is designed to provide powerful analytics on call records, with a clean and maintainable architecture. The application uses Entity Framework Core for database management, follows the SOLID principles, and implements a flexible service and data access layer to ensure scalability.
