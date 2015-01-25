using System.Collections;
using UnityEngine;

public class Room {

	private int width;
	private int height;
	private int x;
	private int y;

	public Room (Container cont){
		x = cont.getX;
		y = cont.getY;

	}

}

public class Container {
	private int x;
	private int y;
	private int w;
	private int h;
	private Vector2 centre;
	private Container parent = null;

	private int minSpan = 5;
	private int buffer = 3;
	private float W_RATIO = .3;
	private float H_RATIO = .3;
	private bool ENFORCE_RATIO = true;


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
			if (ENFORCE_RATIO){
				float r1_rat = r1.w/r1.h;
				float r2_rat = r2.w/r2.h;
				if (r1_rat < W_RATIO || r2_rat < W_RATIO)
					return randomSplit(cont);
			}
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
			if (ENFORCE_RATIO){
				float r1_rat = r1.h/r1.w;
				float r2_rat = r2.h/r2.w;
				if (r1_rat < H_RATIO || r2_rat < H_RATIO)
					return randomSplit(cont);
			}
		}
		Continer [] ret = {r1, r2};
		return ret;
	}
}

