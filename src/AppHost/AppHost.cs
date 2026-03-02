var builder = DistributedApplication.CreateBuilder(args);

var mongo = builder.AddMongoDB("mongo")
    .WithVolume("mongoDb");

var mongodb = mongo.AddDatabase("mongodb");

builder.AddProject<Projects.Api>("api")
    .WaitFor(mongodb)
    .WithReference(mongodb);

await builder.Build().RunAsync();
