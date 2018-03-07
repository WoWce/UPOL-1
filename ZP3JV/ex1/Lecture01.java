/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package klunko.zp3jv.s01;

/**
 *
 * @author Misha
 */
public class Lecture01 {

    /**
     * @param args the command line arguments
     */
    public static void main(String[] args) {
        /* Úkol 1 */
        int vstup = 6;          /* lokální proměnná */
        for(int i = vstup; i > 0; i--){
            for(int k = 0; k < vstup; k++){
                if((k == (i - 1)) || (k == vstup - 1) || (i == 1)){
                    System.out.print("*");
                }
                else
                    System.out.print(" ");
            }
                System.out.println();
       }
        /* Úkol 2 */
        for(int l = 0; l < vstup; l++){
            for(int m = 0; m <= vstup * (vstup - 1); m++)
                if((m % (vstup - 1) == 0 && m != 0) || 
                        (l + m) % (vstup - 1) == 0 && l != 0)
                    System.out.print("*");
                else
                    System.out.print(" ");
            System.out.println();
        }
        /* Úkol 3 */
        int[] array = {10, 2, 13, -4, 15, -6, 7};
        int min = array[0];
        int max = array[0];
        for (int i = 1; i < 7; i++){
            if(array[i] < min){
                min = array[i];
            }
            if(array[i] > max){
                max = array[i];
            }
        }
        System.out.println("Minimum: " + min);
        System.out.println("Maximum: " + max);
        /* Úkol 4 */
        int N = 100;
        int pocet = 1;
        int t, a[] = new int[N];
        for (t = 2; t < N; t++) a[t] = 1;
        for (t = 2; t < N; t++) 
         if (a[t]==1)
             for(int j = t; j*t < N; j++) a[t*j] = 0;
        for (t = 2; t < N; t++)
         if (a[t]==1){
             System.out.print(t + " "); 
             pocet++;
             if((pocet - 1) % 10 == 0)
                 System.out.println(" ");
         }
    }
    
}
