using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace SeleniumFixture.xUnit.Impl
{
    public class SeleniumTheoryDiscoverer : IXunitTestCaseDiscoverer
    {
        public IEnumerable<IXunitTestCase> Discover(ITestMethod testMethod, IAttributeInfo factAttribute)
        {
            // Special case Skip, because we want a single Skip (not one per data item), and a skipped test may
            // not actually have any data (which is quasi-legal, since it's skipped).
            if (factAttribute.GetNamedArgument<string>("Skip") != null)
                return new[] { new XunitTestCase(testMethod) };

            var dataAttributes = testMethod.Method.GetCustomAttributes(typeof(DataAttribute));

            try
            {
                using (var memoryStream = new MemoryStream())
                {
                    foreach (var dataAttribute in dataAttributes)
                    {
                        return new XunitTestCase[] { new SeleniumTheoryTestCase(testMethod) };
                    }

                    foreach (IAttributeInfo customAttribute in testMethod.TestClass.Class.GetCustomAttributes(typeof(DataAttribute)))
                    {
                        return new XunitTestCase[] { new SeleniumTheoryTestCase(testMethod) };
                        
                    }

                    foreach (IAttributeInfo customAttribute in testMethod.TestClass.Class.Assembly.GetCustomAttributes(typeof(DataAttribute)))
                    {
                        return new XunitTestCase[] { new SeleniumTheoryTestCase(testMethod) };
                    }

                    return new XunitTestCase[] {new LambdaTestCase(testMethod,
                                                    () =>
                                                    {
                                                        throw new InvalidOperationException(
                                                            String.Format("No data found for {0}.{1}",
                                                                testMethod.TestClass.Class.Name,
                                                                testMethod.Method.Name));

                                                    })};
                }
            }
            catch
            {
                return new XunitTestCase[] { new XunitTheoryTestCase(testMethod) };
            }
        }
    }
}
