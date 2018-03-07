/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package klunko.zp3jv.s02;

/**
 * Třida přdedstvujicí čtverec
 * @author Michael Klunko
 * @version 1.0
 */
public class Square {
        
    public int height = 0;
    public Point a = new Point(0, 0);
    public Point b = new Point(0, 0);
    public Point c = new Point(0, 0);
    public Point d = new Point(0, 0);
    
    public Square(Point d, int height){
        a.x = d.x;
        a.y = d.y + height;
        c.y = d.y;        
        c.x = d.x + height;
        b.x = c.x;
        b.y = a.y;        
    }
    /**
     * Vypočet plochy
     * @return int plocha
     */
    public int getArea() {
    return height * height;
    }
    /**
     * Vypočet delky do libovolneho bodu 
     * @param p - bod
     * @return double delka
     */
    public double distance(Point p){
        
        Line AB = new Line(a, b);
        Line BC = new Line(b, c);
        Line CD = new Line(c, d);
        Line AD = new Line(a, d);
        double ABC = Math.min(AB.distance(p), BC.distance(p));
        double CDA = Math.min(CD.distance(p), AD.distance(p));
        return Math.min(ABC, CDA);
    }
}
