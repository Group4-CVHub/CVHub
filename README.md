# CVHub #

## Introduction ##
CVHub was created using ASP.NET MVC Web Application. The idea behind CVHub and how it came to be, was because of the lack of proffessional websites without needing to have any coding skills. We wanted to create something that even someone with the least amount of knowledge of coding could use. 

## Prerequisites ##
1. Clone project into your folder by choise.
2. Make sure you got an SQL-server installed on your computer. Otherwise download it here https://www.microsoft.com/sv-se/sql-server/sql-server-downloads 
3. Make sure you got an IIS server installed on your computer.
4. Open Package Manager Console.
5. Type: add-migration 'message'
6. Type: update-database

## Techniques Used For Login Functionallity ##
We choose not to use the indentity package when constructing the login because we wanted more controll over the logged in user. Instead we used the: Authentication, Authorazation and Session middleware to handle logged in users. The users authenticate onto the server via the default cookie authentication scheme and will then start a session in the client browser that is easy to handle when coding the rest of the application via HttpContext requests. The sessions are now also able to get accessed in the views after we added the HttpContextAccessor, SessionStateTempDataProvider and DistributedMemoryCache middleware. 

The Facebook and Google authentication services were added to the application. Early on we maniged to get the facebook login to work flawlessly while there was some struggle with Google. But due to some resent changes in the developers.facebook application system and lack of time to read up about it there has unfortunately been some issues with the Facebook login lately.

## Techniques Used For Handling The Database ##
We have used Entity Framework Core with the code first approach. 

## Techniques Used For Handling Testing ##
To run the E2E tests we have used the Selenium WebDriver NuGet package together with the xunit nuget package. All the E2E tests are beeing executed in the ChromeDriver.exe thats in the Tests folder. 

The Integration tests are using a "ContentHelper" class that converts the requests and responses into Json. It also used "testFixure" class that helps finding the project source folder path on your computer and creates a test server.

## How To Examples ##

### First Time Using: ###
1. Press <code>F5</code>
2. Press <code>Register</code>
3. Enter your personal info, to complete registration <code>Create Account</code>
4. When your account is created you can create your own CV by pressing <code>Templates</code> in the Navbar.
5. Then you can either <code>View</code> then you'll have the option to <code>Choose this template </code> or  you can <code>Choose</code> right away.
6. You will then proceed to fill in information.
7. When you're finished > <code>Submit</code>
8. After submitting you can press <code>My CVs</code> to view your newly created CV.

### Test ###
To run tests run the application without debug mode (ctrl+F5) so that the iss server runs locally on your computer. Then go into the test explorer and run all the tests. 

### Swagger ###
Swagger is implemented in the application. To access Swagger write https://localhost:44382/swagger/index.html in your browser.

Thank you for using CVHub! :)

