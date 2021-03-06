[![Cloudey](https://raw.githubusercontent.com/CloudeyIT/FluentValidation.Rules/master/docs/logo-dark%400.5x.png#gh-light-mode-only)](https://cloudey.net)

# FluentValidation Rules
_✔ A collection of useful validation rules for FluentValidation and Entity Framework Core ✨_

---
[![GitHub](https://img.shields.io/github/license/CloudeyIT/FluentValidation.Rules)](https://github.com/CloudeyIT/FluentValidation.Rules/blob/master/LICENSE)
[![Nuget](https://img.shields.io/nuget/v/Cloudey.FluentValidation.Rules)](https://www.nuget.org/packages/Cloudey.FluentValidation.Rules/)
[![Nuget](https://img.shields.io/nuget/dt/Cloudey.FluentValidation.Rules)](https://www.nuget.org/packages/Cloudey.FluentValidation.Rules/)

## What is this

This NuGet package provides two useful validation rules for FluentValidation when used with Entity Framework Core.

### Unique

The `Unique` validation rule checks whether the given value is _unique_ (i.e. does not exist) in the given database context, for the given entity property. For example, you can validate whether the email of a user is unique when they register.

_Usage_
```c#
// Check if an e-mail is unique in the given DbContext for the User entity's Email property
RuleFor(x => x.Email)
    .Unique(context, (User user) => user.Email)
    
// Optionally, apply a transformation to the value you are validating before it is compared
RuleFor(x => x.Email)
    .Unique(context, (User user) => user.NormalizedEmail, value => value.ToUpper())
```

### Exists

The `Exists` validation rules check whether the given value _exists_ in the given database context as the given entity property. For example, you can validate whether the country code a user submitted is valid.

_Usage_
```c#
// Check if country code exists in the given DbContext as the Country entity's Code property
RuleFor(x => x.CountryCode)
    .Exists(context, (Country country) => country.Code)
    
// Optionally, apply a transformation to the value you are validating before it is compared
RuleFor(x => x.CountryCode)
    .Exists(context, (Country country) => country.Code, value => value.ToUpper())
```

## Dependencies

- .NET 6
- FluentValidation 10.3.6 or higher
- Entity Framework Core 6.0.1 or higher

## Install

Install with [NuGet](https://www.nuget.org/packages/Cloudey.FluentValidation.Rules/)

## License

Licensed under MIT without trademark grant.  
Copyright © 2021 Cloudey IT Ltd  

**Cloudey® is a registered trademark of Cloudey IT Ltd**. Use of the Cloudey trademark is **NOT GRANTED** as part of the license of this repository and/or package, notwithstanding any notice in the license or other published material. Users of the code or material contained and/or distributed as part of this repository **ARE NOT** granted a license to use the Cloudey trademark by virtue of this repository and package being license under the MIT license.  