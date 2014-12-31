SeleniumFixture
===============
Is a companion library for Selenium C# that provides functionality to easily populate forms, navigate pages and construct PageObjects.

```C#
var driver = new FirefoxDriver();

var fixture = new Fixture(driver);

fixture.Navigate.To("http://ipjohnson.github.io/SeleniumFixture/TestSite/InputForm.html");

fixture.AutoFill("//form");
```

###Click & DoubleClick
Provide short hands for clicking and double clicking.

```C#
// click button with id someButton
fixture.Click("#someButton");

// click input button of type submit
fixture.Click("input[type='submit']");

// DoubleClick all buttons that have the css class 'some-css-class'
fixture.DoubleClick("button.some-css-class", ClickMode.Any);
```

###Fill
Fill allows you to easily populate form element with a given set of values. Currently textboxs, radiobutton, checkbox, select and textarea are supported.

```C#
// Navigate to page and fill out form
fixture.Navigate.To("http://ipjohnson.github.io/SeleniumFixture/TestSite/InputForm.html");

fixture.Fill("//form")
       .With(new 
              { 
                FirstName = "Sterling", 
                LastName = "Archer",
                Active = true,
                Gender = "Male"
              })
       .ThenSubmit();
```

###AutoFill
Sometimes its useful to populate a form with random data. The AutoFill method can be used to fill a html input elements.

```C#
// AutoFill form element
fixture.AutoFill("//form");

// AutoFill form but use Bob to will the FirstName field
fixture.AutoFill("//form", seedWith: new { FirstName = "Bob"});

// AutoFill all input & select elements in the div with id someDiv
fixture.AutoFill("#someDiv");
```

###Wait
It's useful to wait for certain things to happen on page, the Wait API provides a number of helpful wait method for things like ajax or for elements to exists.

```C#
// wait for ajax calls to finish
fixture.Wait.ForAjax();

// wait for #someElement to be present on the page
fixture.Wait.ForElement("#someElement");

// long hand for waiting for element 20 seconds
fixture.Wait.Until(i => i.CheckForElement("#someElement"), 20);
```

###PageObject Pattern
The [PageObject](http://martinfowler.com/bliki/PageObject.html) pattern is very useful and SeleniumFixture fully supports using PageObject classes to help package logic. [SimpleFixture](https://github.com/ipjohnson/SimpleFixture) is used to create instance of PageObjects allowing for very complex object including injecting constructor parameters and properties (using [[Import]] attribute).

```C#
var driver = new FirefoxDriver();

var fixture = new Fixture(driver, "http://ipjohnson.github.io/SeleniumFixture/TestSite/");

var formPage = fixture.Navigate.To<FormPage>("InputForm.html");

formPage.FillOutForm();
```

###IActionProvider Property
PageObjects that have a property of type IActionProvider will be populated upon creation. Usually this propery is named I so your syntax looks like the example below

```C#
I.Fill("//form").With(new { FirstName = "Sterling" });

I.Click("#submitButton").Wait.ForAjax().Then.Yields<HomePage>();
```

###Validate 
PageObjects can validate themselves upon creation one of two ways. You can either have a method name Validate or a property called Validate that is a Action. Either will be called once the page object has been created

```C#
public class HomePage
{
    public HomePage()
    {
      Validate = () => I.Get.PageTitle.Should().Be("Home");
    }
    
    protected Action Validate { get; private set; } 
    
    protected IActionProvider I { get; private set;
}
```

###Yields
Creating new PageObject can be done by calling Yields.

```C#
// click the submit button and yield a new HomePage object 
I.Click("#submitButton").Yields<HomePage>();

// click the link Some Text then yield new OtherPage object and pass the value 5 into the constructor param someParam
I.Click(By.LinkText("Some Text").Yields<OtherPage>(constraints: new { someParam = 5 });
```

###xUnit support
[xUnit 2.0](https://github.com/xunit/xunit) is a very extensible testing framework that SeleniumFixture.xUnit provides support for out of the box. Currently there are a set of attributes that make setup and tear down of IWebDrivers and Fixture easier

```C#
    public class AutoFillTests
    {
        [SeleniumTheory]
        [ChromeDriver]
        [FireFoxDriver]
        [InternetExplorerDriver]
        public void Fixture_FillForm_PopulatesCorrectly(Fixture fixture)
        {
            fixture.Navigate.To("http://ipjohnson.github.io/SeleniumFixture/TestSite/InputForm.html");

            fixture.AutoFill("//form");

            fixture.Get.Value.From("#FirstName").All(char.IsLetter).Should().BeTrue();

            fixture.Get.Value.From("#LastName").All(char.IsLetter).Should().BeTrue();
        }
    }
```


