using System.Threading.Tasks;
using FluentAssertions;
using MongoSpike;
using NUnit.Framework;


namespace MongoSpikeTest
{	
	[TestFixture]
	class MongoShould
	{
		private Mongo Mongo { get; set; }

		[SetUp]
		public void GivenAMongoDatabase()
		{
			Mongo = new Mongo();
		}

		[Test]
		public async Task have_25359_documents()
		{
			var totalRestaurantDocuments = await Mongo.TotalRestaurantDocuments();

			totalRestaurantDocuments.Should().Be(25359);
		}

		[Test]
		public async Task have_10259_restaurants_in_manhattan_via_driver_filter()
		{
			var restaurantsInManhattan = await Mongo.RestaurantsInManhattan();

			restaurantsInManhattan.Should().Be(10259);
		}

		[Test]
		public async Task have_10259_restaurants_in_manhattan_via_linq()
		{
			var restaurantsInManhattan = await Mongo.RestaurantsInManhattanLinq();

			restaurantsInManhattan.Should().Be(10259);
		}

		[Test]
		public async Task have_99_restaurants_with_zipcode_10075_via_driver_filter()
		{
			var restaurantsWithZipCode10075 = await Mongo.RestaurantsWithZipcode10075();

			restaurantsWithZipCode10075.Should().Be(99);
		}

		[Test]
		public async Task have_99_restaurants_with_zipcode_10075_via_linq()
		{
			var restaurantsWithZipcode10075 = await Mongo.RestaurantsWithZipcode10075Linq();

			restaurantsWithZipcode10075.Should().Be(99);
		}

		[Test]
		public async Task have_1959_restaurants_with_a_grade_score_greater_than_30()
		{
			var numberOfRestaurants = await Mongo.RestaurantsWithAScoreGreaterThan30();

			numberOfRestaurants.Should().Be(1959);
		}
	}
}
