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

using System.Collections.Generic;
using biz.dfch.CS.Web.Utilities.OData;
using Microsoft.Data.OData;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Telerik.JustMock;
using System.Web.Http.OData;
using System.Web.Http.OData.Extensions;
using System.Web.Http.OData.Routing;
using System.Web.Http;
using System.Net.Http;
using System.Net;

namespace biz.dfch.CS.Web.Utilities.Tests.OData
{
    [TestClass]
    public class ODataControllerHelperTest
    {
        private static ODataController controller;
        private static HttpRequestMessage httpRequestMessage;
        private const string ODATA_LINK = "http://localhost/api/Utilities.svc/BaseEntities(1)";

        [TestInitialize]
        public void TestInitialize()
        {
            httpRequestMessage =
                new HttpRequestMessage(HttpMethod.Post, "http://localhost/api/Utilities.svc/BaseEntities");
            httpRequestMessage.SetConfiguration(new HttpConfiguration());

            Mock.SetupStatic(typeof(System.Web.Http.OData.Extensions.HttpRequestMessageExtensions));
        }

        [TestMethod]
        [ExpectedException(typeof(ODataErrorException))]
        public void DoResponseCreatedForControllerNotContainingControllerValueInRouteDataThrowsODataErrorException()
        {
            controller = Mock.Create<ODataController>();
            Mock.Arrange(() => controller.ControllerContext.RouteData.Values["controller"])
                .Returns("test")
                .MustBeCalled();
            Mock.Arrange(() => controller.ControllerContext.RouteData.Values.ContainsKey("controller"))
                .Returns(false)
                .MustBeCalled();

            Mock.Assert(controller);

            ODataControllerHelper.ResponseCreated(controller, new BaseEntity(1));
        }

        [TestMethod]
        public void DoResponseCreatedReturnsHttpResponseMessageWithStatusCreated()
        {
            controller = Mock.Create<ODataController>();
            Mock.Arrange(() => controller.Request).Returns(httpRequestMessage).MustBeCalled();
            Mock.Arrange(() => controller.ControllerContext.RouteData.Values.ContainsKey("controller"))
                .Returns(true)
                .MustBeCalled();
            Mock.Arrange(() => controller.ControllerContext.RouteData.Values["controller"])
                .Returns("test")
                .MustBeCalled();
            Mock.Arrange(() => controller.Url.CreateODataLink(Arg.IsAny<string>(), Arg.IsAny<IODataPathHandler>(), Arg.IsAny<IList<ODataPathSegment>>()))
                .Returns(ODATA_LINK).MustBeCalled();

            var response = ODataControllerHelper.ResponseCreated(controller, new BaseEntity(1));
            Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);

            Mock.Assert(controller);
        }

        [TestMethod]
        public void DoResponseCreatedReturnsHttpResponseMessageWithLocationSetInHeaders()
        {
            controller = Mock.Create<ODataController>();
            Mock.Arrange(() => controller.Request)
                .Returns(httpRequestMessage)
                .MustBeCalled();
            Mock.Arrange(() => controller.ControllerContext.RouteData.Values.ContainsKey("controller"))
                .Returns(true)
                .MustBeCalled();
            Mock.Arrange(() => controller.ControllerContext.RouteData.Values["controller"])
                .Returns("test")
                .MustBeCalled();
            Mock.Arrange(() => controller.Url.CreateODataLink(Arg.IsAny<string>(), Arg.IsAny<IODataPathHandler>(), Arg.IsAny<IList<ODataPathSegment>>()))
                .Returns(ODATA_LINK)
                .MustBeCalled();

            var response = ODataControllerHelper.ResponseCreated(controller, new BaseEntity(1));
            Assert.IsNotNull(response.Headers.Location);
            Assert.AreEqual(ODATA_LINK, response.Headers.Location.ToString());

            Mock.Assert(controller);
        }

        [TestMethod]
        public void DoResponseCreatedReturnsHttpResponseMessageWithETagSetInHeaders()
        {
            controller = Mock.Create<ODataController>();
            Mock.Arrange(() => controller.Request).Returns(httpRequestMessage).MustBeCalled();
            Mock.Arrange(() => controller.ControllerContext.RouteData.Values.ContainsKey("controller"))
                .Returns(true)
                .MustBeCalled();
            Mock.Arrange(() => controller.ControllerContext.RouteData.Values["controller"])
                .Returns("test")
                .MustBeCalled();
            Mock.Arrange(() => controller.Url.CreateODataLink(Arg.IsAny<string>(), Arg.IsAny<IODataPathHandler>(), Arg.IsAny<IList<ODataPathSegment>>()))
                .Returns(ODATA_LINK)
                .MustBeCalled();

            var response = ODataControllerHelper.ResponseCreated(controller, new BaseEntity(1));
            Assert.IsNotNull(response.Headers.ETag);

            Mock.Assert(controller);
        }

        [TestMethod]
        [ExpectedException(typeof(ODataErrorException))]
        public void DoResponseAcceptedForControllerNotContainingControllerValueInRouteDataThrowsODataErrorException()
        {
            controller = Mock.Create<ODataController>();
            Mock.Arrange(() => controller.ControllerContext.RouteData.Values.ContainsKey("controller"))
                .Returns(false)
                .MustBeCalled();

            Mock.Assert(controller);

            ODataControllerHelper.ResponseAccepted(controller, new BaseEntity(1));
        }

        [TestMethod]
        public void DoResponseAcceptedReturnsHttpResponseMessageWithStatusCreated()
        {
            controller = Mock.Create<ODataController>();
            Mock.Arrange(() => controller.Request).Returns(httpRequestMessage).MustBeCalled();
            Mock.Arrange(() => controller.ControllerContext.RouteData.Values.ContainsKey("controller"))
                .Returns(true)
                .MustBeCalled();
            Mock.Arrange(() => controller.ControllerContext.RouteData.Values["controller"])
                .Returns("test")
                .MustBeCalled();
            Mock.Arrange(() => controller.Url.CreateODataLink(Arg.IsAny<string>(), Arg.IsAny<IODataPathHandler>(), Arg.IsAny<IList<ODataPathSegment>>()))
                .Returns(ODATA_LINK)
                .MustBeCalled();

            var response = ODataControllerHelper.ResponseAccepted(controller, new BaseEntity(1));
            Assert.AreEqual(HttpStatusCode.Accepted, response.StatusCode);

            Mock.Assert(controller);
        }

        [TestMethod]
        public void DoResponseAcceptedReturnsHttpResponseMessageWithLocationSetInHeaders()
        {
            controller = Mock.Create<ODataController>();
            Mock.Arrange(() => controller.Request)
                .Returns(httpRequestMessage)
                .MustBeCalled();
            Mock.Arrange(() => controller.ControllerContext.RouteData.Values.ContainsKey("controller"))
                .Returns(true)
                .MustBeCalled();
            Mock.Arrange(() => controller.ControllerContext.RouteData.Values["controller"])
                .Returns("test")
                .MustBeCalled();
            Mock.Arrange(() => controller.Url.CreateODataLink(Arg.IsAny<string>(), Arg.IsAny<IODataPathHandler>(), Arg.IsAny<IList<ODataPathSegment>>()))
                .Returns(ODATA_LINK)
                .MustBeCalled();

            var response = ODataControllerHelper.ResponseAccepted(controller, new BaseEntity(1));
            Assert.IsNotNull(response.Headers.Location);
            Assert.AreEqual(ODATA_LINK, response.Headers.Location.ToString());

            Mock.Assert(controller);
        }

        [TestMethod]
        public void DoResponseAcceptedReturnsHttpResponseMessageWithETagSetInHeaders()
        {
            controller = Mock.Create<ODataController>();
            Mock.Arrange(() => controller.Request)
                .Returns(httpRequestMessage)
                .MustBeCalled();
            Mock.Arrange(() => controller.ControllerContext.RouteData.Values.ContainsKey("controller"))
                .Returns(true)
                .MustBeCalled();
            Mock.Arrange(() => controller.ControllerContext.RouteData.Values["controller"])
                .Returns("test")
                .MustBeCalled();
            Mock.Arrange(() => controller.Url.CreateODataLink(Arg.IsAny<string>(), Arg.IsAny<IODataPathHandler>(), Arg.IsAny<IList<ODataPathSegment>>()))
                .Returns(ODATA_LINK)
                .MustBeCalled();

            var response = ODataControllerHelper.ResponseAccepted(controller, new BaseEntity(1));
            Assert.IsNotNull(response.Headers.ETag);

            Mock.Assert(controller);
        }
    }

    public class BaseEntity
    {
        public BaseEntity(long id)
        {
            Id = id;
        }

        public long Id { get; set; }
    }
}
