using System;

namespace MatrixCalculator {
  class MatrixException : Exception {
    public MatrixException(string message) : base(message)
    {
    }
  }
}