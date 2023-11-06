﻿using System.Text.Json;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using MongoDB.Entities;

namespace SearchService;

public class DbInitializer
{

    public DbInitializer()
    {

    }

    public static async Task InitDb(WebApplication app)
    {
        await DB.InitAsync("SearchDb", MongoClientSettings.FromConnectionString(app.Configuration.GetConnectionString("MongoDbConnection")));

        await DB.Index<Item>()
            .Key(x => x.Make, KeyType.Text)
            .Key(x => x.Model, KeyType.Text)
            .Key(x => x.Color, KeyType.Text)
            .CreateAsync();

        var count = await DB.CountAsync<Item>();

        using var scope = app.Services.CreateScope();

        var auctionSvcHttpClient = scope.ServiceProvider.GetRequiredService<AuctionSvcHttpClient>();

        var items = await auctionSvcHttpClient.GetItemsForSearchDb();

        System.Console.WriteLine(items.Count + " returned from the aution service");

        if (items.Count > 0)
        {
            await DB.SaveAsync(items);
        }
    }
}
