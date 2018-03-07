/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package klunko.zp3jv.s09;

import java.io.*;
import java.util.ArrayList;
import java.util.List;
import java.util.Scanner;

/**
 * Matrix reader/writer
 * @author Michael Klunko
 */
public class MatrixRW {

    /**
     * Reads matrix from text file
     * @param r - class Reader
     * @return binary matrix in int array
     * @throws IOException 
     */
    int[][] readTextMatrix(Reader r) throws IOException {
        List<List<Integer>> matrix = new ArrayList<>();

        Scanner scanner = null;

        try {
            scanner = new Scanner(new BufferedReader(r));
            for (int i = 0; scanner.hasNext(); i++) {
                Scanner s = new Scanner(scanner.nextLine());

                List<Integer> row = new ArrayList<>();

                for (int col = 0; s.hasNextInt(); col++) {
                    row.add(s.nextInt());
                }

                matrix.add(row);

                s.close();
            }

            scanner.close();

            int[][] result = new int[matrix.size()][];
            for (int i = 0; i < result.length; i++) {
                List<Integer> row = matrix.get(i);

                result[i] = new int[row.size()];

                for (int j = 0; j < row.size(); j++) {
                    result[i][j] = row.get(j);
                }
            }

            return result;
        } finally {
            if (scanner != null) {
                scanner.close();
            }
        }
        
    }
    /**
     * Writes matrix to text file from int array
     * @param w - class Writer
     * @param matrix - two dimensional int array with matrix
     * @throws IOException 
     */
    void writeTextMatrix(Writer w, int[][] matrix) throws IOException {
        BufferedWriter bw = null;
        try{
            bw = new BufferedWriter(w);
            for(int i = 0; i < matrix.length; i++){
                for(int j = 0; j < matrix[i].length; j++){
                    bw.write(String.valueOf(matrix[i][j]));
                    bw.write(" ");
                }
                bw.newLine();
            }
            bw.flush();
            bw.close();
        }
        finally{
            bw.close();
        }
    }
    /**
     * Writes matrix to the Output Stream in binary format
     * @param s - Output Stream
     * @param matrix - two-dimensional array
     * @throws IOException 
     */
    public static void writeBinaryMatrix(OutputStream s, int[][] matrix) throws IOException{
        DataOutputStream a = new DataOutputStream(s);
        try{
            
            for(int[] row : matrix){
                int i = 0;
                for(int x : row){
                    a.writeInt(x);
                    if(i < row.length - 1){
			a.writeChar(' ');
                    }
                    i++;
                }
                a.writeChar('|');   
            }
            a.close();
        }
        finally{
            a.close();
        }
        
    }
    /**
     * Reads matrix from Input Stream in binary format
     * @param s - Input Stream
     * @return - two-dimensional array
     * @throws IOException 
     */
    int[][] readBinaryMatrix(InputStream s) throws IOException{
        DataInputStream a = null;
        List<List<Integer>> matrix = new ArrayList<>();
        
        List<Integer> row = new ArrayList<Integer>();
        try {
            a = new DataInputStream(s);
            while(a.available() > 0) {
		int number = a.readInt();
		row.add(number);

		char c = a.readChar();
		if(c == '|') {
                    matrix.add(row);
                    row = new ArrayList<Integer>();
		}
            }
            a.close();
            int[][] result = new int[matrix.size()][];
            for (int i = 0; i < result.length; i++) {
                 row = matrix.get(i);

                result[i] = new int[row.size()];

                for (int j = 0; j < row.size(); j++) {
                    result[i][j] = row.get(j);
                }
            }
            return result;
        } 
        finally {
            a.close();   
        }
        
    }
    
    
}
