using System.Collections.Generic;
using System.Threading.Tasks;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Postcore.AdApi.Infrastructure.Models;

namespace Postcore.AdApi.Infrastructure.Repositories
{
    public class AdRepository : IAdRepository
    {
        private readonly IAmazonDynamoDB _client;
        private readonly DynamoDBContext _context;

        public AdRepository(IAmazonDynamoDB dynamoDbClient)
        {
            _client = dynamoDbClient;
            _context = new DynamoDBContext(dynamoDbClient);
        }

        public async Task<string> Add(AdData ad)
        {
            await _context.SaveAsync(ad);
            return ad.Id;
        }

        public async Task Delete(AdData ad)
        {
            await _context.DeleteAsync<AdData>(ad);
        }

        public async Task<AdData> Get(string id)
        {
            return await _context.LoadAsync<AdData>(id);
        }

        public async Task<IEnumerable<AdData>> GetAll()
        {
            return await _context.ScanAsync<AdData>(new List<ScanCondition>()).GetRemainingAsync();
        }

        public async Task<bool> CheckHealth()
        {
            var table = await _client.DescribeTableAsync("Ads");
            return string.Compare(table.Table.TableStatus, "active", true) == 0;
        }
    }
}
