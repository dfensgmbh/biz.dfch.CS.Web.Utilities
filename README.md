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

* Contracts for endpoints
* ODataController helper
* REST helper

**Telerik JustMock has to be licensed separately. Only the code samples (source code files) are licensed under the Apache 2.0 license. The Telerik JustMock software has to be licensed separately. See the NOTICE file for more information about this.**

## [Release Notes](https://github.com/dfensgmbh/biz.dfch.CS.Web.Utilities/releases)

See also [Releases](https://github.com/dfensgmbh/biz.dfch.CS.Web.Utilities/releases) and [Tags](https://github.com/dfensgmbh/biz.dfch.CS.Web.Utilities/tags)

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
