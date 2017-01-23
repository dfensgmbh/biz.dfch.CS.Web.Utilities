/**
 * Copyright 2015-2017 d-fens GmbH
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
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Web.Http.Filters;
using System.Diagnostics.Contracts;
using System.Web.Http.OData.Extensions;
using biz.dfch.CS.Commons.Diagnostics;
using Microsoft.Data.OData;
using TraceSource = biz.dfch.CS.Commons.Diagnostics.TraceSource;

namespace biz.dfch.CS.Web.Utilities.Http
{
    // see Exception Handling in ASP.NET Web API
    // http://www.asp.net/web-api/overview/error-handling/exception-handling

    // use with Contract.Requires as follows:
    // Contract.Requires(true == precondition, "|400|custom-error-message|");
    // or
    // Contract.Requires(true == precondition, "|400|");
    public class ContractRequiresExceptionFilterAttribute : ExceptionFilterAttribute
    {
        private const string CONTRACT_REQUIRES_EXCEPTION_FULLNAME = "System.Diagnostics.Contracts.__ContractsRuntime+ContractException";
        private const int CONTRACT_EXCEPTION_MESSAGE_HTTP_STATUS_CODE_SEGMENT = 1;
        private const char CONTRACT_EXCEPTION_MESSAGE_HTTP_STATUS_CODE_DELIMITER = '|';

        private static readonly TraceSource _traceSource = Logger.Get("biz.dfch.CS.Web.Utilities");

        public override void OnException(HttpActionExecutedContext context)
        {
            if ((null == context.Exception) || (CONTRACT_REQUIRES_EXCEPTION_FULLNAME != context.Exception.GetType().FullName))
            {
                return;
            }

            var ex = context.Exception;
            Contract.Assert(null != ex);

            var exMessage = string.IsNullOrWhiteSpace(ex.Message) ? string.Empty : ex.Message;
            var httpParams = exMessage.Split(CONTRACT_EXCEPTION_MESSAGE_HTTP_STATUS_CODE_DELIMITER);

            var statusCodeSegment = CONTRACT_EXCEPTION_MESSAGE_HTTP_STATUS_CODE_SEGMENT < httpParams.Length ? httpParams[CONTRACT_EXCEPTION_MESSAGE_HTTP_STATUS_CODE_SEGMENT].Trim() : null;
            int statusCode;
            var isValidNumber = int.TryParse(statusCodeSegment, out statusCode);
            if (!isValidNumber || 100 > statusCode || 599 < statusCode)
            {
                statusCode = 500;
            }

            var message = string.Format("{0}-EX {1}", Trace.CorrelationManager.ActivityId, exMessage);
            if (statusCode >= 500)
            {
                _traceSource.TraceException(ex, message);
            }
            else
            {
                _traceSource.TraceException(ex, message);

                var innerException = ex.InnerException;
                while (null != innerException)
                {
                    message = string.Format("{0}-EX {1}", Trace.CorrelationManager.ActivityId, innerException.Message);
                    _traceSource.TraceException(innerException, message);
                    innerException = innerException.InnerException;
                }
            }

            if (CONTRACT_EXCEPTION_MESSAGE_HTTP_STATUS_CODE_SEGMENT >= httpParams.Length)
            {
                context.Response = context.Request.CreateErrorResponse(
                    HttpStatusCode.InternalServerError
                    ,
                    new ODataError
                    {
                        Message = string.Concat("[ActivityID: ", Trace.CorrelationManager.ActivityId, "] ", ex.Message)
                    });
                return;
            }

            string statusMessage;
            if (2 < httpParams.Length && !string.IsNullOrWhiteSpace(httpParams[2].Trim()))
            {
                statusMessage = string.Concat("[ActivityID: ", Trace.CorrelationManager.ActivityId, "] ", httpParams[2].Trim());
            }
            else
            {
                statusMessage = string.Concat("[ActivityID: ", Trace.CorrelationManager.ActivityId, "] ", httpParams[0].Trim());
            }

            context.Response = context.Request.CreateErrorResponse(
                (HttpStatusCode)statusCode,
                new ODataError
                {
                    Message = statusMessage
                });
        }
    }
}