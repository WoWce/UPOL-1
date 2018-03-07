/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package klunko.zp3jv.s07;

/**
 *
 * @author Michael Klunko
 */
public class Ingredient {
    private String name;
    private String unit;
    private int uPrice;
    
    Ingredient(String name, String unit, int price){
        this.name = name;
        this.unit = unit;
        this.uPrice = price;
    }
    
    String getName(){
        return this.name;
    }
    
    int getUPrice(){
        return this.uPrice;
    }
}
