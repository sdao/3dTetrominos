using System;
using System.Collections.Generic;

public static class TetrisPieceFactory {
  readonly static List<IntVector3> I;
  readonly static List<IntVector3> J;
  readonly static List<IntVector3> L;
  readonly static List<IntVector3> O;
  readonly static List<IntVector3> S;
  readonly static List<IntVector3> T;
  readonly static List<IntVector3> Z;

  static TetrisPieceFactory() {
    I = new List<IntVector3>();
    I.Add(new IntVector3(0, -2, 0)); // Pivot.
    I.Add(new IntVector3(0, -1, 0));
    I.Add(new IntVector3(0, -3, 0));
    I.Add(new IntVector3(0, -4, 0));

    J = new List<IntVector3>();
    J.Add(new IntVector3(0, -2, 0)); // Pivot.
    J.Add(new IntVector3(0, -1, 0));
    J.Add(new IntVector3(0, -3, 0));
    J.Add(new IntVector3(-1, -3, 0));

    L = new List<IntVector3>();
    L.Add(new IntVector3(0, -2, 0)); // Pivot.
    L.Add(new IntVector3(0, -1, 0));
    L.Add(new IntVector3(0, -3, 0));
    L.Add(new IntVector3(1, -3, 0));

    O = new List<IntVector3>();
    O.Add(new IntVector3(0, -1, 0)); // Pivot.
    O.Add(new IntVector3(0, -2, 0));
    O.Add(new IntVector3(-1, -1, 0));
    O.Add(new IntVector3(-1, -2, 0));

    S = new List<IntVector3>();
    S.Add(new IntVector3(0, -1, 0)); // Pivot.
    S.Add(new IntVector3(1, -1, 0));
    S.Add(new IntVector3(0, -2, 0));
    S.Add(new IntVector3(-1, -2, 0));

    T = new List<IntVector3>();
    T.Add(new IntVector3(0, -1, 0)); // Pivot.
    T.Add(new IntVector3(0, -2, 0));
    T.Add(new IntVector3(1, -1, 0));
    T.Add(new IntVector3(-1, -1, 0));

    Z = new List<IntVector3>();
    Z.Add(new IntVector3(0, -1, 0)); // Pivot.
    Z.Add(new IntVector3(-1, -1, 0));
    Z.Add(new IntVector3(0, -2, 0));
    Z.Add(new IntVector3(1, -2, 0));
  }

  public static List<IntVector3> NewPiece(TetrisPiece type) {
    switch (type) {
      case TetrisPiece.I:
        return I;
      case TetrisPiece.J:
        return J;
      case TetrisPiece.L:
        return L;
      case TetrisPiece.O:
        return O;
      case TetrisPiece.S:
        return S;
      case TetrisPiece.T:
        return T;
      case TetrisPiece.Z:
        return Z;
      default:
        throw new ArgumentException("Unknown piece type");
    }
  }
}

public enum TetrisPiece {
  I,
  J,
  L,
  O,
  S,
  T,
  Z
}
