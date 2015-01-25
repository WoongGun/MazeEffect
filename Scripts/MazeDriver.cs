using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MazeDriver : MonoBehaviour {

	public int boardWidth = 100;
	public int boardHeight = 100;
	public int iterations = 9;
	public int maxContainerSize = 20;
	public int [,] blocks;
	public Material mat;

	private BinaryTree tree;
	private List<MazeContainer> containers = new List<MazeContainer>();


	void Awake(){
		blocks = new int[boardWidth, boardHeight];
		setZeros();
		printBlocks();
		splitContainers();
		containers[0].createRooms();
		Debug.Log("Creating " + containers.Count + " rooms");
		drawRooms();
		printBlocks();
//		tree = new BinaryTree(mainContainer.split(mainContainer, iterations));
//		tree.getLeaves(tree);
//
//		rooms = new List<Room>();
//		Debug.Log("Drawing " + rooms.Count + " Rooms");
//
//		foreach (MazeContainer mc in tree.getAllNodes()){
//			Room newRoom = new Room(mc);
//			rooms.Add(newRoom);
//			newRoom.drawRoom(ref blocks);
//		}
//		Debug.Log("Drawing " + rooms.Count + " Rooms");
//		printBlocks();
	}
	
	void splitContainers(){
		MazeContainer mainContainer = new MazeContainer(0,0, boardWidth, boardHeight);
		
		containers.Add(mainContainer);
		
		bool pushSuccess = true;
		
		while (pushSuccess) {
			pushSuccess = false;
			
			for (int i = 0; i < containers.Count; i++){
				//MazeContainer mc in containers) {
				if (containers[i].getRight() == null && containers[i].getLeft() == null){
					if (containers[i].getW() > maxContainerSize || containers[i].getH() > maxContainerSize
					    || Random.Range(0.0f, 1.0f) > .25f){
						if (containers[i].split()){
							containers.Add(containers[i].getLeft());
							containers.Add(containers[i].getRight());
							pushSuccess = true;
						}
					}
				}
				
			}
		}
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

	void drawRooms(){
		foreach (MazeContainer r in containers){
			// room info is in a vector4 because I'm an asshole
			// Room:	x, y, w, h
			// Vector: 	w, x, y, z
			for (int i = 0; i < r.getRoom().y; i++){
				for (int j = 0; j < r.getRoom().z; j++){
					blocks[i + (int) r.getRoom().w,j + (int) r.getRoom().x] = 1;
				}
			}
		}
	}
}
