using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static HttpClientHelper.HttpClientHelper;

namespace HttpClientHelper
{
	interface IHttpClientHelper
	{
		dynamic InvokeDataServiceQuery(string queryString);
		dynamic InvokeDataServiceQuery(string queryString, RequestType type);

		string InvokeWebApiJson(Uri webApiUri, JObject jsonData);
		string InvokeWebApiJson(Uri webApiUri, System.Text.Encoding encoding, JObject jsonData);

		string InvokeWebApiJsonWebRequest(Uri webApiUri, JObject jsonData);

		string InvokeWebApiHttpVerb(HttpVerb httpVerb, Uri webApiUri);
        string InvokeWebApiHttpVerb(HttpVerb httpVerb, Uri webApiUri, System.Text.Encoding encoding);
        string InvokeWebApiHttpVerb(HttpVerb httpVerb, Uri webApiUri, Encoding encoding, JObject jsonData);
	}
}
