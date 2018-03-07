/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package klunko.zp3jv.s06;
import java.util.Iterator;
import java.util.List;
import java.util.Arrays;

/**
 *
 * @author Misha
 */
public class Point {
    public int x;
    public int y;
    public Point (int a, int b){
        x = a;
        y = b;
    }
    @Override
    public int hashCode() { //wrong! x.y override y.x
        final int prime = 31;
	int result = 1;
	
        result = prime * result + x;
	result = prime * result + y;
	return result;
    }
    @Override
	public boolean equals(Object obj) {
		if (this == obj)
			return true;
		if (obj == null)
			return false;
		if (getClass() != obj.getClass())
			return false;
		Point other = (Point) obj;
		if (x != other.x)
			return false;
		if (y != other.y)
			return false;
		return true;
	}
}

