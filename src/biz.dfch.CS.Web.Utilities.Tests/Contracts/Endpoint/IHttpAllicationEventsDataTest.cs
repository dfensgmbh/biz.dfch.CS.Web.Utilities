﻿/**
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

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace biz.dfch.CS.Web.Utilities.Tests.Contracts.Endpoint
{
    [TestClass]
    public class IHttpAllicationEventsDataTest
    {
        [TestMethod]
        public void DefaultPriorityReturnsZero()
        {
            // Arrange
            var expectedPriority = 0;
            var endpointData = new HttpApplicationEventsDataWithPriority();

            // Act
            var result = endpointData.Priority;

            // Assert
            Assert.AreEqual(expectedPriority, result);
        }

        [TestMethod]
        public void GetPriorityReturnsPriority()
        {
            // Arrange
            var expectedPriority = 42;
            var endpointData = new HttpApplicationEventsDataWithPriority(expectedPriority);

            // Act
            var result = endpointData.Priority;

            // Assert
            Assert.AreEqual(expectedPriority, result);
        }

        [TestMethod]
        public void DefaultDataReturnsEmptyString()
        {
            // Arrange
            var expectedData = "";
            var endpointData = new HttpApplicationEventsDataWithData();

            // Act
            var result = endpointData.Data;

            // Assert
            Assert.AreEqual(expectedData, result);
        }

        [TestMethod]
        public void DefaultDataReturnsData()
        {
            // Arrange
            var expectedData = "arbitrary-data";
            var endpointData = new HttpApplicationEventsDataWithData(expectedData);

            // Act
            var result = endpointData.Data;

            // Assert
            Assert.AreEqual(expectedData, result);
        }
    }
}
