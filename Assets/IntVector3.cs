public struct IntVector3 {
  public readonly int x;
  public readonly int y;
  public readonly int z;

  public IntVector3(int x, int y, int z) {
    this.x = x;
    this.y = y;
    this.z = z;
  }

  public static IntVector3 operator +(IntVector3 lhs, IntVector3 rhs) {
    return new IntVector3(lhs.x + rhs.x, lhs.y + rhs.y, lhs.z + rhs.z);
  }

  public static IntVector3 operator -(IntVector3 lhs, IntVector3 rhs) {
    return new IntVector3(lhs.x - rhs.x, lhs.y - rhs.y, lhs.z - rhs.z);
  }

  public static IntVector3 operator -(IntVector3 rhs) {
    return new IntVector3(-rhs.x, -rhs.y, -rhs.z);
  }
}
