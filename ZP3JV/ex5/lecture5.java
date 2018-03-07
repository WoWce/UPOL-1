/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package klunko.zp3jv.s05;

import java.util.Arrays;
import java.util.List;
import klunko.zp3jv.s05.Filtration.Condition;
import klunko.zp3jv.s05.Map.Mapping;


/**
 *
 * @author Michael KLunko
 */
public class lecture5 {

    static String repeatChar1(char c, int n){
        long beginTime = System.currentTimeMillis();
        String s = new String();
        for(int i = 0; i < n; i++){
            s += c;
        }
        long endTime = System.currentTimeMillis();
        long difference = endTime - beginTime;
        System.out.println("repeatChar1 100k chars time:" + difference);
        return s;
    }
    static String repeatChar2(char c, int n){
        long beginTime = System.currentTimeMillis();
        String s = new String();
        StringBuilder sb = new StringBuilder();
        for(int i = 0; i < n; i++){
            sb.append(c);
        }
        s = sb.toString();
        long endTime = System.currentTimeMillis();
        long difference = endTime - beginTime;
        System.out.println("repeatChar2 100k chars time:" + difference);
        return s;
    }
    
    static Condition c1 = new Condition() {
            @Override
            public Object filter(Object o){
                if((int)o % 2 == 0){
                    return o;
                }
                else{return "odd";}
            }
        };
    /**
     * @param args the command line arguments
     */
    public static void main(String[] args) {
        List<Object> list = Arrays.asList(1, 2, 3, 4, 5, 6, 7, 8, 9);
        
        Map add = new Map();
        Mapping twice = o -> (int)o + (int)o;
        System.out.println(add.map(list, twice));
        
        Filtration even = new Filtration();
        Filtration.Cond f = even.new Cond();  //inner class Cond
        System.out.println(even.filter(list, f));
        System.out.println(even.filter(list, c1));  //anonymous class
        
        Condition c2 = (Object o) -> {
        if((int)o % 2 == 0){
            return o;
        }
        else{return "odd";}
    };
        System.out.println(even.filter(list, c2));  //lambda
        repeatChar1('a', 100000);
        repeatChar2('a', 100000);
    }
    
}
