var builder = DistributedApplication.CreateBuilder(args);

var username = builder.AddParameter("username");
var password = builder.AddParameter("password", secret: true);

var mongo = builder.AddMongoDB("mongo", 27017, username, password)
    // Disable SHSTK (Shadow Stack) CPU feature for glibc to prevent compatibility issues
    // with MongoDB in Linux containers (can cause crashes on some systems)
    .WithEnvironment("GLIBC_TUNABLES", "glibc.cpu.hwcaps=-SHSTK")
    .WithLifetime(ContainerLifetime.Persistent)
    .WithDataVolume("mongo-data");

var mongodb = mongo.AddDatabase("mongodb");

builder.AddProject<Projects.Api>("api")
    .WaitFor(mongodb)
    .WithReference(mongodb);

await builder.Build().RunAsync();
