LoanAPI:
LoanAPI users can take a loan.

About
- NET 5
- MSSQL Server

Overview:
In LoanAPI, users can register, authorize and borrow.
"Admin" type user:
Can view, change and delete any user's loan (status no matter)
has the right to block the user for a certain time.
"User" type user:
can return information about the user by ID,
An unauthorized user has no right to change anything.
The user must be able to view, update and delete their own loans.
You should only be able to update and delete if the status is InProcess.

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
- Clone github repository, or download and unzip it.

Usage instructions: 
When you run the project, you will see Swagger, where you can get the following information.
- Create User:
{
  "firstName": "string",
  "lastName": "string",
  "age": 0,
  "email": "string",
  "salary": 0,
  "userName": "string",
  "password": "string",
  "role": "string",
  "isBlocked": false
}

- Login:
{
  "userName": "string",
  "password": "string"
}

- Get user by ID:
insert id


Update user by ID:
insert id
{
  "firstName": "string",
  "lastName": "string",
  "age": 0,
  "email": "string",
  "salary": 0,
  "userName": "string",
  "password": "string",
  "role": "string",
  "isBlocked": false
}

here are endpoints that only Admin user can use:
- Get all users
- Block user
insert id
- Delete user
insert id

- Add Loan
{
  "loanType": 1,
  "amount": 0,
  "currency": 1,
  "loanPeriod": 0
}

- Get loan by id
insert id

- Updateloan by id
insert id
{
  "loanType": 1,
  "amount": 0,
  "currency": 1,
  "loanPeriod": 0
}
here are endpoints that only Admin user can use:
- ChangeLoanstatus by id
insert id
insert loanstatus
 
- Get all loans
- Delete Loan by id
insert id

Basic Flow
first of all you need to login, get generated token.  
copy and paste generated token in Authorization header.

Contact information:
Please don't hesitate to contact me if you have any questions.
Email: ma.tateladze@gmail.com