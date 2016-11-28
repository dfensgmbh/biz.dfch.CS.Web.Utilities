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

﻿using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http.OData.Routing;
using System.Web.Http.OData;
﻿using System.Web.Http.OData.Extensions;
﻿using Microsoft.Data.OData;

namespace biz.dfch.CS.Web.Utilities.OData
{
    public static class ODataControllerHelper
    {
        public static HttpResponseMessage ResponseCreated<T>(ODataController controller, T entity, string key = "Id")
        {
            return CreateResponseWithETagAndLocation(controller, entity, key, HttpStatusCode.Created);
        }

        public static HttpResponseMessage ResponseAccepted<T>(ODataController controller, T entity, string key = "Id")
        {
            return CreateResponseWithETagAndLocation(controller, entity, key, HttpStatusCode.Accepted);
        }

        private static HttpResponseMessage CreateResponseWithETagAndLocation<T>(ODataController controller, T entity, string key, HttpStatusCode httpStatusCode)
        {
            var segments = new List<ODataPathSegment>();

            if (!controller.ControllerContext.RouteData.Values.ContainsKey("controller"))
            {
                const string msg = "Controller RouteData is missing 'controller' value!";
                System.Diagnostics.Debug.WriteLine(msg);
                throw new ODataErrorException(msg);
            }

            segments.Add(new EntitySetPathSegment(controller.ControllerContext.RouteData.Values["controller"].ToString()));
            var propertyInfo = entity.GetType().GetProperty(key);
            var value = propertyInfo.GetValue(entity, null);
            
            segments.Add(new KeyValuePathSegment(
                value is string
                ?
                string.Format("'{0}'", value)
                :
                value.ToString())
                );
            
            var uri = new Uri(controller.Url.CreateODataLink(
                controller.Request.ODataProperties().RouteName
                ,
                controller.Request.ODataProperties().PathHandler
                ,
                segments
                ));
            
            var response = controller.Request.CreateResponse(httpStatusCode, entity);
            response.Headers.Location = uri;
            
            var eTag = string.Format("\"{0}\"", System.Convert.ToString(DateTime.UtcNow.ToBinary()));
            response.Headers.ETag = new EntityTagHeaderValue(eTag);
            
            return response;
        }
    }
}
