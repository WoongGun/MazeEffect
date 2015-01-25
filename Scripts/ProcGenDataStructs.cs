using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room {

	private int width;
	private int height;
	private int x;
	private int y;

	public int minSpan = 3;

	public Room (MazeContainer cont){
		x = cont.getX() + Random.Range(minSpan, cont.getW() / 3);
		y = cont.getY() + Random.Range(minSpan, cont.getH() / 3);
		width = cont.getW() - (x - cont.getX());
		height = cont.getH() - (y - cont.getY());
		width -= Random.Range(0, width/3);
		height -= Random.Range(0, height/3);
	}

	// Draws the given room into a bitmap
	public void drawRoom(ref int[,] bmp){
		if (x + width > bmp.GetLength(0) || y + height > bmp.GetLength(1)){
			Debug.Log("Something went wrong with your room drawing");
		} else {
			for (int i = 0; i < width; i++){
				for (int j = 0; j < height; j++){
					bmp [x + i,y + j] = 1;
				}
			}
		}
	}
}


// from http://gamedevelopment.tutsplus.com/tutorials/how-to-use-bsp-trees-to-generate-game-maps--gamedev-12268
public class MazeContainer {
	private int minSpan = 3;

	private int x;
	private int y;
	private int w;
	private int h;

	private MazeContainer leftChild = null; 
	private MazeContainer rightChild = null;
	private Vector4 room;


	public MazeContainer(int x, int y, int w, int h){
		this.x = x;
		this.y = y;
		this.h = h;
		this.w = w;
	}
	
	public bool split(){
		if (leftChild != null || rightChild != null){
			return false;
		} else {
			bool splitH = Random.Range(0.0f,1.0f) > .5f;
			if (w > h && h / w >= 0.05){
				splitH = false;
			} else if (h > w && w / h >= 0.05){
				splitH = true;
			}
			int max = (splitH ? h : w) - minSpan;

			if (max < minSpan)
				return false;

			int splitAt = Random.Range(minSpan, max);

			if (splitH) {
				leftChild = new MazeContainer(x, y, w, splitAt);
				rightChild = new MazeContainer(x, y + splitAt, w, h - splitAt);
			} else {
				leftChild = new MazeContainer(x, y, splitAt, h);
				rightChild = new MazeContainer(x + splitAt, y, w - splitAt, h);
			}
			return true;
		}
	}


	public int getX (){
		return x;
	}
	public int getY (){
		return y;
	}
	public int getW (){
		return w;
	}
	public int getH (){
		return h;
	}
	public MazeContainer getLeft(){
		return leftChild;
	}
	public MazeContainer getRight(){
		return rightChild;
	}
	public Vector4 getRoom(){
		return room;
	}
	
	public void createRooms(){
		if (leftChild != null || rightChild != null){
			if (leftChild != null)
				leftChild.createRooms();
			if (rightChild != null)
				rightChild.createRooms();
		} else {
			int roomX, roomY, roomW, roomH;
			roomW = Random.Range(3, w - 2);
			roomH = Random.Range(3, h - 2);
			roomX = Random.Range(1, w - roomW - 1);
			roomY = Random.Range(1, h - roomH - 1);
			room = new Vector4(roomX, roomY, roomW, roomH);
		}
	}
}

public class BinaryTree {
	
	private MazeContainer node = null;
	private BinaryTree left = null;
	private BinaryTree right = null;

	private List<MazeContainer> allNodes = new List<MazeContainer>();

	public BinaryTree (MazeContainer node){
		this.node = node;
	}

	public void getLeaves(BinaryTree root){
		if (root.getLeft() == null && root.getRight() == null){
			allNodes.Add(root.getNode());
		} else {
			getLeaves(root.left);
			getLeaves(root.right);
		}
	}

	public List<MazeContainer> getAllNodes(){
		return allNodes;
	}
	
	public MazeContainer getNode(){
		return node;
	}
	public BinaryTree getLeft(){
		return left;
	}
	public BinaryTree getRight(){
		return right;
	}
	public void setLeft (BinaryTree left){
		this.left = left;
	}
	public void setRight (BinaryTree right){
		this.right = right;
	}

} 

