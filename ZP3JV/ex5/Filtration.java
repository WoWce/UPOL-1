/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package klunko.zp3jv.s05;

import java.util.ArrayList;
import java.util.Iterator;
import java.util.List;

/**
 *
 * @author Misha
 */
public class Filtration {
    public interface Condition{
        Object filter(Object o);
    }
    public class Cond {
        public Object filter(Object o){
            if((int)o % 2 == 0){
                return o;
            }
            else{return "odd";}
        }
    }
    List<Object> filter(List<Object> list, Condition c){
        List<Object> list2 = new ArrayList();
        for (Iterator<Object> iterator = list.iterator(); iterator.hasNext();){
            Object element = iterator.next();
            if(c.filter(element) != "odd"){
                list2.add(c.filter(element));
            }
        }
        return list2;
    }
    List<Object> filter(List<Object> list, Cond c){
        List<Object> list2 = new ArrayList();
        for (Iterator<Object> iterator = list.iterator(); iterator.hasNext();){
            Object element = iterator.next();
            if(c.filter(element) != "odd"){
                list2.add(c.filter(element));
            }
        }
        return list2;
    }
    
    
}
