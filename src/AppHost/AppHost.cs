var builder = DistributedApplication.CreateBuilder(args);

var username = builder.AddParameter("username");
var password = builder.AddParameter("password", secret: true);

var mongo = builder.AddMongoDB("mongo", 27017, username, password)
    .WithLifetime(ContainerLifetime.Persistent)
    .WithDataVolume();

var mongodb = mongo.AddDatabase("mongodb");

builder.AddProject<Projects.Api>("api")
    .WaitFor(mongodb)
    .WithReference(mongodb);

await builder.Build().RunAsync();
