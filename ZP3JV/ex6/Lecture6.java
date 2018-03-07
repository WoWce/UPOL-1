/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package klunko.zp3jv.s06;

import java.util.ArrayDeque;
import java.util.Deque;
import java.util.HashSet;
import java.util.HashMap;
import java.util.Map;
import java.util.Set;
import static jdk.nashorn.internal.runtime.JSType.isNumber;

/**
 *
 * @author Misha
 */
public class Lecture6 {

    static Map<String, Integer> freq(String s){
        Map<String, Integer> occurrences = new HashMap<>(); 
        String temp = s.replaceAll("[!-@]", " ");
        for ( String word : temp.split("\\s+")) {
            Integer oldCount = occurrences.get(word);
            if ( oldCount == null ) {
                oldCount = 0;
            }
            occurrences.put(word, oldCount + 1);
        }
        return occurrences;
    }
    
    static Map<String, Integer> freqIgnoreCase(String s){
        Map<String, Integer> occurrences = new HashMap<>(); 
        String temp = s.toLowerCase().replaceAll("[!-@]", " ");
        for ( String word : temp.split("\\s+")) {
            Integer oldCount = occurrences.get(word);
            if ( oldCount == null ) {
                oldCount = 0;
            }
            occurrences.put(word, oldCount + 1);
        }
        return occurrences;
    }
    
    static double rpnCalc(String expr){
        Deque<Double> stack = new ArrayDeque<>();
        for (String symbol : expr.split("\\s+")) {
            switch (symbol) {
                case "+":
                    stack.push(stack.pop() + stack.pop());
                    break;
                case "-":
                    stack.push(-stack.pop() + stack.pop());
                    break;
                case "*":
                    stack.push(stack.pop() * stack.pop());
                    break;
                case "/":
                    double divide = stack.pop();
                    stack.push(stack.pop() / divide);
                    break;
                default:
                    stack.push(Double.parseDouble(symbol));
                    break;
            } 
        }
        return stack.pop();
    }
    static double rpnCalc(String expr, Map <String, Integer> variables)
                    throws IllegalArgumentException
    {
        Deque<Double> stack = new ArrayDeque<>();
        for (String symbol : expr.split("\\s+")) {
            switch (symbol) {
                case "+":
                    stack.push(stack.pop() + stack.pop());
                    break;
                case "-":
                    stack.push(-stack.pop() + stack.pop());
                    break;
                case "*":
                    stack.push(stack.pop() * stack.pop());
                    break;
                case "/":
                    double divide = stack.pop();
                    stack.push(stack.pop() / divide);
                    break;
                default:
                    if(variables.containsKey(symbol)){
                        Double a = (double)variables.get(symbol);
                        stack.push(a);
                    }                    
                    else {
                        stack.push(Double.parseDouble(symbol));
                    }
                    break;
            } 
        }
        return stack.pop();
    }
    /**
     * @param args the command line arguments
     */
    public static void main(String[] args) {
        Set<Point> setp = new HashSet();
        Point a = new Point(1, 2);
        Point a1 = new Point(2, 1);
        Point b = new Point(5, 7);
        Point c = new Point(9, 0);
        Point d = new Point(1, 2);
        Point e = new Point(-1, -1);
        Point f = new Point(9, 0);
        setp.add(a);
        setp.add(a1);
        setp.add(b);
        setp.add(c);
        setp.add(d);
        setp.add(e);
        setp.add(f);
        setp.forEach(n -> System.out.println(n.x + "." + n.y + " "));
        
        String s = "Descriptive writing creates an impression in the reader’s "
                + "mind of an event, a place, a person, or thing. descriptive";
        System.out.println(freq(s));
        System.out.println(freqIgnoreCase(s));
        
        
        System.out.println("rpn #1: " + rpnCalc("5 1 2 + 4 * + 3 -"));
        
        Map<String, Integer> var = new HashMap<>();
        var.put("foo", 7);
        System.out.println("rpn #2: " + rpnCalc("1 foo +", var));
        
    }
    
}
