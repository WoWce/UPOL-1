/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package klunko.zp3jv.s02;

/**
 *
 * @author Michael Klunko
 * @version 1.0
 */
public class Lecture2 {

    /**
     * @param args the command line arguments
     */
    public static void main(String[] args) {
        
        Point bod = new Point(3, 8);
        Point bod1 = new Point(1, 1);
        Point bod2 = new Point(1, 5);
        Point bod3 = new Point(5, 6);
        Point bod4 = new Point(6, 1);
        Point bod_c = new Point(4, 0);
        Point bod_c2 = new Point(0, 0);
        //nový kruh
        Circle circle = new Circle(bod_c2, 2);
        //nový úsek
        Line line = new Line(bod1, bod2);
        //nový obdelník
        Rectangle rectangle = new Rectangle(bod1, bod3);
        Rectangle rectangle2 = new Rectangle(bod1, 4, 5);
        System.out.println("Point 'bod' coordinates: (" 
                + bod.x + "," + bod.y + ")");
        System.out.println("From point 'bod' to segment 'line':" 
                + line.distance(bod));
        System.out.println("Rectangle sqr: " + rectangle.getArea());
        System.out.println("'bod' to Rect.: " + rectangle.distance(bod));
        System.out.println("Circle to 'bod_c': " + circle.distance(bod_c) +
                ", area: " + circle.getArea());
        
            
                
        
    }
    
}

