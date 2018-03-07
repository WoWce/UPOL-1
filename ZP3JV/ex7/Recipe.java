/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package klunko.zp3jv.s07;


import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;
import java.util.Map;
import java.util.stream.Collectors;

/**
 *
 * @author Michael Klunko
 */
public class Recipe {
    
    Map<Ingredient, Integer> ingredients = new HashMap<>();
    
    void add(Ingredient i, int quantity){
        ingredients.put(i, quantity);
    }
    
    public String toString(){
        String result = new String();
        Map<String, Integer> nameQuantity = new HashMap<>();
        
        ingredients.entrySet().stream()
                .forEach(x -> nameQuantity
                        .put(x.getKey().getName(), x.getValue()));
        for(Map.Entry<String, Integer> name: nameQuantity.entrySet()){

            String key =name.getKey();
            String value = name.getValue().toString();  
            result += key + " - " + value + ", ";  

        } 
        return result;
    }
    
    boolean isCookable(List<Ingredient> availableIngredients){
        List<Ingredient> names = new ArrayList<>();
        ingredients.keySet().stream()
                .forEach(i -> names.add(i));
        if(availableIngredients.containsAll(names)){
            return true;
        }
        else{return false;}
    }
    
    int getPrice(){
        int price = ingredients.entrySet().stream()
                            .mapToInt(i -> i.getKey().getUPrice() * i.getValue())
                            .sum();
        return price;
    }
    
    List<String> getIngredientsByPrice(){
        Map<Integer, String> byPrice = new HashMap<>(); 
        ingredients.keySet().stream()
                .forEach(x -> byPrice.put(x.getUPrice(), x.getName()));
        
        List<String> byprice = byPrice.entrySet().stream()
                .sorted(Map.Entry.<Integer, String>comparingByKey())
                .map(i -> i.getValue())
                .collect(Collectors.toList());
        return byprice;
    }
    
    String getTheMostExpensiveIngredient(){
        int topprice = ingredients.entrySet().stream()
                            .mapToInt(i -> i.getKey().getUPrice())
                            .max()
                            .getAsInt();
        String name = ingredients.keySet().stream()
                    .filter(i -> i.getUPrice() == topprice)
                    .map(i -> i.getName())
                    .collect(Collectors.joining());
        return "The most expensive ingredient: " + name;
    }
}

