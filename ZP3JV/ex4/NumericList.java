/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package klunko.zp3jv.s04;
import java.util.Iterator;
import java.util.List;
/**
 *
 * @author Misha
 * @param <T>
 */
public class NumericList<T extends Number> {
    
    private List<T> list;
    
    public NumericList(List<T> value){
        this.list = value;
    }
    
    T first(){
        return list.get(0);
    }
    
    NumericList<T> next(){
        NumericList<T> subl = 
                new NumericList<>(list.subList(1, list.size()));
        
        return subl;
    }
    
    int size(){
        return list.size();      
    }
    double sum(){
        double suml = 0;
        Double num;
        for(int i = 0; i < list.size(); i++){
            num = list.get(i).doubleValue();
            suml += (double)num;
        }
        return suml;
}
    
}
