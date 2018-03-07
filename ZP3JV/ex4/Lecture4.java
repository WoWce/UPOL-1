/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package klunko.zp3jv.s04;
import java.util.ArrayList;
import java.util.Arrays;
import java.util.List;
/**
 *
 * @author Misha
 */
public class Lecture4 {

    static String formatStr(String format, Object... args){
        String out = new String();
        for (int k = 0, i = 0; i < format.length(); i++){
            if(format.charAt(i) == '%'){
                k = (format.charAt(i + 1) - 48);
                out += args[k].toString(); 
                i++; 
            }
            else out += format.charAt(i);
            
        }
        return out;
    }
    /**
     * @param args the command line arguments
     */
    public static void main(String[] args) {
        
        //System.out.println(num.size());
        //new NumericList(2);
        //System.out.println(num.size());
        
        List nlist = new ArrayList<>();
        nlist.add(1);
        nlist.add(4);
        nlist.add(3);
        nlist.add(4);
        NumericList numl = new NumericList<>(nlist); 
        NumericList num2 = numl.next();
        System.out.println("Size of numl.next():" + num2.size());
        System.out.println("Sum of numl: " + numl.sum());
        
        
        System.out.println(formatStr("A: %0; B: %1", 1, "XY"));
        AnimalFarm a = new AnimalFarm();
        a.add(Animal.pes, "Alik", Pohlavi.M);
        a.list();
        a.add(Animal.kacena, "Bobik", Pohlavi.F);
        a.list();
    }
    
}
