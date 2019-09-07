using Amazon.DynamoDBv2.DataModel;
using System;

namespace Postcore.AdApi.Infrastructure.Models
{
    [DynamoDBTable("Ads")]
    public class AdData
    {
        [DynamoDBHashKey] public string Id { get; set; }

        [DynamoDBProperty] public string Title { get; set; }

        [DynamoDBProperty] public string Description { get; set; }

        [DynamoDBProperty] public double Price { get; set; }

        [DynamoDBProperty] public DateTime CreationDateTime { get; set; }

        [DynamoDBProperty] public AdDataStatus Status { get; set; }

        [DynamoDBProperty] public string FilePath { get; set; }

        [DynamoDBProperty] public string Username { get; set; }
    }
}
