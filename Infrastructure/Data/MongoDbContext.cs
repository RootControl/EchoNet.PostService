using Domain.Entities;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;

namespace Infrastructure.Data;

public class MongoDbContext
{
    private readonly IMongoDatabase _database;

    public MongoDbContext(IConfiguration configuration)
    {
        BsonSerializer.RegisterSerializer(new GuidSerializer(GuidRepresentation.Standard));
        
        var connectionString = configuration.GetValue<string>("MongoDb:ConnectionString", string.Empty) ?? Environment.GetEnvironmentVariable("MONGO_CONNECTION_STRING");
        var databaseName = Environment.GetEnvironmentVariable("MONGO_DATABASE_NAME") ?? configuration.GetValue<string>("MongoDb:DatabaseName", string.Empty);
        
        var client = new MongoClient(connectionString);
        _database = client.GetDatabase(databaseName);
        
        // Create indexes
        var users = Posts;
        var indexKeys = Builders<Post>.IndexKeys.Ascending(p => p.UserId);
        users.Indexes.CreateOne(new CreateIndexModel<Post>(indexKeys));
    }
    
    public IMongoCollection<Post> Posts => _database.GetCollection<Post>("Posts");
}