/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package klunko.zp3jv.s07;

import java.util.ArrayList;
import static java.util.Arrays.stream;
import java.util.List;
import java.util.stream.Collectors;
/**
 *
 * @author Michael Klunko
 */
public class Lecture7 {

    static int[] odd(int[] arg){
        return arg = stream(arg)
                        .filter(n -> n % 2 == 1)
                        .toArray();
                   
    }
    
    static List<Integer> oddNumbers(List<Integer> arg){
        return arg = arg.stream()
            .filter(n -> (n % 2 == 1))
            .collect(Collectors.toList());
        
    }
    /**
     * @param args the command line arguments
     */
    public static void main(String[] args) {
        //List<Integer> oddNumbers(List<Integer> arg) using stream
        List<Integer> a = new ArrayList<>();
        a.add(1);
        a.add(2);
        a.add(3);
        a.add(4);
        a = oddNumbers(a);
        a.forEach(n -> System.out.print(n + ", "));
        System.out.println();
        
        //int[] odd(int[] arg) using stream
        int[] b = {1, 2, 3, 4, 5, 6, 7};
        for(int i = 0; i < odd(b).length; i++){
            System.out.print(odd(b)[i] + ", ");
        }
        System.out.println();
        
        
        //cooker
        Ingredient potato = new Ingredient("potato", "kg", 10);
        Ingredient water = new Ingredient("water", "l", 4);
        Ingredient mushrooms = new Ingredient("mushrooms", "kg", 70);
        Recipe dish = new Recipe();
        dish.add(potato, 1);
        dish.add(water, 3);
        dish.add(mushrooms, 1);
        //list of availiable ingredients
        List<Ingredient> foodlist = new ArrayList<>();
        foodlist.add(water);
        foodlist.add(potato);
        foodlist.add(mushrooms);
        
        //class methods usage
        System.out.println("Ingredients - quantity: " + dish.toString());
        System.out.println("Total price: " + dish.getPrice() + " Kc");
        System.out.println("Is dish cookable: " + dish.isCookable(foodlist));
        System.out.println(dish.getTheMostExpensiveIngredient());
        System.out.println("Cheapest to the most expensive: " + 
                dish.getIngredientsByPrice());
    }
    
}
