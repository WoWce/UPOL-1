/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package klunko.zp3jv.s04;

/**
 *
 * @author Michael Klunko
 */
public enum Animal {
    pes("haf-haf"),
    fena("haf-haf"),
    kacer("ga-ga"),
    kacena("ga-ga");
    private String sound;
    Animal(String sound){
        this.sound = sound;
    }
    public String getSound(){
        return sound;
    }
    
}
