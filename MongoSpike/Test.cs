using MongoDB.Bson;
using MongoDB.Driver;
using System;


namespace MongoSpike
{
	public class Test
	{
		protected static IMongoClient Client { get; set; }
		protected static IMongoDatabase Database { get; set; }

		public Test()
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
	}
}