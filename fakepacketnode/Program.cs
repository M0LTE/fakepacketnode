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

app.MapGet("/xrouter/api/v1/mail/msgs", (
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
            rcvd = "Mon, 14 Sep 1997 23:47:00 GMT",
            size = 31,
            type = "P",
            status = "R",
            to = "M0LTE@GB7RDG.#42.GBR.EURO",
            from = "g8pzt",
            subject = "Hello world"
        }, new MessageListing{
            id = 445,
            mid = "m_445",
            rcvd = "Tue, 16 Oct 1997 22:43:15 GMT",
            size = 12,
            type = "P",
            status = "R",
            to = "g8pzt",
            from = "M0LTE@GB7RDG.#42.GBR.EURO",
            subject = "Re: Hello world"
        },
    };
})
.WithName("GetMessageList")
.WithOpenApi();

app.MapGet("/xrouter/api/v1/mail/msgs/{msgNumber}", (int msgNumber) => {
    return msgNumber == 444
        ? Results.Ok(new Message
        {
            id = 444,
            mid = "m_444",
            rcvd = "Mon, 14 Sep 1997 23:47:00 GMT",
            size = 10,
            type = "P",
            status = "R",
            to = "M0LTE@GB7RDG.#42.GBR.EURO",
            from = "g8pzt",
            subject = "Hello world",
            text = "This is the body of the message"
        })
        : Results.NotFound();
})
.WithName("GetMessage")
.WithOpenApi();

app.MapPost("/xrouter/api/v1/mail/msgs", (PostMessage msg) => {
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
    /// <summary>
    /// in format "Mon, 14 Sep 1997 23:47:00 GMT"
    /// </summary>
    public required string rcvd { get; set; }
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
}

internal record Message : MessageListing
{
    public required string text { get; set; }
}

