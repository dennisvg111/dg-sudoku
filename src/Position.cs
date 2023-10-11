using DG.Common;
using DG.Sudoku.Units;
using System;

namespace DG.Sudoku
{
    /// <summary>
    /// This class represents the zero-indexed position of a cell.
    /// </summary>
    public readonly struct Position : IEquatable<Position>
    {
        private readonly int _x;
        private readonly int _y;
        private readonly int _box;

        /// <summary>
        /// The zero-indexed column this cell is located in.
        /// </summary>
        public int X => _x;

        /// <summary>
        /// The zero-indexed row this cell is located in.
        /// </summary>
        public int Y => _y;

        /// <summary>
        /// The zero-indexed box this cell is located in.
        /// </summary>
        public int Box => _box;

        /// <summary>
        /// Creates a new instance of <see cref="Position"/> with the given x and y index.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public Position(int x, int y)
        {
            _x = x;
            _y = y;
            _box = (x / 3) + ((y / 3) * 3);
        }

        /// <summary>
        /// Creates a new instance of <see cref="Position"/> with the given x and y index.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static Position For(int x, int y)
        {
            return new Position(x, y);
        }

        /// <summary>
        /// Returns the zero-based index of this position in the given type of unit.
        /// </summary>
        /// <param name="unit"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public int GetIndex(UnitType unit)
        {
            switch (unit)
            {
                case UnitType.Box:
                    return Box;
                case UnitType.Row:
                    return Y;
                case UnitType.Column:
                    return X;
                default:
                    throw new NotImplementedException($"Method {nameof(GetIndex)} is not implemented for unit type {unit}.");
            }
        }

        /// <inheritdoc/>
        public override bool Equals(object obj)
        {
            return obj is Position && Equals((Position)obj);
        }

        /// <inheritdoc/>
        public bool Equals(Position other)
        {
            return _x == other._x && _y == other._y;
        }

        /// <summary>
        /// Indicates if two positions are equal to eachother.
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator ==(Position left, Position right)
        {
            return left.Equals(right);
        }

        /// <summary>
        /// Indicates if two positions are not equal to eachother.
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator !=(Position left, Position right)
        {
            return !left.Equals(right);
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            return HashCode.Of(_x)
                .And(_y);
        }

        /// <summary>
        /// Returns a string that references this position.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"{_x},{_y}";
        }
    }
}
