using System.Collections;
using UnityEngine;

/*
 * All credit for algorithm to:
 * http://eskerda.com/bsp-dungeon-generation/
 * 
 */

public class Container {
	private int x;
	private int y;
	private int w;
	private int h;
	private Vector2 centre;
	private Container parent = null;

	private int minSpan = 5;
	private int buffer = 3;

	public Container(int x, int y, int w, int h){
		this.x = x;
		this.y = y;
		this.h = h;
		this.w = w;
		this.centre = Vector2(this.x + this.w/2, 
		                      this.y + this.h/2);
	}
	
	public Container split(Container toSplit, int iter){
		BinaryTree root = new BinaryTree(toSplit);

		while (iter != 0){
			Container [] containers = randomSplit(toSplit);
			root.setLeft(split(containers[0], iter-1));
			root.setRight(split(containers[1], iter-1));
		}
		return root;
	}

	public Container [] randomSplit(Container cont){
		Container r1, r2;
		int splitDir = Random.Range(0, 1);

		if (splitDir == 0){
			// Vertical split
			r1 = new Container(cont.x, 
			                   cont.y, 
			                   Random.Range(1, cont.w),
			                   cont.h);
			r2 = new Container(cont.x + r1.w,
			                   cont.y,
			                   cont.w - r1.w,
			                   cont.h);
		} else {
			// Horizontal split
			r1 = new Container(cont.x, 
			                   cont.y, 
			                   cont.w,
			                   Random.Range(1, cont.h));
			r2 = new Container(cont.x + r1.w,
			                   cont.y,
			                   cont.w,
			                   cont.h - r1.h);
		}
		Continer [] ret = {r1, r2};
		return ret;
	}
}












