using System.Collections;

public class Container {
	private int x;
	private int y;
	private int w;
	private int h;
	private Vector2 centre;
	private Container parent = null;

	public Container(int x, int y, int w, int h){
		this.x = x;
		this.y = y;
		this.h = h;
		this.w = w;
		this.centre = Vector2(this.x + this.w/2, 
		                      this.y + this.h/2);
	}
}
