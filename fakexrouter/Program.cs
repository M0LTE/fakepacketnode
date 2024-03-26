var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// https://learn.microsoft.com/en-us/aspnet/core/fundamentals/minimal-apis?view=aspnetcore-8.0

app.MapGet("/api/v1/mail/msgs", (
    int? offset, 
    int? maxitems,
    string? to,
    string ? at,
    string ? from,
    string? subject) =>
{
    return new[] {
        new MessageListing{
            id = 444,
            mid = "m_444",
            rcvd = 1711252093,
            size = 31,
            type = "P",
            status = "R",
            to = "M0LTE@GB7RDG.#42.GBR.EURO",
            from = "g8pzt",
            subject = "Hello world",
            links = new ListingLinks { view = "blah" }
        }, new MessageListing{
            id = 445,
            mid = "m_445",
            rcvd = 1711272093,
            size = 12,
            type = "P",
            status = "R",
            to = "g8pzt",
            from = "M0LTE@GB7RDG.#42.GBR.EURO",
            subject = "Re: Hello world",
            links = new ListingLinks { view = "blah" }
        },
    };
})
.WithName("GetMessageList")
.WithOpenApi();

app.MapGet("/api/v1/mail/msgs/{msgNumber}", (int msgNumber) =>
{
    return msgNumber == 444
        ? Results.Ok(new Message
        {
            id = 444,
            mid = "m_444",
            rcvd = 1711252093,
            size = 10,
            type = "P",
            status = "R",
            to = "M0LTE@GB7RDG.#42.GBR.EURO",
            from = "g8pzt",
            subject = "Hello world",
            text = "This is the body of the message",
            links = new ListingLinks { view = "blah" }
        })
        : Results.NotFound();
})
.WithName("GetMessage")
.WithOpenApi()
.Produces<Message>(StatusCodes.Status200OK)
.Produces(StatusCodes.Status404NotFound);

app.MapPost("/api/v1/mail/msgs", (PostMessage msg) => {
    return new PostResponse { id = new Random().Next(1000) };
})
.WithName("PostMessage")
.WithOpenApi();

app.Run();

internal record PostResponse
{
    public required int id { get; set; }
}

internal record PostMessage
{
    public required string from { get; set; }
    public required string to { get; set; }
    public required string type { get; set; }
    public required string subject { get; set; }
    public required string text { get; set; }
}

internal record MessageListing
{
    public required int id { get; set; }
    public required string mid { get; set; }
    public required long rcvd { get; set; }
    public required int size { get; set; }
    /// <summary>
    /// type and status may in future be unambiguous words
    /// </summary>
    public required string type { get; set; }
    /// <summary>
    /// type and status may in future be unambiguous words
    /// </summary>
    public required string status { get; set; }
    /// <summary>
    /// e.g. "g8pzt", "all@gbr", "g8pzt@gb7pzt.#24.gbr.eu"
    /// </summary>
    public required string to { get; set; }
    public required string from { get; set; }
    public required string subject { get; set; }
    public required ListingLinks links { get; set; }
}

internal record ListingLinks
{
    public required string view { get; set; }
}

internal record Message : MessageListing
{
    public required string text { get; set; }
}

