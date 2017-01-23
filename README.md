# biz.dfch.CS.Web.Utilities
[![Build Status](https://build.dfch.biz/app/rest/builds/buildType:(id:CSharpDotNet_BizDfchCsWebUtilities_Build)/statusIcon)](https://build.dfch.biz/project.html?projectId=CSharpDotNet_BizDfchCsWebUtilities&tab=projectOverview)
[![License](https://img.shields.io/badge/license-Apache%20License%202.0-blue.svg)](https://github.com/dfensgmbh/biz.dfch.CS.Web.Utilities/blob/master/LICENSE)
[![Version](https://img.shields.io/nuget/v/biz.dfch.CS.Web.Utilities.svg)](https://www.nuget.org/packages/biz.dfch.CS.Web.Utilities/)

Utilities for web applications

Assembly: biz.dfch.CS.Web.Utilities.dll

d-fens GmbH, General-Guisan-Strasse 6, CH-6300 Zug, Switzerland

## Download

* Get it on [NuGet](https://www.nuget.org/packages/biz.dfch.CS.Web.Utilities/)

* See [Releases](https://github.com/dfensgmbh/biz.dfch.CS.Web.Utilities/releases) on [GitHub](https://github.com/dfch/biz.dfch.CS.Web.Utilities)

## Description

This project containts a collection of utility classes that provide functionalities like

* ODataController helper
* REST call executor
* ExceptionFilterAttributes

**Telerik JustMock has to be licensed separately. Only the code samples (source code files) are licensed under the Apache 2.0 license. The Telerik JustMock software has to be licensed separately. See the NOTICE file for more information about this.**

## [Release Notes](https://github.com/dfensgmbh/biz.dfch.CS.Web.Utilities/releases)

See also [Releases](https://github.com/dfensgmbh/biz.dfch.CS.Web.Utilities/releases) and [Tags](https://github.com/dfensgmbh/biz.dfch.CS.Web.Utilities/tags)

### 4.0.0 - 20170123

* Upgraded to .NET Framework 4.6.2
* Removed dependency to Log4net
* Updated dependency `biz.dfch.CS.Commons` to version `1.11.0`
* Updated dependency `Microsoft.AspNet.WebApi.OData` to version `5.7.0`
* Updated dependency `JustMock` to version `2016.3.914.2`
* Cleaned up packages.config

### 3.1.0 - 20161128

* removed dependency on biz.dfch.CS.System.Utilities
* removed dependency on log4net
* updated to Newtonsoft.Json 9.0.1
* added biz.dfch.CS.Commons 1.4.0
* changed logging to log to TraceSource (`biz.dfch.CS.Web.Utilities`)

### 3.0.0 - 20161123

* Removed dependency to EntityFramework
* Added strong name for assembly

### 2.0.0 - 20161116

* Upgraded biz.dfch.CS.System.Utilities dependency to version 3.1.1
  * Upgraded log4net dependency to version 2.0.5
* Upgraded .NET runtime to 4.6 (from 4.5)
* Activated Code Contracts properly

### 1.2.0 - 20160819

RestCallExecutor
* ContentType handling adjusted to allow custom content types

### 1.1.0 - 20160818

RestCallExecutor
* Changed adding DefaultRequestHeaders of httpClient by using TryAddWithoutValidation instead of using Add to allow custom Accept header

### 1.0.1 - 20160201

* Improved ContractRequiresExceptionAttribute class

### 1.0.0 - 20151222

* Initial release
* Extracted from biz.dfch.CS.System.Utilities

[![TeamCity Logo](https://github.com/dfensgmbh/biz.dfch.CS.Web.Utilities/blob/develop/TeamCity.png)](https://www.jetbrains.com/teamcity/)

Built and released with TeamCity
