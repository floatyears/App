//using UnityEngine;
//using System.Collections;
//
//public struct Coordinate {
//	public int X;
//	public int Y;
//}
//
//
//public class MTQP {
//	private Coordinate[][] checkerBoard;
//	private const int length = 8; // this value must > 5 && value must be even number;
//
//	public MTQP (Coordinate roleInit) {
//		checkerBoard = new Coordinate[length] [length];
//
//		for (int i = 0; i < length; i++) {
//			for (int j = 0; j < length; j++) {
//				Coordinate coor = new Coordinate();
//				coor.X = i;
//				coor.Y = j;
//				checkerBoard[i][j] = coor;
//			}
//		}
//
//		Coordinate coorDinate = roleInit;
//
//	}
//}
