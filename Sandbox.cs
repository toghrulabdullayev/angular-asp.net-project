using System;
using Extensions;

namespace Sandbox
{
  class Sand
  {
    static void Main(string[] args)
    {
      string name = "Toghrul";
      Console.WriteLine(name.Greet());
      Console.WriteLine("Vusal".Greet());
    }
  }
}

// Extension Methods
namespace Extensions
{
  public static class StringExtensions
  {
    public static string Greet(this string str)
    {
      return "Hello " + str.Trim();
    }
  }
}