    readonly struct CompareResult : IEquatable<CompareResult>
    {
        public readonly int Value;
        public CompareResult(int result)
        {
            Value = result;
        }

        public override int GetHashCode() => Value.GetHashCode();

        public override bool Equals(object obj) => Equals(obj is CompareResult);

        public bool Equals(CompareResult other) => Value == other.Value;

        public bool LessThanRight => Value < 0;
        public bool GreaterThanRight => Value > 0;
        public bool EqualToRight => Value == 0;

        public static implicit operator CompareResult(int from) => new CompareResult(from);
    }