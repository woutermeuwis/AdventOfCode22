var input = await File.ReadAllTextAsync("input.txt");

var directories = ParseData(input);
var root = directories.First(d => d.Name == "/");

Console.WriteLine($"Part 1: {directories.Where(d => d.Size < 100000).Sum(d => d.Size)}");
Console.WriteLine($"Part 2: {directories.Where(d => d.Size > 30000000 - (70000000 - root.Size)).Min(d => d.Size)}");

List<Folder> ParseData(string input)
{
    Folder? current = new("/", null);
    var folders = new List<Folder> { current };

    var commands = input.Split("$ ").Skip(1).Select(c => c.Trim());
    foreach (var command in commands)
    {
        switch (command[0..2])
        {
            case "cd":
                var dest = command[3..];
                switch (dest)
                {
                    case "/":
                        current = folders.FirstOrDefault(f => f.Name == "/");
                        break;
                    case "..":
                        current = current?.Parent;
                        break;
                    default:
                        current = current?.Folders[dest];
                        break;
                }
                break;
            case "ls":
                foreach (var content in command.Split('\n').Skip(1))
                {
                    if (content.StartsWith("dir"))
                    {
                        var folder = new Folder(content[4..].Trim(), current);
                        folders.Add(folder);
                        current?.Folders.Add(folder.Name, folder);
                    }
                    else
                        current?.Files.Add(content.Split(' ')[1].Trim(), long.Parse(content.Split(' ')[0]));
                }
                break;
        }
    }
    return folders;
}

class Folder
{
    public Folder(string name, Folder? parent)
    {
        Name = name;
        Parent = parent;
        Folders = new Dictionary<string, Folder>();
        Files = new Dictionary<string, long>();
    }

    public string Name { get; }
    public Folder? Parent { get; }
    public long Size => Folders.Values.Sum(c => c.Size) + Files.Values.Sum();
    public Dictionary<string, long> Files { get; }
    public Dictionary<string, Folder> Folders { get; }
}