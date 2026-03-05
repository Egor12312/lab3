using System;

namespace MatrixCalculator {
  class Program {
    private static int OperationAdd = 1;
    private static int OperationMultiply = 2;
    private static int OperationDetA = 3;
    private static int OperationDetB = 4;
    private static int OperationCompare = 5;
    private static int OperationInverseA = 6;
    private static int OperationInverseB = 7;
    private static int OperationDemo = 8;
    private static int OperationExit = 0;

    static void Main(string[] args)
    {
      double detA, detB;
      int matrixSize;
      int userChoice;

      try
      {
        SquareMatrix firstMatrix;
        SquareMatrix secondMatrix;
        SquareMatrix resultMatrix;
        SquareMatrix inverseMatrix;
        userChoice = 0;

        Console.WriteLine(" MATRIX CALCULATOR \n");

        Console.Write("Enter matrix size: ");
        matrixSize = int.Parse(Console.ReadLine());

        firstMatrix = new SquareMatrix(matrixSize);
        secondMatrix = new SquareMatrix(matrixSize);

        Console.WriteLine("\nEnter first matrix:");
        firstMatrix.InputMatrix();

        Console.WriteLine("\nEnter second matrix:");
        secondMatrix.InputMatrix();

        Console.WriteLine("\nFirst Matrix:");
        Console.WriteLine(firstMatrix.ToString());
        Console.WriteLine("Second Matrix:");
        Console.WriteLine(secondMatrix.ToString());

        do
        {
          try
          {
            Console.WriteLine("\n OPERATIONS ");
            // ИСПРАВЛЕНО: Вывод значений, а не названий переменных
            Console.WriteLine($"{OperationAdd}. A + B");
            Console.WriteLine($"{OperationMultiply}. A * B");
            Console.WriteLine($"{OperationDetA}. Determinant of A");
            Console.WriteLine($"{OperationDetB}. Determinant of B");
            Console.WriteLine($"{OperationCompare}. Compare A and B");
            Console.WriteLine($"{OperationInverseA}. Inverse of A");
            Console.WriteLine($"{OperationInverseB}. Inverse of B");
            Console.WriteLine($"{OperationDemo}. Class methods demo");
            Console.WriteLine($"{OperationExit}. Exit");
            Console.Write("Choose operation: ");
            userChoice = int.Parse(Console.ReadLine());

            if (userChoice == OperationAdd)
            {
              resultMatrix = firstMatrix + secondMatrix;
              Console.WriteLine("\nA + B:");
              Console.WriteLine(resultMatrix.ToString());
            }
            else if (userChoice == OperationMultiply)
            {
              resultMatrix = firstMatrix * secondMatrix;
              Console.WriteLine("\nA * B:");
              Console.WriteLine(resultMatrix.ToString());
            }
            else if (userChoice == OperationDetA)
            {
              detA = firstMatrix.Determinant();
              Console.WriteLine("\nDeterminant of A: " + detA.ToString("F4"));
            }
            else if (userChoice == OperationDetB)
            {
              detB = secondMatrix.Determinant();
              Console.WriteLine("\nDeterminant of B: " + detB.ToString("F4"));
            }
            else if (userChoice == OperationCompare)
            {
              Console.WriteLine("\nComparison Results:");
              Console.WriteLine("A > B: " + (firstMatrix > secondMatrix));
              Console.WriteLine("A < B: " + (firstMatrix < secondMatrix));
              Console.WriteLine("A == B: " + (firstMatrix == secondMatrix));
              Console.WriteLine("A != B: " + (firstMatrix != secondMatrix));
              Console.WriteLine("CompareTo: " + firstMatrix.CompareTo(secondMatrix));
            }
            else if (userChoice == OperationInverseA)
            {
              inverseMatrix = firstMatrix.Inverse();
              Console.WriteLine("\nInverse of A:");
              Console.WriteLine(inverseMatrix.ToString());
            }
            else if (userChoice == OperationInverseB)
            {
              inverseMatrix = secondMatrix.Inverse();
              Console.WriteLine("\nInverse of B:");
              Console.WriteLine(inverseMatrix.ToString());
            }
            else if (userChoice == OperationDemo)
            {
              Console.WriteLine("\n CLASS METHODS DEMONSTRATION ");
              Console.WriteLine("Equals: " + firstMatrix.Equals(secondMatrix));
              Console.WriteLine("GetHashCode of A: " + firstMatrix.GetHashCode());
              Console.WriteLine("GetHashCode of B: " + secondMatrix.GetHashCode());

              SquareMatrix clonedMatrix = firstMatrix.Clone();
              Console.WriteLine("\nClone of A:");
              Console.WriteLine(clonedMatrix.ToString());
              Console.WriteLine("Original equals clone: " + firstMatrix.Equals(clonedMatrix));
            }
          }
          catch (MatrixException error)
          {
            Console.WriteLine("Matrix Error: " + error.Message);
          }
          catch (Exception error)
          {
            Console.WriteLine("Unexpected Error: " + error.Message);
          }

        } while (userChoice != OperationExit);
      }
      catch (MatrixException error)
      {
        Console.WriteLine("Matrix Error during initialization: " + error.Message);
      }
      catch (Exception error)
      {
        Console.WriteLine("Standard Error during initialization: " + error.Message);
      }

      Console.WriteLine("\nPress any key to exit...");
      Console.ReadKey();
    }
  }
}