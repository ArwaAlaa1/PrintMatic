using PrintMatic.Core.Entities;
using StackExchange.Redis;
using System.Text.Json;
using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace PrintMatic.Helper
{
	public class HandleExpirationCart
	{
		private readonly ConnectionMultiplexer _redis;
		private readonly IWebHostEnvironment _env;

		public HandleExpirationCart(string connectionString, IWebHostEnvironment env)
		{
			_redis = ConnectionMultiplexer.Connect(connectionString);
			_env = env; // Store the environment object to access the root paths
			SubscribeToExpiredBaskets();
		}

		private void SubscribeToExpiredBaskets()
		{
			var subscriber = _redis.GetSubscriber();
			subscriber.Subscribe("__keyevent@0__:expired", (channel, value) =>
			{
				string basketId = value.ToString();
				HandleExpiredBasket(basketId);
			});
		}

		private void HandleExpiredBasket(string basketId)
		{
			try
			{
				DeletePhotoPaths(basketId);
			}
			catch (Exception ex)
			{
				// Log the error (you can replace Console with actual logging)
				Console.WriteLine($"Error handling expired basket {basketId}: {ex.Message}");
			}
		}

		private void DeletePhotoPaths(string basketId)
		{
			List<string> photoPaths = GetPhotoPaths(basketId);

			foreach (var photoPath in photoPaths)
			{
				try
				{
					if (File.Exists(photoPath))
					{
						File.Delete(photoPath); // Delete the photo file
						Console.WriteLine($"Deleted file: {photoPath}");
					}
				}
				catch (Exception ex)
				{
					// Log the error (you can replace Console with actual logging)
					Console.WriteLine($"Error deleting file {photoPath}: {ex.Message}");
				}
			}
		}

		private List<string> GetPhotoPaths(string basketId)
		{
			// Get the full basket key (with prefix)
			string redisKey = "basket:" + basketId;

			// Retrieve the basket data from Redis
			var db = _redis.GetDatabase();
			string basketData = db.StringGet(redisKey);

			if (string.IsNullOrEmpty(basketData))
			{
				return new List<string>(); // Return empty if no basket is found
			}

			// Deserialize the basket data (assuming basket is stored as JSON)
			var customerCart = JsonSerializer.Deserialize<CustomerCart>(basketData);

			// Extract and return photo paths from the cart items
			List<string> photoPaths = new List<string>();
			if (customerCart?.Items != null)
			{
				foreach (var item in customerCart.Items)
				{
					if (!string.IsNullOrEmpty(item.ImageUrl))
					{
						// Get the full path using the web root path (wwwroot)
						string fullImagePath = Path.Combine(_env.WebRootPath, "Custome", "Image", item.ImageUrl);
						photoPaths.Add(fullImagePath);
					}
				}
			}

			return photoPaths;
		}
	}
}
