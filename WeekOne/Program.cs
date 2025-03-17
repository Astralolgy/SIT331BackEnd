using System;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;

namespace MoonSim
{
  /*
  You're probably wondering why theres so many notes everywhere when you see this and theres one reason for it.
  When it comes to demonstrations, if I havent written the code right there I will forget everything, stumble and 
  as a result look stupid and/or like I plagarised.
  The notes help me not look like a bufoon when I demonstrate in class, thanks - Kira
  */

  enum Direction { NORTH, EAST, SOUTH, WEST }

  public class Map
  {
    int Size { get; }

    public Map(int size)
    {
      Size = size;
    }

    public void MapRobot(int robotx, int roboty)
    {
      for (int i = 0; i < Size; i++)
      {
        Console.Write("\t \t");
        for (int j = 0; j < Size; j++)
        {
          if (i == robotx && j == roboty)
          {
            Console.Write("X "); //X marks the spot!.. for the robot
          }
          else
          {
            Console.Write("O "); //O for 'O'pen spot, get it?
          }
        }
        Console.WriteLine();
      }
    } //THIS ONE writes the map and accounts for robot position!

    public void MapMaker(int size)
    {
      for (int i = 0; i < Size; i++)
      {
        Console.Write("\t \t");
        for (int j = 0; j < Size; j++)
        {
          Console.Write("O ");
        }
        Console.WriteLine();
      }
    } //This Map function prints the map without requiring the robot to be placed, a bit like staring at the environment before a player joins

    public bool ValidMovement(int x, int y) //If its valid, it'll be true, if not, false, therefore bool
    {
      return x >= 0 && x < Size && y >= 0 && y < Size;
      /*if x is greater than or equal to 0 and smaller than map size,
      / and if y is greater than or equal to 0 and smaller than map size, return the result*/
    }
  }

  class Robot
  {
    private int x, y;
    private bool isPlaced = false;
    private Direction direction;
    private Map map;

    /*All these are set to private to "encapsulate" or hide them from the rest of the functions
    / this also makes it less susceptibe to breaking the program, and only functions in the robot class will be able to edit these
    / but other programs can access them with extra steps*/

    public Robot(Map map)
    {
      this.map = map;
      //Lets the robot class and map class interact
    }

    private void Move()
    {
      int moveX = x;
      int moveY = y;

      switch (direction)
      {
        case Direction.NORTH:
          moveY++;
          break;
        case Direction.SOUTH:
          moveY--;
          break;
        case Direction.EAST:
          moveX++;
          break;
        case Direction.WEST:
          moveX--;
          break;
      }

      if (map.ValidMovement(moveX, moveY))
      {
        x = moveX;
        y = moveY;
      }
      else
      {
        Console.WriteLine("You're gonna fall off the Moon!");
      }
    }

    private void Turn(int turn)
    {
      direction = (Direction)(((int)direction + turn + 4) % 4);
    }

    public void Listen(string command)
    {
      //Oh boy we gotta listen to instructions with this one!
      if (command.StartsWith("PLACE")) //MAKE SURE LISTENING WORDS ARE IN CAPITALS OR ITS BORKEN
      {
        var parts = command.Split([' ', ',']); //transforms the place command into an array

        if (parts.Length == 4
            && int.TryParse(parts[1], out int moveX)
            && int.TryParse(parts[2], out int moveY)
            && Enum.TryParse(parts[3], true, out Direction newDirection)
            && map.ValidMovement(moveX, moveY))
        {
          x = moveX;
          y = moveY;
          direction = newDirection;
          isPlaced = true;
        }
        else
        {
          Console.WriteLine("Command misunderstood, please ensure you use: PLACE x,y,Direction");
          Console.WriteLine("For example, PLACE 3,4,WEST");
        }
      }
      else if (!isPlaced) //Reminder that ! is "is not" so this is else if the robot isnt placed
      {
        Console.WriteLine("You haven't launched the robot yet!");
      }
      else
      {
        switch (command) //switch cases my beloved
        {
          case "MOVE":
            Move();
            break;
          case "LEFT":
            Turn(-1);
            break;
          case "RIGHT":
            Turn(1);
            break;
          default:
            Console.WriteLine("That's not a command...");
            break;
        }
        map.MapRobot(x, y);
      }
    }
  }

  class Simulation
  {
    static void Main(string[] args)
    {
      int size = 0;
      bool validSize = false;

      while (!validSize)
      {
        Console.Write("Please enter a map size between 2 and 100");
        try
        {
          string input = Console.ReadLine();
          size = int.Parse(input); 

          if (size < 2 || size > 100)
          {
            Console.WriteLine("That moon is isn't explorable! Please enter a size between 2 and 100.");
          }
          else
          {
            validSize = true; // Valid input, break the loop
          }
        }
        catch (FormatException)
        {
          Console.WriteLine("Invalid input. Please enter a valid number.");
        }
      }

      Map map = new Map(size);
      Robot robot = new Robot(map);
      Console.WriteLine("Welcome to the Moon Robot Simulation!");
      Console.WriteLine("Enter commands: \n PLACE X,Y,D | MOVE | LEFT | RIGHT | EXIT"); //pro tip \n just means new line! Its good formatting
      map.MapMaker(size);

      while (true)
      {
        Console.Write("Command: ");
        string input = Console.ReadLine()?.Trim().ToUpper();

        if (input == "EXIT") break;

        robot.Listen(input);
      }
    }
  }
}

//Theres some weird formatting I dont like but programs functional, realised this 17/03/25