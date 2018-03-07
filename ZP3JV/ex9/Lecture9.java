/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package klunko.zp3jv.s09;

import java.io.FileInputStream;
import java.io.FileOutputStream;
import java.io.FileReader;
import java.io.FileWriter;
import java.io.IOException;
import java.io.InputStream;
import java.io.OutputStream;
import java.io.Reader;
import java.io.Writer;

/**
 * I/O Streams
 * @author Michael Klunko
 */
public class Lecture9 {

    /**
     * Prints two-dimensional array
     * @param a - array
     */
    static void printMatrix(int[][] a){
        for(int i = 0; i < a.length; i++){
            for(int j = 0; j < a[i].length; j++){
                System.out.print(a[i][j] + " ");
            }
            System.out.println();
        }
    }
    /**
     * @param args the command line arguments
     */
    public static void main(String[] args) throws IOException {
        MatrixRW m = new MatrixRW();
        //Reader class with path to file with matrix
        Reader r = new FileReader("E:\\matrix.txt");
        //Writer class with path to output file
        Writer w = new FileWriter("E:\\out.txt");
        //Output stream
        OutputStream s = new FileOutputStream("E:\\bin.txt");
        //Input stream
        InputStream in = new FileInputStream("E:\\bin.txt");
        //uncomment to try StringWriter class: 
        //Writer w = new StringWriter();
        int[][] a = m.readTextMatrix(r);
        printMatrix(a);
        
        m.writeTextMatrix(w, a);
        
        m.writeBinaryMatrix(s, a);
        System.out.println();
        
        int[][] x = m.readBinaryMatrix(in);
        printMatrix(x);
        
        
        
    }
    
}
