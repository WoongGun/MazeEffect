using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class MazeDriver : MonoBehaviour {

	public int boardWidth = 100;
	public int boardHeight = 100;
	public int iterations = 1;
	public int [,] blocks;
	public Material mat;

	private BinaryTree tree;
	private List<Room> rooms;


	void Awake(){
		blocks = new int[boardWidth, boardHeight];
		setZeros();
		printBlocks();

		MazeContainer mainContainer = new MazeContainer(0,0, boardWidth, boardHeight);

		tree = new BinaryTree(mainContainer.split(mainContainer, iterations));
		tree.getLeaves(tree);

		rooms = new List<Room>();
		Debug.Log("Drawing " + rooms.Count + " Rooms");

		foreach (MazeContainer mc in tree.getAllNodes()){
			Room newRoom = new Room(mc);
			rooms.Add(newRoom);
			newRoom.drawRoom(ref blocks);
		}
		Debug.Log("Drawing " + rooms.Count + " Rooms");
		printBlocks();
	}

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void setZeros (){
		for (int i = 0; i < boardWidth; i++){
			for (int j = 0; j < boardHeight; j++){
				blocks[i,j] = 0;
			}
		}
	}

	void printBlocks(){
		string s = "";
		for (int i = 0; i < boardWidth; i++){
			for (int j = 0; j < boardHeight; j++){
				s = s + blocks[i,j].ToString();
			}
			s += "\n";
		}
		print(s);
	}

	void drawMaze(){

	}
}
