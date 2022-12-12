using Advent;

namespace Advent
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Analysing commands...");
            var commands = File.ReadLines("input.txt").ToList();

            var rootDirectory = new Dict("root", null);
            var currentDirectory = rootDirectory;
            var directories = new List<Dict> { rootDirectory };

            // 1
            for (var i = 0; i < commands.Count; i++)
            {
                var commandParts = commands[i].Split(' ');
                switch (commandParts[1])
                {
                    case "cd": // Navigace
                        currentDirectory = commandParts[2] switch
                        {
                            "/" => rootDirectory,
                            ".." => currentDirectory.Parent ?? currentDirectory,
                            _ => currentDirectory.SubDirectories.First(d => d.Name == commandParts[2])
                        };
                        break;
                    case "ls": // Listing
                        var listOutput = commands.Skip(i + 1).TakeWhile(s => !s.StartsWith("$"));
                        foreach (var item in listOutput)
                        {
                            var itemParts = item.Split(' ');
                            if (itemParts[0] == "dir")
                            {
                                var subDirectory = new Dict(itemParts[1], currentDirectory);
                                currentDirectory.SubDirectories.Add(subDirectory);
                                directories.Add(subDirectory);
                            }
                            else
                            {
                                var fileSize = int.Parse(itemParts[0]);
                                currentDirectory.Files.Add(new SystemFile(itemParts[1], fileSize));
                                currentDirectory.Size += fileSize;
                            }
                        }

                        // Done listing, skip to next command
                        i += listOutput.Count();
                        break;
                }
            }

            var sum = directories.Select(CalculateDirectorySize).Where(dirSize => dirSize <= 100000).Sum();

            Console.WriteLine($"Výsledek 1: {sum}");

            // 2
            const int totalCapacity = 70000000;
            const int spaceRequired = 30000000;
            var spaceUsed = CalculateDirectorySize(rootDirectory);
            var spaceRemaining = totalCapacity - spaceUsed;

            var (smallestDir, size) = directories
                .Select(d => (Directory: d, Size: CalculateDirectorySize(d)))
                .OrderByDescending(x => spaceRemaining + x.Size >= spaceRequired)
                .ThenBy(x => x.Size)
                .FirstOrDefault();

            Console.WriteLine($"Vásledek 2: Smazat {smallestDir.Name} s velikostí {size}");


            int CalculateDirectorySize(Dict directory)
            {
                if (!directory.SubDirectories.Any()) return directory.Size;
                return directory.Size + directory.SubDirectories.Sum(CalculateDirectorySize);
            }
        }
    }
}