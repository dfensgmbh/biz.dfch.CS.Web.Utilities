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

using System.Net;
using System.Net.Http;
using System.Web.Http.Filters;
using biz.dfch.CS.Commons.Diagnostics;
using Microsoft.Data.OData;
using TraceSource = biz.dfch.CS.Commons.Diagnostics.TraceSource;

namespace biz.dfch.CS.Web.Utilities.OData
{
    public class ODataExceptionFilterAttribute : ExceptionFilterAttribute
    {
        private static readonly TraceSource _traceSource = Logger.Get("biz.dfch.CS.Web.Utilities");

        public override void OnException(HttpActionExecutedContext context)
        {
            if (context.Exception is ODataException)
            {
                var ex = context.Exception as ODataException;
                
                _traceSource.TraceException(ex, ex.Message);
                
                context.Response = context.Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message);
            }
        }
    }
}