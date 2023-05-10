LoanAPI:
LoanAPI users can take a loan.

Overview:
In LoanAPI, users can register, authorize and borrow.
"Admin" type user:
Can view, change and delete any user's loan (status no
does it matter)
has the right to block the user for a certain time, that is, the right to take a loan will be limited to the user.
"User" type user:
can return information about the user by ID,
An unauthorized user has no right to change anything.
The user must be able to view, update and delete their own loans
You should only be able to update and delete if the status is in processing.

Libraries and dependencies:
- FluentValidation; 
- MSTest.TestAdapter; 
- MSTest.TestFramework; 
- NLog;
- NLog.Extensions.Logging
- Moq;
- Microsoft.EntityFrameworkCore.SqlServer;
- Microsoft.EntityFrameworkCore.Tools;
- Microsoft.NET.Test.Sdk;
- Coverlet.collector;
- FluentAssertions;
- Microsoft.AspNetCore.Authentication.JwtBearer;
- Swashbuckle.AspNetcore
- TweetinviAPI
you can install these packages using NuGet. Here's how you can do this:
1. Open the Package Manager Console in Visual Studio.
2. Run the desired command and press instal.

Installation
To develop my project, you will need to have Microsoft Visual Studio 2019 and Microsoft SQL Server 2017 or later installed on your machine. If you don't have these tools installed, you can download and install them using the following links:
- Microsoft Visual Studio 2019: [Download Link](https://visualstudio.microsoft.com/downloads/)
- Microsoft SQL Server 2017: [Download Link](https://www.microsoft.com/en-us/sql-server/sql-server-downloads)

Usage instructions: 
When you run the project, you will see Swagger, where you get the endpoints what you are interested in to get information.

Contact information:
please don't hesitate to contact me if you have any questions.
Email: ma.tateladze@gmail.com