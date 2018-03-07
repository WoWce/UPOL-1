/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package klunko.zp3jv.s05;
import java.util.List;
import java.util.ArrayList;
import java.util.Iterator;
/**
 *
 * @author Misha
 */
public class Map {
    
    public interface Mapping {
    Object map(Object o);
}
    
    List<Object> map(List<Object> list, Mapping m){
        List<Object> list2 = new ArrayList();
        for (Iterator<Object> iterator = list.iterator(); iterator.hasNext();){
            Object element = iterator.next();
            list2.add(m.map(element));
        }
        return list2;
    }

    
}
