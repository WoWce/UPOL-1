/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package klunko.zp3jv.s03;
import java.util.Arrays;
import java.util.ArrayList;
import java.util.List;
import java.util.Iterator;
import java.lang.Object;

/**
 *
 * @author Misha
 */
public class Lecture3 {

    static int[] odd(int[] foo) {
        int numberOdds = 0;
        for (int i = 0; i < foo.length; i++) {
            if (foo[i] % 2 != 0) {
                numberOdds++;
            }  
        }
        int[] odds = new int[numberOdds];
        int count = 0;
        for (int i = 0; i < foo.length; i++) {
            if (foo[i] % 2 == 1 && foo[i] != 0) {
                odds[count] = foo[i];
                count++;
            }      
        }      
        return odds;
        
    }
    
    public static List<Object> odd(List<Object> foo){
        
        for (Iterator<Object> iterator = foo.iterator(); iterator.hasNext();){
            Object number = iterator.next();
            
            if (!(number instanceof Integer) ||(int)number % 2 == 0) {
                 iterator.remove();
            }

        }
        
        
        
        return foo;
    }
    
    public static List<Integer> oddNumbers(List<Integer> foo){
        for (Iterator<Integer> iterator = foo.iterator(); iterator.hasNext();){
            Integer number = iterator.next();
            if (number % 2 == 0) {
            iterator.remove();
            }

        }
        return foo;
    }
    
    static int[] merge(int[] a, int[] b){
        int m = a.length;
        int n = b.length;
        int i = 0;
        int j = 0;
        int index = 0;
        int[] out_array = new int[m + n];
        while (i < m && j < n){
            if (a[i] < b[j]){
                out_array[index] = a[i];
                i++;
            }
            else{
                out_array[index] = b[j];
                j++;
            }

                index++;
                
        }
        while (i < m){
            out_array[index] = a[i];
            index++;
            i++;
        }

        while (j < n){
            out_array[index] = b[j];
            index++;
            j++;
        }
        return out_array;
    }
   
    static List<Integer> merge(List<Integer> a, List<Integer> b){
        List<Integer> out_list = new ArrayList();
        int m = a.size();
        int n = b.size();
        int i = 0;
        int j = 0;
        while (i < m && j < n){
            if (a.get(i) < b.get(j)){
                out_list.add(a.get(i));
                i++;
            }
            else{
                out_list.add(b.get(j));
                j++;
            }                
        }
        while (i < m){
            out_list.add(a.get(i));
            i++;
        }

        while (j < n){
            out_list.add(b.get(j));
            j++;
        }
        return out_list;
    }
    
    static List<Integer> mergeSort(List<Integer> a){
        if(a.size() < 2) return a;
        int m = a.size() / 2;
	List<Integer> list1 = new ArrayList(a.subList(0, m));
	List<Integer> list2 = new ArrayList(a.subList(m, a.size()));
	return merge(mergeSort(list1), mergeSort(list2));
    }
    
    public static void main(String[] args) {
        //1
        int numbers[] = {4, 5, 2, 4, 7, 9, 3, 12, 22, 120, 7, 99};
        String intArrayString = Arrays.toString(odd(numbers));
        System.out.println("N1 " + intArrayString);
        
        //2
        List<Object> intlist = new ArrayList();
        intlist.add(5);
        intlist.add(3);
        intlist.add("string");
        intlist.add(14);
        intlist.add(137);
        intlist.add(1);
        System.out.println("N2 " + odd(intlist));
        
        //3
        List<Integer> a = new ArrayList<>();
        a.add(3);
        a.add(128);
        a.add(7);
        a.add(99);
        System.out.println("N3 " + oddNumbers(a));
        
        //4
        Point mainpoint = new Point(0, 0);
        Point one = new Point(4,3);
        Point two = new Point(5,7);
        Point three = new Point(-1,4);
        Point four = new Point(5,0);
        Point five = new Point(3,-2);
        List<Point> points = new ArrayList<Point>();
        points.add(one);
        points.add(two);
        points.add(three);
        points.add(four);
        points.add(five);
        Point.nearest(mainpoint, points);
        
        //5.1
        int[] first = {1, 4, 8, 9, 18, 43};
        int[] second = {2, 3, 5, 6, 7, 25, 61};
        String intArrayString2 = Arrays.toString(merge(first, second));
        System.out.println("N5.1 " + intArrayString2);
        
        
        //5.2
        List<Integer> list1 = new ArrayList<>();
        List<Integer> list2 = new ArrayList<>();
        list1.add(1);
        list1.add(3);
        list1.add(7);
        list1.add(35);
        list1.add(72);
        list2.add(2);
        list2.add(4);
        list2.add(6);
        list2.add(120);
        System.out.println("N5.2 " + merge(list1, list2));
        
        //6
        List<Integer> list3 = new ArrayList<>();
        list3.add(5);
        list3.add(22);
        list3.add(1);
        list3.add(43);
        list3.add(12);
        list3.add(24);
        list3.add(3);
        list3.add(7);
        list3.add(39);
        list3.add(50);
        System.out.println("N6 " + mergeSort(list3));
    }
    
}
