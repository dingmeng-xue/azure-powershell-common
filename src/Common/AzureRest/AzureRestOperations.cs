﻿using Microsoft.Rest;
using Microsoft.Rest.Azure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.WindowsAzure.Commands.Common.AzureRest
{
    internal partial class AzureRestOperations : IServiceOperations<AzureRestClient>, IAzureRestOperations
    {

        /// <summary>
        /// Initializes a new instance of the AzureRestOperations class.
        /// </summary>
        /// <param name='client'>
        /// Reference to the service client.
        /// </param>
        /// <exception cref="System.ArgumentNullException">
        /// Thrown when a required parameter is null
        /// </exception>
        internal AzureRestOperations(AzureRestClient client)
        {
            if (client == null)
            {
                throw new System.ArgumentNullException("client");
            }
            Client = client;
        }

        /// <summary>
        /// Gets a reference to the AzureRestClient
        /// </summary>
        public AzureRestClient Client { get; private set; }

        public async Task<AzureOperationResponse<T>> BeginHttpMessagesAsync<T>(HttpMethod method, string path, Dictionary<string, List<string>> queries = null, string fragment = null, string content = null, Dictionary<string, List<string>> customHeaders = null, CancellationToken cancellationToken = default)
        {
            if (path == null)
            {
                throw new ValidationException(ValidationRules.CannotBeNull, "path");
            }
            if (method == null)
            {
                throw new ValidationException(ValidationRules.CannotBeNull, "method");
            }
            // Tracing
            bool _shouldTrace = ServiceClientTracing.IsEnabled;
            string _invocationId = null;
            if (_shouldTrace)
            {
                _invocationId = ServiceClientTracing.NextInvocationId.ToString();
                Dictionary<string, object> tracingParameters = new Dictionary<string, object>();
                tracingParameters.Add("cancellationToken", cancellationToken);
                ServiceClientTracing.Enter(_invocationId, this, "BeginDelete", tracingParameters);
            }
            // Construct URL
            var _baseUrl = Client.BaseUri.AbsoluteUri;
            var _url = new System.Uri(_baseUrl + path).ToString();
            foreach(string key in queries.Keys)
            {
                _url += (_url.Contains("?") ? "&" : "?") + key + "=" + string.Join(",", queries[key]);
            }
            if(!String.IsNullOrEmpty(fragment))
            {
                _url += "#" + fragment;
            }
            // Create HTTP transport objects
            var _httpRequest = new System.Net.Http.HttpRequestMessage();
            System.Net.Http.HttpResponseMessage _httpResponse = null;
            _httpRequest.Method = method;
            _httpRequest.RequestUri = new System.Uri(_url);
            // Set Headers
            if (Client.GenerateClientRequestId != null && Client.GenerateClientRequestId.Value)
            {
                _httpRequest.Headers.TryAddWithoutValidation("x-ms-client-request-id", System.Guid.NewGuid().ToString());
            }
            if (Client.AcceptLanguage != null)
            {
                if (_httpRequest.Headers.Contains("accept-language"))
                {
                    _httpRequest.Headers.Remove("accept-language");
                }
                _httpRequest.Headers.TryAddWithoutValidation("accept-language", Client.AcceptLanguage);
            }
            if (customHeaders != null)
            {
                foreach (var _header in customHeaders)
                {
                    if (_httpRequest.Headers.Contains(_header.Key))
                    {
                        _httpRequest.Headers.Remove(_header.Key);
                    }
                    _httpRequest.Headers.TryAddWithoutValidation(_header.Key, _header.Value);
                }
            }

            // Serialize Request
            string _requestContent = content;
            if (_requestContent != null)
            {
                _httpRequest.Content = new StringContent(_requestContent, System.Text.Encoding.UTF8);
                _httpRequest.Content.Headers.ContentType = System.Net.Http.Headers.MediaTypeHeaderValue.Parse("application/json; charset=utf-8");
            }
            // Set Credentials
            if (Client.Credentials != null)
            {
                cancellationToken.ThrowIfCancellationRequested();
                await Client.Credentials.ProcessHttpRequestAsync(_httpRequest, cancellationToken).ConfigureAwait(false);
            }
            // Send Request
            if (_shouldTrace)
            {
                ServiceClientTracing.SendRequest(_invocationId, _httpRequest);
            }
            cancellationToken.ThrowIfCancellationRequested();
            _httpResponse = await Client.HttpClient.SendAsync(_httpRequest, cancellationToken).ConfigureAwait(false);
            if (_shouldTrace)
            {
                ServiceClientTracing.ReceiveResponse(_invocationId, _httpResponse);
            }
            HttpStatusCode _statusCode = _httpResponse.StatusCode;
            cancellationToken.ThrowIfCancellationRequested();
            string _responseContent = null;
            if ((int)_statusCode >= 300 && (int)_statusCode < 400)
            {
                var ex = new NotImplementedException(string.Format("Redirection status code '{0}' is not supported", _statusCode));
                throw ex;
            }
            else if ((int)_statusCode >= 400 && (int)_statusCode < 500)
            {
                var ex = new CloudException(string.Format("Operation returned an invalid status code '{0}'", _statusCode));
                try
                {
                    _responseContent = await _httpResponse.Content.ReadAsStringAsync().ConfigureAwait(false);
                    CloudError _errorBody = Rest.Serialization.SafeJsonConvert.DeserializeObject<CloudError>(_responseContent, Client.DeserializationSettings);
                    if (_errorBody != null)
                    {
                        ex = new CloudException(_errorBody.Message);
                        ex.Body = _errorBody;
                    }
                }
                catch (Newtonsoft.Json.JsonException)
                {
                    // Ignore the exception
                }
                ex.Request = new HttpRequestMessageWrapper(_httpRequest, _requestContent);
                ex.Response = new HttpResponseMessageWrapper(_httpResponse, _responseContent);
                if (_httpResponse.Headers.Contains("x-ms-request-id"))
                {
                    ex.RequestId = _httpResponse.Headers.GetValues("x-ms-request-id").FirstOrDefault();
                }
                if (_shouldTrace)
                {
                    ServiceClientTracing.Error(_invocationId, ex);
                }
                _httpRequest.Dispose();
                if (_httpResponse != null)
                {
                    _httpResponse.Dispose();
                }
                throw ex;
            }
            else if ((int)_statusCode >= 500 && (int)_statusCode < 600)
            {
                var ex = new CloudException(string.Format("Server Error with status code '{0}'", _statusCode));
                throw ex;
            }
            else if ((int)_statusCode < 200 && (int)_statusCode > 600)
            {
                var ex = new CloudException(string.Format("Unknow status code '{0}'", _statusCode));
                throw ex;
            }

            // Create Result
            var _result = new AzureOperationResponse<T>();
            _result.Request = _httpRequest;
            _result.Response = _httpResponse;
            if (_httpResponse.Headers.Contains("x-ms-request-id"))
            {
                _result.RequestId = _httpResponse.Headers.GetValues("x-ms-request-id").FirstOrDefault();
            }
            // Deserialize Response
            if ((int)_statusCode == 200)
            {
                _responseContent = await _httpResponse.Content.ReadAsStringAsync().ConfigureAwait(false);
                try
                {
                    _result.Body = Rest.Serialization.SafeJsonConvert.DeserializeObject<T>(_responseContent, Client.DeserializationSettings);
                }
                catch (Newtonsoft.Json.JsonException ex)
                {
                    _httpRequest.Dispose();
                    if (_httpResponse != null)
                    {
                        _httpResponse.Dispose();
                    }
                    throw new SerializationException("Unable to deserialize the response.", _responseContent, ex);
                }
            }

            if (_shouldTrace)
            {
                ServiceClientTracing.Exit(_invocationId, _result);
            }
            return _result;
        }

        public async Task<T> GetByResouceIdAsync<T>(string resourceId, string apiVersion, Dictionary<string, List<string>> customHeaders = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            Dictionary<string, List<string>> queries = new Dictionary<string, List<string>>();
            queries.Add("api-version", new List<string> { apiVersion });
            using (var _result = await BeginHttpMessagesAsync<T>(method: HttpMethod.Get,
                                                                path: resourceId,
                                                                queries: queries,
                                                                fragment: null,
                                                                content: null,
                                                                customHeaders: null,
                                                                cancellationToken: cancellationToken).ConfigureAwait(false))
            {
                return _result.Body;
            }
        }

        public T GetByResouceId<T>(string resourceId, string apiVersion, Dictionary<string, List<string>> customHeaders = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            return GetByResouceIdAsync<T>(resourceId, apiVersion, customHeaders, cancellationToken).GetAwaiter().GetResult();
        }

        public async Task<List<T>> GetResouceListAsync<T>(string resourceUri, string apiVersion, Dictionary<string, List<string>> customHeaders = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            Dictionary<string, List<string>> queries = new Dictionary<string, List<string>>();
            queries.Add("api-version", new List<string> { apiVersion });
            using (var _result = await BeginHttpMessagesAsync<List<T>>(method: HttpMethod.Get,
                                                                path: resourceUri,
                                                                queries: queries,
                                                                fragment: null,
                                                                content: null,
                                                                customHeaders: null,
                                                                cancellationToken: cancellationToken).ConfigureAwait(false))
            {
                return _result.Body;
            }
        }

        public List<T> GetResouceList<T>(string resourceUri, string apiVersion, Dictionary<string, List<string>> customHeaders = null, CancellationToken cancellationToken = default(CancellationToken))
        { 
            return GetResouceListAsync<T>(resourceUri, apiVersion, customHeaders, cancellationToken).GetAwaiter().GetResult();
        }

        public async Task<IPage<T>> GetResoucePageAsync<T>(string resourceUri, string apiVersion, Dictionary<string, List<string>> customHeaders = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            Dictionary<string, List<string>> queries = new Dictionary<string, List<string>>();
            queries.Add("api-version", new List<string> { apiVersion });
            using (var _result = await BeginHttpMessagesAsync<IPage<T>>(method: HttpMethod.Get,
                                                                path: resourceUri,
                                                                queries: queries,
                                                                fragment: null,
                                                                content: null,
                                                                customHeaders: null,
                                                                cancellationToken: cancellationToken).ConfigureAwait(false))
            {
                return _result.Body;
            }
        }

        public IPage<T> GetResoucePage<T>(string resourceUri, string apiVersion, Dictionary<string, List<string>> customHeaders = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            return GetResoucePageAsync<T>(resourceUri, apiVersion, customHeaders, cancellationToken).GetAwaiter().GetResult();
        }

    }
}
