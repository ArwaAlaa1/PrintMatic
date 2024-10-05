using Microsoft.Extensions.Caching.Memory;

namespace PrintMatic.Helper
{
	public  class CodeVerificationService
	{
		private readonly IMemoryCache _cache;

		public CodeVerificationService(IMemoryCache cache)
        {
			_cache = cache;
		}
		public async Task SaveVerificationCodeAsync(string userId, string verificationCode)
		{
		
			string cacheKey = $"Verification_{userId}";

			
			var cacheEntryOptions = new MemoryCacheEntryOptions()
				.SetAbsoluteExpiration(TimeSpan.FromMinutes(10)); 

			
			_cache.Set(cacheKey, verificationCode, cacheEntryOptions);
		}

		public async Task<string> GetVerificationCode(string userId)
		{
			string cacheKey = $"Verification_{userId}";
			_cache.TryGetValue(cacheKey, out string verificationCode);
			return verificationCode;
		}
	}
}
