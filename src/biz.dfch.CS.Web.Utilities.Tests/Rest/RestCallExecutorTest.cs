/**
 * Copyright 2015 d-fens GmbH
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 * http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using System;
using System.Collections.Generic;
using biz.dfch.CS.Web.Utilities.Rest;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net.Http;
using System.Net.Http.Headers;
using biz.dfch.CS.Utilities.Attributes;
using Telerik.JustMock;
using HttpMethod = biz.dfch.CS.Web.Utilities.Rest.HttpMethod;

namespace biz.dfch.CS.Web.Utilities.Tests.Rest
{
    [TestClass]
    public class RestCallExecutorTest
    {
        private const string URI = "http://test/api/entities";
        private const string CONTENT_TYPE_KEY = "Content-Type";
        private const string CONTENT_TYPE_VALUE = "application/arb.itrary+json;version=1.0";
        private const string USER_AGENT_KEY = "User-Agent";
        private const string AUTHORIZATION_HEADER_KEY = "Authorization";
        private const string ACCEPT_HEADER_KEY = "Accept";
        private const string ACCEPT_HEADER_VALUE = "application/arb.itrary+json;version=1.0";
        private const string TEST_USER_AGENT = "test-agent";
        private const string SAMPLE_REQUEST_BODY = "{\"Property\":\"value\"}";
        private const string SAMPLE_RESPONSE_BODY = "{\"Property2\":\"value2\"}";
        private const string BEARER_AUTH_SCHEME = "Bearer";
        private const string SAMPLE_BEARER_TOKEN = "AbCdEf123456";

        private HttpClient HttpClient;

        [TestInitialize]
        public void TestInitilize()
        {
            HttpClient = Mock.Create<HttpClient>();
        }

        [TestMethod]
        public void RestCallExecutorConstructorSetsProperties()
        {
            // Arrange
            
            // Act
            var restCallExecutor = new RestCallExecutor();
            
            // Assert
            Assert.AreEqual(true, restCallExecutor.EnsureSuccessStatusCode);
            Assert.AreEqual(ContentType.ApplicationJson, restCallExecutor.ContentType);
            Assert.AreEqual(90, restCallExecutor.Timeout);
        }

        [TestMethod]
        public void RestCallExecutorConstructorSetsEnsureSuccessCodePropertyAccordingConstructorParameter()
        {
            // Arrange

            // Act
            var restCallExecutor = new RestCallExecutor(false);

            // Assert
            Assert.AreEqual(false, restCallExecutor.EnsureSuccessStatusCode);
            Assert.AreEqual(ContentType.ApplicationJson, restCallExecutor.ContentType);
            Assert.AreEqual(90, restCallExecutor.Timeout);
        }

        [TestMethod]
        [ExpectedException(typeof(UriFormatException))]
        public void InvokeWithInvalidUriThrowsUriFormatException1()
        {
            // Arrange
            var invalidUri = "abc";
            RestCallExecutor restCallExecutor = new RestCallExecutor();
            
            // Act
            restCallExecutor.Invoke(invalidUri);

            // Assert
        }

        [TestMethod]
        [ExpectedException(typeof(UriFormatException))]
        public void InvokeWithInvalidUriThrowsUriFormatException2()
        {
            // Arrange
            var invalidUri = "abc";
            RestCallExecutor restCallExecutor = new RestCallExecutor();
            
            // Act
            restCallExecutor.Invoke(invalidUri, null);

            // Assert
        }

        [TestMethod]
        [ExpectedException(typeof(UriFormatException))]
        public void InvokeWithInvalidUriThrowsUriFormatException3()
        {
            // Arrange
            var invalidUri = "abc";
            RestCallExecutor restCallExecutor = new RestCallExecutor();

            // Act
            restCallExecutor.Invoke(HttpMethod.Head, invalidUri, null, null);

            // Assert
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void InvokeWithNullUriThrowsArgumentException1()
        {
            // Arrange
            string nullUri = null;
            RestCallExecutor restCallExecutor = new RestCallExecutor();

            // Act
            restCallExecutor.Invoke(nullUri);

            // Assert
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void InvokeWithNullUriThrowsArgumentException2()
        {
            // Arrange
            string nullUri = null;
            RestCallExecutor restCallExecutor = new RestCallExecutor();
            
            // Act
            restCallExecutor.Invoke(nullUri, null);

            // Assert
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void InvokeWithNullUriThrowsArgumentException3()
        {
            // Arrange
            string nullUri = null;
            RestCallExecutor restCallExecutor = new RestCallExecutor();
            
            // Act
            restCallExecutor.Invoke(HttpMethod.Head, nullUri, null, null);

            // Assert
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void InvokeWithWhitespaceUriThrowsArgumentException1()
        {
            // Arrange
            var whitespaceUri = " ";
            RestCallExecutor restCallExecutor = new RestCallExecutor();
            
            // Act
            restCallExecutor.Invoke(whitespaceUri);

            // Assert
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void InvokeWithWhitespaceUriThrowsArgumentException2()
        {
            // Arrange
            var whitespaceUri = " ";
            RestCallExecutor restCallExecutor = new RestCallExecutor();
            
            // Act
            restCallExecutor.Invoke(whitespaceUri, null);

            // Assert
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void InvokeWithWhitespaceUriThrowsArgumentException3()
        {
            // Arrange
            var whitespaceUri = " ";
            RestCallExecutor restCallExecutor = new RestCallExecutor();

            // Act
            restCallExecutor.Invoke(HttpMethod.Head, whitespaceUri, null, null);

            // Assert
        }

        [TestMethod]
        public void InvokeGetExecutesGetRequestOnUriWithProvidedHeadersEnsuresSuccessStatusCodeAndReturnsResponseContent()
        {
            // Arrange
            var mockedResponseMessage = Mock.Create<HttpResponseMessage>();
            var mockedRequestHeaders = Mock.Create<HttpRequestHeaders>();

            Mock.Arrange(() => HttpClient.DefaultRequestHeaders)
                .IgnoreInstance()
                .Returns(mockedRequestHeaders)
                .Occurs(3);

            Mock.Arrange(() => mockedRequestHeaders.Add(USER_AGENT_KEY, TEST_USER_AGENT))
                .OccursOnce();

            Mock.Arrange(() => mockedRequestHeaders.TryAddWithoutValidation(ACCEPT_HEADER_KEY, ACCEPT_HEADER_VALUE))
                .OccursOnce();

            Mock.Arrange(() => HttpClient.GetAsync(Arg.Is(new Uri(URI))).Result)
                .IgnoreInstance()
                .Returns(mockedResponseMessage)
                .OccursOnce();

            Mock.Arrange(() => mockedResponseMessage.EnsureSuccessStatusCode())
                .OccursOnce();

            Mock.Arrange(() => mockedResponseMessage.Content.ReadAsStringAsync().Result)
                .Returns(SAMPLE_RESPONSE_BODY)
                .OccursOnce();

            RestCallExecutor restCallExecutor = new RestCallExecutor();

            // Act
            var result = restCallExecutor.Invoke(URI, CreateSampleHeaders());

            // Assert
            Assert.AreEqual(SAMPLE_RESPONSE_BODY, result);

            Mock.Assert(HttpClient);
            Mock.Assert(mockedRequestHeaders);
            Mock.Assert(mockedResponseMessage);
        }

        [TestMethod]
        public void InvokeGetExecutesGetRequestOnUriWithProvidedHeadersNotEnsuringSuccessStatusCodeReturnsResponseContent()
        {
            // Arrange
            var mockedResponseMessage = Mock.Create<HttpResponseMessage>();
            var mockedRequestHeaders = Mock.Create<HttpRequestHeaders>();

            Mock.Arrange(() => HttpClient.DefaultRequestHeaders)
                .IgnoreInstance()
                .Returns(mockedRequestHeaders)
                .Occurs(3);

            Mock.Arrange(() => mockedRequestHeaders.Add(USER_AGENT_KEY, TEST_USER_AGENT))
                .OccursOnce();

            Mock.Arrange(() => mockedRequestHeaders.TryAddWithoutValidation(ACCEPT_HEADER_KEY, ACCEPT_HEADER_VALUE))
                .OccursOnce();

            Mock.Arrange(() => HttpClient.GetAsync(Arg.Is(new Uri(URI))).Result)
                .IgnoreInstance()
                .Returns(mockedResponseMessage)
                .OccursOnce();

            Mock.Arrange(() => mockedResponseMessage.Content.ReadAsStringAsync().Result)
                .Returns(SAMPLE_RESPONSE_BODY)
                .OccursOnce();

            RestCallExecutor restCallExecutor = new RestCallExecutor(false);
            
            // Act
            var result = restCallExecutor.Invoke(URI, CreateSampleHeaders());
            Assert.AreEqual(SAMPLE_RESPONSE_BODY, result);

            // Assert
            Mock.Assert(HttpClient);
            Mock.Assert(mockedRequestHeaders);
            Mock.Assert(mockedResponseMessage);
        }

        [TestMethod]
        [ExpectedException(typeof(HttpRequestException))]
        public void InvokeExecutesThrowsHttpRequestExceptionIfEnsureSuccessStatusCodeFails()
        {
            // Arrange
            var mockedResponseMessage = Mock.Create<HttpResponseMessage>();
            var mockedRequestHeaders = Mock.Create<HttpRequestHeaders>();

            Mock.Arrange(() => HttpClient.DefaultRequestHeaders)
                .IgnoreInstance()
                .Returns(mockedRequestHeaders)
                .Occurs(3);

            Mock.Arrange(() => mockedRequestHeaders.Add(USER_AGENT_KEY, TEST_USER_AGENT))
                .OccursOnce();

            Mock.Arrange(() => mockedRequestHeaders.TryAddWithoutValidation(ACCEPT_HEADER_KEY, ACCEPT_HEADER_VALUE))
                .OccursOnce();

            Mock.Arrange(() => mockedRequestHeaders.Add(CONTENT_TYPE_KEY, ContentType.ApplicationJson.GetStringValue()))
                .OccursOnce();

            Mock.Arrange(() => HttpClient.GetAsync(Arg.Is(new Uri(URI))).Result)
                .IgnoreInstance()
                .Returns(mockedResponseMessage)
                .OccursOnce();

            Mock.Arrange(() => mockedResponseMessage.EnsureSuccessStatusCode())
                .Throws<HttpRequestException>()
                .OccursOnce();

            RestCallExecutor restCallExecutor = new RestCallExecutor();
            
            // Act
            restCallExecutor.Invoke(URI, CreateSampleHeaders());

            // Assert
            Mock.Assert(HttpClient);
            Mock.Assert(mockedRequestHeaders);
            Mock.Assert(mockedResponseMessage);
        }

        [TestMethod]
        public void InvokeSetsDefaultUserAgentHeaderIfNoCustomUserAgentHeaderProvided()
        {
            // Arrange
            var mockedResponseMessage = Mock.Create<HttpResponseMessage>();
            var mockedRequestHeaders = Mock.Create<HttpRequestHeaders>();

            Mock.Arrange(() => HttpClient.DefaultRequestHeaders)
                .IgnoreInstance()
                .Returns(mockedRequestHeaders)
                .Occurs(2);

            Mock.Arrange(() => mockedRequestHeaders.Add(USER_AGENT_KEY, "RestCallExecutor"))
                .OccursOnce();

            Mock.Arrange(() => HttpClient.GetAsync(Arg.Is(new Uri(URI))).Result)
                .IgnoreInstance()
                .Returns(mockedResponseMessage)
                .OccursOnce();

            Mock.Arrange(() => mockedResponseMessage.EnsureSuccessStatusCode())
                .OccursOnce();

            Mock.Arrange(() => mockedResponseMessage.Content.ReadAsStringAsync().Result)
                .Returns(SAMPLE_RESPONSE_BODY)
                .OccursOnce();

            RestCallExecutor restCallExecutor = new RestCallExecutor();
            var result = restCallExecutor.Invoke(URI);

            // Act
            Assert.AreEqual(SAMPLE_RESPONSE_BODY, result);

            // Assert
            Mock.Assert(HttpClient);
            Mock.Assert(mockedRequestHeaders);
            Mock.Assert(mockedResponseMessage);
        }

        [TestMethod]
        public void InvokePostOverwritesDefaultContentTypeHeaderIfCustomContentTypeHeaderProvided()
        {
            // Arrange
            var mockedResponseMessage = Mock.Create<HttpResponseMessage>();
            var mockedRequestHeaders = Mock.Create<HttpRequestHeaders>();

            Mock.Arrange(() => HttpClient.DefaultRequestHeaders)
                .IgnoreInstance()
                .Returns(mockedRequestHeaders)
                .Occurs(4);

            Mock.Arrange(() => mockedRequestHeaders.Add(USER_AGENT_KEY, TEST_USER_AGENT))
                .OccursOnce();

            Mock.Arrange(() => mockedRequestHeaders.TryAddWithoutValidation(ACCEPT_HEADER_KEY, CONTENT_TYPE_VALUE))
                .OccursOnce();

            var content = new StringContent(SAMPLE_REQUEST_BODY, null, CONTENT_TYPE_VALUE);
            Mock.Arrange(() => HttpClient.PostAsync(Arg.Is(new Uri(URI)), Arg.Is(content)).Result)
                .IgnoreInstance()
                .Returns(mockedResponseMessage)
                .OccursOnce();

            Mock.Arrange(() => mockedResponseMessage.EnsureSuccessStatusCode())
                .OccursOnce();

            Mock.Arrange(() => mockedResponseMessage.Content.ReadAsStringAsync().Result)
                .Returns(SAMPLE_RESPONSE_BODY)
                .OccursOnce();

            RestCallExecutor restCallExecutor = new RestCallExecutor();
            var headers = CreateSampleHeaders();
            headers.Add(CONTENT_TYPE_KEY, CONTENT_TYPE_VALUE);

            // Act
            var result = restCallExecutor.Invoke(HttpMethod.Post, URI, headers, SAMPLE_RESPONSE_BODY);

            // Assert
            Assert.AreEqual(SAMPLE_RESPONSE_BODY, result);

            Mock.Assert(HttpClient);
            Mock.Assert(mockedRequestHeaders);
            Mock.Assert(mockedResponseMessage);
        }

        [TestMethod]
        public void InvokeHeadExecutesHeadRequestOnUriWithProvidedHeadersEnsuresSuccessStatusCodeAndReturnsResponseContent()
        {
            // Arrange
            var mockedResponseMessage = Mock.Create<HttpResponseMessage>();
            var mockedRequestHeaders = Mock.Create<HttpRequestHeaders>();

            Mock.Arrange(() => HttpClient.DefaultRequestHeaders)
                .IgnoreInstance()
                .Returns(mockedRequestHeaders)
                .Occurs(3);

            Mock.Arrange(() => mockedRequestHeaders.Add(USER_AGENT_KEY, TEST_USER_AGENT))
                .OccursOnce();

            Mock.Arrange(() => mockedRequestHeaders.TryAddWithoutValidation(ACCEPT_HEADER_KEY, ACCEPT_HEADER_VALUE))
                .OccursOnce();

            Mock.Arrange(() => HttpClient.GetAsync(Arg.Is(new Uri(URI))).Result)
                .IgnoreInstance()
                .Returns(mockedResponseMessage)
                .OccursOnce();

            Mock.Arrange(() => mockedResponseMessage.EnsureSuccessStatusCode())
                .OccursOnce();

            Mock.Arrange(() => mockedResponseMessage.Content.ReadAsStringAsync().Result)
                .Returns(SAMPLE_RESPONSE_BODY)
                .OccursOnce();

            RestCallExecutor restCallExecutor = new RestCallExecutor();

            // Act
            var result = restCallExecutor.Invoke(HttpMethod.Get, URI, CreateSampleHeaders(), null);

            // Assert
            Assert.AreEqual(SAMPLE_RESPONSE_BODY, result);

            Mock.Assert(HttpClient);
            Mock.Assert(mockedRequestHeaders);
            Mock.Assert(mockedResponseMessage);
        }

        [TestMethod]
        public void InvokePostExecutesPostRequestOnUriWithProvidedHeadersAndBodyEnsuresSuccessStatusCodeAndReturnsResponseContent()
        {
            // Arrange
            var mockedResponseMessage = Mock.Create<HttpResponseMessage>();
            var mockedRequestHeaders = Mock.Create<HttpRequestHeaders>();

            Mock.Arrange(() => HttpClient.DefaultRequestHeaders)
                .IgnoreInstance()
                .Returns(mockedRequestHeaders)
                .Occurs(3);

            Mock.Arrange(() => mockedRequestHeaders.Add(USER_AGENT_KEY, TEST_USER_AGENT))
                .OccursOnce();

            Mock.Arrange(() => mockedRequestHeaders.TryAddWithoutValidation(ACCEPT_HEADER_KEY, ACCEPT_HEADER_VALUE))
                .OccursOnce();

            HttpContent content = new StringContent(SAMPLE_REQUEST_BODY, null, ContentType.ApplicationJson.GetStringValue());
            Mock.Arrange(() => HttpClient.PostAsync(Arg.Is(new Uri(URI)), Arg.Is(content)).Result)
                .IgnoreInstance()
                .Returns(mockedResponseMessage)
                .OccursOnce();

            Mock.Arrange(() => mockedResponseMessage.EnsureSuccessStatusCode())
                .OccursOnce();

            Mock.Arrange(() => mockedResponseMessage.Content.ReadAsStringAsync().Result)
                .Returns(SAMPLE_RESPONSE_BODY)
                .OccursOnce();

            RestCallExecutor restCallExecutor = new RestCallExecutor();

            // Act
            var result = restCallExecutor.Invoke(HttpMethod.Post, URI, CreateSampleHeaders(), SAMPLE_REQUEST_BODY);

            // Assert
            Assert.AreEqual(SAMPLE_RESPONSE_BODY, result);
            Assert.AreEqual(ContentType.ApplicationJson, restCallExecutor.ContentType);

            Mock.Assert(HttpClient);
            Mock.Assert(mockedRequestHeaders);
            Mock.Assert(mockedResponseMessage);
        }

        [TestMethod]
        public void InvokeDeleteExecutesDeleteRequestOnUriWithProvidedHeadersAndBodyEnsuresSuccessStatusCodeAndReturnsResponseContent()
        {
            // Arrange
            var mockedResponseMessage = Mock.Create<HttpResponseMessage>();
            var mockedRequestHeaders = Mock.Create<HttpRequestHeaders>();

            Mock.Arrange(() => HttpClient.DefaultRequestHeaders)
                .IgnoreInstance()
                .Returns(mockedRequestHeaders)
                .Occurs(3);

            Mock.Arrange(() => mockedRequestHeaders.Add(USER_AGENT_KEY, TEST_USER_AGENT))
                .OccursOnce();

            Mock.Arrange(() => mockedRequestHeaders.TryAddWithoutValidation(ACCEPT_HEADER_KEY, ACCEPT_HEADER_VALUE))
                .Returns(true)
                .OccursOnce();

            Mock.Arrange(() => HttpClient.DeleteAsync(Arg.Is(new Uri(URI))).Result)
                .IgnoreInstance()
                .Returns(mockedResponseMessage)
                .OccursOnce();

            Mock.Arrange(() => mockedResponseMessage.EnsureSuccessStatusCode())
                .OccursOnce();

            Mock.Arrange(() => mockedResponseMessage.Content.ReadAsStringAsync().Result)
                .Returns(SAMPLE_RESPONSE_BODY)
                .OccursOnce();

            RestCallExecutor restCallExecutor = new RestCallExecutor();

            // Act
            var result = restCallExecutor.Invoke(HttpMethod.Delete, URI, CreateSampleHeaders(), SAMPLE_REQUEST_BODY);
            Assert.AreEqual(SAMPLE_RESPONSE_BODY, result);

            // Assert
            Mock.Assert(HttpClient);
            Mock.Assert(mockedRequestHeaders);
            Mock.Assert(mockedResponseMessage);
        }

        [TestMethod]
        public void InvokePutExecutesPutRequestOnUriWithProvidedHeadersAndBodyEnsuresSuccessStatusCodeAndReturnsResponseContent()
        {
            // Arrange
            var mockedResponseMessage = Mock.Create<HttpResponseMessage>();
            var mockedRequestHeaders = Mock.Create<HttpRequestHeaders>();

            Mock.Arrange(() => HttpClient.DefaultRequestHeaders)
                .IgnoreInstance()
                .Returns(mockedRequestHeaders)
                .Occurs(3);

            Mock.Arrange(() => mockedRequestHeaders.Add(USER_AGENT_KEY, TEST_USER_AGENT))
                .OccursOnce();

            Mock.Arrange(() => mockedRequestHeaders.TryAddWithoutValidation(ACCEPT_HEADER_KEY, ACCEPT_HEADER_VALUE))
                .OccursOnce();

            HttpContent content = new StringContent(SAMPLE_REQUEST_BODY, null, ContentType.ApplicationJson.GetStringValue());
            Mock.Arrange(() => HttpClient.PutAsync(Arg.Is(new Uri(URI)), Arg.Is(content)).Result)
                .IgnoreInstance()
                .Returns(mockedResponseMessage)
                .OccursOnce();

            Mock.Arrange(() => mockedResponseMessage.EnsureSuccessStatusCode())
                .OccursOnce();

            Mock.Arrange(() => mockedResponseMessage.Content.ReadAsStringAsync().Result)
                .Returns(SAMPLE_RESPONSE_BODY)
                .OccursOnce();

            RestCallExecutor restCallExecutor = new RestCallExecutor();

            // Act
            var result = restCallExecutor.Invoke(HttpMethod.Put, URI, CreateSampleHeaders(), SAMPLE_REQUEST_BODY);

            // Assert
            Assert.AreEqual(SAMPLE_RESPONSE_BODY, result);
            Assert.AreEqual(ContentType.ApplicationJson, restCallExecutor.ContentType);

            Mock.Assert(HttpClient);
            Mock.Assert(mockedRequestHeaders);
            Mock.Assert(mockedResponseMessage);
        }

        [TestMethod]
        public void InvokeWhenAuthSchemeIsSetSetsAuthorizationHeaderAccordingAuthScheme()
        {
            // Arrange
            var mockedResponseMessage = Mock.Create<HttpResponseMessage>();
            var mockedRequestHeaders = Mock.Create<HttpRequestHeaders>();

            Mock.Arrange(() => HttpClient.DefaultRequestHeaders)
                .IgnoreInstance()
                .Returns(mockedRequestHeaders)
                .Occurs(4);

            Mock.ArrangeSet(() => mockedRequestHeaders.Authorization = Arg.Is(new AuthenticationHeaderValue(BEARER_AUTH_SCHEME, SAMPLE_BEARER_TOKEN)))
                .OccursOnce();

            Mock.Arrange(() => mockedRequestHeaders.Add(USER_AGENT_KEY, TEST_USER_AGENT))
                .OccursOnce();

            Mock.Arrange(() => mockedRequestHeaders.TryAddWithoutValidation(ACCEPT_HEADER_KEY, ACCEPT_HEADER_VALUE))
                .OccursOnce();

            Mock.Arrange(() => HttpClient.GetAsync(Arg.Is(new Uri(URI))).Result)
                .IgnoreInstance()
                .Returns(mockedResponseMessage)
                .OccursOnce();

            Mock.Arrange(() => mockedResponseMessage.EnsureSuccessStatusCode())
                .OccursOnce();

            Mock.Arrange(() => mockedResponseMessage.Content.ReadAsStringAsync().Result)
                .Returns(SAMPLE_RESPONSE_BODY)
                .OccursOnce();

            RestCallExecutor restCallExecutor = new RestCallExecutor();
            restCallExecutor.AuthScheme = BEARER_AUTH_SCHEME;
            var headers = CreateSampleHeaders();
            headers.Add(AUTHORIZATION_HEADER_KEY, SAMPLE_BEARER_TOKEN);

            // Act
            var result = restCallExecutor.Invoke(HttpMethod.Get, URI, headers, null);
            Assert.AreEqual(SAMPLE_RESPONSE_BODY, result);

            // Assert
            Mock.Assert(HttpClient);
            Mock.Assert(mockedRequestHeaders);
            Mock.AssertSet(() => mockedRequestHeaders.Authorization = Arg.Is(new AuthenticationHeaderValue(BEARER_AUTH_SCHEME, SAMPLE_BEARER_TOKEN)));
            Mock.Assert(mockedResponseMessage);
        }

        [TestMethod]
        public void InvokeSetsAuthorizationHeaderAccordingHeadersDictionaryWhenAuthSchemeIsNotSet()
        {
            // Arrange
            var mockedResponseMessage = Mock.Create<HttpResponseMessage>();
            var mockedRequestHeaders = Mock.Create<HttpRequestHeaders>();

            Mock.Arrange(() => HttpClient.DefaultRequestHeaders)
                .IgnoreInstance()
                .Returns(mockedRequestHeaders)
                .Occurs(4);

            Mock.ArrangeSet(() => mockedRequestHeaders.Authorization = new AuthenticationHeaderValue(BEARER_AUTH_SCHEME, SAMPLE_BEARER_TOKEN))
                .OccursNever();

            Mock.Arrange(() => mockedRequestHeaders.TryAddWithoutValidation(AUTHORIZATION_HEADER_KEY, SAMPLE_BEARER_TOKEN))
                .OccursOnce();

            Mock.Arrange(() => mockedRequestHeaders.Add(USER_AGENT_KEY, TEST_USER_AGENT))
                .OccursOnce();

            Mock.Arrange(() => mockedRequestHeaders.TryAddWithoutValidation(ACCEPT_HEADER_KEY, ACCEPT_HEADER_VALUE))
                .OccursOnce();

            Mock.Arrange(() => HttpClient.GetAsync(Arg.Is(new Uri(URI))).Result)
                .IgnoreInstance()
                .Returns(mockedResponseMessage)
                .OccursOnce();

            Mock.Arrange(() => mockedResponseMessage.EnsureSuccessStatusCode())
                .OccursOnce();

            Mock.Arrange(() => mockedResponseMessage.Content.ReadAsStringAsync().Result)
                .Returns(SAMPLE_RESPONSE_BODY)
                .OccursOnce();

            RestCallExecutor restCallExecutor = new RestCallExecutor();
            var headers = CreateSampleHeaders();
            headers.Add(AUTHORIZATION_HEADER_KEY, SAMPLE_BEARER_TOKEN);

            // Act
            var result = restCallExecutor.Invoke(HttpMethod.Get, URI, headers, null);
            
            // Assert
            Assert.AreEqual(SAMPLE_RESPONSE_BODY, result);

            Mock.Assert(HttpClient);
            Mock.Assert(mockedRequestHeaders);
            Mock.AssertSet(() => mockedRequestHeaders.Authorization = Arg.Is(new AuthenticationHeaderValue(BEARER_AUTH_SCHEME, SAMPLE_BEARER_TOKEN)));
            Mock.Assert(mockedResponseMessage);
        }

        private IDictionary<String, String> CreateSampleHeaders()
        {
            var headers = new Dictionary<String, String>();
            headers.Add(USER_AGENT_KEY, TEST_USER_AGENT);
            headers.Add(ACCEPT_HEADER_KEY, ACCEPT_HEADER_VALUE);
            return headers;
        }
    }
}
