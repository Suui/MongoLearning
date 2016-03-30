using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Linq;
using System.Threading.Tasks;


namespace MongoSpike
{
	public class Mongo
	{
		protected static IMongoClient Client { get; set; }
		protected static IMongoDatabase Database { get; set; }

		public Mongo()
		{
			Client = new MongoClient();
			Database = Client.GetDatabase("test");
		}

		public async void InsertDocument()
		{
			var document = new BsonDocument
			{
				{"address", new BsonDocument
					{
						{"street", "2 Avenue"},
						{"zipcode", "10075"},
						{"building", "1480"},
						{"coord", new BsonArray {73.9557413, 40.7720266}}
					}
				},
				{"borough", "Manhattan"},
				{"cuisine", "Italian"},
				{"grades", new BsonArray
					{
						new BsonDocument
						{
							{"date", new DateTime(2014, 10, 1, 0, 0, 0, DateTimeKind.Utc)},
							{"grade", "A"},
							{"score", 11}
						},
						new BsonDocument
						{
							{"date", new DateTime(2014, 1, 6, 0, 0, 0, DateTimeKind.Utc)},
							{"grade", "B"},
							{"score", 17}
						}
					}
				},
				{"name", "Vella"},
				{"restaurant_id", "41704620"}
			};

			var collection = Database.GetCollection<BsonDocument>("restaurants");
			await collection.InsertOneAsync(document);
		}

		public async Task<int> TotalRestaurantDocuments()
		{
			var collection = Database.GetCollection<BsonDocument>("restaurants");
			var filteredNone = new BsonDocument();
			var count = 0;

			var cursor = await collection.FindAsync(filteredNone);
			while (await cursor.MoveNextAsync())
			{
				var batch = cursor.Current;
				foreach (var document in batch)
				{
					count += 1;
				}
			}

			return count;
		}

		public async Task<int> RestaurantsInManhattan()
		{
			var collection = Database.GetCollection<BsonDocument>("restaurants");
			var filteredInManhattan = Builders<BsonDocument>.Filter.Eq("borough", "Manhattan");
			var manhattanRestaurants = await collection.Find(filteredInManhattan).ToListAsync();

			return manhattanRestaurants.Count;
		}

		public async Task<int> RestaurantsInManhattanLinq()
		{
			var collection = Database.GetCollection<BsonDocument>("restaurants");
			var filteredNone = new BsonDocument();
			var manhattanRestaurants = await collection.Find(filteredNone).ToListAsync();

			return manhattanRestaurants.Where(restaurant => restaurant.GetValue("borough").Equals("Manhattan"))
									   .ToList()
									   .Count;
		}

		public async Task<int> RestaurantsWithZipcode10075()
		{
			var collection = Database.GetCollection<BsonDocument>("restaurants");
			var filter = Builders<BsonDocument>.Filter.Eq("address.zipcode", "10075");
			var restaurants = await collection.Find(filter).ToListAsync();

			return restaurants.Count;
		}

		public async Task<int> RestaurantsWithZipcode10075Linq()
		{
			var collection = Database.GetCollection<BsonDocument>("restaurants");
			var filteredNone = new BsonDocument();
			var restaurants = await collection.Find(filteredNone).ToListAsync();

			return restaurants.Where(restaurant => restaurant.GetValue("address").AsBsonDocument.GetValue("zipcode").Equals("10075"))
							  .ToList()
							  .Count;
		}

		public async Task<int> RestaurantsWithAScoreGreaterThan30()
		{
			var collection = Database.GetCollection<BsonDocument>("restaurants");
			var filter = Builders<BsonDocument>.Filter.Gt("grades.score", 30);
			var restaurants = await collection.Find(filter).ToListAsync();

			return restaurants.Count;
		}
	}
}