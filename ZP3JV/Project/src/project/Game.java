/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package project;

import java.awt.EventQueue;
import javax.swing.JFrame;
/**
 * 
 * @author Mykhailo Klunko
 */

public class Game extends JFrame {
    static Field f;
    public Game() {
        f = new Field();
        add(f);
        pack();
        setResizable(false);
        setTitle("Java 2D Game");
        setLocationRelativeTo(null);
        setDefaultCloseOperation(JFrame.EXIT_ON_CLOSE);
    }
    

    public static void main(String[] args) {
        
        
        EventQueue.invokeLater(new Runnable() {
            @Override
            public void run() {                
                JFrame ex = new Game();
                ex.setVisible(true);    
                getT();
            }
        });
        
        
    }
    
    public static void getT(){
        while(true){
            System.out.println(f.getRight());
        }
    }
}

