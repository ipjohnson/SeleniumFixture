SeleniumFixture
===============
Is a companion library for Selenium C# that provides functionality to easily populate forms, navigate pages and construct PageObjects.

```C#
var driver = new FirefoxDriver();

var fixture = new Fixture(driver);

fixture.Navigate.To("http://ipjohnson.github.io/SeleniumFixture/TestSite/InputForm.html");

fixture.AutoFill("//form");
```

###PageObject Pattern
The [PageObject](http://martinfowler.com/bliki/PageObject.html) pattern is a very useful pattern and SeleniumFixture full supports using PageObject classes to help package logic. [SimpleFixture](https://github.com/ipjohnson/SimpleFixture) is used to create instance of PageObjects allowing for very complex object including injecting constructor parameters and properties (using [[Import]] attribute).

```C#
var driver = new FirefoxDriver();

var fixture = new Fixture(driver, http://ipjohnson.github.io/SeleniumFixture/TestSite/");

var formPage = fixture.Navigate.To<FormPage>("InputForm.html");

formPage.FillOutForm();
```

###xUnit support
[xUnit 2.0](https://github.com/xunit/xunit) is a very extensible testing framework that SeleniumFixture provides support for out of the box. Currently there are a set of attributes that make setup and tear down of IWebDrivers and Fixture easier

```C#

```
