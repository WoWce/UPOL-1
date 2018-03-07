/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package project;

import java.awt.Color;
import java.awt.Dimension;
import java.awt.Font;
import java.awt.FontMetrics;
import java.awt.Graphics;
import java.awt.Image;
import java.awt.Toolkit;
import java.awt.event.ActionEvent;
import java.awt.event.ActionListener;
import java.awt.event.KeyAdapter;
import java.awt.event.KeyEvent;
import java.io.File;

import javax.swing.ImageIcon;
import javax.swing.JPanel;
import javax.swing.Timer;

/**
 * Class Field represents game action processing
 * @author Mykhailo Klunko
 */
public class Field extends JPanel implements ActionListener {

    //size of board
    private final int FIELD_WIDTH = 500;
    private final int FIELD_HEIGHT = 500;
    //size of apple & snake
    private final int DOT_SIZE = 10;
    //max dots on board (900 = (300*300)/(10*10))
    //                  (2500 = (500*500)/(10*10))
    private final int ALL_DOTS = 2500;
    //random pos of apple
    private final int RAND_POS = 49;
    //Timer delay. Less delay - faster speed of game
    private final int DELAY = 160;

    //x and y coordinates of all joints of a snake
    private final int x[] = new int[ALL_DOTS];
    private final int y[] = new int[ALL_DOTS];

    private int dots;
    private int aim_x;
    private int aim_y;

    private boolean leftDirection = false;
    private boolean rightDirection = true;
    private boolean upDirection = false;
    private boolean downDirection = false;
    private boolean atStart = true;
    private boolean inGame = true;

    private Timer timer;
    private Image part;
    private Image apple;
    private Image head;
    

    public Field() {

        addKeyListener(new KBoard());
        setBackground(Color.black);
        setFocusable(true);
        setPreferredSize(new Dimension(FIELD_WIDTH, FIELD_HEIGHT));
       
    }
    
    /**
     * Keys for gameplay implementing
     */
    private class KBoard extends KeyAdapter {

        @Override
        public void keyPressed(KeyEvent e) {

            int key = e.getKeyCode();

            /**
             * Start with "Enter" key
             */
            if ((key == KeyEvent.VK_ENTER)){
                openImages();
                start();
            }
            
            if ((key == KeyEvent.VK_LEFT) && (!rightDirection)) {
                leftDirection = true;
                upDirection = false;
                downDirection = false;
            }

            if ((key == KeyEvent.VK_RIGHT) && (!leftDirection)) {
                rightDirection = true;
                upDirection = false;
                downDirection = false;
            }

            if ((key == KeyEvent.VK_UP) && (!downDirection)) {
                upDirection = true;
                rightDirection = false;
                leftDirection = false;
            }

            if ((key == KeyEvent.VK_DOWN) && (!upDirection)) {
                downDirection = true;
                rightDirection = false;
                leftDirection = false;
            }
            
            if((key == KeyEvent.VK_ESCAPE)){
                System.exit(0);
            }
            
        }
    }

   
    /**
     * Loads images for the game
     * To load your own images, replace dot.png, apple.png & head.png in
     * "imgs" folder
     */
    private void openImages() {

        ImageIcon a = new ImageIcon("resources/dot.png");
        part = a.getImage();

        ImageIcon b = new ImageIcon("resources/apple.png");
        apple = b.getImage();

        ImageIcon c = new ImageIcon("resources/head.png");
        head = c.getImage();
    }

    /**
     * Create snake, locate apple, start timer
     */
    private void start() {

        atStart = false;
        dots = 4;

        for (int z = 0; z < dots; z++) {
            x[z] = 50 - z * 10;
            y[z] = 50;
        }

        locateAim();

        timer = new Timer(DELAY, this);
        timer.start();
    }

    @Override
    public void paintComponent(Graphics g) {
        super.paintComponent(g);

        draw(g);
    }
    
    private void draw(Graphics g) {
        if (atStart){
            startGame(g);
        }
        else if (inGame) {

            g.drawImage(apple, aim_x, aim_y, this);

            for (int z = 0; z < dots; z++) {
                if (z == 0) {
                    g.drawImage(head, x[z], y[z], this);
                } else {
                    g.drawImage(part, x[z], y[z], this);
                }
            }

            Toolkit.getDefaultToolkit().sync();

        } 
        else {

            gameOver(g);
            
        }        
    }

    private void gameOver(Graphics g){
        paintMsg(g, "Game Over");
    }
    
    private void startGame(Graphics g){
        paintMsg(g, "Press ENTER to start game");
    }
    
    private void paintMsg(Graphics g, String s) {
        
        String msg = s;
        String help = "Press ESC to exit";
        Font gameFont = new Font("Helvetica", Font.BOLD, 14);
        FontMetrics metr = getFontMetrics(gameFont);

        g.setColor(Color.white);
        g.setFont(gameFont);
        g.drawString(msg, (FIELD_WIDTH - metr.stringWidth(msg)) 
                / 2, FIELD_HEIGHT / 2);
        g.drawString(help, (FIELD_WIDTH - metr.stringWidth(help)) / 2, 
                FIELD_HEIGHT / 2 + 20);
    }

    /**
     * Check if apple collides with head
     */
    private void checkAim() {

        if ((x[0] == aim_x) && (y[0] == aim_y)) {

            dots++;
            locateAim();
        }
    }

    /**
     * Control of snake direction
     */
    private void move() {

        for (int z = dots; z > 0; z--) {
            x[z] = x[(z - 1)];
            y[z] = y[(z - 1)];
        }

        if (leftDirection) {
            x[0] -= DOT_SIZE;
        }

        if (rightDirection) {
            x[0] += DOT_SIZE;
        }

        if (upDirection) {
            y[0] -= DOT_SIZE;
        }

        if (downDirection) {
            y[0] += DOT_SIZE;
        }
    }

    /**
     * Check if the snake has hit itself or one of the walls
     */
    private void checkCollision() {

        for (int z = dots; z > 0; z--) {

            if ((z > 4) && (x[0] == x[z]) && (y[0] == y[z])) {
                inGame = false;
            }
        }

        if (y[0] >= FIELD_HEIGHT) {
            inGame = false;
        }

        if (y[0] < 0) {
            inGame = false;
        }

        if (x[0] >= FIELD_WIDTH) {
            inGame = false;
        }

        if (x[0] < 0) {
            inGame = false;
        }
        
        if(!inGame) {
            timer.stop();
        }
    }

    private void locateAim() {

        int r = (int) (Math.random() * RAND_POS);
        aim_x = ((r * DOT_SIZE));

        r = (int) (Math.random() * RAND_POS);
        aim_y = ((r * DOT_SIZE));
    }

    @Override
    public void actionPerformed(ActionEvent e) {

        if (inGame) {

            checkAim();
            checkCollision();
            move();
            
        }

        repaint();
    }

    public boolean getRight(){
        return rightDirection;
    }
}
