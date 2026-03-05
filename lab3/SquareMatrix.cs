using System;

namespace MatrixCalculator {
  class SquareMatrix {
    private static double Epsilon = 1e-10;
    private static int HashMultiplier = 31;
    private static int HashPrecision = 1000;

    private int size;
    private double[,] data;

    public SquareMatrix(int matrixSize)
    {
      if (matrixSize <= 0)
        throw new MatrixException("Error: matrix size must be positive.");

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
        throw new MatrixException("Error: matrices have different sizes.");

      SquareMatrix result = new SquareMatrix(left.size);

      for (rowIndex = 0; rowIndex < left.size; ++rowIndex)
        for (colIndex = 0; colIndex < left.size; ++colIndex)
          result.data[rowIndex, colIndex] = left.data[rowIndex, colIndex] + right.data[rowIndex, colIndex];

      return result;
    }

    public static SquareMatrix operator *(SquareMatrix left, SquareMatrix right)
    {
      double sum;
      int rowIndex, colIndex;

      if (left.size != right.size)
        throw new MatrixException("Error: matrices have different sizes.");

      SquareMatrix result = new SquareMatrix(left.size);

      for (rowIndex = 0; rowIndex < left.size; ++rowIndex)
      {
        for (colIndex = 0; colIndex < left.size; ++colIndex)
        {
          sum = 0.0;
          for (int innerIndex = 0; innerIndex < left.size; ++innerIndex)
            sum += left.data[rowIndex, innerIndex] * right.data[innerIndex, colIndex];
          result.data[rowIndex, colIndex] = sum;
        }
      }
      return result;
    }

    public static bool operator >(SquareMatrix left, SquareMatrix right)
    {
      double leftDet, rightDet;

      if (left.size != right.size)
        throw new MatrixException("Error: matrices have different sizes.");

      leftDet = left.Determinant();
      rightDet = right.Determinant();
      return leftDet > rightDet;
    }

    public static bool operator <(SquareMatrix left, SquareMatrix right)
    {
      double leftDet, rightDet;

      if (left.size != right.size)
        throw new MatrixException("Error: matrices have different sizes.");

      leftDet = left.Determinant();
      rightDet = right.Determinant();
      return leftDet < rightDet;
    }

    public static bool operator ==(SquareMatrix left, SquareMatrix right)
    {
      int rowIndex, colIndex;

      if (ReferenceEquals(left, null) && ReferenceEquals(right, null))
        return true;

      if (ReferenceEquals(left, null) || ReferenceEquals(right, null))
        return false;

      if (left.size != right.size)
        return false;

      for (rowIndex = 0; rowIndex < left.size; ++rowIndex)
        for (colIndex = 0; colIndex < left.size; ++colIndex)
          if (Math.Abs(left.data[rowIndex, colIndex] - right.data[rowIndex, colIndex]) > Epsilon)
            return false;

      return true;
    }

    public static bool operator !=(SquareMatrix left, SquareMatrix right) => !(left == right);

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
        for (colIndex = 0; colIndex < size; ++colIndex)
          hash = hash * HashMultiplier + (int)(data[rowIndex, colIndex] * HashPrecision);

      return hash;
    }

    public double Determinant()
    {
      double det, sign;
      int subColIndex;
      int rowIndex, colIndex, origColIndex;

      if (size == 1)
        return data[0, 0];

      if (size == 2)
        return data[0, 0] * data[1, 1] - data[0, 1] * data[1, 0];

      det = 0.0;
      SquareMatrix subMatrix;

      for (colIndex = 0; colIndex < size; ++colIndex)
      {
        subMatrix = new SquareMatrix(size - 1);

        for (rowIndex = 1; rowIndex < size; ++rowIndex)
        {
          subColIndex = 0;
          for (origColIndex = 0; origColIndex < size; ++origColIndex)
          {
            if (origColIndex == colIndex)
              continue;
            subMatrix.data[rowIndex - 1, subColIndex] = data[rowIndex, origColIndex];
            ++subColIndex;
          }
        }

        sign = (colIndex % 2 == 0) ? 1.0 : -1.0;
        det += sign * data[0, colIndex] * subMatrix.Determinant();
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
        throw new MatrixException("Error: matrix is singular. Inverse does not exist.");

      SquareMatrix result = new SquareMatrix(size);
      SquareMatrix subMatrix;

      if (size == 1)
      {
        result.data[0, 0] = 1.0 / data[0, 0];
        return result;
      }

      for (rowIndex = 0; rowIndex < size; ++rowIndex)
      {
        for (colIndex = 0; colIndex < size; ++colIndex)
        {
          subMatrix = new SquareMatrix(size - 1);
          subRowIndex = 0;

          for (origRowIndex = 0; origRowIndex < size; ++origRowIndex)
          {
            if (origRowIndex == rowIndex)
              continue;

            subColIndex = 0;
            for (origColIndex = 0; origColIndex < size; ++origColIndex)
            {
              if (origColIndex == colIndex)
                continue;

              subMatrix.data[subRowIndex, subColIndex] = data[origRowIndex, origColIndex];
              ++subColIndex;
            }
            ++subRowIndex;
          }

          sign = ((rowIndex + colIndex) % 2 == 0) ? 1.0 : -1.0;
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
        return 1;

      thisDet = Determinant();
      otherDet = other.Determinant();

      if (Math.Abs(thisDet - otherDet) < Epsilon)
        return 0;

      return thisDet < otherDet ? -1 : 1;
    }

    public bool Equals(SquareMatrix other) => this == other;

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
          result += data[rowIndex, colIndex].ToString("F2") + " ";
        result += "]\n";
      }

      return result;
    }
  }
}