/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package klunko.zp3jv.s02;

/**
 * Třida přdedstvujicí bod
 * @author Michael Klunko
 * @version 1.0
 */
public class Point {
    public int x;
    public int y;
    public Point (int a, int b){
        x = a;
        y = b;
    }
    /**
    * Delka do libovolného bodu
    * @param p - libovolný bod
    * @return - double 
    **/
    double distance(Point p){
        return Math.sqrt(Math.pow((p.x - x), 2) + Math.pow((p.y - y), 2));
    }
}
