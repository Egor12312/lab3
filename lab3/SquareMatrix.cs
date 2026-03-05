using System;

namespace MatrixCalculator {
  class SquareMatrix {
    private static double Epsilon;
    private static int HashMultiplier;
    private static int HashPrecision;

    private static int MatrixSizeOne;
    private static int MatrixSizeTwo;
    private static int IndexOffset;
    private static int StartIndex;
    private static double PositiveSign;
    private static double NegativeSign;
    private static int CompareEqual;
    private static int CompareLess;
    private static int CompareGreater;

    static SquareMatrix()
    {
      Epsilon = 1.0e-10;
      HashMultiplier = 31;
      HashPrecision = 1000;

      MatrixSizeOne = 1;
      MatrixSizeTwo = 2;
      IndexOffset = 1;
      StartIndex = 0;
      PositiveSign = 1.0;
      NegativeSign = -1.0;
      CompareEqual = 0;
      CompareLess = -1;
      CompareGreater = 1;
    }

    private int size;
    private double[,] data;

    public SquareMatrix(int matrixSize)
    {
      if (matrixSize <= 0)
      {
        throw new MatrixException("Error: matrix size must be positive.");
      }

      size = matrixSize;
      data = new double[size, size];
    }

    public void InputMatrix()
    {
      string input;
      int rowIndex, colIndex;

      Console.WriteLine("Enter matrix elements:");

      for (rowIndex = 0; rowIndex < size; ++rowIndex)
      {
        for (colIndex = 0; colIndex < size; ++colIndex)
        {
          Console.Write($"Element [{rowIndex}][{colIndex}]: ");
          input = Console.ReadLine();
          data[rowIndex, colIndex] = double.Parse(input);
        }
      }
    }

    public static SquareMatrix operator +(SquareMatrix left, SquareMatrix right)
    {
      int rowIndex, colIndex;

      if (left.size != right.size)
      {
        throw new MatrixException("Error: matrices have different sizes.");
      }

      SquareMatrix result = new SquareMatrix(left.size);

      for (rowIndex = 0; rowIndex < left.size; ++rowIndex)
      {
        for (colIndex = 0; colIndex < left.size; ++colIndex)
        {
          result.data[rowIndex, colIndex] = left.data[rowIndex, colIndex] + right.data[rowIndex, colIndex];
        }
      }

      return result;
    }

    public static SquareMatrix operator *(SquareMatrix left, SquareMatrix right)
    {
      double sum;
      int rowIndex, colIndex;

      if (left.size != right.size)
      {
        throw new MatrixException("Error: matrices have different sizes.");
      }

      SquareMatrix result = new SquareMatrix(left.size);

      for (rowIndex = 0; rowIndex < left.size; ++rowIndex)
      {
        for (colIndex = 0; colIndex < left.size; ++colIndex)
        {
          sum = 0.0;
          for (int innerIndex = 0; innerIndex < left.size; ++innerIndex)
          {
            sum += left.data[rowIndex, innerIndex] * right.data[innerIndex, colIndex];
          }
          result.data[rowIndex, colIndex] = sum;
        }
      }
      return result;
    }

    public static bool operator >(SquareMatrix left, SquareMatrix right)
    {
      double leftDet, rightDet;

      if (left.size != right.size)
      {
        throw new MatrixException("Error: matrices have different sizes.");
      }

      leftDet = left.Determinant();
      rightDet = right.Determinant();
      return leftDet > rightDet;
    }

    public static bool operator <(SquareMatrix left, SquareMatrix right)
    {
      double leftDet, rightDet;

      if (left.size != right.size)
      {
        throw new MatrixException("Error: matrices have different sizes.");
      }

      leftDet = left.Determinant();
      rightDet = right.Determinant();
      return leftDet < rightDet;
    }

    public static bool operator ==(SquareMatrix left, SquareMatrix right)
    {
      int rowIndex, colIndex;

      if (ReferenceEquals(left, null) && ReferenceEquals(right, null))
      {
        return true;
      }

      if (ReferenceEquals(left, null) || ReferenceEquals(right, null))
      {
        return false;
      }

      if (left.size != right.size)
      {
        return false;
      }

      for (rowIndex = 0; rowIndex < left.size; ++rowIndex)
      {
        for (colIndex = 0; colIndex < left.size; ++colIndex)
        {
          if (Math.Abs(left.data[rowIndex, colIndex] - right.data[rowIndex, colIndex]) > Epsilon)
          {
            return false;
          }
        }
      }

      return true;
    }

    public static bool operator !=(SquareMatrix left, SquareMatrix right)
    {
      return !(left == right);
    }

    public override bool Equals(object obj)
    {
      SquareMatrix other = obj as SquareMatrix;
      return this == other;
    }

    public override int GetHashCode()
    {
      int hash;
      int rowIndex, colIndex;

      hash = size;

      for (rowIndex = 0; rowIndex < size; ++rowIndex)
      {
        for (colIndex = 0; colIndex < size; ++colIndex)
        {
          hash = hash * HashMultiplier + (int)(data[rowIndex, colIndex] * HashPrecision);
        }
      }

      return hash;
    }

    public double Determinant()
    {
      double det, sign;
      int subColIndex;
      int rowIndex, colIndex, origColIndex;

      if (size == MatrixSizeOne)
      {
        return data[StartIndex, StartIndex];
      }

      if (size == MatrixSizeTwo)
      {
        return data[StartIndex, StartIndex] * data[MatrixSizeOne, MatrixSizeOne] - data[StartIndex, MatrixSizeOne] * data[MatrixSizeOne, StartIndex];
      }

      det = 0.0;
      SquareMatrix subMatrix;

      for (colIndex = 0; colIndex < size; ++colIndex)
      {
        subMatrix = new SquareMatrix(size - MatrixSizeOne);

        for (rowIndex = MatrixSizeOne; rowIndex < size; ++rowIndex)
        {
          subColIndex = 0;
          for (origColIndex = 0; origColIndex < size; ++origColIndex)
          {
            if (origColIndex == colIndex)
            {
              continue;
            }
            subMatrix.data[rowIndex - MatrixSizeOne, subColIndex] = data[rowIndex, origColIndex];
            ++subColIndex;
          }
        }

        if (colIndex % MatrixSizeTwo == 0)
        {
          sign = PositiveSign;
        }
        else
        {
          sign = NegativeSign;
        }
        det += sign * data[StartIndex, colIndex] * subMatrix.Determinant();
      }

      return det;
    }

    public SquareMatrix Inverse()
    {
      double det;
      double sign;
      double cofactor;
      int subRowIndex, subColIndex;
      int rowIndex, colIndex, origRowIndex, origColIndex;

      det = Determinant();
      if (Math.Abs(det) < Epsilon)
      {
        throw new MatrixException("Error: matrix is singular. Inverse does not exist.");
      }

      SquareMatrix result = new SquareMatrix(size);
      SquareMatrix subMatrix;

      if (size == MatrixSizeOne)
      {
        result.data[StartIndex, StartIndex] = PositiveSign / data[StartIndex, StartIndex];
        return result;
      }

      for (rowIndex = 0; rowIndex < size; ++rowIndex)
      {
        for (colIndex = 0; colIndex < size; ++colIndex)
        {
          subMatrix = new SquareMatrix(size - MatrixSizeOne);
          subRowIndex = 0;

          for (origRowIndex = 0; origRowIndex < size; ++origRowIndex)
          {
            if (origRowIndex == rowIndex)
            {
              continue;
            }

            subColIndex = 0;
            for (origColIndex = 0; origColIndex < size; ++origColIndex)
            {
              if (origColIndex == colIndex)
              {
                continue;
              }

              subMatrix.data[subRowIndex, subColIndex] = data[origRowIndex, origColIndex];
              ++subColIndex;
            }
            ++subRowIndex;
          }

          if ((rowIndex + colIndex) % MatrixSizeTwo == 0)
          {
            sign = PositiveSign;
          }
          else
          {
            sign = NegativeSign;
          }
          cofactor = sign * subMatrix.Determinant();
          result.data[colIndex, rowIndex] = cofactor / det;
        }
      }

      return result;
    }

    public int CompareTo(SquareMatrix other)
    {
      double thisDet, otherDet;

      if (other == null)
      {
        return CompareGreater;
      }

      thisDet = Determinant();
      otherDet = other.Determinant();

      if (Math.Abs(thisDet - otherDet) < Epsilon)
      {
        return CompareEqual;
      }

      if (thisDet < otherDet)
      {
        return CompareLess;
      }
      else
      {
        return CompareGreater;
      }
    }

    public bool Equals(SquareMatrix other)
    {
      return this == other;
    }

    public SquareMatrix Clone()
    {
      SquareMatrix clone = new SquareMatrix(size);
      Array.Copy(data, clone.data, data.Length);
      return clone;
    }

    public override string ToString()
    {
      string result;
      int rowIndex, colIndex;

      result = $"[{size}x{size}]:\n";

      for (rowIndex = 0; rowIndex < size; ++rowIndex)
      {
        result += "[ ";
        for (colIndex = 0; colIndex < size; ++colIndex)
        {
          result += data[rowIndex, colIndex].ToString("F2") + " ";
        }
        result += "]\n";
      }

      return result;
    }
  }
}