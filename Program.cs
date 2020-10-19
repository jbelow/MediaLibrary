using System;
using NLog.Web;
using System.IO;
using System.Linq;


namespace MediaLibrary
{
    class Program
    {
        // create static instance of Logger
        private static NLog.Logger logger = NLogBuilder.ConfigureNLog(Directory.GetCurrentDirectory() + "\\nlog.config").GetCurrentClassLogger();
        static void Main(string[] args)
        {
            logger.Info("Program started");



            string movieFilePath = "movies.scrubbed.csv";

            // When trying to get the MoiveFile it comes up with "Program.cs(16,13): error CS0246: The type or namespace name 'MovieFile' could not be found (are you missing a using directive or an assembly reference?)"
            // but if I want to try and call moive it works just fine (besides for the part of how the program doesn't work because of the MoiveFile not working)
            MovieFile movieFile = new MovieFile(movieFilePath);
            Movie moive = new Movie();

            string choice = "";
            do
            {
                // display choices to user
                Console.WriteLine("1) Add Movie");
                Console.WriteLine("2) Display All Movies");
                Console.WriteLine("3) Search based on the title of a movie");
                Console.WriteLine("Enter to quit");
                // input selection
                choice = Console.ReadLine();
                logger.Info("User choice: {Choice}", choice);

                if (choice == "1")
                {
                    // Add movie
                    Movie movie = new Movie();
                    // ask user to input movie title
                    Console.WriteLine("Enter movie title");
                    // input title
                    movie.title = Console.ReadLine();
                    // verify title is unique
                    if (movieFile.isUniqueTitle(movie.title))
                    {
                        // input genres
                        string input;
                        do
                        {
                            // ask user to enter genre
                            Console.WriteLine("Enter genre (or done to quit)");
                            // input genre
                            input = Console.ReadLine();
                            // if user enters "done"
                            // or does not enter a genre do not add it to list
                            if (input != "done" && input.Length > 0)
                            {
                                movie.genres.Add(input);
                            }
                        } while (input != "done");
                        // specify if no genres are entered
                        if (movie.genres.Count == 0)
                        {
                            movie.genres.Add("(no genres listed)");
                        }

                        Console.WriteLine("Enter movie director");
                        movie.director = Console.ReadLine();

                        Console.WriteLine("Enter running time (h:m:s)");
                        movie.runningTime = TimeSpan.Parse(Console.ReadLine());

                        // add movie
                        movieFile.AddMovie(movie);
                    }
                }
                else if (choice == "2")
                {

                    // Display All Movies
                    foreach (Movie m in movieFile.Movies)
                    {
                        Console.WriteLine(m.Display());
                    }
                }
                else if (choice == "3")
                {
                    string input;

                    Console.WriteLine("what do you want to search for:");
                    input = Console.ReadLine();

                    var titles = movieFile.Movies.Where(m => m.title.Contains(input)).Select(m => m.title);
                    // LINQ - Count aggregation method
                    Console.WriteLine($"There are {titles.Count()} movies with \"{input}\" in the title:");
                    foreach (string t in titles)
                    {
                        Console.WriteLine($"  {t}");
                    }


                }


            } while (choice == "1" || choice == "2" || choice == "3");

            logger.Info("Program ended");



        }
    }
}




// Movie movie = new Movie
// {
//     mediaId = 123,
//     title = "Greatest Movie Ever, The (2020)",
//     director = "Jeff Grissom",
//     // timespan (hours, minutes, seconds)
//     runningTime = new TimeSpan(2, 21, 23),
//     genres = { "Comedy", "Romance" }
// };

// Console.WriteLine(movie.Display());

// Album album = new Album
// {
//     mediaId = 321,
//     title = "Greatest Album Ever, The (2020)",
//     artist = "Jeff's Awesome Band",
//     recordLabel = "Universal Music Group",
//     genres = { "Rock" }
// };
// Console.WriteLine(album.Display());

// Book book = new Book
// {
//     mediaId = 111,
//     title = "Super Cool Book",
//     author = "Jeff Grissom",
//     pageCount = 101,
//     publisher = "",
//     genres = { "Suspense", "Mystery" }
// };
// Console.WriteLine(book.Display());

// string scrubbedFile = FileScrubber.ScrubMovies("movies.csv");
// logger.Info(scrubbedFile);