/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package klunko.zp3jv.s02;

/**
 * Třida přdedstvujicí kruh
 * @author Michael Klunko
 * @version 1.0
 */
public class Circle {
    
    public Point center;
    public int radius;
    
    public Circle(Point center, int radius) {
    super();
    this.center = center;
    this.radius = radius;
    }
    
    /**
     * Vypočet delky mezí centrem a libovolným bodem
     * @param p - parametr typu Point
     * @return Čislo
     */
    double distance(Point p){
      double dist = Math.sqrt(Math.pow(p.x - center.x, 2) 
               + Math.pow(p.y - center.y, 2)); 
      return Math.abs(dist - radius);
    }
    
    /**
     * Vypočet plochy
     * @return Čislo
     */
    double getArea() {
        return Math.PI * radius * radius;
    }
    
}
