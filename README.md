
  
  

# OverlyExcitedReviewsFilter

This codes offers a solution for the problem where KGB has been noticing a ressurgence of overly excited reviews being left in a certain dealership in Texas on the website DealerRater.com.

  

This piece of software solves the problem using a .NET Core Console Application called **ReviewScrapper** that utilizes Regex and a list of words and sentences to score the reviews's Excitment based of a criteria, backed by a dependency injection architecture in order to modularize the services and get to test the methods in a more separte and easier way.

  

In order to explain how it manages to scrap the reviews from the Internet and how it scores each one of them, I'll split the process in three steps: how I **INPUT** data onto the software, how I **PROCESS** them and how the **OUTPUT** should look like once its completed.

  

## STEPS

-  **INPUT**:

- In order to retrieve data from DealerRater.com the software uses HtmlAgilityPack to scrap the website page.

- The website URL looks like this: https://www.dealerrater.com/dealer/McKaig-Chevrolet-Buick-A-Dealer-For-The-People-dealer-reviews-23685/page1/?filter=#link

- Depending on the number of pages that the user wants to scrap from the website I change the URL the tag page**1**, to page**2**, page**3** and so forth.

- Foreach page there are 10 reviews that I iterate through and create and object off of it.

- I gather these information from the website in order to create a Review object: Author, Date of Publish, Title, Stars Score and Review Text.

-  **PROCESS - Criteria Explained**:

- The Process starts by removing duplicate reviews using the criteria: Same Author and same Review Text.

- After that it starts to gather the Matches found on the Regex Pattern. The list of sentences and words are inside a .json file named **expressions.json** which is located on the root folder of the application.

> If you need to calibrate the algorithm, you just have to add or remove certain words/sentences from this file and run the program again. Depending on how much you changed the expressions on expressions.json file, you might need to change the score criteria too on the appsettings.json file.

- There is another file that calibrates the software called **scores.json**. In this file you give word and sentences a **weight** (referenced here as **Score**), so for example: Words have a score of 1 and Sentences have a score of 2 (Sentences are "heavier" then words in my criteria).

- The standard scores.json file states that:

- Word: 1 point

- Sentence: 2 points

- Any occurence of an Exclamation Mark (!) on the match: 1 point + word/sentence wieght

- Each occurence of the same user on the reviews list : 1 point

- Word contained inside a sentence previousvly matched: Half their points.

- Examples (considering that the user left just one review)

> I had a **great** experience

This review would be scored as 1 point. Why? Because **great** is inside the **expressions.json** list and it's a word that count as 1 point on the **scores.json** file.

> This is the **best dealership** in town!

  

This review would be scored as 2 points. Why? Because **best dealership** is inside the **expressions.json** list and it's a sentence that count as 2 points on the **scores.json** file.

>That is **great!!**

  

This review would be scored as 3 points. Why? Because **great** count as 1 point and there are **two** occurences of the exclamation mark, meaning that the it adds 2 more points to the score.

>I had the **best** day. This is the **best dealership** of Texas.

This review would be scored as 2.5 points. Why? Because **best** count as 0.5 point and **best dealership** count as two points. Note that the word score was cut in half because it was contained inside a sentence.

-  **OUTPUT**:

- The tiebreak rule:

- Order by descending scores: TotalScore then by SentenceScore then by WordScore then by StarScore then by Author (asc).

- This is an examples of the output:
![Output](https://i.ibb.co/Svt0GNZ/image.png)
    
## Ideas I had along the way

I thought about using the Azure Text Analytics Cognitive Service in order to evaluate the sentiment of the Review Text but then I wouldn't be able to explain what criteria it was using, just its results. So for the sake of this test I chose to create my own criteria.

Reference: https://azure.microsoft.com/en-us/services/cognitive-services/text-analytics/

I started this project creating two modules: an API that would scrap the website and return a response with the list os Reviews to the Service and the Service itself (stated in this document), but for the sake of brevity and simplicity I chose to go with the service-only approach.

The criteria changed a bunch of times but I stood with an easier version of it.

## Unit tests

- I created some unit tests for this project in order to test some of the algorithm and the functiong of the process itself.

- There are 20 unit tests that tests all the 4 services included in my project.

- Tests were made to cover the extent of this exercise and are enough to ensure functional execution, nevertheless, more tests could be created as this software evolves.

  
## How to Run the Application

### Application

-  **Windows**

- Download and install .NET Core SDK v3.1 from link: https://dotnet.microsoft.com/download/dotnet/thank-you/sdk-3.1.415-windows-x64-installer

- Clone this package in any folder you prefer.

- Open cmd and from the application root folder using *cd* command:
- cd ReviewScrapper\bin\Debug\netcoreapp3.1\

- Inside that folder run the command: dotnet ReviewScrapper.dll

- The code should run.

-  **MacOS**

- Download and install .NET Core SDK v3.1 from link: https://dotnet.microsoft.com/download/dotnet/thank-you/sdk-3.1.415-macos-x64-installer

- Run commands on console:

> sudo su

> ln -s /usr/local/share/dotnet/x64/dotnet /usr/local/bin/

- Clone this package in any folder you prefer.

- Open Terminal and from the application root folder using *cd* command go to /OverlyExcitedReviewsFilter/ReviewScrapper/bin/Debug/netcoreapp3.1

- On that folder run the command: dotnet ReviewScrapper.dll

- The code should run.

  

### Unit Tests

-  **Windows**

- Download and install .NET Core SDK v3.1 from link: https://dotnet.microsoft.com/download/dotnet/thank-you/sdk-3.1.415-windows-x64-installer

- Clone this package in any folder you prefer.

- Open cmd and from the application root folder using _cd_ command:
- cd ReviewScrapper.Tests\bin\Debug\netcoreapp3.1\

- Inside that folder run the command: dotnet test ReviewScrapper.Tests.dll

- The unit test should run.

-  **MacOS**

- Download and install .NET Core SDK v3.1 from link: https://dotnet.microsoft.com/download/dotnet/thank-you/sdk-3.1.415-macos-x64-installer

- Run commands on console:

> sudo su
> ln -s /usr/local/share/dotnet/x64/dotnet /usr/local/bin/

- Clone this package in any folder you prefer.

- Open Terminal and from the application root folder using _cd_ command go to /OverlyExcitedReviewsFilter/ReviewScrapper.Tests/bin/Debug/netcoreapp3.1

- Inside that folder run the command: dotnet test ReviewScrapper.Tests.dll

- The unit test should run.
