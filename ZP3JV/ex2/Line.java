/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package klunko.zp3jv.s02;

/**
 * Třida přdedstvujicí úsečku
 * @author Michael Klunko
 * @version 1.0
 */
public class Line {
    public Point a;
    public Point b;
    
    
    public Line (Point a, Point b){
        this.a = a;
        this.b = b;
    }
    //Delka úsečky
    private double getLength(){
        return Math.sqrt(Math.pow(b.x - a.x, 2) + Math.pow(b.y - a.y, 2));
    }
    
    /**
     * Výpočet delky mezi libovolným bodem a úsečkou
     * @param p - libovolný bod
     * @return  delká typu double
     **/
    public double distance(Point p){
        float px = b.x - a.x;
        float py = b.y - a.y;
        float temp = (px * px) + (py * py);
        float u=((p.x - a.x) * px + (p.y - a.y) * py) / (temp);
        if(u > 1){
            u = 1;
        }
        else if(u < 0){
            u = 0;
        }
        float x = a.x + u * px;
        float y = a.y + u * py;

        float dx = x - p.x;
        float dy = y - p.y;
        double dist = Math.sqrt(dx * dx + dy * dy);
        return dist;
    }
}
