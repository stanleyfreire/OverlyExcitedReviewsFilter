# OverlyExcitedReviewsFilter

This solution solves the problem where overly excited reviews are being published on DealerRater.com website and they need some way to filter the ones that stand out the most.\
\
To achieve the results this solution consist in two main projects:\
    -   ReviewScrapperAPI - API responsible for fetching data from DealerRater.com\
    -   ReviewScrapper - Service responsible for filtering reviews given "excitment criteria" (listed below).\

## ReviewScrapperAPI

This API uses HtmlAgilityPack and System.Net.Http libraries to fetch data from DealerRater.com.\
\
// talk about how it works and how it scraps data from the website.\
// show how to call this API if needed - GET Request\
// show how the API returns data to the client - GET Response\
// maybe show Swagger or link it?\

// talk about Unit Tests and what was tested here.\

## ReviewScrapper

This service is responsible for getting data back from ReviewScrapperAPI, filtering the reviews and displaying them on a Console.\
To filter the reviews the service uses what I like to call an "Excitment Criteria" and a Regex pattern.\

### The "Excitment Criteria"

In order to filter the reviews, the software needs to look for overly positive adjectives in English - main language that the McKaig Chevrolet Buick dealership customers use - like "Great", "Awesome", "Terrific", "Wonderful" or even sentences like "Best place in the world" that are listed below:\

// add words list here.\

It's important to point that this list has a weight too, so for simple adjectives the score would be **initially** 1 and for small sentences the weight would be **initially** 2.\

~~Why initially?~~\
    The list starts with 1 and 2 as weigths, but after the algorithm finishes it will run an update on the lists that reduces the weight of a word/sentece given how many times it was used on the last run. So for example if the world "wonderful" was used 30 times in all reviews the new weight for this word is now 0.7 (I removed 0.3 points that are 30/100.)\

Does that mean that a review that looks like this: "I was very well treated inside the dealership and had a wonderful experience!" should be voted down just because it contains the word "wonderful" inside it?  \
No. That's when the "Excitment Score" comes in play.

### The "Excitment Score"

The Excitment Score consists in 2 sub-scores:__
    - UserScore
        - How many reviews the same User published? Each review count as 1 point here. Lower value is better.
    - WordScore
        - How excited was his(her) writing? This score depends on how **many** matches were found inside the Review Text and the **weight** of each match.

The ReviewScrapper service will iterate through all reviews and score them as Normal, Excited and Overly Excited.
    - How the algorithm will know that?\
        Whenever a match occurs inside the review text, I'll add some points to this review, so for example:
        Using the list above, if I have the review text as: "Today I had a great experience! So awesome to be inside the dealership!" the words score would be 2 (2 words had a match: great and awesome).

        Now for another review that the text looks like this: "The perfect dealership in Texas. Best place in the world. Fantastic!" the score would be 4. Why 4? Because **perfect** and **fantastic** have a weight of 1 but the sentence **Best place in the world** has a weight of 2. Summing it all you got 4.

        I'll save the ocurrences count on each word/sentence for later use. It'll help me decrease the score of some of the most used words.