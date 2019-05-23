using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HttpClientHelper
{
	interface IHttpClientHelper
	{
		dynamic InvokeDataServiceQuery(string queryString);
		dynamic InvokeDataServiceQuery(string queryString, ClientHelpers.RequestType type);

		string InvokeWebApiJson(Uri webApiUri, JObject jsonData);
		string InvokeWebApiJson(Uri webApiUri, System.Text.Encoding encoding, JObject jsonData);

		string InvokeWebApiJsonWebRequest(Uri webApiUri, JObject jsonData);

		string InvokeWebApiHttpVerb(WEBZEN.GCPS.ClientHelpers.ClientHelpers.HttpVerb httpVerb, Uri webApiUri);
		string InvokeWebApiHttpVerb(WEBZEN.GCPS.ClientHelpers.ClientHelpers.HttpVerb httpVerb, Uri webApiUri, System.Text.Encoding encoding);
		string InvokeWebApiHttpVerb(WEBZEN.GCPS.ClientHelpers.ClientHelpers.HttpVerb httpVerb, Uri webApiUri, System.Text.Encoding encoding, JObject jsonData);
	}
}
