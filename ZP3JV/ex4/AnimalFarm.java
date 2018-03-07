/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package klunko.zp3jv.s04;
import java.util.ArrayList;
import java.util.List;
/**
 *
 * @author Misha
 */
public class AnimalFarm {
    private Animal type;
    private String name;
    private Pohlavi pohlavi;
    private String sound;
    
    
   public void add(Animal type, String name, Pohlavi pohlavi){
       this.type = type;
       this.name = name;
       this.pohlavi = pohlavi;
       this.sound = type.getSound();
   }
    
    public void list(){
        System.out.println(name + " je " + type + " a dělá " + sound + ".");
    }
}
