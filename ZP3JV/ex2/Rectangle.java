/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package klunko.zp3jv.s02;

/**
 * Třida přdedstvujicí obdélník
 * @author Michael Klunko
 * @version 1.0
 */
public class Rectangle {
    public int width = 0;
    public int height = 0;
    public Point a = new Point(0, 0);
    public Point b = new Point(0, 0);
    public Point c = new Point(0, 0);
    public Point d = new Point(0, 0);
    
    public Rectangle(Point a,Point c){
        
        this.a = a;
        this.c = c;
        this.b.y = a.y;
        this.b.x = c.x;
        this.d.y = c.y;
        this.d.x = a.x;
        this.height = Math.abs(a.y - d.y);
        this.width = Math.abs(a.x - b.x);
    }
    public Rectangle(Point d, int h, int w){
        
        this.d = d;
        this.a.y = d.y + height;
        this.a.x = d.x;
        this.c.x = d.x + width;
        this.c.y = d.y;
        width = w;
        height = h;
    }
    
    /**
     * Vypočet plochy
     * @return 
     */
    public int getArea() {
    return width * height;
    }
    /**
     * Vypočet delky do libovolného bodu
     * @param p - libovolny bod
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
