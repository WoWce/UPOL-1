/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package klunko.zp3jv.s08;

/**
 *
 * @author Michael Klunko
 */
public class Lecture8 {

    /**
     * @param args the command line arguments
     */
    public static void main(String[] args) {
        RPN a = new RPN();
        System.out.println(a.rpnCalc("1.1 2 +"));
        System.out.println(a.rpnCalc("1 3 +"));
        System.out.println(a.rpnCalc("1.1 2 <"));
        System.out.println(a.rpnCalc("#f #t <"));
        System.out.println(a.rpnCalc("#f #t >"));
        System.out.println(a.rpnCalc("1 2 3 4 < ?"));
        //Exception tests
        //System.out.println(a.rpnCalc("1 #f ="));
        //System.out.println(a.rpnCalc("1 2 3 4 + ?"));
    }
    
}
