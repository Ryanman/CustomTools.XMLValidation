# CustomTools.XMLValidation
A tool that Validates XML against a schema, and produces a validation report.

Sometimes, you need to validate an XML file. Well, that's easy - there are a lot of web based forms that'll instantly tell you if it's malformed. Some will even let you add a schema in another text box, and they'll validate XML just as fast. Plus, any *reasonable* application that uses XML validates it on the fly and produces beautiful error reports... right?
Well what if you have to validate 100 XML files? 1000? 10000? And the client who provided them to you is hoping you'll trust they're perfect before you dump them into a biztalk pipeline? Or, what if you're trying to make sure your schema fits all 10000 XML files from 100 different vendors? Welcome to the big time, boys!

**Features:**
* A basic Console GUI and "Progress Bar" 
* Produces a detailed report of validation errors, including the line in each XML file

**Prerequisites:**
None. The tool works with default MS libraries that should be in your GAC. If you're working with < .NET 1.1 well then you're SOL!

**Description:**
The tool asks you for the necessary information - the directory of the folder containing the XML file collection, and the location of the schema. It also allows you to specify where it will write the logfile. By default, it'll create "ValidationReport.txt" in the same directory as the executable for you to view at your leisure.

**Future Changes and Bugfixes:**

This thing was written quickly and used often. However there are some QOL improvements that really need to be made.
* I need to echo the trace to the console and the log file like I do in my test case alignment utility.
**Acknowledgements:**

I have a thing for console progress bars, and [@DanielSWolf](https://gist.github.com/DanielSWolf) has a great implementation. I'll be ripping it off for this console app in the near future.
