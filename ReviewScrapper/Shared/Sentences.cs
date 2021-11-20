using System;
using System.Collections.Generic;
using System.Text;

namespace ReviewScrapper.Shared
{
    public static class Sentences
    {

        public static List<string> SentencesToMatch()
        {

            //TODO: TRANSFORMAR EM JSON/TEXT
            //TODO: TRANSFORMAR EM OBJETO COM STRING,PESO

            List<string> sentences = new List<string>();
            sentences.Add("Best place in the world");
            sentences.Add("Best dealership");
            sentences.Add("Perfect Dealership");
            sentences.Add("Awesome Experience");
            sentences.Add("Above and Beyond");
            sentences.Add("Far and Beyond");
            sentences.Add("Absolutely the Best");
            sentences.Add("Will Visit Again");
            sentences.Add("Should Visit Again");
            sentences.Add("Must Stop");
            sentences.Add("Wonderful Experience");
            sentences.Add("Great Experience");
            sentences.Add("Happily Satisfied");

            return sentences;
        }



    }
}
