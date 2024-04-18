using Microsoft.AspNetCore.Builder;
using WebApplication1.Handlers;

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

app.UseHttpsRedirection();

// end-node / node
// Http Verb (RESTful):
// GET    - retrieve data,
// POST   - post new data,
// PUT    - update whole data,
// PATCH  - update part of data,
// DELETE - delete data

// https://localhost:7024/hello-get
app.MapGet("/hello-get", () => "[GET] Hello World!");

// https://localhost:7024/test-post
app.MapPost("/hello-post", () => "[POST] Hello World!");

// https://localhost:7024/hello-put
app.MapPut("/hello-put", () => "[PUT] Hello World!");

// https://localhost:7024/hello-delete
app.MapDelete("/hello-delete",  () => "[DELETE] Hello World!");

// https://localhost:7024/hello-delete
app.MapMethods("/hello-patch", new[] { HttpMethods.Patch }, () => "[PATCH] Hello World!");

// string hello() => "[LOCAL FUNCTION] Hello World!";
string hello()  
{
    string name = "John Doe";
    return $"[LOCAL FUNCTION] Hello {name}!";
}

app.MapGet("/hello", hello);

// demo using class and method
var handler = new HelloHandler();
app.MapGet("/hello2", handler.Hello);

// Route with parameter
// https://localhost:7024/users/johndoe/product/200
app.MapGet("/users/{username}/product/{productId}", 
    (string username, int productId) => $"The username is {username} and the product Id is {productId}");

// Route Constraint
app.MapGet("/users/{userid:int:min(10)}", (int userid) => $"The userid is {userid}");

// Route with Query String
// https://localhost:7024/hello3?name=John
app.MapGet("/hello3", (string name) => $"Hello {name}");

// Route receive body data
// conclusion: when receive body data as JSON, Minimal API auto convert to object
app.MapPut("/people/{id:int}", (int id, Person person) => { return $"{person.FirstName} {person.LastName}"; });

// Exploring Response (see page 29)
// conclusion: when return an obj, Minimal API automatically convert to JSON
app.MapGet("/ok", () => Results.Ok(new Person("Donald", "Duck"))); // Ok = 200

app.MapGet("/notfound/{id:int}", (int id) =>
{
    if (id < 10)
    {
        return Results.NotFound("Id should > 10"); // 404
    } 
    else if(id < 20)
    {
        // auto convert record to JSON
        return Results.BadRequest(new { ErrorMessage = "Unable to complete the request" });// 400
    }
    else
    {
        return Results.Ok("Id is good"); // 200
    }
});

// auto convert List to JSON
app.MapGet("/person-list", () =>
{
    List<Person> persons = new List<Person>();
    persons.Add(new Person("John", "Doe"));
    persons.Add(new Person("Abu", "Bakar"));
    persons.Add(new Person("Ali", "Abu"));
    return persons;
});

// CRUD - Create, Retrieve, Update, Delete
List<Person> persons = new List<Person>() 
{ 
    new Person("John", "Doe"),
    new Person("Jane", "Doe"),
    new Person("Abu", "Bakar")
};

// insert Person
app.MapPost("/person-create", (Person person) => 
{
    persons.Add(person);
    return Results.Ok(new {Message = "Person successfully inserted"});
});

// list all persons
app.MapGet("/person-all", () => persons);

app.MapGet("/person-search/{firstName}", (string firstName) => 
{
    // Linq
    Person p = persons.FirstOrDefault(x => x.FirstName == firstName);
    return p;
});

app.MapDelete("/person-delete/{firstName}", (string firstName) =>
{
    persons = persons.FindAll(x => x.FirstName != firstName);
    return persons;
});

PeopleHandler.MapEndPoints(app);
EmployeeHandler.MapEndPoints(app);
app.Run();

// record and class almost similar, but record only cater for data not action
public record class Person(string FirstName, string LastName);

class HelloHandler
{
    public string Hello()
    {
        string name = "Jane Doe";
        return $"[INSTANCE METHOD] Hello {name}";
    }
}