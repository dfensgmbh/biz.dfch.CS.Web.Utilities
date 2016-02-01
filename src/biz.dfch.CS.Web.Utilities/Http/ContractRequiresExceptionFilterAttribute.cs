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
using System.Net;
using System.Net.Http;
using System.Web.Http.Filters;
using biz.dfch.CS.Utilities.Logging;
using System.Diagnostics.Contracts;

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

        public override void OnException(HttpActionExecutedContext context)
        {
            if ((null == context.Exception) || (CONTRACT_REQUIRES_EXCEPTION_FULLNAME != context.Exception.GetType().FullName))
            {
                return;
            }

            var ex = context.Exception;
            Contract.Assert(null != ex);
            var message = string.Format(
                "{0}-EX {1}"
                ,
                context.ActionContext.Request.GetCorrelationId()
                ,
                ex.Message
                );
            Trace.WriteException(message, ex);

            var innerExceptionCount = 0;
            var innerException = ex.InnerException;
            while (null != innerException)
            {
                innerExceptionCount++;
                var exceptionMessage = string.Format("{0}-EX {1}", context.ActionContext.Request.GetCorrelationId(), innerException.Message);
                Trace.WriteException(exceptionMessage, innerException);
                innerException = innerException.InnerException;
            }

            var exMessage = string.IsNullOrWhiteSpace(ex.Message) ? string.Empty : ex.Message;
            var httpParams = exMessage.Split('|');
            if (1 >= httpParams.Length)
            {
                context.Response = context.Request.CreateErrorResponse(
                    HttpStatusCode.InternalServerError
                    ,
                    string.Concat("[CorrelationId: ", context.ActionContext.Request.GetCorrelationId(), "] ", ex.Message)
                    ,
                    ex
                    );
                return;
            }

            int statusCode;
            try
            {
                statusCode = Convert.ToInt32(httpParams[1].Trim());
                statusCode = ((100 > statusCode) || (599 < statusCode)) ? 500 : statusCode;
            }
            catch
            {
                statusCode = 500;
            }

            string statusMessage;
            if (2 < httpParams.Length && !string.IsNullOrWhiteSpace(httpParams[2].Trim()))
            {
                statusMessage = string.Concat("[CorrelationId: ", context.ActionContext.Request.GetCorrelationId(), "] ", httpParams[2].Trim());
            }
            else
            {
                statusMessage = string.Concat("[CorrelationId: ", context.ActionContext.Request.GetCorrelationId(), "] ", httpParams[0].Trim());
            }
            context.Response = context.Request.CreateErrorResponse((HttpStatusCode)statusCode, statusMessage);
        }
    }
}