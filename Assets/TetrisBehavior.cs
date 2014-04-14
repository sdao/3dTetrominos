using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Random = UnityEngine.Random;

public class TetrisBehavior : MonoBehaviour {
  public const int WIDTH = 10;
  public const int HEIGHT = 10;
  public const int HEIGHT_EXTRA = 5;
  public const int LENGTH = 10;

  static readonly TetrisPiece[] TETRIS_VALUES =
    (TetrisPiece[])System.Enum.GetValues(typeof(TetrisPiece));

  readonly TetrisBlock[,,] blocks =
    new TetrisBlock[WIDTH, HEIGHT + HEIGHT_EXTRA, LENGTH];
  readonly List<TetrisBlock> activeBlocks = new List<TetrisBlock>();

  float lastTime = 0;
  bool gameOver = false;

  public GameObject iCube;
  public GameObject jCube;
  public GameObject lCube;
  public GameObject oCube;
  public GameObject sCube;
  public GameObject tCube;
  public GameObject zCube;
  public Camera defaultCamera;

	// Use this for initialization
	void Start () {
	}

	// Update is called once per frame
	void Update () {
    if (gameOver) return;

    if (activeBlocks.Count > 0) {
      if (Input.GetKeyDown(KeyCode.L)) {
        NudgePieceLeft();
      } else if (Input.GetKeyDown(KeyCode.Quote)) {
        NudgePieceRight();
      } else if (Input.GetKeyDown(KeyCode.P)) {
        NudgePieceForward();
      } else if (Input.GetKeyDown(KeyCode.Semicolon)) {
        NudgePieceBackward();
      } else if (Input.GetKeyDown(KeyCode.Space)) {
        while (activeBlocks.Count > 0) {
          AttemptPushPiece();
        }
      }
    }

    if (Time.time - lastTime > 1f) {
      // TODO: Check if a level can be destroyed.

      // If not, insert a new piece.
      if (activeBlocks.Count == 0) {
        NewPiece();
      } else {
        gameOver = !AttemptPushPiece();
      }

      lastTime = Time.time;
    }
	}

  bool IsCubeFilled(IntVector3 pt) {
    if (pt.x >= WIDTH || pt.y >= HEIGHT + HEIGHT_EXTRA || pt.z >= LENGTH) {
      throw new IndexOutOfRangeException("Cube indices were out of range");
    }

    return blocks[pt.x, pt.y, pt.z] != null;
  }

  void NewPiece() {
    if (activeBlocks.Count > 0) {
      throw new InvalidOperationException("Cannot add new piece at this time");
    }

    TetrisPiece t = TETRIS_VALUES[Random.Range(0, TETRIS_VALUES.Length - 1)];
    List<IntVector3> coords = TetrisPieceFactory.NewPiece(t);

    GameObject templateCube;
    switch (t) {
      case TetrisPiece.I:
        templateCube = iCube;
        break;
      case TetrisPiece.J:
        templateCube = jCube;
        break;
      case TetrisPiece.L:
        templateCube = lCube;
        break;
      case TetrisPiece.O:
        templateCube = oCube;
        break;
      case TetrisPiece.S:
        templateCube = sCube;
        break;
      case TetrisPiece.T:
        templateCube = tCube;
        break;
      case TetrisPiece.Z:
      default:
        templateCube = zCube;
        break;
    }

    foreach (IntVector3 pt in coords) {
      TetrisBlock newBlock = new TetrisBlock(templateCube,
        this,
        new IntVector3(WIDTH / 2 + pt.x,
          HEIGHT + HEIGHT_EXTRA + pt.y,
          LENGTH / 2 + pt.z));
      activeBlocks.Add(newBlock);
    }
  }

  bool AttemptPushPiece() {
    return NudgePiece(new IntVector3(0, -1, 0), true);
  }

  IntVector3 FindRightDirection() {
    Vector3 eye = defaultCamera.transform.position;
    float posX = Vector3.Dot(new Vector3(1f, 0f, 0f), eye);
    float negX = Vector3.Dot(new Vector3(-1f, 0f, 0f), eye);
    float posZ = Vector3.Dot(new Vector3(0f, 0f, 1f), eye);
    float negZ = Vector3.Dot(new Vector3(0f, 0f, -1f), eye);

    if (posX >= negX && posX >= posZ && posX >= negZ) {
      return new IntVector3(0, 0, 1);
    } else if (negX >= posX && negX >= posZ && negX >= negZ) {
      return new IntVector3(0, 0, -1);
    } else if (posZ >= posX && posZ >= negX && posZ >= negZ) {
      return new IntVector3(-1, 0, 0);
    } else {
      return new IntVector3(1, 0, 0);
    }
  }

  IntVector3 FindForwardDirection() {
    Vector3 eye = defaultCamera.transform.position;
    float posX = Vector3.Dot(new Vector3(1f, 0f, 0f), eye);
    float negX = Vector3.Dot(new Vector3(-1f, 0f, 0f), eye);
    float posZ = Vector3.Dot(new Vector3(0f, 0f, 1f), eye);
    float negZ = Vector3.Dot(new Vector3(0f, 0f, -1f), eye);

    if (posX >= negX && posX >= posZ && posX >= negZ) {
      return new IntVector3(-1, 0, 0);
    } else if (negX >= posX && negX >= posZ && negX >= negZ) {
      return new IntVector3(1, 0, 0);
    } else if (posZ >= posX && posZ >= negX && posZ >= negZ) {
      return new IntVector3(0, 0, -1);
    } else {
      return new IntVector3(0, 0, 1);
    }
  }

  /// <summary>
  /// Nudges a piece in the indicated direction.
  /// </summary>
  /// <param name="dir">The direction to nudge the piece.</param>
  /// <param name="freezeIfCantNudge">If the piece cannot be nudged in the
  /// given direction, determines whether the piece will be frozen in place
  /// instead.</param>
  /// <returns>Whether the piece is currently in the valid bounds.</returns>
  bool NudgePiece(IntVector3 dir, bool freezeIfCantNudge = false) {
    bool canNudge = true;
    bool inBounds = true;

    foreach (TetrisBlock block in activeBlocks) {
      IntVector3 pt = block.GetPosition();
      IntVector3 pushedPt = pt + dir;

      if (pushedPt.x < 0 || pushedPt.x >= WIDTH ||
          pushedPt.y < 0 || pushedPt.y >= HEIGHT + HEIGHT_EXTRA ||
          pushedPt.z < 0 || pushedPt.z >= LENGTH ||
          IsCubeFilled(pushedPt)) {
        canNudge = false;
      }

      if (pt.y >= HEIGHT) {
        inBounds = false;
      }
    }

    if (canNudge) {
      foreach (TetrisBlock block in activeBlocks) {
        IntVector3 pt = block.GetPosition();
        block.MoveToPosition(pt + dir);
      }

      return true;
    } else if (inBounds) {
      // Freeze the active blocks in place.
      if (freezeIfCantNudge) {
        foreach (TetrisBlock block in activeBlocks) {
          IntVector3 pt = block.GetPosition();
          blocks[pt.x, pt.y, pt.z] = block;
        }

        activeBlocks.Clear();
      }

      return true;
    } else {
      // Game over.
      return false;
    }
  }

  bool NudgePieceLeft() {
    return NudgePiece(-FindRightDirection());
  }

  bool NudgePieceRight() {
    return NudgePiece(FindRightDirection());
  }

  bool NudgePieceForward() {
    return NudgePiece(FindForwardDirection());
  }

  bool NudgePieceBackward() {
    return NudgePiece(-FindForwardDirection());
  }
}
