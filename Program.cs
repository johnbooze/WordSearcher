using System;
using System.Collections.Generic;
using System.Text;

namespace WordSearchSearcher
{
	class Program
	{
		static void Main(string[] args)
		{
			var wordSearch = new WordSearch();
			wordSearch.LoadStrings(args[0]);

			Console.WriteLine($"{wordSearch.numberOfColumns} x {wordSearch.numberOfRows}");
			Console.Write(wordSearch.Print());
			Console.WriteLine();
			string searchPatternString;

			while (true)
			{
				Console.Write("Search pattern: ");
				searchPatternString = Console.ReadLine();
				if (string.IsNullOrEmpty(searchPatternString))
				{
					break;
				}
				char[] searchPattern = searchPatternString.ToCharArray();
				char firstLetter = searchPattern[0];
				var firstCharCoordinates = wordSearch.charIndex[firstLetter];
				foreach (var coordinate in firstCharCoordinates)
				{
					if (wordSearch.SearchRight(coordinate, searchPattern))
					{
						Console.WriteLine($"Right: {coordinate}");
					}
					if (wordSearch.SearchDown(coordinate, searchPattern))
					{
						Console.WriteLine($"Down: {coordinate}");
					}
					if (wordSearch.SearchDownRight(coordinate, searchPattern))
					{
						Console.WriteLine($"DownRight: {coordinate}");
					}
					if (wordSearch.SearchDownLeft(coordinate, searchPattern))
					{
						Console.WriteLine($"DownLeft: {coordinate}");
					}
				}
			}
		}
	}

	public class Coordinate
	{
		public int RowIndex { get; set; }
		public int ColumnIndex { get; set; }

		public Coordinate()
		{
			this.RowIndex = 0;
			this.ColumnIndex = 0;
		}
		public Coordinate(Coordinate coordinate)
		{
			this.RowIndex = coordinate.RowIndex;
			this.ColumnIndex = coordinate.ColumnIndex;
		}
		public override string ToString()
		{
			return $"Row: {this.RowIndex} Column: {this.ColumnIndex}";
		}

		public void MoveRight()
		{
			this.ColumnIndex++;
		}

		public void MoveLeft()
		{
			this.ColumnIndex--;
		}

		public void MoveUp()
		{
			this.RowIndex--;
		}

		public void MoveDown()
		{
			this.RowIndex++;
		}
	}

	class WordSearch
	{
		public int numberOfColumns = 0;
		public int numberOfRows = 0;
		public List<List<char>> grid = new List<List<char>>();
		public Dictionary<char, List<Coordinate>> charIndex = new Dictionary<char,List<Coordinate>>();
		public void LoadStrings(string filePath)
		{
			int columnIndex = 0;
			int rowIndex = 0;
			List<Coordinate> currentCharIndex;
			foreach (string line in System.IO.File.ReadLines(filePath))
			{
				columnIndex = 0;
				List<char> rowOfChars = new List<char>();
				var chars = line.ToCharArray();
				this.numberOfColumns = chars.Length;
				foreach (char c in chars)
				{
					rowOfChars.Add(c);
					Coordinate coordinate = new Coordinate()
					{
						ColumnIndex = columnIndex,
						RowIndex = rowIndex
					};
					if (! charIndex.TryGetValue(c, out currentCharIndex))
					{
						currentCharIndex = new List<Coordinate>();
						this.charIndex.Add(c, currentCharIndex);
					}
					currentCharIndex.Add(coordinate);
					columnIndex++;
				}
				grid.Add(rowOfChars);
				this.numberOfRows++;
				rowIndex++;
			}
		}

		public bool SearchRight(Coordinate startingCoordinate, char[] chars)
		{
			// Check if the search pattern would be too long
			if (startingCoordinate.ColumnIndex + chars.Length > this.numberOfColumns)
			{
				return false;
			}
			Coordinate currentPosition = new Coordinate(startingCoordinate);
			int patternIndex = 0;
			while (patternIndex < chars.Length && currentPosition.RowIndex < this.numberOfRows && currentPosition.ColumnIndex < this.numberOfColumns)
			{
				if (this.grid[currentPosition.RowIndex][currentPosition.ColumnIndex] != chars[patternIndex])
				{
					return false;
				}
				patternIndex++;
				currentPosition.MoveRight();
			}
			return true;
		}

		public bool SearchDown(Coordinate startingCoordinate, char[] chars)
		{
			// Check if the search pattern would be too long
			if (startingCoordinate.RowIndex + chars.Length > this.numberOfRows)
			{
				return false;
			}
			Coordinate currentPosition = new Coordinate(startingCoordinate);
			int patternIndex = 0;
			while (patternIndex < chars.Length && currentPosition.RowIndex < this.numberOfRows && currentPosition.ColumnIndex < this.numberOfColumns)
			{
				if (this.grid[currentPosition.RowIndex][currentPosition.ColumnIndex] != chars[patternIndex])
				{
					return false;
				}
				patternIndex++;
				currentPosition.MoveDown();
			}
			return true;
		}

		public bool SearchDownRight(Coordinate startingCoordinate, char[] chars)
		{
			// Check if the search pattern would be too long
			if (startingCoordinate.RowIndex + chars.Length > this.numberOfRows
				|| startingCoordinate.ColumnIndex + chars.Length > this.numberOfColumns)
			{
				return false;
			}
			Coordinate currentPosition = new Coordinate(startingCoordinate);
			int patternIndex = 0;
			while (patternIndex < chars.Length && currentPosition.RowIndex < this.numberOfRows && currentPosition.ColumnIndex < this.numberOfColumns)
			{
				if (this.grid[currentPosition.RowIndex][currentPosition.ColumnIndex] != chars[patternIndex])
				{
					return false;
				}
				patternIndex++;
				currentPosition.MoveRight();
				currentPosition.MoveDown();
			}
			return true;
		}

		public bool SearchDownLeft(Coordinate startingCoordinate, char[] chars)
		{
			// Check if the search pattern would be too long
			if (startingCoordinate.RowIndex + chars.Length > this.numberOfRows
				|| startingCoordinate.ColumnIndex - chars.Length < 0)
			{
				return false;
			}
			Coordinate currentPosition = new Coordinate(startingCoordinate);
			int patternIndex = 0;
			while (patternIndex < chars.Length && currentPosition.RowIndex < this.numberOfRows && currentPosition.ColumnIndex >= 0)
			{
				if (this.grid[currentPosition.RowIndex][currentPosition.ColumnIndex] != chars[patternIndex])
				{
					return false;
				}
				patternIndex++;
				currentPosition.MoveLeft();
				currentPosition.MoveDown();
			}
			return true;
		}

		public string Print()
		{
			StringBuilder builder = new StringBuilder();
			foreach (List<char> row in grid)
			{
				foreach (char c in row )
				{
					builder.Append(c);
				}
				builder.AppendLine();
			}
			return builder.ToString();
		}
	}
}
