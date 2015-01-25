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

}

public class MazeContainer {
	private int x;
	private int y;
	private int w;
	private int h;
	private Vector2 centre;
//	private MazeContainer parent = null;
//
//	private int minSpan = 5;
//	private int buffer = 3;
	private float W_RATIO = .3f;
	private float H_RATIO = .3f;
	private bool ENFORCE_RATIO = true;


	public MazeContainer(int x, int y, int w, int h){
		this.x = x;
		this.y = y;
		this.h = h;
		this.w = w;
		this.centre = new Vector2(this.x + this.w/2, 
		                      this.y + this.h/2);
	}
	
	public MazeContainer split(MazeContainer toSplit, int iter){
		BinaryTree root = new BinaryTree(toSplit);

		while (iter != 0){
			MazeContainer [] containers = randomSplit(toSplit);
			root.setLeft(new BinaryTree(split(containers[0], iter-1)));
			root.setRight(new BinaryTree(split(containers[1], iter-1)));
		}
		return root.getNode();
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

	public MazeContainer [] randomSplit(MazeContainer cont){
		MazeContainer r1, r2;
		int splitDir = Random.Range(0, 1);

		if (splitDir == 0){
			// Vertical split
			r1 = new MazeContainer(cont.x, 
			                   cont.y, 
			                   Random.Range(1, cont.w),
			                   cont.h);
			r2 = new MazeContainer(cont.x + r1.w,
			                   cont.y,
			                   cont.w - r1.w,
			                   cont.h);
			if (ENFORCE_RATIO){
				float r1_rat = r1.w/r1.h;
				float r2_rat = r2.w/r2.h;
				if (r1_rat < W_RATIO || r2_rat < W_RATIO)
					return randomSplit(cont);
			}
		} else {
			// Horizontal split
			r1 = new MazeContainer(cont.x, 
			                   cont.y, 
			                   cont.w,
			                   Random.Range(1, cont.h));
			r2 = new MazeContainer(cont.x + r1.w,
			                   cont.y,
			                   cont.w,
			                   cont.h - r1.h);
			if (ENFORCE_RATIO){
				float r1_rat = r1.h/r1.w;
				float r2_rat = r2.h/r2.w;
				if (r1_rat < H_RATIO || r2_rat < H_RATIO)
					return randomSplit(cont);
			}
		}
		MazeContainer [] ret = {r1, r2};
		return ret;
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
	
//	public int getHeight (BinaryTree tree){
//		// return height of tree
//		int maxH;
//		if (left != null){
//			 left.getHeight();
//		}
//		return 0;
//	}
} 

