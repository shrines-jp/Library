using System;
using System.ComponentModel;
using System.Data.Services.Client;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace HttpClientHelper
{
	/// <summary>
	/// HttpClientHelper
	/// 2013.05.27 박정환
	/// 클라이언트 헬퍼 클래스
	/// 
	/// .NetFramework 4.0 이상에서 동작합니다. 
	/// 
	/// - Microsoft.Data.Services.Client.dll 5.4.0.0
	/// - Microsoft.Data.OData.dll 5.4.0.0
	/// - Microsoft.Data.Edm.dll 5.4.0.0
	/// - Newtonsoft.Json.dll 4.5.0.0
	/// - System.Net.Http 2.0.0.0
	/// - System.Net.Http.Formatting 4.0.0.0
	/// - System.Net.Http.WebRequest 2.0.0.0
	/// - System.Spatial 5.4.0.0
	/// </summary>
    public class HttpClientHelper
    {
		#region Properties
		private readonly String CONTENT_TYPE = "application/json";
		private readonly Encoding ENCODE_TYPE = System.Text.Encoding.UTF8;
		private DataServiceContext context;
		private Uri serviceUri;
		private HttpVerb httpVerb;
		private String contentType;
		private JObject postData;
		#endregion


		#region Enum Properties
		/// <summary>
		/// RequestType
		/// Http Request Type
		/// </summary>
		public enum RequestType
		{
			[Description("ATOM")]
			ATOM = 0,

			[Description("JSON")]
			JSON = 1,
		}


		/// <summary>
		/// RESTFul HttpVerb
		/// </summary>
		public enum HttpVerb
		{
			[Description("GET")]
			GET,

			[Description("POST")]
			POST,

			[Description("PUT")]
			PUT,

			[Description("DELETE")]
			DELETE
		}
		#endregion


		#region Custructor
		/// <summary>
		/// ClientHelpers
		/// </summary>
		public HttpClientHelper()
		{
			this.httpVerb = HttpVerb.POST;
			this.contentType = CONTENT_TYPE;
		}

		/// <summary>
		/// GCPSClient
		/// </summary>
		/// <param name="uri">DataServiceContext Service Uri</param>
		public HttpClientHelper(Uri uri)
		{
			this.httpVerb = HttpVerb.POST;
			this.contentType = CONTENT_TYPE;
			this.serviceUri = uri;
			this.context = new DataServiceContext(this.serviceUri);
		}

		/// <summary>
		/// ClientHelpers
		/// </summary>
		/// <param name="uri">DataService Uri</param>
		/// <param name="method">HttpVerb : GET, PUT, POST, DELETE</param>
		public HttpClientHelper(Uri uri, HttpVerb method)
		{
			this.httpVerb = method;
			this.contentType = CONTENT_TYPE;
			this.serviceUri = uri;
			this.context = new DataServiceContext(this.serviceUri);
		}

		/// <summary>
		/// ClientHelpers
		/// </summary>
		/// <param name="uri">DataService Uri</param>
		/// <param name="method">HttpVerb : GET, PUT, POST, DELETE</param>
		/// <param name="postData">JsonObject Data</param>
		public HttpClientHelper(Uri uri, HttpVerb method, JObject postData)
		{
			this.httpVerb = method;
			this.contentType = CONTENT_TYPE;
			this.serviceUri = uri;
			this.postData = postData;
			this.context = new DataServiceContext(this.serviceUri);
		}
		#endregion


		#region OData Helper Method
		/// <summary>
		/// InvokeDataServiceQuery
		/// </summary>
		/// <param name="queryString">OData QueryString</param>
		/// <returns>dynamic</returns>
		public dynamic InvokeDataServiceQuery(String queryString)
		{
			return InvokeDataServiceQuery(queryString, RequestType.ATOM);
		}


		/// <summary>
		/// InvokeDataServiceQuery
		/// </summary>
		/// <param name="queryString">OData QueryString</param>
		/// <param name="type">RequestType : Atom, Json</param>
		/// <returns>dynamic</returns>
		public dynamic InvokeDataServiceQuery(String queryString, RequestType type)
		{
			try
			{
				if (type == RequestType.JSON && queryString.Contains("json"))
				{
					throw new DataServiceClientException("Wrong Query!! could not use both '$format=json' and 'RequestType.JSON'");
				}

				//if (queryString.IndexOf('?') < 0)
				//{
				//	queryString = queryString.Insert(0, "?");
				//}

				var result = context.Execute<dynamic>(new Uri(queryString, UriKind.Relative));

				if (type == RequestType.JSON)
				{
					///JSon SerializeObject String타입으로 변경
					String jsonResult = JsonConvert.SerializeObject(result);
					return jsonResult;
				}
				else
				{
					return result;
				}
			}
			catch (System.Data.Services.Client.DataServiceQueryException ex)
			{
				QueryOperationResponse response = ex.Response;

				string message = response.Error.Message;
				int statusCode = response.StatusCode;

				throw;
			}
			catch (Exception)
			{
				throw;
			}
		}
		#endregion


		#region WEB API Helper Method
		/// <summary>
		/// InvokeWebApiJson
		/// 
		/// System.Net.Http
		/// System.Net.Http.Formatting
		/// </summary>
		/// <param name="webApiUri">API URL</param>
		/// <param name="jsonData">JSON Objects</param>
		/// <returns>Result String</returns>
		public String InvokeWebApiJson(Uri webApiUri, JObject jsonData)
		{
			return this.InvokeWebApiJson(webApiUri, System.Text.Encoding.UTF8, jsonData);
		}


		/// <summary>
		/// InvokeWebApiJson
		/// 
		/// System.Net.Http
		/// System.Net.Http.Formatting
		/// </summary>
		/// <param name="webApiUri">API URL</param>
		/// <param name="encoding">System.Text.Encoding</param>
		/// <param name="jsonData">JSON Objects</param>
		/// <returns>Result String</returns>
		public String InvokeWebApiJson(Uri webApiUri, System.Text.Encoding encoding, JObject jsonData)
		{
			try
			{
				string serializedJson = SerializeJsonObject(jsonData);

				if (String.IsNullOrWhiteSpace(serializedJson)) return String.Empty;

				using (var client = new HttpClient())
				{
					HttpResponseMessage responseMessage;

					client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(CONTENT_TYPE));

					responseMessage = client.PostAsync(webApiUri.ToString(), new StringContent(serializedJson, System.Text.Encoding.UTF8, CONTENT_TYPE)).Result;
					responseMessage.EnsureSuccessStatusCode();

					return responseMessage.Content.ReadAsStringAsync().Result;
				}
			}
			catch (Exception)
			{
				throw;
			}
		}


		/// <summary>
		/// InvokeWebApiJsonWebRequest
		/// HttpClient 가 아닌, HttpWebRequest를 이용하여 API호출
		/// </summary>
		/// <param name="webApiUri">WebApi Service Uri</param>
		/// <param name="jsonData">Json Object</param>
		/// <returns>Result String</returns>
		public String InvokeWebApiJsonWebRequest(Uri webApiUri, JObject jsonData)
		{
			var request = (HttpWebRequest)WebRequest.Create(webApiUri);

			request.Method = this.httpVerb.ToString();
			request.ContentLength = 0;
			request.ContentType = this.contentType;

			if (!string.IsNullOrEmpty(JsonConvert.SerializeObject(jsonData)) && this.httpVerb == HttpVerb.POST)
			{
				var encoding = new UTF8Encoding();
				var bytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(jsonData));
				request.ContentLength = bytes.Length;

				using (var writeStream = request.GetRequestStream())
				{
					writeStream.Write(bytes, 0, bytes.Length);
				}
			}

			using (var response = (HttpWebResponse)request.GetResponse())
			{
				var responseValue = string.Empty;

				if (response.StatusCode != HttpStatusCode.OK)
				{
					var message = String.Format("Request failed. Received HTTP {0}", response.StatusCode);
					throw new ApplicationException(message);
				}

				// grab the response
				using (var responseStream = response.GetResponseStream())
				{
					if (responseStream != null)
						using (var reader = new StreamReader(responseStream))
						{
							responseValue = reader.ReadToEnd();
						}
				}

				return responseValue;
			}
		}


		/// <summary>
		/// InvokeWebApiHttpVerb
		/// </summary>
		/// <param name="httpVerb">HttpVerb : GET, POST, PUT, DELETE</param>
		/// <param name="webApiUri">WebApi Service Uri</param>
		/// <returns>Result String</returns>
		public String InvokeWebApiHttpVerb(HttpVerb httpVerb, Uri webApiUri)
		{
			return InvokeWebApiHttpVerb(httpVerb, webApiUri, ENCODE_TYPE, null);
		}

		/// <summary>
		/// InvokeWebApiHttpVerb
		/// </summary>
		/// <param name="httpVerb">HttpVerb : GET, POST, PUT, DELETE</param>
		/// <param name="webApiUri">WebApi Service Uri</param>
		/// <param name="encoding"></param>
		/// <returns>Result String</returns>
		public String InvokeWebApiHttpVerb(HttpVerb httpVerb, Uri webApiUri, Encoding encoding)
		{
			return InvokeWebApiHttpVerb(httpVerb, webApiUri, encoding, null);
		}

		/// <summary>
		/// InvokeWebApiHttpVerb
		/// </summary>
		/// <param name="httpVerb">HttpVerb : GET, POST, PUT, DELETE</param>
		/// <param name="webApiUri">WebApi Service Uri</param>
		/// <param name="encoding">Content Encoding Type</param>
		/// <param name="jsonData">Json Object</param>
		/// <returns>Result String</returns>
		public String InvokeWebApiHttpVerb(HttpVerb httpVerb, Uri webApiUri, Encoding encoding, JObject jsonData)
		{
			string serializedJson = SerializeJsonObject(jsonData);

			if (String.IsNullOrWhiteSpace(serializedJson)) return String.Empty;

			try
			{
				using (var client = new HttpClient())
				{
					client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(CONTENT_TYPE));
					StringContent content = new StringContent(serializedJson, encoding, CONTENT_TYPE);

					HttpResponseMessage responseMessage;

					switch (httpVerb)
					{
						case HttpVerb.GET:
							responseMessage = client.GetAsync(webApiUri.ToString()).Result;
							responseMessage.EnsureSuccessStatusCode();
							return responseMessage.Content.ReadAsStringAsync().Result;

						case HttpVerb.POST:

							responseMessage = client.PostAsync(webApiUri.ToString(), content).Result;
							responseMessage.EnsureSuccessStatusCode();
							return responseMessage.Content.ReadAsStringAsync().Result;

						case HttpVerb.PUT:
							responseMessage = client.PutAsync(webApiUri.ToString(), content).Result;
							responseMessage.EnsureSuccessStatusCode();
							return responseMessage.Content.ReadAsStringAsync().Result;

						case HttpVerb.DELETE:
							responseMessage = client.DeleteAsync(webApiUri.ToString()).Result;
							responseMessage.EnsureSuccessStatusCode();
							return responseMessage.Content.ReadAsStringAsync().Result;

						default:
							return String.Empty;
					}
				}
			}
			catch (Exception)
			{
				throw;
			}
		}

		#endregion


		#region JSON Serialize, Deserialize Method

		/// <summary>
		/// SerializeJsonObject
		/// 시리얼라이즈
		/// </summary>
		/// <param name="jsonObject">Json형식의 데이터</param>
		/// <returns>Selialized된 Json String</returns>
		public static string SerializeJsonObject(JObject jsonObject)
		{
			if (jsonObject == null) return String.Empty;

			return JsonConvert.SerializeObject(jsonObject, Formatting.Indented, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore } );
		}

		/// <summary>
		/// DeserializeJsonObject
		/// 디시리얼라이즈
		/// </summary>
		/// <param name="serializedJsonString">Selialized된 Json String</param>
		/// <returns>Json형식의 데이터</returns>
		public static JObject DeserializeJsonObject(String serializedJsonString)
		{
			if (String.IsNullOrWhiteSpace(serializedJsonString)) return null;

			return (JObject)JsonConvert.DeserializeObject(serializedJsonString);
		}

		#endregion


		#region Convert PlainText To SecureString Method
		/// <summary>
		/// convertToSecureString
		/// </summary>
		/// <param name="strPassword">plainText</param>
		/// <returns>SecureString</returns>
		public SecureString convertToSecureString(string strPassword)
		{
			var secureStr = new SecureString();
			if (strPassword.Length > 0)
			{
				foreach (var c in strPassword.ToCharArray()) secureStr.AppendChar(c);
			}
			return secureStr;
		}
		#endregion
    }
}
