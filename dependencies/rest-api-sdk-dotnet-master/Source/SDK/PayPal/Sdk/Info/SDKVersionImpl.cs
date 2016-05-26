using PayPal;

namespace PayPal
{
	public class SDKVersionImpl : SDKVersion
	{
		
		/// <summary>
		/// SDK ID used in User-Agent HTTP header
		/// </summary>
		private const string SdkId = "rest-sdk-dotnet";
		
		/// <summary>
		/// SDK Version used in User-Agent HTTP header
		/// </summary>
		private const string SdkVersion = "0.7.4";
		
		public string GetSDKId()
		{
			return SdkId;
		}
		
		public string GetSDKVersion()
		{
			return SdkVersion;
		}
	}

}


