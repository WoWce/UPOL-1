/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package klunko.zp3jv.s03;
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
    double distance(Point p){
        return Math.sqrt(Math.pow((p.x - x), 2) + Math.pow((p.y - y), 2));
    }
    static Point nearest(Point p, List<Point> points){
        double[] distances = new double[points.size()];
        int i = 0;
        for (Iterator<Point> iterator = points.iterator(); iterator.hasNext();){
            Point number = iterator.next();
            
            double dist = Math.sqrt(Math.pow((p.x - number.x), 2) 
                    + Math.pow((p.y - number.y), 2));
            
            distances[i] = dist;
            i++;
            
        }
        double min = distances[0];
        int pos = 0;
        for(int j = 0; j < points.size(); j++){
            if(distances[j] < min){
                min = distances[j];
                pos = j;
            }
        }
            
        System.out.println("Nearest Point from List to Point p - x:" 
                + points.get(pos).x + ", y:" + points.get(pos).y);
        return points.get(pos);
    }
}

