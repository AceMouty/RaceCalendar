var fileName= "drivers.txt";
var currentDir = Directory.GetCurrentDirectory();
var filePath = Path.Join(currentDir, fileName);

var raceCalendar = new RaceCalendar();
string[] lines = Array.Empty<string>();

try
{
  lines = File.ReadAllLines(filePath);
}
catch (FileNotFoundException)
{
  Console.WriteLine($"File not found: {filePath}");
}
catch (IOException e)
{
  Console.WriteLine($"An error occured while reading the file: {e.Message}");
}

var raceDate = new DateOnly(2023, 1, 15);
var wetlandChargeRace = new Race(raceDate, "Wetland Charge", "Street Swamp");
foreach (var driver in lines)
{
  wetlandChargeRace.AddDriver(new Driver(driver));
}

wetlandChargeRace.PrintWaitingList();

raceCalendar.AddRace(raceDate, wetlandChargeRace);

raceDate = new DateOnly(2023, 2, 14);
raceCalendar.AddRace(raceDate, new Race(raceDate, "Volcan Sprint", "Volcano Hillside"));
raceCalendar.PrintCalendar();

public class RaceCalendar
{
  private readonly Dictionary<string, Race> _calendar = new Dictionary<string, Race>();

  public bool AddRace(DateOnly date, Race newRace)
  {
    var raceExists = _calendar.ContainsKey(date.ToString());
    if (raceExists)
    {
      return false;
    }
    
    _calendar.Add(date.ToString(), newRace);
    return true;
  }

  public void PrintCalendar()
  {
    foreach (Race race in _calendar.Values)
    {
      Console.WriteLine($"{race.RaceDate}: {race.Name} at {race.Track}");
    }
  }
}

public class Race
{
  private int _driverLimit = 20;
  private Queue<Driver> _waitingList = new Queue<Driver>();
  private List<Driver> _drivers = new List<Driver>();

  public DateOnly RaceDate { get; }
  public string Name { get; }
  public string Track { get; }

  public Race(DateOnly date, string name, string track)
  {
    RaceDate = date;
    Name = name;
    Track = track;
  }
  
  public bool AddDriver(Driver newDriver)
  {
    if (_drivers.Count() == _driverLimit)
    {
      _waitingList.Enqueue(newDriver);
      return false;
    }
    
    _drivers.Add(newDriver);
    return true;
  }

  public void PrintWaitingList()
  {
    foreach (var driverName in _waitingList)
    {
      Console.WriteLine(driverName);
    }
  }
}

public record Driver(string Name);