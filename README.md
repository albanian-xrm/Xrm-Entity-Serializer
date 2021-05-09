# Xrm-Entity-Serializer

|Build Status|Coverage Status|
|------------|------------|
|[![Build Status](https://dev.azure.com/Albanian-Xrm/Xrm-Entity-Serializer/_apis/build/status/albanian-xrm.Xrm-Entity-Serializer?branchName=master)](https://dev.azure.com/Albanian-Xrm/Xrm-Entity-Serializer/_build/latest?definitionId=2&branchName=master)|[![Coverage Status](https://coveralls.io/repos/github/albanian-xrm/Xrm-Entity-Serializer/badge.svg?branch=master)](https://coveralls.io/github/albanian-xrm/Xrm-Entity-Serializer?branch=master)|

|Package|NuGet|
|------------|------------|
|XrmEntitySerializer.Core|[![XrmEntitySerializer.Core](https://buildstats.info/nuget/xrmentityserializer.Core)](https://www.nuget.org/packages/XrmEntitySerializer.Core)|
|XrmEntitySerializer.9|[![XrmEntitySerializer.9](https://buildstats.info/nuget/xrmentityserializer.9)](https://www.nuget.org/packages/XrmEntitySerializer.9)|
|XrmEntitySerializer.8|[![XrmEntitySerializer.8](https://buildstats.info/nuget/xrmentityserializer.8)](https://www.nuget.org/packages/XrmEntitySerializer.8)|
|XrmEntitySerializer.7|[![XrmEntitySerializer.7](https://buildstats.info/nuget/xrmentityserializer.7)](https://www.nuget.org/packages/XrmEntitySerializer.7)|
|XrmEntitySerializer.6|[![XrmEntitySerializer.6](https://buildstats.info/nuget/xrmentityserializer.6)](https://www.nuget.org/packages/XrmEntitySerializer.6)|
|XrmEntitySerializer.5|[![XrmEntitySerializer.5](https://buildstats.info/nuget/xrmentityserializer.5)](https://www.nuget.org/packages/XrmEntitySerializer.5)|

This library can serialize an Microsoft.Xrm.Sdk.Entity to Json and back using [Newtonsoft.Json](https://github.com/JamesNK/Newtonsoft.Json) library.

## Usage
To use the library:
1. Get the package from NuGet (There are packages for each CRM SDK major version):
```
Install-Package XrmEntitySerializer.9
```
2. Use EntitySerializer or the JsonConverters with your JsonSerializer instance to serialize/deserialize your data.

A sample code can be found [here](https://gist.github.com/BetimBeja/832924babb4dc8355b730c43cb9ec61a).
