using System.Collections;

public class BinaryTree {

	private Container node;
	private BinaryTree left = null;
	private BinaryTree right = null;

	public BinaryTree (Container node){
		this.node = node;
	}

	public void setLeft (BinaryTree left){
		this.left = left;
	}
	public void setRight (BinaryTree right){
		this.right = right;
	}

	public int getHeight (BinaryTree tree){
		// return height of tree
	}
} 